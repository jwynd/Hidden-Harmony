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
        listTag(tag);
    }

    private string GetPath(GameObject g, out bool active){
        string path = "/" + g.name;
        active = g.activeSelf;
        while (g.transform.parent != null)
        {
            g = g.transform.parent.gameObject;
            if(g.activeSelf == false) active = false;
            path = "/" + g.name + path;
        }
        return path;
    }

    public void listTag(string t){
        GameObject[] objs = Resources.FindObjectsOfTypeAll<GameObject>();
        int count = 0;
        Debug.Log("BEGIN SEARCH FOR OBJECTS WITH TAG \"" + t +"\"");
        foreach(GameObject o in objs){
            if(o.tag == t){
                bool a = true;
                string path = GetPath(o, out a);
                Debug.Log((a?"ACTIVE":"INACTIVE") + " OBJECT WITH TAG \"" + t +"\": " + path);
                count++;
            }
        }
        Debug.Log("END SEARCH. FOUND " + count + " OBJECTS WITH TAG \""+t+"\"");
    }
}
