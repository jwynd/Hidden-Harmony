using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FindScript : MonoBehaviour
{
    [Header("Enter the name of the class you want to search for")]
    public string scriptName;

    private UnityEngine.Object[] objects;

    // Start is called before the first frame update
    void Start()
    {
        Type type = Type.GetType(scriptName);
        objects = FindObjectsOfType(Type.GetType(scriptName));
        print(objects);
        print(objects[0]);
        foreach(UnityEngine.Object obj in objects)
        {
            print(obj.name);
        }
    }
}
