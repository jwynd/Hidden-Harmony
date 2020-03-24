using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    public RingPuzzle parent; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Collect this ring.
        // Send collection to RingPuzzle.cs and disable this object
        parent.Collect();
        gameObject.SetActive(false);
    }
}
