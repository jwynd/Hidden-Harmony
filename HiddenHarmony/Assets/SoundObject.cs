using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    private GameObject player;
    private AudioSource aS;
    private float dist;
    // Start is called before the first frame update
    void Start(){
        player = GameObject.Find("/Player");
        aS = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update(){
        dist = Vector3.Distance(player.transform.position, this.transform.position);
        // if dist < 3 && Input.getKeyDown(keycode.E) then the object is maintained at a short distance in front of the player
        // possibly use raycast instead so that you can differentiate between several objects
        // constant raycast down to make see when over stump
        // play sound in correct manner when over stump.

    }
}
