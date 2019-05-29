using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Cutscene : MonoBehaviour
{
    [Tooltip("Place the mp4 file for the cutscene here")]
    [SerializeField] private VideoClip cutscene;
    [Tooltip("The distance at which the cutscene can be started")]
    [SerializeField] private float interactDistance = 10.0f;
    [Tooltip("All game objects in the array will be enabled during the cutscene")]
    [SerializeField] private GameObject[] toEnable;
    [Tooltip("Do not put this game object in this list")]
    [SerializeField] private GameObject[] toDestroy;

    private GameObject player;
    private GameObject camera;
    private GameObject intMsg;
    private GameObject canvas;
    private VideoPlayer vp;
    private bool prepared;
    private bool played = false;

    void Awake(){
        player = GameObject.Find("Player");
        camera = GameObject.Find("Player/MainCamera");
        intMsg = GameObject.Find("InteractMessageController");
        canvas = GameObject.Find("Canvas");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray cutRay = new Ray(camera.transform.position, camera.transform.forward);
        if (Input.GetKeyDown(KeyCode.E) && !played)
        {
            if (Physics.Raycast(cutRay, out hit, interactDistance))
            {
                if(hit.collider.gameObject == this.gameObject)
                {
                    player.GetComponent<CharacterController>().enabled = false;
                    player.GetComponent<PlayerMovement>().enabled = false;
                    player.GetComponent<FirstPersonControl>().enabled = false;
                    player.GetComponent<InventoryAdd>().enabled = false;
                    player.GetComponent<InteractScript>().enabled = false;
                    camera.GetComponent<FirstPersonControl>().enabled = false;
                    canvas.SetActive(false);
                    camera.GetComponent<Camera>().far = 0.31f;
                    vp = camera.AddComponent<VideoPlayer>();
                    vp.clip = cutscene;
                    vp.renderMode = VideoRenderMode.CameraNearPlane;
                    vp.playOnAwake = false;
                    vp.targetCameraAlpha = 1.0f;
                    vp.isLooping = false;
                    vp.Play();
                    played = true;
                    foreach(GameObject o in toEnable){
                        o.SetActive(true);
                    }
                }
            }
        } 
        if(!prepared && vp != null) prepared = vp.isPrepared;
        if(played && prepared && !vp.isPlaying){
            player.GetComponent<CharacterController>().enabled = true;
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<FirstPersonControl>().enabled = true;
            player.GetComponent<InventoryAdd>().enabled = true;
            player.GetComponent<InteractScript>().enabled = true;
            camera.GetComponent<FirstPersonControl>().enabled = true;
            canvas.SetActive(true);
            camera.GetComponent<Camera>().far = 1000.0f;
            foreach(GameObject o in toDestroy){
                Destroy(o);
            }
            Destroy(vp);
            Destroy(this.gameObject);
        }
    }
}
