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
    private GameObject camera;
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
        camera = GameObject.Find("Player/MainCamera");
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
                camera.GetComponent<FirstPersonControl>().enabled = false;
                camera.transform.SetParent(null);
                cameraOrigin = camera.transform.position;
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
                camera.GetComponent<FirstPersonControl>().enabled = true;
                
                compose = false;
                transitioning = true;
                startTime = Time.time;
                journeyLength = Vector3.Distance(composeCameraPosition.position, cameraOrigin);
            }
        }
        if(compose && transitioning){
            camera.transform.LookAt(cameraTarget);

            distCovered = (Time.time - startTime) * transitionSpeed;

            fracJourney = distCovered / journeyLength;

            camera.transform.position = Vector3.Lerp(cameraOrigin, composeCameraPosition.position, fracJourney);
            if(Vector3.Distance(camera.transform.position, composeCameraPosition.position) < 0.01f){
                camera.transform.position = composeCameraPosition.position;
                camera.transform.LookAt(cameraTarget);
                transitioning = false;
            }
        } else if (!compose && transitioning){
            camera.transform.LookAt(cameraTarget);

            distCovered = (Time.time - startTime) * transitionSpeed;

            fracJourney = distCovered / journeyLength;

            camera.transform.position = Vector3.Lerp(composeCameraPosition.position, cameraOrigin, fracJourney);
            if(Vector3.Distance(camera.transform.position, cameraOrigin) < 0.01f){
                camera.transform.position = cameraOrigin;
                camera.transform.LookAt(cameraTarget);
                player.transform.LookAt(cameraTarget);
                transitioning = false;
                camera.transform.SetParent(player.transform);
            }
            Cursor.visible = false;
        }
    }
}
