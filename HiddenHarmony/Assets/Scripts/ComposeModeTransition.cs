using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComposeModeTransition : MonoBehaviour
{
    [Header("Requires 3 audio Sources. In, Out, and Invalid")]
    [SerializeField] private Transform composeCameraPosition;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float transitionSpeed = 20.0f;
    [SerializeField] private float orthoSize = 18f;
    [SerializeField] private float orthoClipPlane = 2f;
    [SerializeField] private float perspectiveFOV = 60f;
    [SerializeField] private float perspectiveClipPlane = 0.3f;
    private Vector3 cameraOrigin;
    private Vector3 playerOrigin;
    private GameObject interactMessage;
    private float startTime;
    private float distCovered;
    private float fracJourney;
    private float journeyLength;
    private bool transitioning = false;
    private bool compose = false;
    private bool canTransition = true;
    private bool firstPress = true; // Enables/handles the tutorial shit
    private GameObject playerCamera;
//    private GameObject composeCamera;
    private GameObject player;
    private Transform cameraReturn;
    private AudioSource[] sources;
    private CanvasGroup SequencerCG;
    private bool inHub = false;

    public GameObject tutorialArrow;

    public void AllowCompose(){
        inHub = true;
    }

    public void ForbidCompose(){
        inHub = false;
    }

    public bool CanCompose(){
        return inHub;
    }


    public bool Compose(){
        return compose;
    }
    public bool Changing(){
        return transitioning;
    }
    // Start is called before the first frame update
    void Awake(){
        player = GameObject.Find("Player");
        playerCamera = GameObject.Find("Player/MainCamera");
//        composeCamera = new GameObject("composeCamera");
        cameraReturn = GameObject.Find("Player/CameraReturn").transform;
        interactMessage = GameObject.Find("Canvas/InteractMessage");
        sources = this.GetComponents<AudioSource>();
        /*      composeCamera.tag = "MainCamera";
                composeCamera.transform.SetAsLastSibling();
                composeCamera.AddComponent<Camera>();
                composeCamera.SetActive(false);
        */
        SequencerCG = Sequencer.Instance.gameObject.GetComponent<CanvasGroup>();
        SequencerCG.alpha = 0.0f;
        SequencerCG.interactable = false;
        SequencerCG.blocksRaycasts = false;

    }

    // Update is called once per frame
    void Update(){
        if(inHub && Input.GetKeyDown(KeyCode.Tab) && canTransition && !PauseMenu.Instance.GetPaused()){
            //print("Tab key pressed");
            //print(compose?"compose":"!compose");
            //print(transitioning?"transitioning":"!transitioning");
            if(firstPress)
            {
                // Enable the arrow thing on the UI and set firstPress to false
                firstPress = false;
                tutorialArrow.SetActive(true);
            }
            if(!compose && !transitioning){
                //print("Transitioning to compose");
                player.GetComponent<CharacterController>().enabled = false;
                player.GetComponent<PlayerMovement>().enabled = false;
                player.GetComponent<FirstPersonControl>().enabled = false;
                player.GetComponent<InventoryAdd>().enabled = false;
                player.GetComponent<InteractScript>().enabled = false;
                playerCamera.GetComponent<FirstPersonControl>().enabled = false;
                playerCamera.transform.SetParent(null);
                interactMessage.transform.gameObject.SetActive(false);
                player.transform.Find("GlideParticles").GetComponent<ParticleSystem>().Stop();
                player.transform.Find("Audio/GlideAudio").GetComponent<AudioSource>().Stop();
                SequencerCG.alpha = 1.0f;
                SequencerCG.interactable = true;
                SequencerCG.blocksRaycasts = true;
                cameraOrigin = cameraReturn.position;
                sources[0].Play();
                //composeCamera.transform.position = cameraOrigin;
                compose = true;
                transitioning = true;
                startTime = Time.time;
                journeyLength = Vector3.Distance(cameraOrigin, composeCameraPosition.position);
                Cursor.visible = true;
                //composeCamera.transform.SetAsFirstSibling();
                //playerCamera.SetActive(false);
                //composeCamera.SetActive(true);
            } else if (compose && !transitioning){
                //print("Transitioning to First person");
                playerCamera.GetComponent<Camera>().orthographic = false;
                playerCamera.GetComponent<Camera>().fieldOfView = perspectiveFOV;
                playerCamera.GetComponent<Camera>().nearClipPlane = perspectiveClipPlane;

                player.GetComponent<CharacterController>().enabled = true;
                player.GetComponent<PlayerMovement>().enabled = true;
                player.GetComponent<FirstPersonControl>().enabled = true;
                player.GetComponent<InventoryAdd>().enabled = true;
                player.GetComponent<InteractScript>().enabled = true;
                playerCamera.GetComponent<FirstPersonControl>().enabled = true;
                interactMessage.transform.gameObject.SetActive(true);
                SequencerCG.alpha = 0.0f;
                SequencerCG.interactable = false;
                SequencerCG.blocksRaycasts = false;
                sources[1].Play();
                //composeCamera.transform.SetAsLastSibling();

                compose = false;
                transitioning = true;
                startTime = Time.time;
                journeyLength = Vector3.Distance(composeCameraPosition.position, cameraReturn.position);
            }
        }
        else if((!inHub || !canTransition) && Input.GetKeyDown(KeyCode.Tab)){
            sources[2].Play();
        }
        if(compose && transitioning){
            playerCamera.transform.LookAt(cameraTarget);

            distCovered = (Time.time - startTime) * transitionSpeed;

            fracJourney = distCovered / journeyLength;

            playerCamera.transform.position = Vector3.Lerp(cameraOrigin, composeCameraPosition.position, fracJourney);
            if(Vector3.Distance(playerCamera.transform.position, composeCameraPosition.position) < 0.01f){
                // Make Orthographic at the end of the transition to exploremode
                playerCamera.GetComponent<Camera>().orthographic = true;
                playerCamera.GetComponent<Camera>().orthographicSize = orthoSize;
                playerCamera.GetComponent<Camera>().nearClipPlane = orthoClipPlane;
                playerCamera.transform.position = composeCameraPosition.position;
                playerCamera.transform.LookAt(cameraTarget);
                transitioning = false;
            }
        } else if (!compose && transitioning){
            playerCamera.transform.LookAt(cameraTarget);

            distCovered = (Time.time - startTime) * transitionSpeed;

            fracJourney = distCovered / journeyLength;

            playerCamera.transform.position = Vector3.Lerp(composeCameraPosition.position, cameraReturn.position, fracJourney);
            if(Vector3.Distance(playerCamera.transform.position, cameraReturn.position) < 0.01f){
                //                composeCamera.SetActive(false);
                //                playerCamera.SetActive(true);
                playerCamera.transform.position = cameraReturn.position;
                playerCamera.transform.LookAt(cameraTarget);
                Vector3 playerLook = cameraTarget.position;
                playerLook.y = player.transform.position.y;
                player.transform.LookAt(playerLook);

                transitioning = false;
                playerCamera.transform.SetParent(player.transform);
            }
            Cursor.visible = false;
        }
    }
    
    public void composeImmediate(){
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<FirstPersonControl>().enabled = false;
        player.GetComponent<InventoryAdd>().enabled = false;
        player.GetComponent<InteractScript>().enabled = false;
        playerCamera.GetComponent<FirstPersonControl>().enabled = false;
        playerCamera.transform.SetParent(null);
        interactMessage.transform.gameObject.SetActive(false);
        player.transform.Find("GlideParticles").GetComponent<ParticleSystem>().Stop();
        player.transform.Find("Audio/GlideAudio").GetComponent<AudioSource>().Stop();
        cameraOrigin = cameraReturn.position;
        compose = true;
        playerCamera.transform.position = composeCameraPosition.position;
        playerCamera.transform.LookAt(cameraTarget);
        transitioning = false;
    }

    public bool getTransition(){
        return canTransition;
    }

    public bool setTransition(bool t){
        canTransition = t;
        return canTransition;
    }
}
