using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListTag : MonoBehaviour
{
    public static ListTag Instance;
    public string tag = "Untagged";
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(this);
        }
        Instance = this;
        ListActiveTag(tag);
    }

    private string GetPath(GameObject g){
        string path = "/" + g.name;
        while (g.transform.parent != null)
        {
            g = g.transform.parent.gameObject;
            path = "/" + g.name + path;
        }
        return path;
    }

    public void ListActiveTag(string t){
        GameObject[] objs = GameObject.FindGameObjectsWithTag(t);
        int count = 0;
        Debug.Log("BEGIN SEARCH FOR ACTIVE OBJECTS WITH TAG \"" + t +"\"");
        foreach(GameObject o in objs){
            Debug.Log("LISTED OBJECT WITH TAG \"" + t +"\": " + GetPath(o));
            count++;
        }
        Debug.Log("END SEARCH. FOUND " + count + " ACTIVE SOUND OBJECTS WITH TAG \""+t+"\"");
    }
}
