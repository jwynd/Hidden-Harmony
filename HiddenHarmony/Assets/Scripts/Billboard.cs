using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Billboard : MonoBehaviour
{
    private Transform player;
    private Vector3 lookTarget;
    // Start is called before the first frame update
    void Awake(){
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update(){
        lookTarget = player.position;
        lookTarget.y = this.transform.position.y;
        this.transform.LookAt(lookTarget);
    }
}
