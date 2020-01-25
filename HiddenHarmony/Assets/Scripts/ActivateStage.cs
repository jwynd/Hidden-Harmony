using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateStage : MonoBehaviour
{
    [Tooltip("Put the stage that should be activated when this is clicked on")]
    public GameObject activatedStage;
    [Tooltip("The dead stage that should be destroyed, this game object will be destroyed automatically")]
    public GameObject deadStage;

    public void Activate(){
        activatedStage.SetActive(true);
        Destroy(deadStage);
        Destroy(this.transform.parent.gameObject);
    }
}
