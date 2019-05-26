using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 origin;
    public bool isDragging = false;
    private AudioSource dragObjSound;
    private GameObject hItems;
    private GameObject sItems;
    private GameObject bdItems;
    private GameObject oItems; 
    private GameObject controller; 

    // Start is called before the first frame update
    void Start(){
    origin = transform.Find("ItemSprite").localPosition;
    dragObjSound = GameObject.Find("SoundObjSFX/PlaceSound").GetComponent<AudioSource>();
    hItems = GameObject.Find("Canvas/CTabs/HTabs/HItemsHeld");
    sItems = GameObject.Find("Canvas/CTabs/STabs/SItemsHeld");
    bdItems = GameObject.Find("Canvas/CTabs/BDTabs/BDItemsHeld");
    oItems = GameObject.Find("Canvas/CTabs/OTabs/OItemsHeld");

    controller = GameObject.Find("Canvas/Controllers/ComposeObjectController");
    }

  

    public void OnBeginDrag(PointerEventData evenData)
    {
        //dragObjSound.GetComponents<AudioSource>()[1].Play();
        //transform.Find("ItemSprite").position = Input.mousePosition;

    }

    public void OnDrag(PointerEventData evenData){
        transform.Find("ItemSprite").position = Input.mousePosition;
        isDragging = true;

    }
    public void OnEndDrag(PointerEventData evenData){
        isDragging = false;
        transform.Find("ItemSprite").localPosition = origin;

        if(hItems.activeSelf == true)
        {
            controller.GetComponent<SoundObjectCompose>().checkStage();
        }
       
        if(sItems.activeSelf == true)
        {
            controller.GetComponent<SoundObjectCompose>().checkStage();
        }
        
        if(bdItems.activeSelf == true)
        {
            controller.GetComponent<SoundObjectCompose>().checkStage();
        }
        if(oItems.activeSelf == true)
        {
            controller.GetComponent<SoundObjectCompose>().checkStage();
        }
    }
}
