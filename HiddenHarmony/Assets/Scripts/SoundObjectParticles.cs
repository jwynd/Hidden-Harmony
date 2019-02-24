using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions; // used for Regex Matching

public class SoundObjectParticles : MonoBehaviour {
    private ParticleSystem particle;
    private Pickup pickup;

    private bool onGround;
    private string pattern = "StageObj";

    public float dropDist = 0.55f;
    // Start is called before the first frame update
    void Start() {
        particle = GetComponent<ParticleSystem>();
        pickup = GameObject.Find("Player").GetComponent<Pickup>();
    }

    // Update is called once per frame
    void Update() {
        RaycastHit hit;
        Ray stageRay = new Ray(transform.position, Vector3.down);
        bool held = pickup.held;
        if(Physics.Raycast(stageRay, out hit, dropDist) && !onGround && !held){
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
