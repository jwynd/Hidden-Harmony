using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodMode : MonoBehaviour
{
    private GameObject player;
    private bool active; // whether God Mode is currently active or not

    // Start is called before the first frame update
    void Start()
    {
        active = false;
    }

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.G))
        {  
            // Toggle God Mode
            active = !active;
        }

        if(active)
        {
            // Enable God mode things!
        }
    }
}
