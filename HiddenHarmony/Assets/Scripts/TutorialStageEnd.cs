using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStageEnd : MonoBehaviour
{
    public GameObject[] destroy;
    public GameObject[] create;

    private Stage stageScript;

    // Start is called before the first frame update
    void Start()
    {
        stageScript = gameObject.GetComponentInChildren<Stage>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player has moved an object onto this stage AND then moved that object OFF of this stage
        if(gameObject.GetComponent<TutorialStage>() == null)
        {
            if(stageScript.IsOccupied() == false)
            {
                foreach (GameObject g in destroy)
                {
                    g.SetActive(false);
                }
                foreach (GameObject g in create)
                {
                    g.SetActive(true);
                }
                Destroy(this);
            }
           
        }
    }
}
