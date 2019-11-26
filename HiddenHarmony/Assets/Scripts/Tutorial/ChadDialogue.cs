using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChadDialogue : MonoBehaviour
{
    // This is a collection of functions to be triggered by Chad's 
    // text boxes. 

    private Transform player;
    [SerializeField] private GameObject areaTrigger;
    [SerializeField] private GameObject highlightUI;
    [SerializeField] private GameObject composePrompt;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
    }

    public void DestroyCurrentText()
    {
        // For use when the current text box is a "one-time use" sort of deal
        Destroy(this.transform.parent.GetChild(0).gameObject);
    }

    public void HighlightInventory()
    {
        highlightUI.SetActive(true);
        DestroyCurrentText();
    }

    public void DisableInventory()
    {
        highlightUI.SetActive(false);
    }

    public void EnableComposeMode()
    {
        GameObject collider;
        Transition script;
        // Enable compose mode for the first time 
        // by spawning an area trigger at player
        collider = Instantiate(areaTrigger, player.position, Quaternion.identity);
        script = collider.GetComponent<Transition>();
        script.canCompose = true;

        // Compose mode prompt
        composePrompt.SetActive(true);

        // Set Tutorial-to-Hub collider to allow compose now
        collider = GameObject.Find("HubTransitions/TutorialToHub");
        script = collider.GetComponent<Transition>();
        script.canCompose = true;
        DestroyCurrentText();
    }
}
