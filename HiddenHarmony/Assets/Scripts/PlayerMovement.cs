﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    // Based on code from Stephen Barr

    private float yVelocity = 0f; //the counter to determine at which point in the jumpTime the player is at.
    private bool jumping = false; //checks if the player is jumping
    private CharacterController character; //creates a game object for storage
    //private Rigidbody rigidbody;

    public float speed = 6f; //sets speed multiplier
    //speed suggested value 6f
    public float gravity = 7f; //the value subtracted from the y axis to calculate gravity
    //gravity suggested value 7f. Actual value 9.8f
    public float jumpTime = 17f; //the maximun value of the jumpTime
    //jumpTime suggested value 17f;
    public float jumpSpeed = 0.5f; //how fast the jumpTime accelerates per frame
    //jumpSpeed suggested value 0.5f
    public float canJump = 0.55f; //the distance between the player and the ground at which the player can jumpTime
    //canJump suggested value 1.25f. Assumes player is of height 2 and takes half, which is 1. The 0.25 is added for wiggle room.

    // Use this for initialization
    void Start () {
        character = GetComponent<CharacterController> (); //gets the character controller from the GameObject
        //rigidbody = GetComponent<Rigidbody> ();
    }    

    // Update is called once per frame
    void Update () {
        if(Input.GetKeyDown(KeyCode.Space) && Physics.Raycast (transform.position, Vector3.down, canJump)) { // add && Physics.Raycast (transform.position, Vector3.down, canJump) for ground check?
            if (!jumping){
                jumping = true;            
                yVelocity = jumpTime;
            }
        }

        if(jumping && yVelocity > 0){
            yVelocity -= jumpSpeed; //increases jumpTime velocity when jumping and before jumpTime total is reached
        }
        else{
            jumping = false;
            yVelocity = 0; //resets jumpTime velocity when jumpTime is finished
        }

        float moveX = Input.GetAxis ("Horizontal") * speed; //
        float moveZ = Input.GetAxis ("Vertical") * speed;
        Vector3 movement = new Vector3 (moveX, 0, moveZ);

        movement = Vector3.ClampMagnitude (movement, speed); //Limits the max speed of the player
        movement.y = movement.y - gravity + yVelocity;
        movement *= Time.deltaTime; //Ensures the speed the player moves does not change based on frame rate
        movement = transform.TransformDirection(movement);
        character.Move (movement);
    }
}