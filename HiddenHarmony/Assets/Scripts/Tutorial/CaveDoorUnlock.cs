using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveDoorUnlock : MonoBehaviour
{
    public GameObject door;

    private Count counter;
    // Start is called before the first frame update
    void Start()
    {
        counter = GameObject.Find("GameplayObjects/Count").GetComponent<Count>();
    }

    // Update is called once per frame
    void Update()
    {
        // check each frame if the player has collected all 3 sound objects
        if(counter.HubCount() == 3)
        {
            // Play dialogue and destroy the door
            Destroy(door);

            Destroy(this.gameObject);
        }
    }
}
