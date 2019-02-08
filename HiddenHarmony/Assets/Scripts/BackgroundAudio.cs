using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour {

    public Transform player;
    private AudioSource aS;
    // Use this for initialization
    void Start () {
        aS = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if(player.localPosition.x < -17.97f){
            aS.Play();
        }
    }
}
