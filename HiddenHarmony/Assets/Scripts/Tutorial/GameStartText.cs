using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartText : MonoBehaviour
{
    private ComposeModeTransition cmt;
    // Start is called before the first frame update
    void Start()
    {
        cmt = GameObject.Find("GameplayObjects/CameraChange").GetComponent<ComposeModeTransition>();
        // Disable Tab until player has spoken with Chad directly
        cmt.setTransition(false);


        // Destroy self upon completion of tasks
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
