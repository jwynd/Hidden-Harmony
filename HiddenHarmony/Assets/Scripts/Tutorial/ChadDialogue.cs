using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChadDialogue : MonoBehaviour
{
    // This is a collection of functions to be triggered by Chad's 
    // text boxes. 

    public void DestroyCurrentText()
    {
        // For use when the current text box is a "one-time use" sort of deal
        Destroy(this.transform.parent.GetChild(0).gameObject);
    }
}
