using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObjectParticles : MonoBehaviour
{
    private ParticleSystem particle;
    private bool onGround;

    public float dropDist = 0.55f;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
    	RaycastHit hit;
        Ray stageRay = new Ray(transform.position, Vector3.down);
    	if(Physics.Raycast(stageRay, out hit, dropDist) && !onGround){
    		particle.Play();
    		onGround = true;
    	}
        if(Physics.Raycast(stageRay, out hit, dropDist)){
        	onGround = true;
        }
        else{
        	onGround = false;
        }
    }
}
