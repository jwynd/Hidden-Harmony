using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Count : MonoBehaviour
{
    [Header("Parents of sound objects")]
    [SerializeField] private GameObject den;
    [SerializeField] private GameObject forest;
    [SerializeField] private GameObject cavern;
    [SerializeField] private GameObject hub;

    private string[] denChildren;
    private string[] forestChildren;
    private string[] cavernChildren;
    private string[] hubChildren;

    private string[] denNames;
    private string[] forestNames;
    private string[] cavernNames;
    private string[] hubNames;

    private int denCount = 0;
    private int forestCount = 0;
    private int cavernCount = 0;
    private int hubCount = 0;

    void Awake(){
        denChildren = new string[den.transform.childCount];
        for(int i = 0; i < den.transform.childCount; i++){
            denChildren[i] = den.transform.GetChild(i).gameObject.name+"(Clone)";
        }
        forestChildren = new string[forest.transform.childCount];
        for(int i = 0; i < forest.transform.childCount; i++){
            forestChildren[i] = forest.transform.GetChild(i).gameObject.name+"(Clone)";
        }
        cavernChildren = new string[cavern.transform.childCount];
        for(int i = 0; i < cavern.transform.childCount; i++){
            cavernChildren[i] = cavern.transform.GetChild(i).gameObject.name+"(Clone)";
        }
        hubChildren = new string[hub.transform.childCount];
        for(int i = 0; i < hub.transform.childCount; i++){
            hubChildren[i] = hub.transform.GetChild(i).gameObject.name+"(Clone)";
        }
        denNames = new string[denChildren.Length];
        forestNames = new string[forestChildren.Length];
        cavernNames = new string[cavernChildren.Length];
        hubNames = new string[hubChildren.Length];
    }

    #if UNITY_ENGINE
    void Update(){
        if(Input.GetKeyDown(KeyCode.C)){
            print("Den Count: "+denCount);
            print("Forest Count: "+forestCount);
            print("Cavern Count: "+cavernCount);
            print("Hub Count: "+hubCount);
        }
    }
    #endif

    public void IncrementCount(string n){

        foreach(string name in denNames){
            if(Equals(n, name)) return;
        }
        foreach(string name in forestNames){
            if(Equals(n, name)) return;
        }
        foreach(string name in cavernNames){
            if(Equals(n, name)) return;
        }
        foreach(string name in hubNames){
            if(Equals(n, name)) return;
        }
        foreach(string o in denChildren){
            if(n == o){
                denNames[denCount++] = n;
                return;
            }
        }
        foreach(string o in forestChildren){
            if(Equals(n, o)){
                forestNames[forestCount++] = n;
                return;
            }
        }
        foreach(string o in cavernChildren){
            if(Equals(n, o)){
                cavernNames[cavernCount++] = n;
                return;
            }
        }
        foreach(string o in hubChildren){
            if(Equals(n, o)){
                hubNames[hubCount++] = n;
                return;
            }
        }
        throw new System.ArgumentException("incrementCount failed to return");
    }

    public int DenCount(){
        return denCount;
    }
    public int ForestCount(){
        return forestCount;
    }
    public int CavernCount(){
        return cavernCount;
    }
    public int HubCount(){
        return hubCount;
    }

}
