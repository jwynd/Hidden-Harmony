using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTutorial : MonoBehaviour
{
    // When player performs actions in compose mode, create/destroy tutorial images
    // on the canvas

    // This list of slices may expand indefinitely:
    // dependent on how many steps there are in the tutorial
    // slice ex: "HighlightInventory" is a circle around the player's inv UI
    [SerializeField] private GameObject slice1; // PressTab
    [SerializeField] private GameObject slice2; // ClickFirstStage
    [SerializeField] private GameObject slice3; // AddFirstSound
    [SerializeField] private GameObject slice4; // AddSecondSound
    [SerializeField] private GameObject slice5;

    [SerializeField] private GameObject stagesParent;
    private ComposeModeTransition cmt;
    private DeadStageController dsc;

    private GameObject firstStage;
    private GameObject secondStage;
    private Transform[] stages = new Transform[9];

    private bool firstCompose = false;
    private bool firstUse = false;

    // Start is called before the first frame update
    void Start()
    {
        cmt = GameObject.Find("GameplayObjects/CameraChange").GetComponent<ComposeModeTransition>();
        dsc = GameObject.Find("GameplayObjects/Canvas/Controllers/DeadStageController").GetComponent<DeadStageController>();
        print(dsc);
        for(int n = 0; n < stagesParent.transform.childCount; n++)
        {
            stages[n] = stagesParent.transform.GetChild(n);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(firstCompose == false && cmt.Compose())
        {
            firstCompose = true;
            slice1.SetActive(false); // "Press Tab to Compose" text
            slice2.SetActive(true); // 'click on stage' prompt
        } else if(firstCompose && cmt.Compose() == false)
        {
            // Done with the tutorial sequence
            slice4.SetActive(false);
        }
        if(firstStage == null)
        {
            // Loop through (actual) stages until one becomes active
            // This event signifies the player activating a stage, triggering new dialogue
            foreach(Transform s in stages)
            {
                if (s.gameObject.activeSelf)
                {
                    print("first stage activated");
                    firstStage = s.gameObject;
                    slice2.SetActive(false); // 'click on stage' prompt
                    slice3.SetActive(true); // AddFirstSound
                }
            }
        } else if(secondStage == null)
        {
            // Check for first stage usage
            if (firstUse == false && firstStage.GetComponentInChildren<Stage>().IsOccupied())
            {
                print("firstStage used");
                firstUse = true;
                dsc.AddActivatable(1);
                slice3.SetActive(false); //AddFirstSound
                slice4.SetActive(true); //AddSecondStage
            }

            // Check for new second stage
            foreach (Transform s in stages)
            {
                if (s.gameObject.activeSelf && s.gameObject != firstStage)
                {
                    print("second stage activated");
                    secondStage = s.gameObject;
                    slice4.SetActive(false);
                    slice5.SetActive(true);
                }
            }
        }
    }
}
