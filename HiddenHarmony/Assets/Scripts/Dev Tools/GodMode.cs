using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodMode : MonoBehaviour
{
    private GameObject player;
    private bool active; // whether God Mode is currently active or not
    private GameObject indicator;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        indicator = GameObject.Find("GameplayObjects/Canvas/GodModeIndicator");
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
            if(indicator.active == false)
            {
                indicator.SetActive(true);
            }

            if(Input.GetKeyDown(KeyCode.F1))
            {
                player.transform.position = GameObject.Find("PlayerSpawnTutorial").transform.position;
            } else if(Input.GetKeyDown(KeyCode.F2))
            {
                player.transform.position = GameObject.Find("PlayerSpawnHub").transform.position;
            } else if(Input.GetKeyDown(KeyCode.F3))
            {
                player.transform.position = GameObject.Find("PlayerSpawnForest").transform.position;
            } else if(Input.GetKeyDown(KeyCode.F4))
            {
                player.transform.position = GameObject.Find("PlayerSpawnDen").transform.position;
            }
        } else
        {
            if (indicator.active)
            {
                indicator.SetActive(false);
            }
        }
    }
}
