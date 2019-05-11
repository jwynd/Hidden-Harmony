using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 origin;
    public bool isDragging = false;
    public GameObject test; 
    // Start is called before the first frame update
    void Start(){
    origin = transform.Find("ItemSprite").localPosition;
    

    }

    /*public void OnMouseDown()
    {
        print("SOMETHING");
        transform.Find("ItemSprite").GetComponent<SoundObjectCompose>().checkSoundObj();
    }*/

    public void OnBeginDrag(PointerEventData evenData)
    {
        //test = new GameObject("ItemSprite");
        Debug.Log("YOUVE STARTED TO DRAG");
        //transform.Find("ItemSprite").localPosition = origin;
        //GameObject.Find("Canvas/ItemsHeld").GetComponent<SoundObjectCompose>().checkSoundObj();
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
        /*print("HELLO3 " + GetComponent<SoundObjectCompose>().itemDrop);
        test = GetComponent<SoundObjectCompose>().itemDrop;
        test = null;
        print("test equals " + test);*/
    }
}
