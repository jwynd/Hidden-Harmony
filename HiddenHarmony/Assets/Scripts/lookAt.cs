using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class lookAt : MonoBehaviour
{
    public Transform[] target;
    public int count = 0;

    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        transform.LookAt(target[count]);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Switch"))
        {

            Debug.Log(other.tag);
            if (count == target.Length)
            {
                count = 0;
            }
            else
            {
                count++;
            }
        }
    }
}