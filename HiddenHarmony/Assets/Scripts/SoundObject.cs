using System; // Used for Single.TryParse
using System.Text.RegularExpressions; // used for Regex Matching
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SoundObject : MonoBehaviour
{
    [SerializeField] private float offsetRange = 0.05f;
//    [SerializeField] private float vfxDuration = 1.0f;
    [SerializeField] [RangeAttribute(1.0f,5.0f)] private float interactDist = 1.0f;
//    [SerializeField] private Material passive;
//    [SerializeField] private Material active;
    [SerializeField] private Timekeeper timekeeper;
    [SerializeField] private Color crystalColor;
    [SerializeField][ColorUsageAttribute(true,true)] private Color emissionColor;
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
    private int elapsed;

    public bool OnStage(){
        return onStage;
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(this.transform.position, Vector3.down*interactDist);
    }
    void Awake(){
        player = GameObject.Find("Player");
    }
    // Start is called before the first frame update
    void Start(){
        shader = Shader.Find("Custom/ToonOutlineEmission");
        rendered = this.transform.GetChild(0).GetChild(0).gameObject;
        if(timekeeper == null) throw new System.ArgumentException("Timekeeper null");
        audioSources = this.GetComponents<AudioSource>();

        origin = transform.position;

        if(player == null)print("Null player!!!");
        beatIndex = 0;

        notGlowing = new Material(shader);
        notGlowing.SetColor("_Color", crystalColor);
        glowing = new Material(shader);
        glowing.SetFloat("_UseEmission", 1.0f);
        glowing.SetColor("_EmissionColor", emissionColor);

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
            elapsed = 0;
            for (int i = 0; i < cutoffs.Length; i++){
                if(cbeat % mod < cutoffs[i]){
                    beatIndex = i;
                    elapsed = (i==0 ? stg.halfBeats[stg.halfBeats.Length - 1] - (cbeat % mod) : (cbeat % mod) - cutoffs[i - 1]);
                    break;
                }
            }
            playOnBeat =  cbeat + stg.halfBeats[beatIndex] - elapsed;
            played = false;
        }

        RaycastHit hit;
        Ray stageRay = new Ray(this.transform.position, Vector3.down);
        if(Physics.Raycast(stageRay, out hit, interactDist)){
            if(hit.transform.tag == "StageObj"){
                stg = hit.transform.gameObject.GetComponent<Stage>();
                crystals = stg.crystals;
                onStage = true;
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

        if(onStage && cbeat == playOnBeat && !played){
            // print("Playing sound at time "+nextTimer);
            // print("stg.pitches[beatIndex] = "+stg.pitches[beatIndex]);
            audioSources[stg.pitches[beatIndex]].volume = 1.0f;
            audioSources[stg.pitches[beatIndex]].Play();
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


/*        if(vfxTimerActive){
            // print("light on");
            vfxTimer += Time.deltaTime;
            rendered.GetComponent<MeshRenderer>().material = active;
        }
        else{
            // print("light off");
            rendered.GetComponent<MeshRenderer>().material = passive;
        }

        if(vfxTimer > vfxDuration*beat){
            vfxTimer = 0.0f;
            vfxTimerActive = false;
        }
*/
    }
}
