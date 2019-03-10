using System; // Used for Single.TryParse
using System.Text.RegularExpressions; // used for Regex Matching
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Backup_SoundObject : MonoBehaviour
{
    [SerializeField] private int loopLength = 4;
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private float offsetRange = 0.05f;
    [SerializeField] private Material passive;
    [SerializeField] private Material active;
    [HideInInspector] public bool onStage = false;
    [HideInInspector] public Vector3 origin;

    private GameObject stage; // used to snap sound object to the center of the stage
    private GameObject snapPoint;
    private GameObject rendered;
    private GameObject player;
    private bool reActivateSnapPoint = false;
    private float beat;
    private float measureTime;
    private float vfxTimer;
    private bool vfxTimerActive = false;
    private float resetTimer = 0.0f;
    private AudioSource aS;
    private float interactDist = 1.0f;
    private float stageOffset = 0.0f;
    private string pattern = "StageObj";
    private string suffix;
    private Timekeeper timekeeper;
    private AudioSource[] bgs;
    private bool thisIsHeld = false;

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
        timekeeper = GameObject.Find("Timekeeper").GetComponent<Timekeeper>();
        if(timekeeper == null) throw new System.ArgumentException("Timekeeper null");
        aS = gameObject.GetComponent<AudioSource>();
//        light = gameObject.GetComponent<Light>();
        if(offsetRange <= 0.0f) throw new System.ArgumentException("Offset Range must be greater than 0");
        if(loopLength <= 1) throw new System.ArgumentException("Loop Length must be at least 1");
        origin = transform.position;
        if(passive == null || active == null){
            throw new System.ArgumentException("Place materials in SoundObject script");
        }
        if(player == null)print("Null player!!");
    }

    // Update is called once per frame
    void FixedUpdate(){
        thisIsHeld = player.GetComponent<Pickup>().IsHeld(this.transform.gameObject);

        if(timekeeper == null) throw new System.ArgumentException("Timekeeper null");
        beat = timekeeper.GetBeat();
        measureTime = beat*loopLength;
        resetTimer += Time.fixedDeltaTime;
        if(resetTimer > measureTime){
            resetTimer = 0.0f;
        }
        // determine stage by checking a ray cast, then use expression matching to determine the offset by the tag.
        RaycastHit hit;
        Ray stageRay = new Ray(this.transform.position, Vector3.down);
        if(Physics.Raycast(stageRay, out hit, interactDist) && !thisIsHeld){
            Match match = Regex.Match(hit.collider.tag, pattern);
            if(match.Success){
                suffix = hit.collider.tag.Substring(8);
                if(Single.TryParse(suffix, out stageOffset)){
                    stageOffset = (stageOffset-1)*beat;
                    if(stageOffset <= measureTime && stageOffset >= 0.0f){

                        onStage = true;
                    } 
                    else{
                        onStage = false;
                    }
                }
                else{
                    onStage = false;
                }
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
            if(stage.transform.childCount > 0 && !thisIsHeld && !reActivateSnapPoint){
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

        if(onStage && resetTimer > stageOffset-offsetRange && resetTimer < stageOffset+offsetRange){
            // print("Playing sound at time "+resetTimer);
            aS.Play();
            vfxTimerActive = true;
        }

        if(vfxTimerActive){
            // print("light on");
            vfxTimer += Time.fixedDeltaTime;
            rendered.GetComponent<MeshRenderer>().material = active;
        }
        else{
            // print("light off");
            rendered.GetComponent<MeshRenderer>().material = passive;
        }

        if(vfxTimer > duration*beat){
            vfxTimer = 0.0f;
            vfxTimerActive = false;
        }

    }
}
