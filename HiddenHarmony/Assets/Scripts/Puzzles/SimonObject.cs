using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonObject : MonoBehaviour
{
    public SimonPuzzle controller;
    public int note;

    private RaycastHit hit;
    private Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Input.GetMouseButton(0) && Physics.Raycast(ray, out hit, 11.0f) && hit.transform == transform)
        {
            controller.CheckNote(note);
        }
    }
}
