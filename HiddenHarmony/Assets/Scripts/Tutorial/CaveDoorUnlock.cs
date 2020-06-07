using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveDoorUnlock : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        text.SetActive(false); // in case it's not already
    }

    // Run when all necessary tutorial objects have been collected
    public void TriggerUnlock()
    {
        // Play dialogue and destroy the door blocking progression
        //text.GetComponent<TextBox>().ActivateTextBox();
        Destroy(door);

        Destroy(this.gameObject);
    }
}
