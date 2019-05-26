using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSounds : MonoBehaviour
{
    void OnTriggerEnter(){
        GetComponent<AudioSource>().Play();
    }
    void OnTriggerExit(){
        GetComponent<AudioSource>().Stop();
    }
}
