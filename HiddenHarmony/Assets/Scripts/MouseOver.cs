using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {
        //print("working");
        //this.GetComponent<RectTransform>().localScale = new Vector3(2f, 2f, 1f);
    }

    public void OnMouseExit()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

}
