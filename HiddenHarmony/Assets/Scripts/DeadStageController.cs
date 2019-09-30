using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadStageController : MonoBehaviour
{
    [Tooltip("Place the stages that are totally dead as the first child of deadStages, and those that can be activated as the second child")]
    [SerializeField] private Transform deadStages;
    // Keep a count of the number of sound objects collected
    // Have a thing that returns a number of stages available to be unlocked
    // If the number is greater than zero have stages light up and get T or F
    // to activate the stage.

    // Dead Stage objects should have a script that lets a activatable version be triggered

    private int collectedCount = 0;
    private int unlockableCount = 0; // increment if after incrementing CollectedCount, UnlockableCount mod 3 == 0

    private RaycastHit hit;
    private Ray mouseRay;

    void Start(){
        deadStages.GetChild(0).gameObject.SetActive(true);
        deadStages.GetChild(1).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update(){
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(mouseRay, out hit) && unlockableCount > 0 && Input.GetMouseButtonDown(0)){
            foreach(Transform child in deadStages.GetChild(1)){
                if(GameObject.ReferenceEquals(hit.transform.parent.gameObject, child.gameObject)){
                    hit.transform.gameObject.GetComponent<ActivateStage>().Activate();
                    removeDeadStage(hit.transform.GetSiblingIndex());
                    StageActivated();
                }
            }
        }
    }
    // This function is called by inventory add when an object is added to the inventory
    public void CollectedSoundObject(){
        if(++collectedCount % 3 == 0) ++unlockableCount;
        toggleStages();
    }

    // This function is called when a new stage is activated
    private void StageActivated(){
        --unlockableCount;
        toggleStages();
    }

    private void toggleStages(){
        if(unlockableCount > 0){
            deadStages.GetChild(0).gameObject.SetActive(false);
            deadStages.GetChild(1).gameObject.SetActive(true);
        } else {
            deadStages.GetChild(0).gameObject.SetActive(true);
            deadStages.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void removeDeadStage(int i){
        // remove the stage at index i
        Destroy(deadStages.GetChild(0).GetChild(i).gameObject);
        Destroy(deadStages.GetChild(1).GetChild(i).gameObject);
    }
}
