using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateStage : MonoBehaviour
{
    [Tooltip("Put the stage that should be activated when this is clicked on")]
    [SerializeField] private GameObject stage;

    public int Activate(){
        stage.SetActive(true);
        return stage.transform.GetSiblingIndex();
    }
}
