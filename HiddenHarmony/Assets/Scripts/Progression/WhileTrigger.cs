using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] hiddenObjects;
//    [SerializeField] private GameObject[] soundObjects;
    private bool visible = false;
    // Start is called before the first frame update
    void Start(){
        foreach(GameObject obj in hiddenObjects){
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update(){
        visible = this.GetComponent<SoundObject>().onStage;
//        foreach(GameObject obj in soundObjects){
//            visible = obj.GetComponent<SoundObject>().onStage;
//            // print(obj.name+" "+eq);
//            if(!visible) break;
//        }

        foreach(GameObject obj in hiddenObjects){
            obj.SetActive(visible);
        }
    }
}