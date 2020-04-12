using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingPuzzle : MonoBehaviour
{
    public GameObject reward;
    public int timerDuration = 10;

    private bool activated = false;
    private int collected = 0;
    private int total;

    private float timer;
    private float lastTick; // The last timer value at which sfx played
    private float tickRate; // How fast the sfx repeat

    private AudioSource[] audios; // Timer and Victory sfx

    // Start is called before the first frame update
    void Start()
    {
        total = transform.childCount;
        reward.SetActive(false);
        audios = gameObject.GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(activated)
        {
            // Play a ticking sound at a rate depending on how close it is to the end
            if(timer < 3)
            {
                tickRate = 0.25f;
            } else
            {
                tickRate = 1f;
            }

            if (timer < lastTick - tickRate)
            {
                // Timer is a whole number (integer)
                // Play sound!
                lastTick = timer;
                audios[0].Play(); // Timer sound
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
        lastTick = timer;
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
            transform.GetChild(i).gameObject.GetComponentInChildren<Ring>().Reset(); // Resets Ring's 'activated' attribute
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
            audios[1].Play(); // Victory sound
            Stop();
        }
    }
}
