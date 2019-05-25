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
            if (other.gameObject.CompareTag("Switch"))
            {
                orcastra.SetActive(true);
            }
            else
            {
                orcastra.SetActive(false);
            }
        }
    }
}
