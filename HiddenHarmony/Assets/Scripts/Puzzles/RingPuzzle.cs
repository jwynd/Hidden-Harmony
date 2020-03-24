using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingPuzzle : MonoBehaviour
{
    public GameObject reward;

    private int collected = 0;
    private float timer = 10f;
    private int total;

    // Start is called before the first frame update
    void Start()
    {
        total = transform.childCount;
        reward.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Count down timer. When it hits 0, reset puzzle
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            Stop();
        }
    }

    public void Initiate()
    {
        // Starts puzzle, enables Ring objects
        foreach(GameObject ring in gameObject.GetComponentsInChildren<GameObject>())
        {
            ring.SetActive(true);
        }
    }

    private void Stop()
    {
        // Stops puzzle, either when the player wins or the timer runs out
        collected = 0;
        foreach (GameObject ring in gameObject.GetComponentsInChildren<GameObject>())
        {
            ring.SetActive(false);
        }
    }

    public void Collect()
    {
        // Called by Ring.cs when player enters its collider
        collected++;
        if(collected >= total)
        {
            // you did it!
            reward.SetActive(true);
        }
    }
}
