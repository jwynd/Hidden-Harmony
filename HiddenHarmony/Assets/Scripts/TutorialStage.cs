using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStage : MonoBehaviour
{
    private Stage stageScript;

    public GameObject[] create; // Objects
    public GameObject[] destroy;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Stage component on this stage (Stage.cs)
        stageScript = gameObject.GetComponentInChildren<Stage>();
    }

    // Update is called once per frame
    void Update()
    {
        if(stageScript.IsOccupied())
        {
            // A sound object has been placed on this stage

            foreach(GameObject g in create)
            {
                g.SetActive(true);
            }
            foreach(GameObject g in destroy)
            {
                g.SetActive(false);
            }

            // Destroy this component now that it's served its purpose
            Destroy(this);
        }
    }
}
