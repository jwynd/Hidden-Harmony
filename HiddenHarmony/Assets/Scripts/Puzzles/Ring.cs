using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public RingPuzzle controller;

    private ParticleSystem burst;
    private GameObject sprite;
    private bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        burst = gameObject.GetComponent<ParticleSystem>();
        sprite = transform.parent.Find("Sprite").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(activated)
        {
            if(burst.isStopped)
            {
                transform.parent.gameObject.SetActive(false);
                sprite.SetActive(true); // Reenable sprite renderer (even though the parent is disabled)
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(activated == false)
        {
            // Collect this ring.
            print("Colliding with Ring");
            controller.Collect(); // Send data to RingPuzzle.cs
            sprite.SetActive(false); // Disable sprite renderer
            burst.Play(); // Object will disable once particles are finished
            activated = true;
        }
    }
}
