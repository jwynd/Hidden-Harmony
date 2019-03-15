using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComposeModeTransition : MonoBehaviour
{
    [SerializeField] private Transform composeCameraPosition;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float transitionSpeed = 20.0f;
    private Vector3 cameraOrigin;
    private Vector3 playerOrigin;
    private float startTime;
    private float distCovered;
    private float fracJourney;
    private float journeyLength;
    private bool transitioning = false;
    private bool compose = false;
    private GameObject playerCamera;
    private GameObject composeCamera;
    private GameObject player;
    private Transform cameraReturn;

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
        cameraReturn = GameObject.Find("Player/CameraReturn").transform;
        composeCamera.tag = "MainCamera";
        composeCamera.transform.SetAsLastSibling();
        composeCamera.AddComponent<Camera>();
        composeCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            //print("Tab key pressed");
            //print(compose?"compose":"!compose");
            //print(transitioning?"transitioning":"!transitioning");
            if(!compose && !transitioning){
                //print("Transitioning to compose");
                player.GetComponent<CharacterController>().enabled = false;
                player.GetComponent<PlayerMovement>().enabled = false;
                player.GetComponent<FirstPersonControl>().enabled = false;
                player.GetComponent<InventoryAdd>().enabled = false;
                playerCamera.GetComponent<FirstPersonControl>().enabled = false;
                playerCamera.transform.SetParent(null);
                cameraOrigin = cameraReturn.position;
                composeCamera.transform.position = cameraOrigin;
                compose = true;
                transitioning = true;
                startTime = Time.time;
                journeyLength = Vector3.Distance(cameraOrigin, composeCameraPosition.position);
                Cursor.visible = true;
                composeCamera.transform.SetAsFirstSibling();
                playerCamera.SetActive(false);
                composeCamera.SetActive(true);
            } else if (compose && !transitioning){
                //print("Transitioning to First person");
                player.GetComponent<CharacterController>().enabled = true;
                player.GetComponent<PlayerMovement>().enabled = true;
                player.GetComponent<FirstPersonControl>().enabled = true;
                player.GetComponent<InventoryAdd>().enabled = true;
                playerCamera.GetComponent<FirstPersonControl>().enabled = true;
                composeCamera.transform.SetAsLastSibling();
                
                compose = false;
                transitioning = true;
                startTime = Time.time;
                journeyLength = Vector3.Distance(composeCameraPosition.position, cameraReturn.position);
            }
        }
        if(compose && transitioning){
            composeCamera.transform.LookAt(cameraTarget);

            distCovered = (Time.time - startTime) * transitionSpeed;

            fracJourney = distCovered / journeyLength;

            composeCamera.transform.position = Vector3.Lerp(cameraOrigin, composeCameraPosition.position, fracJourney);
            if(Vector3.Distance(composeCamera.transform.position, composeCameraPosition.position) < 0.01f){
                composeCamera.transform.position = composeCameraPosition.position;
                composeCamera.transform.LookAt(cameraTarget);
                transitioning = false;
            }
        } else if (!compose && transitioning){
            composeCamera.transform.LookAt(cameraTarget);

            distCovered = (Time.time - startTime) * transitionSpeed;

            fracJourney = distCovered / journeyLength;

            composeCamera.transform.position = Vector3.Lerp(composeCameraPosition.position, cameraReturn.position, fracJourney);
            if(Vector3.Distance(composeCamera.transform.position, cameraReturn.position) < 0.01f){
                composeCamera.SetActive(false);
                playerCamera.SetActive(true);
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
}
