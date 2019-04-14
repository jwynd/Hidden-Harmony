using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    public AudioClip source;
    public float transitionTime;
    public Material skybox;

    private AudioSource[] bg;
    private float timer = 0.0f;
    private bool fading = false;
    private AudioSource newBG;
    private AudioSource oldBG;

    // Start is called before the first frame update
    void Start(){
        bg = GameObject.Find("BackgroundMusic").GetComponents<AudioSource>();
        print(source.ToString());
//        samples = new float[source.samples * source.channels];
//        source.GetData(samples, 0);
    }

    // Update is called once per frame
    void Update(){
        if(fading){
            // Lerp Skybox
            timer += Time.deltaTime;
            RenderSettings.skybox.Lerp(RenderSettings.skybox, skybox, timer/transitionTime);

            //crossfade music
            oldBG.volume -= Time.deltaTime/transitionTime;
            newBG.volume += Time.deltaTime/transitionTime;

            if(oldBG.volume < 0.05f){
                oldBG.volume = 0.0f;
                newBG.volume = 1.0f;
                RenderSettings.skybox = skybox;
                fading = false;
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.name != "Player") return;
        if(bg[0].volume > 0.0f){
            oldBG = bg[0];
            newBG = bg[1];
        } else {
            oldBG = bg[1];
            newBG = bg[0];
        }
        if(oldBG.clip == source){print("Same Clip");return;}
        newBG.volume = 0.0f;
        newBG.clip = source;
        newBG.Play();
        fading = true;
        timer = 0.0f;

    }
}
