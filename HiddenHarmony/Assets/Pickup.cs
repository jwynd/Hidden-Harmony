using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float interactDistance = 5.0f;

    private Transform camera;
    private Transform player;
    private Transform holdPosition;
    private bool held = false;
    private Rigidbody rigi;
    // Start is called before the first frame update
    /*void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(camera.position, camera.forward*interactDistance);
    }*/

    void Start(){
        rigi = this.GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;
        camera = GameObject.Find("Player/MainCamera").transform;
        holdPosition = GameObject.Find("Player/HoldPosition").transform;
    }

    // Update is called once per frame
    void Update(){
        RaycastHit hit;
        Ray pickRay = new Ray(camera.position, camera.forward);
        if(!held && Input.GetKeyDown(KeyCode.E)){
            if(Physics.Raycast(pickRay, out hit, interactDistance)){
                if(hit.collider.tag == "SoundObj"){
                    rigi.useGravity = false;
                    this.transform.position = holdPosition.position;
                    this.transform.parent = player;
                    held = true;
                }
            }
//            print("!held && key press on E");
        }
        else if(held && Input.GetKeyDown(KeyCode.E)){
//            print("held && key press on E");
            rigi.useGravity = true;
            this.transform.parent = null;
            held = false;
        }
    }
}
