using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChange : MonoBehaviour
{
    private AudioSource[] backgroundMusic;
    // Start is called before the first frame update
    void Start(){
        backgroundMusic = GameObject.Find("BackgroundMusic").GetComponents<AudioSource>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.name != "Player") return;
        if(this.name == "SubWooferTrigger"){
            backgroundMusic[0].Stop();
            backgroundMusic[1].Play();
        } else if(this.name == "HubTrigger"){
            backgroundMusic[1].Stop();
            backgroundMusic[0].Play();
        }
    }
}
