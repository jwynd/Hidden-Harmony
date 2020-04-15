using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonPuzzle : MonoBehaviour
{
    public int[] pattern;
    public GameObject reward;

    private int current = 0;

    private AudioSource victorySFX;

    // Start is called before the first frame update
    void Start()
    {
        reward.SetActive(false);
        victorySFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckNote(int note)
    {
        if(pattern[current] == note)
        {
            current++;
            if(current >= pattern.Length-1)
            {
                // You did it!
                victorySFX.Play();
                reward.SetActive(true);
            }
        }
    }
}
