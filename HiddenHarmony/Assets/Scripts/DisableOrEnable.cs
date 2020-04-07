using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOrEnable : MonoBehaviour
{
    public enum Behave
    {
        Enable = 0,
        Disable = 1
    }
    [Tooltip("Determine if this script will disable or enable the listed game objects on trigger enter")]
    public Behave enableOrDisable = Behave.Enable;
    public GameObject[] gameObjects;
    private GameObject player;
    
    void Awake(){
        player = GameObject.Find("Player");
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject == player){
            foreach(GameObject g in gameObjects){
                g.SetActive(enableOrDisable == Behave.Enable);
            }
        }
    }
}
