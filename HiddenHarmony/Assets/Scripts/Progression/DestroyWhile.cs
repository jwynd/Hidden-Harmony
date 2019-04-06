using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhile : MonoBehaviour
{
    [SerializeField] private GameObject[] deleteObjects;
//    [SerializeField] private GameObject[] soundObjects;
    private bool invisible = false;
    // Start is called before the first frame update
    void Start(){

    }

    // Update is called once per frame
    void Update(){
        invisible = !this.GetComponent<SoundObject>().onStage;
//        foreach(GameObject obj in soundObjects){
//            invisible = obj.GetComponent<SoundObject>().onStage;
//            // print(obj.name+" "+eq);
//            if(!invisible) break;
//        }

        foreach(GameObject obj in deleteObjects){
            obj.SetActive(invisible);
        }
    }
}