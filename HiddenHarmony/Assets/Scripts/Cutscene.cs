
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Rendering.PostProcessing;

public class Cutscene : MonoBehaviour
{
    [Tooltip("Place the mp4 file for the cutscene here")]
    [SerializeField] private VideoClip cutscene;
    [Tooltip("Video clip volume")]
    [SerializeField][Range(0.0f, 1.0f)] private float vol = 0.1f;
    [Tooltip("The distance at which the cutscene can be started")]
    [SerializeField] private float interactDistance = 10.0f;
    [Tooltip("All game objects in the array will be enabled during the cutscene")]
    [SerializeField] private GameObject[] toEnable;
    [Tooltip("Do not put this game object in this list")]
    [SerializeField] private GameObject[] toDestroy;
    [Tooltip("Fade time in seconds")]
    [SerializeField] private float fadeTime = 1.0f;
    [Tooltip("Put main camera here")]
    [SerializeField]private GameObject camera;

    private Count count;
    private GameObject player;
    private GameObject canvas;
    private VideoPlayer vp;
    private Fade fade;
    private bool prepared;
    private bool played = false;
    private bool fadeInitiated = false;
    private bool fadeSet = false;

    void Awake(){
        player = GameObject.Find("Player");
        canvas = GameObject.Find("Canvas");
        fade = GameObject.Find("Canvas/Fade").GetComponent<Fade>();
    }

    void Start()
    {
        count = GameObject.Find("GameplayObjects/Count").GetComponent<Count>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray cutRay = new Ray(camera.transform.position, camera.transform.forward);
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0) && !played)
        {
            if (Physics.Raycast(cutRay, out hit, interactDistance))
            {
                if(hit.collider.gameObject == this.gameObject)
                {
                    // initiate a fadeout
                    fade.FadeOut(fadeTime);
                    fadeInitiated = true;
                    this.tag = "Untagged";
                }
            }
        }
        /*if(fadeInitiated && !fade.IsFading()){
            //Debug.Log("Setting Fade");
            fade.SetFade(0.99f);
        } else {
            fade.UnsetFade();
        }*/
        //initiate cutscene and disable player control once the fadeout is complete
        if(fadeInitiated && !fade.IsFading() && !played){
            player.GetComponent<CharacterController>().enabled = false;
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<FirstPersonControl>().enabled = false;
            player.GetComponent<InventoryAdd>().enabled = false;
            player.GetComponent<InteractScript>().enabled = false;
            camera.GetComponent<FirstPersonControl>().enabled = false;
            camera.GetComponent<PostProcessVolume>().enabled = false;
            AudioListener.volume = 0.0f;
            vp = camera.AddComponent<VideoPlayer>();
            vp.clip = cutscene;
            vp.renderMode = VideoRenderMode.CameraNearPlane;
            vp.playOnAwake = false;
            vp.targetCameraAlpha = 1.0f;
            vp.isLooping = false;
            vp.SetDirectAudioVolume(0, vol);
            vp.Play();
            played = true;
            foreach(GameObject o in toEnable){
                o.SetActive(true);
            }
            fadeInitiated = false;
            fade.SetFade(1.0f);
        }
        // if the video is playing move the far clip plane close to avoid wierd artifacts
        if(vp != null && vp.isPlaying){
            camera.GetComponent<Camera>().farClipPlane = 0.31f;
            fade.UnsetFade();
            canvas.SetActive(false);
        }
        // set the prepared variable so that the cutscene isn't registered as played before it starts
        if(!prepared && vp != null) prepared = vp.isPrepared;

        //Fade back in after the cutscene plays
        if(played && prepared && !vp.isPlaying && !fadeInitiated){
            //Debug.Log("Initiating fade");
            camera.GetComponent<Camera>().farClipPlane = 1000.0f;
            canvas.SetActive(true);
            camera.GetComponent<PostProcessVolume>().enabled = true;
            vp.clip = null;
            Renderer[] rends = GetComponentsInChildren<Renderer>();
            foreach(Renderer r in rends) r.enabled = false;
            foreach(GameObject o in toDestroy){
                #if UNITY_EDITOR
                if(o == this.gameObject){
                    Debug.LogError("Don't put the cutscene object in the to destroy list, it will destroy itself automatically");
                    UnityEditor.EditorApplication.isPlaying = false;
                }
                #endif
                Debug.Log("Destroying "+o.name);
                Destroy(o);
            }
            // foreach(DisableOrEnableTag Instance in DisableOrEnableTag.Instances){
            //     Instance.LocateTags();
            // }
            fade.FadeIn(fadeTime);
            fadeInitiated = true;
        }

        if(played && prepared && !vp.isPlaying && fadeInitiated && !fade.IsFading()){
            player.GetComponent<CharacterController>().enabled = true;
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<FirstPersonControl>().enabled = true;
            player.GetComponent<InventoryAdd>().enabled = true;
            player.GetComponent<InteractScript>().enabled = true;
            camera.GetComponent<FirstPersonControl>().enabled = true;
            AudioListener.volume = 1.0f;
            count.statuesUsed++;
            Destroy(this.gameObject);
        }
    }
}
