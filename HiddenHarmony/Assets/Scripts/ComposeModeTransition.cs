using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComposeModeTransition : MonoBehaviour
{
    [SerializeField] private Transform composeCameraPosition;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float transitionSpeed = 20.0f;
    private Vector3 cameraOrigin;
    private float startTime;
    private float distCovered;
    private float fracJourney;
    private float journeyLength;
    private bool transitioning = false;
    private bool compose = false;
    private GameObject playerCamera;
    private GameObject composeCamera;
    private GameObject player;


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
        composeCamera = new GameObject("composeCamera");
        composeCamera.tag = "MainCamera";
        composeCamera.transform.SetAsLastSibling();
        composeCamera.AddComponent<Camera>();
        composeCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            print("Tab key pressed");
            if(!compose && !transitioning){
                print("Transitioning to compose");
                player.GetComponent<CharacterController>().enabled = false;
                player.GetComponent<PlayerMovement>().enabled = false;
                player.GetComponent<FirstPersonControl>().enabled = false;
                player.GetComponent<Pickup>().enabled = false;
                playerCamera.GetComponent<FirstPersonControl>().enabled = false;
                playerCamera.transform.SetParent(null);
                cameraOrigin = playerCamera.transform.position;
                compose = true;
                transitioning = true;
                startTime = Time.time;
                journeyLength = Vector3.Distance(cameraOrigin, composeCameraPosition.position);
                Cursor.visible = true;
            } else if (compose && !transitioning){
                print("Transitioning to First person");
                player.GetComponent<CharacterController>().enabled = true;
                player.GetComponent<PlayerMovement>().enabled = true;
                player.GetComponent<FirstPersonControl>().enabled = true;
                player.GetComponent<Pickup>().enabled = true;
                playerCamera.GetComponent<FirstPersonControl>().enabled = true;
                composeCamera.transform.SetAsLastSibling();
                composeCamera.SetActive(false);
                playerCamera.SetActive(true);
                compose = false;
                transitioning = true;
                startTime = Time.time;
                journeyLength = Vector3.Distance(composeCameraPosition.position, cameraOrigin);
            }
        }
        if(compose && transitioning){
            playerCamera.transform.LookAt(cameraTarget);

            distCovered = (Time.time - startTime) * transitionSpeed;

            fracJourney = distCovered / journeyLength;

            playerCamera.transform.position = Vector3.Lerp(cameraOrigin, composeCameraPosition.position, fracJourney);
            if(Vector3.Distance(playerCamera.transform.position, composeCameraPosition.position) < 0.01f){
                playerCamera.transform.position = composeCameraPosition.position;
                playerCamera.transform.LookAt(cameraTarget);
                composeCamera.transform.position = composeCameraPosition.position;
                composeCamera.transform.LookAt(cameraTarget);
                composeCamera.transform.SetAsFirstSibling();
                playerCamera.SetActive(false);
                composeCamera.SetActive(true);
                transitioning = false;
            }
        } else if (!compose && transitioning){
            playerCamera.transform.LookAt(cameraTarget);

            distCovered = (Time.time - startTime) * transitionSpeed;

            fracJourney = distCovered / journeyLength;

            playerCamera.transform.position = Vector3.Lerp(composeCameraPosition.position, cameraOrigin, fracJourney);
            if(Vector3.Distance(playerCamera.transform.position, cameraOrigin) < 0.01f){
                playerCamera.transform.position = cameraOrigin;
                playerCamera.transform.LookAt(cameraTarget);
                player.transform.LookAt(cameraTarget);
                transitioning = false;
                playerCamera.transform.SetParent(player.transform);
            }
            Cursor.visible = false;
        }
    }
}
