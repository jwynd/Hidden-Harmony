using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playAudio : MonoBehaviour
{
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;

    private float timer = 0;
    private float waitTime;
    private AudioSource[] getAudio;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > waitTime)
        {
            playClip();
            timer = timer - waitTime;
            waitTime = Random.Range(minTime, maxTime);
        }
    }

    void playClip()
    {
        getAudio = GetComponents<AudioSource>();
        int clipPick = Random.Range(0, getAudio.Length);
        getAudio[clipPick].Play();
    }
}
