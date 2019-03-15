using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions; // used for Regex Matching

public class SoundObjectParticles : MonoBehaviour {
    private ParticleSystem particle;

    private bool onGround;
    private string pattern = "StageObj";

    public float dropDist = 0.55f;


    void Start() {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {
        RaycastHit hit;
        Ray stageRay = new Ray(transform.position, Vector3.down);
        if(Physics.Raycast(stageRay, out hit, dropDist) && !onGround){
            Match match = Regex.Match(hit.collider.tag, pattern);
            if(match.Success) {
                particle.Play();
                onGround = true;
            }
        }
        if(Physics.Raycast(stageRay, out hit, dropDist)){
            //onGround = true;
        }
        else{
            onGround = false;
        }
    }
}