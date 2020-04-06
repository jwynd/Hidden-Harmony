using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodMode : MonoBehaviour
{
    /*
     * God Mode Functions:
     * 
     * Press alt+g to activate God mode
     * 
     * While in God Mode:
     * - press period to increment glide/sprint speed
     * - press comma to decrement glide/sprint speed
     * - press f1 to teleport to the tutorial cave
     * - press f2 to teleport to the Hub 
     * - press f3 to teleport to the Forest
     * - press f4 to teleport to the Den
     * - press = to add an activatable stage
     * - press ` to toggle canvas
     */  
    private GameObject player;
    private bool active; // whether God Mode is currently active or not
    private GameObject indicator;
    private DeadStageController dsc;
    private ComposeModeTransition cmt;
    private GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        indicator = GameObject.Find("GameplayObjects/Canvas/GodModeIndicator");
        dsc = GameObject.Find("GameplayObjects/Canvas/Controllers/DeadStageController").GetComponent<DeadStageController>();
        cmt = GameObject.Find("GameplayObjects/CameraChange").GetComponent<ComposeModeTransition>();
        canvas = GameObject.Find("GameplayObjects/Canvas");
    }

    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
        if((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.G))
        {  
            // Toggle God Mode
            active = !active;
        }
        #endif
        if(active)
        {
            // Enable God mode things!
            if(indicator.activeInHierarchy == false)
            {
                indicator.SetActive(true);
                // Following actions only occur once when God Mode is activated

                // Sprint/Glide changes
                player.GetComponent<PlayerMovement>().glideGravity = 0f;
                player.GetComponent<PlayerMovement>().glideBoost = 16f;
                player.GetComponent<PlayerMovement>().sprintBoost = 16f;
            }

            // Speed increments
            if (Input.GetKeyDown(KeyCode.Period))
            {
                player.GetComponent<PlayerMovement>().glideBoost += 4f;
                player.GetComponent<PlayerMovement>().sprintBoost += 4f;
                print("boost: " + player.GetComponent<PlayerMovement>().glideBoost);
            } else if(Input.GetKeyDown(KeyCode.Comma))
            {
                player.GetComponent<PlayerMovement>().glideBoost -= 4f;
                player.GetComponent<PlayerMovement>().sprintBoost -= 4f;
                print("boost: " + player.GetComponent<PlayerMovement>().glideBoost);
            }

            // Teleports
            if (Input.GetKeyDown(KeyCode.F1))
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

            // Increment activatable stages
            if(Input.GetKeyDown(KeyCode.Equals))
            {
                dsc.AddActivatable(1);
                print("Adding Activatable...");
            }

            // Get Sound Objects 
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                // Get all Hub artifacts

            }

            // Toggle canvas
            if(Input.GetKeyDown(KeyCode.BackQuote))
            {
                canvas.SetActive(!(canvas.activeInHierarchy));
            }
        } else
        {
            if (indicator.activeInHierarchy)
            {
                indicator.SetActive(false);
            }

            // Sprint/Glide changes
            player.GetComponent<PlayerMovement>().glideGravity = 3f;
            player.GetComponent<PlayerMovement>().glideBoost = 4f;
            player.GetComponent<PlayerMovement>().sprintBoost = 4f;

            // Enable canvas
            canvas.SetActive(true);
        }
    }
}
