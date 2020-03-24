using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingPuzzle : MonoBehaviour
{
    public GameObject reward;
    public int timerDuration = 10;

    private bool activated = false;
    private int collected = 0;
    private float timer;
    private int total;
    private AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        total = transform.childCount;
        reward.SetActive(false);
        audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(activated)
        {
            // Play a ticking sound every second
            if (Mathf.Approximately(timer, Mathf.RoundToInt(timer)))
            {
                // Timer is a whole number (integer)
                // Play sound!
                print("Ring Timer: " + timer);
                audio.Play();
            }
            // Count down timer. When it hits 0, reset puzzle
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                // Player ran out of time!
                Stop();
            }
        }
    }

    public void Initiate()
    {
        // Starts puzzle, enables Ring objects
        print("Ring Puzzle Initiated");
        timer = (float)timerDuration;
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        activated = true;
    }

    private void Stop()
    {
        // Stops puzzle, either when the player wins or the timer runs out
        print("Ring Puzzle Stopped");
        collected = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        activated = false;
    }

    public void Collect()
    {
        // Called by Ring.cs when player enters its collider
        print("Ring Collected");
        collected++;
        if(collected >= total)
        {
            // you did it!
            reward.SetActive(true);
        }
    }
}
