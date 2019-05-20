using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Transition : MonoBehaviour
{
    public AudioClip source;
    public float transitionTime;
    public Material skybox;
    [SerializeField] private bool toUnderwater = false;
    [Tooltip("Post Processing Profile")]
    [SerializeField] private PostProcessProfile newPPP;
    [SerializeField] private bool canCompose = false;

    private AudioSource[] bg;
    private float timer = 0.0f;
    private bool fading = false;
    private AudioSource newBG;
    private AudioSource oldBG;
    private bool switchOnce;
    private ComposeModeTransition cmt;
//    private PostProcessProfile p;

    // Start is called before the first frame update
    void Awake(){
        bg = GameObject.Find("BackgroundMusic").GetComponents<AudioSource>();
        cmt = GameObject.Find("GameplayObjects/CameraChange").GetComponent<ComposeModeTransition>();
        //print(source.ToString());
//        p = GameObject.Find("MainCamera").GetComponent<PostProcessVolume>().profile;
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

            if(switchOnce && newBG.volume > 0.5f){
                RenderSettings.skybox = skybox;
                GameObject.Find("MainCamera").GetComponent<PostProcessVolume>().profile = newPPP;
                if(toUnderwater){
                    GameObject.Find("MainCamera").GetComponent<UnderwaterEffect>().enabled = true;
                    RenderSettings.fog = true;
                } else {
                    GameObject.Find("MainCamera").GetComponent<UnderwaterEffect>().enabled = false;
                    RenderSettings.fog = false;
                }
                switchOnce = false;
            }

            if(oldBG.volume < 0.05f){
                oldBG.volume = 0.0f;
                newBG.volume = 1.0f;
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
        if(canCompose){
            cmt.AllowCompose();
        } else {
            cmt.ForbidCompose();
        }
        if(oldBG.clip == source){print("Same Clip");return;}
        newBG.volume = 0.0f;
        newBG.clip = source;
        newBG.Play();
        fading = true;
        switchOnce = true;
        timer = 0.0f;

    }
}
