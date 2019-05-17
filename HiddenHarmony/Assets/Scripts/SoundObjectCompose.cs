using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SoundObjectCompose : MonoBehaviour {
    public GameObject itemDrop;
    private string stagePattern = "StageObj";
    private string soundPattern = "SoundObj";
    private float stageDistance = 1000;
    private float soundDistance = 5;
    private float spawnHeight = 2;
    private bool isDragging = false;
    private GameObject panelAccess;
    private Transform itemFrame;
    private GameObject itemImage;
    private EventSystem es;
    private GameObject destroyedObject;
    private Match matchHitObj;

    // Start is called before the first frame update
    void Start(){
        panelAccess = GameObject.Find("Canvas/ItemsHeld");
    }

    // Update is called once per frame
    void Update(){
        checkSoundObj();
    }

    public void setSoundObject(GameObject soundObject){
        this.itemDrop = soundObject;
    }

    //checks if the item that's clicked is a sound object that can be dragged
    //if it is, the obj gets deleted and then a sprite
    // of the same obj is attached to the mouse's position. 
    public void checkSoundObj()
    {
        RaycastHit hit;
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(mouseRay, out hit, stageDistance))
        {
            Match matchSound = Regex.Match(hit.collider.tag, soundPattern);
            if (matchSound.Success)
            {
                if(Input.GetMouseButtonDown(0)){              
                    for (int i = 0; i < panelAccess.transform.childCount; i++)
                    {
                        itemFrame = panelAccess.transform.GetChild(i);
                        itemImage = itemFrame.Find("ItemSprite").gameObject;
                  

                        print("This stuff" + itemImage.GetComponent<Image>().sprite.name);
                        print("GameObject name is " + hit.collider.gameObject.name);

                        matchHitObj = Regex.Match(hit.collider.gameObject.name, itemImage.GetComponent<Image>().sprite.name);
                        if (matchHitObj.Success)
                        {
                            break;
                        }
                        
                        
                    }

                    isDragging = true;
                    itemFrame.GetComponent<Button>().onClick.Invoke();
                    hit.collider.gameObject.GetComponent<SoundObject>().blankStage();
                    Destroy(hit.collider.gameObject);
                }
            }

        }
        if(isDragging == true)
        {
            es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            itemFrame.GetComponent<Drag>().OnDrag(new PointerEventData(es));
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                itemFrame.GetComponent<Drag>().OnEndDrag(new PointerEventData(es));
            }
        }

        

    }

    public void checkStage(){
        RaycastHit hit;
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(mouseRay, out hit, stageDistance)){
            Match matchStage = Regex.Match(hit.collider.tag, stagePattern);
            Match matchSound = Regex.Match(hit.collider.tag, soundPattern);
            if(matchStage.Success) {
                if (Input.GetMouseButtonUp(0) && itemDrop != null) {
                    Transform stageObjectTransform = hit.collider.gameObject.transform;
                    Ray stageRay = new Ray(stageObjectTransform.position, Vector3.up);
                    if(Physics.Raycast(stageRay, out hit, soundDistance)) {
                        matchSound = Regex.Match(hit.collider.tag, soundPattern);
                        if (matchSound.Success) Destroy(hit.collider.gameObject);
                    }

                    
                        GameObject placedObject = Instantiate(itemDrop, new Vector3(stageObjectTransform.position.x, stageObjectTransform.position.y + spawnHeight, stageObjectTransform.position.z), stageObjectTransform.rotation);
                        placedObject.SetActive(true);
                    

                }
            }
            else if(matchSound.Success)
            {
                if ( Input.GetMouseButtonUp(0) && itemDrop != null) {
                    GameObject hitSoundObject = hit.collider.gameObject;
                    Ray stageRay = new Ray(hitSoundObject.transform.position, Vector3.down);
                    if(Physics.Raycast(stageRay, out hit, soundDistance)) {
                        matchSound = Regex.Match(hit.collider.tag, stagePattern);
                        if (matchSound.Success) Destroy(hitSoundObject);
                        Transform stageObjectTransform = hit.collider.gameObject.transform;
                        
                            GameObject placedObject = Instantiate(itemDrop, new Vector3(stageObjectTransform.position.x, stageObjectTransform.position.y + spawnHeight, stageObjectTransform.position.z), stageObjectTransform.rotation);
                            placedObject.SetActive(true);
                        
                    }

                }
            }
        }
    }

}