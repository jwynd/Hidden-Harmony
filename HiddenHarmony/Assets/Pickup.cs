﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float interactDistance = 5.0f;

    private GameObject currentObject;
    private Transform camera;
    private Transform player;
    private Transform holdPosition;
    private bool held = false;
    private Rigidbody rigi;
    /*void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(camera.position, camera.forward*interactDistance);
    }*/

    void Start(){
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
                    //print("Raycast hit SoundObj");
                    currentObject = hit.collider.gameObject;
                    rigi = currentObject.GetComponent<Rigidbody>();
                    rigi.useGravity = false;
                    currentObject.transform.position = holdPosition.position;
                    // print(currentObject.transform.position);
                    currentObject.transform.parent = player;
                    held = true;
                }
            }
            //print("!held && key press on E");

        }

        else if(held && Input.GetKeyDown(KeyCode.E)){
            //print("hend and keycode e");
            if(currentObject == null) nullObject("currentObject");
            rigi.useGravity = true;
            currentObject.transform.parent = null;
            held = false;
        }

        if(held && currentObject != null){
            currentObject.transform.localPosition = holdPosition.localPosition;
        }
        //maintain the hold position above the ground
        /*Ray posRay = new Ray(holdPosition.position, Vector3.down);
        if(Physics.Raycast(posRay, out hit, 1.5f)){
            if(hit.point.y > holdPosition.position.y){
                holdPosition.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
            }
        }*/
        /*
        if(currentObject != null && held){
            // print(currentObject.transform.localPosition);
            rigi.velocity = ;
        }*/
    }

    void nullObject(string msg){
        throw new System.ArgumentException("Object "+msg+" is null");
    }
}
