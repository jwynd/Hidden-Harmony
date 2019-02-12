﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] hiddenObjects;
    [SerializeField] private GameObject[] soundObjects;
    private bool eq = false;
    // Start is called before the first frame update
    void Start(){
        foreach(GameObject obj in hiddenObjects){
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update(){
        foreach(GameObject obj in soundObjects){
            eq = obj.GetComponent<SoundObject>().onStage;
            // print(obj.name+" "+eq);
            if(!eq) break;
        }
        if(eq){
            print("Creating subWooferObjects");
            foreach(GameObject obj in hiddenObjects){
                obj.SetActive(true);
            }
        }
    }
}
