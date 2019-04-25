using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    // Based on code from Stephen Barr

    [SerializeField] private float yVelocity; //the counter to determine at which point in the jump the player is at.
    private bool jumping = false; //checks if the player is jumping
    private bool canGlide = true; //can the player glide
    private CharacterController character; //creates a game object for storage
    
    public float speed = 6f; //sets speed multiplier
    //speed suggested value 6f
    [SerializeField] float glideBoost = 2f;
    public float gravity = 9f; //the value subtracted from the y axis to calculate gravity
    //gravity suggested value 9f. Actual value 9.8f
    public float glideGravity = 1f;//the value subtracted from the y axis to calculate gravity
    //gravity suggested value 9f. Actual value 9.8f
    public float jump = 17f; //the maximun value of the jump
    //jump suggested value 17f;
    public float jumpAcceleration = 0.5f; //the ammount the jump accelerates per frame
    //jumpAcceleration suggested value 0.5f
    public float canJump = 0.15f; //the distance between the player and the ground at which the player can jump
    //canJump suggested value for capsulecast 0.15. I don't know why this works, but it does.
    //canJump suggested value for raycast 1.25f. Assumes player is of height 2 and takes half, which is 1. The 0.25 is added for wiggle room.

    // Use this for initialization
    void Start () {
        yVelocity = gravity;
        character = GetComponent<CharacterController> (); //gets the character controller from the GameObject
    }    

    // Update is called once per frame
    void FixedUpdate () {
        //Unity p1 and p2 calculation
        Vector3 p1 = transform.position + new Vector3(character.center.x, character.center.y + 0.5f, character.center.z) + Vector3.up * -character.height * 0.5f;
        Vector3 p2 = p1 + Vector3.up * character.height;

        //Jump check
        //Raycast alternative Physics.Raycast (transform.position, Vector3.down, canJump)
        if (Physics.CapsuleCast (p1, p2, character.radius, Vector3.down, canJump)){ //capsule cast checks if capsule is touching the ground 
            if(Input.GetKeyDown(KeyCode.Space)) {         
                jumping = true;
                yVelocity = jump;
            }
            else if(!Input.GetKey(KeyCode.Space)) {        
                yVelocity = gravity;
            }
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            jumping = false;
            if(yVelocity > gravity){
                yVelocity = gravity;
            }
        }

        if(yVelocity > 0){
            if(!Physics.CapsuleCast (p1, p2, character.radius, Vector3.down, canJump)){
                yVelocity -= jumpAcceleration; //increases jump velocity when jumping and before jump total is reached
            }
        }
        else{
            yVelocity = 0; //resets jump velocity when jump is finished
        }

        float moveX;
        float moveZ;
        if(canGlide){
            moveX = Input.GetAxis ("Horizontal") * (speed + Glide().x);
            moveZ = Input.GetAxis ("Vertical") * (speed + Glide().x);
        }
        else{
            moveX = Input.GetAxis ("Horizontal") * speed;
            moveZ = Input.GetAxis ("Vertical") * speed;
        }
        Vector3 movement = new Vector3 (moveX, 0, moveZ);

       // movement = Vector3.ClampMagnitude (movement, speed); //Limits the max speed of the player
        if(canGlide){
           movement.y = movement.y + Glide().y;
        }
        else {
           movement.y = movement.y - gravity + yVelocity;
        }
        movement *= Time.fixedDeltaTime; //Ensures the speed the player moves does not change based on frame rate
        movement = transform.TransformDirection(movement);
        character.Move (movement);
    }

    public Vector2 Glide(){
        Vector2 returnVector = new Vector2();
        Vector3 p1 = transform.position + new Vector3(character.center.x, character.center.y + 0.5f, character.center.z) + Vector3.up * -character.height * 0.5f;
        Vector3 p2 = p1 + Vector3.up * character.height;
        if(Input.GetKey(KeyCode.Space) && !jumping && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !Physics.CapsuleCast (p1, p2, character.radius, Vector3.down, canJump)){
            returnVector.x = glideBoost;
            returnVector.y = -glideGravity;
            yVelocity = gravity;
        }
        else{
            returnVector.x = 0;
            returnVector.y = -gravity + yVelocity;
        }
        return returnVector;
    }
}