using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcaTransTemp : MonoBehaviour
{
    [SerializeField] private bool underWater = false;
    public GameObject orcastra;

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "OrcaTrans")
        {
            Debug.Log("Colliding!");
            if (other.gameObject.CompareTag("Switch"))
            {
                orcastra.SetActive(true);
                Debug.Log("True!");
            }
            else
            {
                orcastra.SetActive(false);
                Debug.Log("False!");
            }
        }
    }
}
