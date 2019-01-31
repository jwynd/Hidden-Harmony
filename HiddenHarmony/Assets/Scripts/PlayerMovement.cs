using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    // Based on code from Stephen Barr
    public float speed = 6f; //sets speed multiplier
    public float jump = 65f;

    private CharacterController character; //creates a game object for storage
    private Rigidbody rigidbody;

    // Use this for initialization
    void Start () {
        character = GetComponent<CharacterController> (); //gets the character controller from the GameObject
        rigidbody = GetComponent<Rigidbody> ();
    }
    
    // Update is called once per frame
    void Update () {
        /*
        if(Input.GetKeyDown(KeyCode.Return)) {
            character.enabled = !character.enabled;
        }
        */
        /*if(this.transform.position.y < -10.0f){
            this.transform.position = new Vector3(this.transform.position.x, 10.0f, this.transform.position.z);
        }*/
        if(Input.GetKeyDown(KeyCode.Space)) {
            //rigidbody.AddForce(new Vector3(0, jump, 0), ForceMode.Impulse);
            Vector3 jumpMovement = new Vector3 (0, jump, 0);
            
            //jumpMovement = Vector3.ClampMagnitude (jumpMovement, speed); //Limits the max speed of the player
            jumpMovement *= Time.deltaTime; //Ensures the speed the player moves does not change based on frame rate
            jumpMovement = transform.TransformDirection(jumpMovement);
            character.Move (jumpMovement);
        }

        // Manual gravity script because we needed something working.
        if(this.transform.position.y > 0.0f){
            character.Move(new Vector3(0.0f, -0.05f, 0.0f));
        }
        /*if(this.transform.position.y < 0.0f){
            this.transform.position = new Vector3(this.transform.position.x, 0.0f, this.transform.position.z);
        }
        else{
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 5.0f*Time.deltaTime, this.transform.position.z);
        }*/

        float moveX = Input.GetAxis ("Horizontal") * speed; //
        float moveZ = Input.GetAxis ("Vertical") * speed;
        Vector3 movement = new Vector3 (moveX, 0, moveZ);
        
        movement = Vector3.ClampMagnitude (movement, speed); //Limits the max speed of the player
        movement *= Time.deltaTime; //Ensures the speed the player moves does not change based on frame rate
        movement = transform.TransformDirection(movement);
        character.Move (movement);
    }
}
