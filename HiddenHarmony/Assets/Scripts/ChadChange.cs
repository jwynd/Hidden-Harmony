using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChadChange : MonoBehaviour
{
    private Count count;

    public int chadCopy = 0; 
    // Start is called before the first frame update
    void Start()
    {
        count = GameObject.Find("GameplayObjects/Count").GetComponent<Count>();
    }

    // Update is called once per frame
    void Update()
    {
        // Switch Chad copies based on the following criteria
        if (chadCopy < 6)
        {
            if (count.AllCounted())
            {
                // All Artifacts have been collected & used
                GameObject.Find("Hub/--AREA-PLATFORMS--/ChadAlive/ChadDialogue6").SetActive(true);
                gameObject.SetActive(false);
            }
            else if (count.statuesUsed == 3)
            {
                // Areas returned, but more Artifacts to collect
                GameObject.Find("Hub/--AREA-PLATFORMS--/ChadAlive/ChadDialogue5").SetActive(true);
                gameObject.SetActive(false);
            }
            else if (count.statuesUsed > 0)
            {
                // At least 1 area has been returned
                GameObject.Find("Hub/--AREA-PLATFORMS--/ChadAlive/ChadDialogue4").SetActive(true);
                gameObject.SetActive(false);
            }
            else if (count.DenCount() >= 3 || count.ForestCount() >= 3 || count.CavernCount() >= 3)
            {
                // Deity is freed but not spoken to
                GameObject.Find("Hub/--AREA-PLATFORMS--/ChadAlive/ChadDialogue3").SetActive(true);
                gameObject.SetActive(false);
            }
            else if (count.DenCount() >= 1 || count.ForestCount() >= 1 || count.CavernCount() >= 1)
            {
                // Collected Objects, but not enough to free a deity
                print(GameObject.Find("Hub/--AREA-PLATFORMS--/ChadAlive/ChadDialogue2"));
                GameObject.Find("Hub/--AREA-PLATFORMS--/ChadAlive/ChadDialogue2").SetActive(true);
                gameObject.SetActive(false);
            }
        } 

    }
}
