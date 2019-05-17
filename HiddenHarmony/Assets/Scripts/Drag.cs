using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 origin;
    public bool isDragging = false;

    // Start is called before the first frame update
    void Start(){
    origin = transform.Find("ItemSprite").localPosition;
    

    }

  

    public void OnBeginDrag(PointerEventData evenData)
    {
        transform.Find("ItemSprite").position = Input.mousePosition;

    }

    public void OnDrag(PointerEventData evenData){
        transform.Find("ItemSprite").position = Input.mousePosition;
        isDragging = true;

    }
    public void OnEndDrag(PointerEventData evenData){
        isDragging = false;
        transform.Find("ItemSprite").localPosition = origin;
        GameObject.Find("Canvas/ItemsHeld").GetComponent<SoundObjectCompose>().checkStage();
        
    }
}
