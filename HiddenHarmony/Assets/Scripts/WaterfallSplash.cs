using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallSplash : MonoBehaviour
{
    private AudioSource geyserSplash;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        geyserSplash = player.transform.Find("Audio/GeyserSplashAudio").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collide){
        if(!geyserSplash.isPlaying){
            geyserSplash.Play();
        }
    }
}
