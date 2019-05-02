using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Transition : MonoBehaviour
{
    public AudioClip source;
    public float transitionTime;
    public Material skybox;
    [Tooltip("Post Processing Profile")]
    [SerializeField] private PostProcessProfile newPPP;

    private AudioSource[] bg;
    private float timer = 0.0f;
    private bool fading = false;
    private AudioSource newBG;
    private AudioSource oldBG;
    private PostProcessProfile p;

    // Start is called before the first frame update
    void Awake(){
        bg = GameObject.Find("BackgroundMusic").GetComponents<AudioSource>();
        print(source.ToString());
        p = GameObject.Find("MainCamera").GetComponent<PostProcessVolume>().profile;
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
                p = newPPP;
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
