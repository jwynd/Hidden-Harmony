using System; // Used for Single.TryParse
using System.Text.RegularExpressions; // used for Regex Matching
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(AudioSource))]
public class SoundObject : MonoBehaviour
{
    [Header("Audio Clips")]
    [Tooltip("Old style soundobjects will still work. Feel free to leave this blank")]
    [SerializeField] private AudioClip[] audioClips;
    [Header("Other settings")]
    [SerializeField] private float offsetRange = 0.05f;
    [SerializeField] [RangeAttribute(1.0f,5.0f)] private float interactDist = 1.0f;
    [SerializeField] private Timekeeper timekeeper;
    [SerializeField] private Color crystalColor;
    [ColorUsageAttribute(true,true)] [SerializeField] private Color emissionColor;
    [HideInInspector] public bool onStage = false;
    [HideInInspector] public Vector3 origin;

    private int beatIndex = 0;
    private Stage stg;
    private GameObject stage; // used to snap sound object to the center of the stage
    private GameObject snapPoint;
    private GameObject rendered;
    private GameObject player;
    private bool reActivateSnapPoint = false;
    private float beat;
    //private float vfxTimer;
    //private bool vfxTimerActive = false;
    private float beatTimer = 0.0f;// use to determine when one beat has passed
    private float nextTimer = 0.0f;// use to determine when to play next beat
    private AudioSource[] audioSources;
    private AudioSource audioSource;
    private string suffix;
    private AudioSource[] bgs;
    private bool resetNext = false;
    private GameObject[] crystals;
    private Shader shader;

    private int cbeat;
    private int playOnBeat;
    private bool played = true;

    private Material notGlowing;
    private Material glowing;

    private int mod;
    private int[] cutoffs;
    private int nextCutoff;

    private Count count;

    private void FormatSoundObjectOld(){
        audioSources = this.GetComponents<AudioSource>();
        if(audioClips.Length > 0){
            foreach(AudioSource aud in audioSources){
                Destroy(aud);
            }
            audioSources = new AudioSource[audioClips.Length];
            for(int i = 0; i < audioClips.Length; i++){
                audioSources[i] = this.gameObject.AddComponent<AudioSource>() as AudioSource;
                // Ensure application exits on error
                if(audioSources[i] == null){
                    throw new System.ArgumentException("FormatSoundObjectOld Failed to create AudioSource"+i);
                    #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                    #else
                    Application.Quit();
                    #endif
                }
                audioSources[i].clip = audioClips[i];
                audioSources[i].spatialize = true;
                audioSources[i].spatialBlend = 1.0f;
                audioSources[i].reverbZoneMix = 1.0f;
                audioSources[i].dopplerLevel = 0.0f;
                audioSources[i].spread = 0.0f;
                audioSources[i].rolloffMode = AudioRolloffMode.Custom;
                audioSources[i].maxDistance = 50;
            }
        }
    }

    private void FormatSoundObjectNew(){
        // This function is called in awake and ensures that the sound object takes the proper form
        audioSources = this.GetComponents<AudioSource>();
        audioSource = audioSources[0];
        if(audioClips.Length == 0){
            audioClips = new AudioClip[audioSources.Length];
            for(int i = 0; i < audioClips.Length; ++i){
                audioClips[i] = audioSources[i].clip;
                if(i > 0) Destroy(audioSources[i]);
            }
        }
        audioSources = null;
    }

    public bool OnStage(){
        return onStage;
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(this.transform.position, Vector3.down*interactDist);
    }
    void Awake(){
        player = GameObject.Find("Player");
        FormatSoundObjectOld();
    }
    // Start is called before the first frame update
    void Start(){
        shader = Shader.Find("Custom/ToonOutlineEmission");
        rendered = this.transform.GetChild(0).GetChild(0).gameObject;
        if(timekeeper == null) throw new System.ArgumentException("Timekeeper null");

        origin = transform.position;

        if(player == null)print("Null player!!!");
        beatIndex = 0;

        notGlowing = new Material(shader);
        notGlowing.SetColor("_Color", crystalColor);
        glowing = new Material(shader);
        glowing.SetFloat("_UseEmission", 1.0f);
        glowing.SetColor("_EmissionColor", emissionColor);

        count = GameObject.Find("GameplayObjects/Count").GetComponent<Count>();

    }

    // Update is called once per frame
    void Update(){
        if(timekeeper == null) throw new System.ArgumentException("Timekeeper null");
        cbeat = timekeeper.CurrentHalfBeat();
        if(stg != null && played){
            mod = 0;
            cutoffs = new int[stg.halfBeats.Length];
            for(int i = 0; i < stg.halfBeats.Length; i++){
                mod += stg.halfBeats[i];
                cutoffs[i] = mod;
            }
            nextCutoff = 0;
            for (int i = 0; i < cutoffs.Length; i++){
                if(cbeat % mod < cutoffs[i]){
                    beatIndex = i;
                    nextCutoff = cutoffs[beatIndex] - (cbeat % mod);
                    break;
                }
            }
            playOnBeat = cbeat + nextCutoff;
            played = false;
        }

        RaycastHit hit;
        Ray stageRay = new Ray(this.transform.position, Vector3.down);
        if(Physics.Raycast(stageRay, out hit, interactDist)){
            if(hit.transform.tag == "StageObj"){
                stg = hit.transform.gameObject.GetComponent<Stage>();
                crystals = stg.crystals;
                onStage = true;
                count.IncrementCount(this.gameObject.name);

            }
            else{
                onStage = false;
            }
        }
        else{
            onStage = false;
        }

        if(onStage){
            stage = hit.transform.gameObject;
            if(stage.transform.childCount > 0 && !reActivateSnapPoint){
                    snapPoint = stage.transform.GetChild(0).gameObject;
                    this.transform.position = snapPoint.transform.position;
                    snapPoint.transform.SetParent(null);
                    reActivateSnapPoint = true;
            }
        }

        if(reActivateSnapPoint && !onStage){
            snapPoint.transform.SetParent(stage.transform);
            snapPoint.transform.SetAsFirstSibling();
            reActivateSnapPoint = false;
        }

//        if(Time.time > timekeeper.FadeOutStartTime(playOnBeat) && Time.time < timekeeper.FadeOutEndTime(playOnBeat)){
//                audioSource.volume -= (float)(Time.deltaTime/0.01);
//        }
        
        if(onStage && cbeat == playOnBeat && !played){
            // print("Playing sound at time "+nextTimer);
            // print("stg.pitches[beatIndex] = "+stg.pitches[beatIndex]);
            audioSources[stg.pitches[beatIndex]].volume = 1.0f;
            audioSources[stg.pitches[beatIndex]].Play();
//            audioSource.clip = audioClips[stg.pitches[beatIndex]];
//            audioSource.volume = 1.0f;
//            audioSource.Play();
            played = true;
            //vfxTimerActive = true;
        }

        // Below, light up crystal for current beat. Assume it has the same index as beatIndex
        for(int i = 0; onStage && i < crystals.Length; ++i){
            if(i == beatIndex){
                crystals[i].GetComponent<Renderer>().material = glowing;
            } else {
                crystals[i].GetComponent<Renderer>().material = notGlowing;
            }
        }

    }
    private int abs(int x){
        if(x < 0) return x * -1;
        else return x;
    }
    public void blankStage(){
        foreach(GameObject crystal in crystals){
            crystal.GetComponent<Renderer>().material = notGlowing;
        }
    }
}

