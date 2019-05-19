using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserLaunch : MonoBehaviour
{
    public float speed = 5f;
    Vector3 movement;
    // Start is called before the first frame update
    void Start(){

    }

    // Update is called once per frame
    void Update(){
        
    }

    void OnTriggerStay(Collider collide){
        TravelUp(speed, collide);

    }
    void OnTriggerExit(Collider collide){
        TravelUp(speed * 1.5f, collide);

    }

    void TravelUp(float travelSpeed , Collider collide){
        if(collide.gameObject.name == "Player"){
            movement.y = travelSpeed *10;
            collide.gameObject.GetComponent<CharacterController> ().Move(movement);
            collide.gameObject.GetComponent<PlayerMovement> ().yVelocity = collide.gameObject.GetComponent<PlayerMovement> ().terminalVelocity;
        }
    }
}
