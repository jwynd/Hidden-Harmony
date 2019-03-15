using System; // Used for Single.TryParse
using System.Text.RegularExpressions; // used for Regex Matching
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SoundObject : MonoBehaviour
{
    [SerializeField] private float offsetRange = 0.05f;
    [SerializeField] private float vfxDuration = 1.0f;
    [SerializeField] private Material passive;
    [SerializeField] private Material active;
    [SerializeField] private Timekeeper timekeeper;
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
    private float vfxTimer;
    private bool vfxTimerActive = false;
    private float beatTimer = 0.0f;// use to determine when one beat has passed
    private float nextTimer = 0.0f;// use to determine when to play next beat
    private AudioSource[] audioSources;
    private float interactDist = 1.0f;
    private string suffix;
    private AudioSource[] bgs;
    private bool resetNext = false;

    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(this.transform.position, Vector3.down*interactDist);
    }
    void Awake(){
        player = GameObject.Find("Player");
    }
    // Start is called before the first frame update
    void Start(){
        rendered = this.transform.GetChild(0).GetChild(0).gameObject;
        if(timekeeper == null) throw new System.ArgumentException("Timekeeper null");
        audioSources = this.GetComponents<AudioSource>();
        foreach(AudioSource aS in audioSources)print(aS);
//        light = gameObject.GetComponent<Light>();
        origin = transform.position;
        //timekeeper = GameObject.Find("Timekeeper").GetComponent<Timekeeper>();
        if(passive == null || active == null){
            throw new System.ArgumentException("Place materials in SoundObject script");
        }
        if(player == null)print("Null player!!!");
    }

    // Update is called once per frame
    void Update(){
        if(timekeeper == null) throw new System.ArgumentException("Timekeeper null");
        timekeeper.SetBPM(100);
        beat = timekeeper.GetBeat();
        // print(beat);
        nextTimer += Time.deltaTime;
        beatTimer += Time.deltaTime;
        if(beatTimer > beat){
            beatIndex++;
            if(stg != null && beatIndex > stg.beats.Length - 1){
                beatIndex = 0;
            }
            if(resetNext) nextTimer = 0.0f;
            if(resetNext) resetNext = false;
            beatTimer = 0.0f;
        }
        /*print("beatTimer");
        print(beatTimer);
        print("nextTimer");
        print(nextTimer);
        print("beatIndex");
        print(beatIndex);
        print("stg");
        print(stg);*/
        //if(stg != null) print("current beatIndex ="+beatIndex);
        if(stg != null && nextTimer/beat > stg.beats[beatIndex]) resetNext = true;
        else if(stg !=null && nextTimer/beat > stg.beats[beatIndex] - offsetRange) audioSources[stg.pitches[beatIndex]].volume -= Time.deltaTime/offsetRange;
        // determine stage by checking a ray cast, then use expression matching to determine the offset by the tag.
        RaycastHit hit;
        Ray stageRay = new Ray(this.transform.position, Vector3.down);
        if(Physics.Raycast(stageRay, out hit, interactDist)){
            if(hit.transform.tag == "StageObj"){
                stg = hit.transform.gameObject.GetComponent<Stage>();
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

        if(onStage && nextTimer == 0.0f){
            // print("Playing sound at time "+nextTimer);
            // print("stg.pitches[beatIndex] = "+stg.pitches[beatIndex]);
            audioSources[stg.pitches[beatIndex]].volume = 1.0f;
            audioSources[stg.pitches[beatIndex]].Play();
            vfxTimerActive = true;
        }

        if(vfxTimerActive){
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

    }
}
