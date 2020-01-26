using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 origin;
    public bool canPlace = false;
    private AudioSource dragObjSound;
    private GameObject hItems;
    private GameObject sItems;
    private GameObject bdItems;
    private GameObject oItems; 
    private GameObject controller;
    private Transform draggedItem;
    private Transform errorSprite;
    private Transform indicator;

    private GameObject snapPoint = null;

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
        draggedItem = transform.Find("ItemSprite");
        errorSprite = transform.Find("ErrorSprite");
        indicator = transform.Find("NewIndicator");

        errorSprite.gameObject.SetActive(true);
        
        //dragObjSound.GetComponents<AudioSource>()[1].Play();
        //transform.Find("ItemSprite").position = Input.mousePosition;

    }

    public void OnDrag(PointerEventData evenData){

        errorSprite.position = Input.mousePosition;
        draggedItem.position = Input.mousePosition;
        canPlace = controller.GetComponent<SoundObjectCompose>().checkError(out snapPoint);
        if(canPlace == true && errorSprite.gameObject.activeSelf==true)
        {
            errorSprite.gameObject.SetActive(false);
            controller.GetComponent<SoundObjectCompose>().makeOutline();
        }
        else if(canPlace == false && errorSprite.gameObject.activeSelf == false)
        {
            errorSprite.gameObject.SetActive(true);
            controller.GetComponent<SoundObjectCompose>().eraseOutline();
        }



    }
    public void OnEndDrag(PointerEventData evenData){
        canPlace = false;
        errorSprite.gameObject.SetActive(false);
        draggedItem.localPosition = origin;

        indicator.gameObject.SetActive(false);

        if (hItems.activeSelf == true)
        {
            controller.GetComponent<SoundObjectCompose>().checkStage(snapPoint);
        }
       
        if(sItems.activeSelf == true)
        {
            controller.GetComponent<SoundObjectCompose>().checkStage(snapPoint);
        }
        
        if(bdItems.activeSelf == true)
        {
            controller.GetComponent<SoundObjectCompose>().checkStage(snapPoint);
        }
        if(oItems.activeSelf == true)
        {
            controller.GetComponent<SoundObjectCompose>().checkStage(snapPoint);
        }
    }

    public void DisableNew()
    {
        if(indicator.gameObject.activeSelf == true)
        {
            indicator.gameObject.SetActive(false);
        }
        
    }
}
