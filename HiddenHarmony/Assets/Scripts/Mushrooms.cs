using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushrooms : MonoBehaviour
{
    private MeshRenderer[] mushrooms;
    // Start is called before the first frame update
    void Start(){
        mushrooms = GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.M)){
            foreach (MeshRenderer shroom in mushrooms){
                shroom.enabled = !shroom.enabled;
            }
        }
    }
}
