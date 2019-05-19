using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    // Based on code from Stephen Barr

    private bool jumping = false; //checks if the player is jumping
    private bool canGlide = true; //can the player glide
    private bool canSprint = true; //can the player sprint
    private CharacterController character; //creates a game object for storage
    
    [Header("Movement")]
    [Tooltip("The speed at which the player moves horizontally")]
    public float speed = 6f; //sets speed multiplier
    //speed suggested value 6f
    [Tooltip("How high the player jumps (Must be greater than terminalVelocity)")]
    public float jump = 70f; //0the maximun value of the jump. value must be larger than terminalVelocity
    //jump suggested value 70f
    [Tooltip("The rate at which the player approaches terminal velocity")]
    public float gravity = 0.6f; //the rate at which yVelocity decreases during a jump unitl terminal velocity is reached
    //gravity suggested value 0.6f
    [Tooltip("The maximum speed at which the player falls")]
    public float terminalVelocity = 55f; //the value subtracted from the y axis to calculate terminalVelocity
    //terminalVelocity suggested value 55f
    [Tooltip("The distance from the ground at which the player can jump")]
    public float canJump = 1.25f; //the distance between the player and the ground at which the player can jump
    //canJump suggested value for capsulecast 1.25f. I don't know why this works, but it does.
    //canJump suggested value for raycast 1.25f. Assumes player is of height 2 and takes half, which is 1. The 0.25 is added for wiggle room.
    [Header("Glide")]
    [Tooltip("The rate at which the player falls during glide (value does not relate to gravity variable)")]
    public float glideGravity = 3f; //the rate at which the player falls while gliding (calculated much differently than the standard gravity)
    //glideGravity suggested value 3f
    [SerializeField][Tooltip("The speed added to the players movement while gliding")]
    private float glideBoost = 4f; //the value added to speed during gliding
    //glideBoost suggested value 4f
    [SerializeField][Tooltip("(0-100) The volume at which the glide music plays")]
    private float glideMusicVolume = 50f; //value divided by 100 in code - the max volume at which the glide music will play
    [SerializeField][Tooltip("The rate at which the glide music fades in and out")]
    private float glideMusicChange = 0.005f; //the rate at which music volume changes during glide
    [Header("Sprint")]
    [SerializeField][Tooltip("Speed added when sprinting")]
    private float sprintBoost = 4f;
    [Header("Field of View")]
    [SerializeField][Tooltip("The original angle for the camera's field of vision")]
    private float baseFOV = 60f; //the original field of vision angle for the player
    [SerializeField][Tooltip("The angle for the camera's field of vision when sprinting or gliding")]
    private float boostedFOV = 75f; //the field of vision angle for the player while gliding or sprinting (hence, boosted)
    [SerializeField][Tooltip("The rate at which the camera changes between fields of view")]
    private float changeSpeedFOV = 0.25f; //the rate at which the field of view changes during glide
    [Header("Miscellaneous")]
    [SerializeField][Tooltip("Hold shift to glide instead of holding spacebar")]
    private bool shiftToGlide = false;
    [Tooltip("(Not for changing) Tracks the velocity of the player")]
    public float yVelocity; //the counter to determine at which point in the jump the player is at.
    private ParticleSystem particles; //the particle system of the player object
    private Camera camera; //the main camera on the player object
    private AudioSource audio; //the audio source on the player object

    // Use this for initialization
    void Start () {
        yVelocity = terminalVelocity;
        character = GetComponent<CharacterController> (); //gets the character controller from the GameObject
        particles = this.transform.Find("GlideParticles").gameObject.GetComponent<ParticleSystem>();
        camera = this.transform.Find("MainCamera").gameObject.GetComponent<Camera>();
        audio = this.transform.Find("AudioSource").gameObject.GetComponent<AudioSource>();
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
                yVelocity = terminalVelocity;
            }
            if(particles.isPlaying){
                particles.Stop();
            }
        }

        if(yVelocity <= terminalVelocity){
            jumping = false;
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            jumping = false;
            if(yVelocity > terminalVelocity + (terminalVelocity * 0.15f)){
                yVelocity = terminalVelocity + (terminalVelocity * 0.15f);
            }
            if(particles.isPlaying){
                particles.Stop();
            }
        }

        if(yVelocity > 0){
            if(!Physics.CapsuleCast (p1, p2, character.radius, Vector3.down, canJump)){
                yVelocity -= gravity; //increases jump velocity when jumping and before jump total is reached
            }
        }
        else{
            yVelocity = 0; //resets jump velocity when jump is finished
        }

        float moveX;
        float moveZ;
        Vector3 movement;
        moveX = Input.GetAxis ("Horizontal") * (speed + Glide().x + Sprint());
        moveZ = Input.GetAxis ("Vertical") * (speed + Glide().x + Sprint());
        movement = new Vector3 (moveX, 0, moveZ);
        movement = Vector3.ClampMagnitude (movement, speed + Glide().x + Sprint()); //Limits the max speed of the player
        if(Glide().x + Sprint() == 0){
            toBaseFOV();
        }

        if(canGlide){
           movement.y = movement.y + Glide().y;
        }
        else{
           movement.y = movement.y - terminalVelocity + yVelocity;
        }
        movement *= Time.fixedDeltaTime; //Ensures the speed the player moves does not change based on frame rate
        movement = transform.TransformDirection(movement);
        character.Move (movement);
    }

    public Vector2 Glide(){
        Vector2 returnVector = new Vector2();
        Vector3 p1 = transform.position + new Vector3(character.center.x, character.center.y + 0.5f, character.center.z) + Vector3.up * -character.height * 0.5f;
        Vector3 p2 = p1 + Vector3.up * character.height;
        if((shiftToGlide ? Input.GetKey(KeyCode.LeftShift) : Input.GetKey(KeyCode.Space)) && !jumping && !Physics.CapsuleCast (p1, p2, character.radius, Vector3.down, canJump)){
            //if we want to hold forward use below code in the if statement
            //(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            returnVector.x = glideBoost;
            returnVector.y = -glideGravity;
            yVelocity = terminalVelocity;
            if(isMoving()){
                if(!particles.isPlaying){
                    particles.Play();
                }
                if(!audio.isPlaying){
                    audio.Play();
                }
                toGlideVolume();
                toBoostFOV();
            }
        }
        else{
            returnVector.x = 0;
            returnVector.y = -terminalVelocity + yVelocity;
            if(audio.isPlaying){
                fadeToMute();
            }
        }
        return returnVector;
    }

    public float Sprint(){
    if(Input.GetKey(KeyCode.LeftShift) && isMoving() && Glide().x == 0){
            toBoostFOV();
            return sprintBoost;
        }
        else{
            return 0;
        }
    }

    public bool isMoving(){
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)
        || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.RightArrow)){
            return true;
        }
        else{
            return false;
        }
    }

    public void toBoostFOV(){
        if(camera.fieldOfView < boostedFOV){
            camera.fieldOfView = Mathf.MoveTowards(camera.fieldOfView, boostedFOV, changeSpeedFOV);
        }
        else if(camera.fieldOfView > boostedFOV){
            camera.fieldOfView = boostedFOV;
        }
    }

    public void toBaseFOV(){
        if(camera.fieldOfView > baseFOV){
                camera.fieldOfView = Mathf.MoveTowards(camera.fieldOfView, baseFOV, changeSpeedFOV);
            }
            else if(camera.fieldOfView < baseFOV){
                camera.fieldOfView = baseFOV;
            }
    }

    public void toGlideVolume(){
        if(audio.volume < glideMusicVolume/100f){
            audio.volume = Mathf.MoveTowards(audio.volume, glideMusicVolume/100f, glideMusicChange);
        }
        else if(audio.volume > glideMusicVolume/100f){
            audio.volume = glideMusicVolume/100f;
        }
    }

    public void fadeToMute(){
        if(audio.volume > 0){
            audio.volume = Mathf.MoveTowards(audio.volume, 0, glideMusicChange);
        }
        else if(audio.volume < 0){
            audio.volume = 0;
        }
        else if(audio.volume == 0){
            audio.Stop();
        }
    }
}