using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    // Based on code from Stephen Barr
    public float speed = 6f; //sets speed multiplier

    private CharacterController character; //creates a game object for storage

    // Use this for initialization
    void Start () {
        character = GetComponent<CharacterController> (); //gets the character controller from the GameObject
    }
    
    // Update is called once per frame
    void Update () {
        float moveX = Input.GetAxis ("Horizontal") * speed; //
        float moveZ = Input.GetAxis ("Vertical") * speed;
        Vector3 movement = new Vector3 (moveX, 0, moveZ);
        
        movement = Vector3.ClampMagnitude (movement, speed); //Limits the max speed of the player
        movement *= Time.deltaTime; //Ensures the speed the player moves does not change based on frame rate
        movement = transform.TransformDirection(movement);
        character.Move (movement);
    }
}
