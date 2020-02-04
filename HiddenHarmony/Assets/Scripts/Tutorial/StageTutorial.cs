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
    [SerializeField] private GameObject slice4; // ClickSecondStage
    [SerializeField] private GameObject slice5; // MoveObject
    [SerializeField] private GameObject slice6; // AddSecondSound
    [SerializeField] private GameObject slice7; // AddThirdSound
    [SerializeField] private GameObject slice8; // SpeakToChad

    [SerializeField] private GameObject stagesParent;
    private ComposeModeTransition cmt;
    private DeadStageController dsc;
    private Count counter;

    private GameObject stageFinder;
    private GameObject stageFog;
    private GameObject firstStage;
    private GameObject secondStage;
    private Transform[] stages = new Transform[9];

    private bool firstCompose = false;
    private bool firstUse = false;

    private int state = 0;

    // Start is called before the first frame update
    void Start()
    {
        cmt = GameObject.Find("GameplayObjects/CameraChange").GetComponent<ComposeModeTransition>();
        dsc = GameObject.Find("GameplayObjects/Canvas/Controllers/DeadStageController").GetComponent<DeadStageController>();
        counter = GameObject.Find("GameplayObjects/Count").GetComponent<Count>();

        stageFinder = GameObject.Find("HubStages/StageFinder");
        stageFinder.SetActive(false);

        stageFog = GameObject.Find("HubStages/StageFog");
        stageFog.SetActive(false);

        for(int n = 0; n < stagesParent.transform.childCount; n++)
        {
            stages[n] = stagesParent.transform.GetChild(n);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case 0:
                if (cmt.Compose())
                {
                    print("entering state 1");
                    state = 1;
                    slice1.SetActive(false); // PressTab
                    slice2.SetActive(true); // ClickFirstStage

                    stageFog.SetActive(true);

                    cmt.setTransition(false);
                }
                break;
            case 1:
                // Loop through (actual) stages until one becomes active
                // This event signifies the player activating a stage, triggering new dialogue
                foreach (Transform s in stages)
                {
                    if (s.gameObject.activeSelf)
                    {
                        print("entering state 2");
                        state = 2;
                        firstStage = s.gameObject;

                        slice2.SetActive(false); // ClickFirstStage
                        slice3.SetActive(true); // AddFirstSound

                        stageFog.SetActive(false);

                        stageFinder.SetActive(true); // Make a visible circle around the first stage
                        stageFinder.transform.position = new Vector3(firstStage.transform.position.x, firstStage.transform.position.y+0.5f, firstStage.transform.position.z);
                    }
                }
                break;
            case 2:
                if (firstStage.GetComponentInChildren<Stage>().IsOccupied())
                {
                    print("entering state 3");
                    state = 3;

                    dsc.AddActivatable(1); // Let player activate another stage

                    stageFinder.SetActive(false);

                    slice3.SetActive(false); // AddFirstSound
                    slice4.SetActive(true); // ClickSecondStage

                    stageFog.SetActive(true);
                }
                break;
            case 3:
                foreach (Transform s in stages)
                {
                    if (s.gameObject.activeSelf && s.gameObject != firstStage)
                    {
                        print("entering state 4");
                        state = 4;

                        secondStage = s.gameObject;

                        stageFinder.SetActive(true);
                        stageFinder.transform.position = new Vector3(secondStage.transform.position.x, secondStage.transform.position.y+0.5f, secondStage.transform.position.z);

                        slice4.SetActive(false); // ClickSecondStage
                        slice5.SetActive(true); // MoveObject

                        stageFog.SetActive(false);
                    }
                }
                break;
            case 4:
                if(secondStage.GetComponentInChildren<Stage>().IsOccupied())
                {
                    print("entering state 5");
                    state = 5;

                    stageFinder.SetActive(false);

                    slice5.SetActive(false); // MoveObject
                    slice6.SetActive(true); // AddSecondSound
                }
                break;
            case 5:
                if(counter.HubCount() > 1)
                {
                    print("entering state 6");
                    state = 6;
                    slice6.SetActive(false); // AddSecondSound
                    slice7.SetActive(true); // AddThirdSound
                }
                break;
            case 6:
                if(counter.HubCount() > 2)
                {
                    print("entering state 7");
                    state = 7;
                    slice7.SetActive(false); // AddThirdSound
                    slice8.SetActive(true); // SpeakToChad

                    // Destroy Chad's Statue's arrow/dialogue prompt
                    Destroy(GameObject.Find("Hub/--AREA-PLATFORMS--/ChadStatueDialogue"));

                    cmt.setTransition(true);
                }
                break;
            case 7:
                if(cmt.Compose() == false)
                {
                    print("entering state 8: tutorial complete");
                    state = 8;
                    slice8.SetActive(false);
                }
                break;
            default:
                Destroy(this.gameObject);
                break;

        }
        /*if(firstCompose == false && cmt.Compose())
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
        }*/
    }
}
