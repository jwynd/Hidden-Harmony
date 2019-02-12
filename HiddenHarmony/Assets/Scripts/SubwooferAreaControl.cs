using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubwooferAreaControl : MonoBehaviour
{
    [SerializeField] private GameObject[] subWooferObjects;
    [SerializeField] private GameObject[] hubSoundObjects;
    private bool eq = false;
    // Start is called before the first frame update
    void Start(){
        foreach(GameObject obj in subWooferObjects){
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update(){
        foreach(GameObject obj in hubSoundObjects){
            eq = obj.GetComponent<SoundObject>().onStage;
            // print(obj.name+" "+eq);
            if(!eq) break;
        }
        if(eq){
            print("Creating subWooferObjects");
            foreach(GameObject obj in subWooferObjects){
                obj.SetActive(true);
            }
        }
    }
}
