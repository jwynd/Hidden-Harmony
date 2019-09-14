using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutObjCollect : MonoBehaviour
{
    // Attach this script to the Sound Objects the player must collect to 
    // trigger "CaveDoorUnlock.cs" and continue the tutorial
    [SerializeField] private GameObject[] soundObjs;

    private CaveDoorUnlock trigger;

    // Start is called before the first frame update
    void Start()
    {
        trigger = GameObject.Find("GameplayObjects/TutorialEvents/CaveDoorUnlock").GetComponent<CaveDoorUnlock>();
    }

    void OnDisable()
    {
        // Check if there are any active objects left in "soundObjs'
        foreach(GameObject obj in soundObjs)
        {
            if(obj.activeSelf)
            {
                // Haven't collected all objects
                Destroy(this);
                return;
            }
        }
        // Collected all objects, trigger event
        trigger.TriggerUnlock();
        Destroy(this);
    }
}
