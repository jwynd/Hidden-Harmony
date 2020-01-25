using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : MonoBehaviour
{
    [Tooltip("How high the player bounces (Must be greater than the terminalVelocity variable of the player)")]
    public float bounceFactor = 70; //must be higher than termialVelocity of the player
    private Transform player;
    private RaycastHit hit;
    [SerializeField] private float bounceDetectionDistance = 3f;
    [SerializeField] private AudioClip bounceAudio;

    void Awake(){
        player = GameObject.Find("Player").transform;
    }

    void Update(){
        if(Physics.Raycast(player.position, Vector3.down, out hit, bounceDetectionDistance)){
            if(hit.collider.gameObject == this.gameObject){
                player.gameObject.GetComponent<PlayerMovement>().yVelocity = bounceFactor;
                player.Find("LandAudio").GetComponent<AudioSource>().PlayOneShot(bounceAudio);
            }
        }
    }
}
