using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockSoundObjRotation : MonoBehaviour
{
    private GameObject[] soundObjs;
    // Start is called before the first frame update
    void Start(){
        soundObjs = GameObject.FindGameObjectsWithTag("SoundObj");
        foreach(GameObject soundObj in soundObjs){
            soundObj.GetComponent<Rigidbody>().constraints = 
               RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
        }
    }
}
