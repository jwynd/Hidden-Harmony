using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    private AudioSource aS;
    private float interactDist = 1.0f;
    private float resetTimer = 0.0f;
    private const float rTime = 6.0f;
    private bool onStage = false;
    private float stageOffset = 0.0f;

    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(this.transform.position, Vector3.down*interactDist);
    }
    // Start is called before the first frame update
    void Start(){
        aS = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate(){
        // print(resetTimer);
        resetTimer += Time.fixedDeltaTime;
        if(resetTimer > rTime){
            resetTimer = 0.0f;
        }

        RaycastHit hit;
        Ray stageRay = new Ray(this.transform.position, Vector3.down);
        if(Physics.Raycast(stageRay, out hit, interactDist)){
            if(hit.collider.tag == "StageObj0"){
                //print("On Stage 0");
                onStage = true;
                stageOffset = 0.0f;
            }
            if(hit.collider.tag == "StageObj3"){
                //print("On Stage 3");
                onStage = true;
                stageOffset = 3.0f;
            }
        }

        if(onStage && resetTimer > stageOffset-0.5f && resetTimer < stageOffset+0.5f){
            //print("Playing sound");
            aS.Play();
        }


        onStage = false;
    }
}
