using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPrimer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] hiddenObjects;

    void Start()
    {
        foreach(GameObject obj in hiddenObjects){
            obj.SetActive(false);
        }
    }

    public void PrimeTrigger(GameObject[] h){
        foreach(GameObject obj in h){
            obj.SetActive(false);
        }
    }
}
