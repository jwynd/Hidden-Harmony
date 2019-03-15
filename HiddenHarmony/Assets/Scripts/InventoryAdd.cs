using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryAdd : MonoBehaviour
{
    [HideInInspector] public bool held = false;
    public float interactDistance = 5.0f;

    private GameObject intMsg;
    private GameObject currentObject;
    private Transform camera;
    private Transform player;
    private Transform holdPosition;
    private GameObject itemPanel;
    private float dist;
    [SerializeField] private GameObject itemButton;

    void Start(){
        intMsg = GameObject.Find("InteractMessageController");
        player = GameObject.Find("Player").transform;
        camera = GameObject.Find("Player/MainCamera").transform;
        holdPosition = GameObject.Find("Player/MainCamera/HoldPosition").transform;
        itemPanel = GameObject.Find("Canvas/ItemsHeld");
    }

    // Update is called once per frame
    void Update(){
        //if(currentObject != null) dist = Vector3.Distance(holdPosition.position, currentObject.transform.position);

        RaycastHit hit;
        Ray pickRay = new Ray(camera.position, camera.forward);
        if(Input.GetKeyDown(KeyCode.E)){
            if(Physics.Raycast(pickRay, out hit, interactDistance)){
                if(hit.collider.tag == "SoundObj"){
                    GameObject newButton;
                    newButton = Instantiate(itemButton, itemPanel.transform);
                    
                }
            }
        }
        else {
            intMsg.GetComponent<InteractMessage>().HideInteractMessage();
            if(Physics.Raycast(pickRay, out hit, interactDistance)){
                if(hit.collider.tag == "SoundObj"){
                    intMsg.GetComponent<InteractMessage>().ShowInteractMessage("Press 'E' to pick up");
                }
            }
        }
    }
}
