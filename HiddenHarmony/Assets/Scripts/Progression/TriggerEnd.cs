using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnd : MonoBehaviour
{
    [Tooltip("Game object to be enabled when all sound objects are found")]
    [SerializeField] private GameObject toEnable;

    private Count c;
    private bool notExecuted = true;
    // Start is called before the first frame update
    void Start(){
        c = GameObject.Find("Count").GetComponent<Count>();
        //print(c);
    }

    // Update is called once per frame
    void Update(){
        if(notExecuted && c.AllCounted()){
            toEnable.SetActive(true);
            notExecuted = false;
        }
    }
}
