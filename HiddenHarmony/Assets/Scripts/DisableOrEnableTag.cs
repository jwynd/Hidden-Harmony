using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOrEnableTag : MonoBehaviour
{
    public static List<DisableOrEnableTag> Instances;
    public enum Behavior
    {
        Enable = 0,
        Disable = 1
    }
    [Tooltip("Determine if this script will disable or enable the listed game objects on trigger enter")]
    public Behavior enableOrDisable = Behavior.Enable;
    public string[] tags;
    private List<List<GameObject>> gameObjects = new List<List<GameObject>>();
    private GameObject player;
    
    void Awake(){
        player = GameObject.Find("Player");
        if(Instances == null){
            Instances = new List<DisableOrEnableTag>();
        }
        Instances.Add(this);
        LocateTags();
    }

    void OnTriggerEnter(Collider other){
        foreach(List<GameObject> l in gameObjects){
            foreach(GameObject g in l){
                try{
                    g.SetActive(enableOrDisable == Behavior.Enable);
                } catch (MissingReferenceException) {
                    // Do nothing
                }
            }
        }
    }

    public void LocateTags(){
        gameObjects.Clear();
        foreach(string t in tags){
            gameObjects.Add(PopulateList(t));
        }
    }

    private List<GameObject> PopulateList(string t){
        GameObject[] gs = Resources.FindObjectsOfTypeAll<GameObject>();
        List<GameObject> rgs = new List<GameObject>();
        foreach(GameObject g in gs){
            if(g.tag == t) rgs.Add(g);
        }
        return rgs;
    }
}
