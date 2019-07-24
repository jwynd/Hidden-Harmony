using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseOver : MonoBehaviour
{
    private GameObject enlargedSprite;

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
        print("working");
        //enlargedSprite = transform.Find("EnlargedSprite");
        //itemFrame = this.gameObject;
        enlargedSprite = this.gameObject;
        enlargedSprite.SetActive(true);
        
        //print("Hi " + enlargedSprite.gameObject);
    }

    public void OnMouseExit()
    {
        enlargedSprite = this.gameObject;
        enlargedSprite.SetActive(false);
        
    }

}
