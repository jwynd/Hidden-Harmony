using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SoundObjectCompose : MonoBehaviour {
    //[HideInInspector] public GameObject itemDrop;
    private GameObject itemDrop;
    private string stagePattern = "StageObj";
    private string soundPattern = "SoundObj";
    private float stageDistance = 1000;
    private float soundDistance = 5;
    private float spawnHeight = 2;

    private bool isDragging = false;
    private GameObject hPanelAccess;
    private GameObject sPanelAccess;
    private GameObject bdPanelAccess;
    private GameObject oPanelAccess;

    private Transform itemFrame;
    private GameObject itemImage;
    private EventSystem es;
    private GameObject destroyedObject;
    private Match matchHitObj;
    private string objAreaName;
    private string hitObjName;
    private GameObject hubObjs;
    private GameObject forestObjs;
    private GameObject denObjs;
    private GameObject cavernObjs;

    private Transform parentObj;
    private Match parentWithHitObj;
    private AudioSource dropObjSound;
    private AudioSource tabSwitchSound;
    private bool foundObj = false;

    private GameObject cTabs;
    private GameObject hTab;
    private GameObject sTab;
    private GameObject bdTab;
    private GameObject oTab;
    private GameObject dTab;
    private GameObject caveTab;
    private GameObject controller;

    // Start is called before the first frame update
    void Start(){
        hPanelAccess = GameObject.Find("Canvas/CTabs/HTabs/HItemsHeld");
        sPanelAccess = GameObject.Find("Canvas/CTabs/STabs/SItemsHeld");
        bdPanelAccess = GameObject.Find("Canvas/CTabs/BDTabs/BDItemsHeld");
        oPanelAccess = GameObject.Find("Canvas/CTabs/OTabs/OItemsHeld");

        hubObjs = GameObject.Find("HubSoundObjs");
        forestObjs = GameObject.Find("ForestSoundObjs");
        denObjs = GameObject.Find("DenSoundObjs");
        cavernObjs = GameObject.Find("CavernSoundObjs");
        

        dropObjSound = GameObject.Find("SoundObjSFX/PlaceSound").GetComponent<AudioSource>();
        tabSwitchSound = GameObject.Find("SoundObjSFX/TabSwitchSound").GetComponent<AudioSource>();
        cTabs = GameObject.Find("Canvas/CTabs");
        hTab = GameObject.Find("Canvas/CTabs/HTabs/HTab");
        sTab = GameObject.Find("Canvas/CTabs/STabs/STab");
        oTab = GameObject.Find("Canvas/CTabs/OTabs/OTab");
        bdTab = GameObject.Find("Canvas/CTabs/BDTabs/BDTab");
        controller = GameObject.Find("Canvas/Controllers/ComposeObjectController");
      

    }

// Update is called once per frame
void Update(){
        checkSoundObj();
        startDragging();
    }

 
    public void setSoundObject(GameObject soundObject) { 
        itemDrop = soundObject;
      
    }

    public void searchHub(GameObject currentObj)
    {
        
        for (int i = 0; i < hPanelAccess.transform.childCount; i++)
        {
            itemFrame = hPanelAccess.transform.GetChild(i);
            itemImage = itemFrame.Find("ItemSprite").gameObject;


            //print("This stuff" + itemImage.GetComponent<Image>().sprite.name);
            //print("GameObject name is " + currentObj.name);

            matchHitObj = Regex.Match(currentObj.name, itemImage.GetComponent<Image>().sprite.name);
       
            if (matchHitObj.Success)
            {
                
                hTab.GetComponent<Button>().onClick.Invoke();
                
                es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
                itemFrame.GetComponent<Drag>().OnBeginDrag(new PointerEventData(es));
                itemFrame.GetComponent<Button>().onClick.Invoke();
                currentObj.GetComponent<SoundObject>().blankStage();
                currentObj.GetComponent<SoundObject>().SnapReturn();
                currentObj.GetComponent<SoundObject>().blankCrystals();
                Destroy(currentObj);
                isDragging = true;
                break;
            }
            

        }
    }

    
    //Looks through the tab of forest items and matches it
    //with the current obj that was found in CheckSoundObj()
    public void searchForest(GameObject currentObj)
    {
        for (int i = 0; i < bdPanelAccess.transform.childCount; i++)
        {
            itemFrame = bdPanelAccess.transform.GetChild(i);
            itemImage = itemFrame.Find("ItemSprite").gameObject;


            //print("This stuff" + itemImage.GetComponent<Image>().sprite.name);
            //print("GameObject name is " + currentObj.name);

            matchHitObj = Regex.Match(currentObj.name, itemImage.GetComponent<Image>().sprite.name);

            if (matchHitObj.Success)
            {
                isDragging = true;
                
                bdTab.GetComponent<Button>().onClick.Invoke();
                
                itemFrame.GetComponent<Button>().onClick.Invoke();
                currentObj.GetComponent<SoundObject>().blankStage();
                currentObj.GetComponent<SoundObject>().SnapReturn();
                currentObj.GetComponent<SoundObject>().blankCrystals();
                Destroy(currentObj);
                break;
            }

        }
    }

    public void searchDen(GameObject currentObj)
    {
        for (int i = 0; i < sPanelAccess.transform.childCount; i++)
        {
            itemFrame = sPanelAccess.transform.GetChild(i);
            itemImage = itemFrame.Find("ItemSprite").gameObject;


            //print("This stuff" + itemImage.GetComponent<Image>().sprite.name);
            //print("GameObject name is " + currentObj.name);

            matchHitObj = Regex.Match(currentObj.name, itemImage.GetComponent<Image>().sprite.name);

            if (matchHitObj.Success)
            {
                isDragging = true;

                sTab.GetComponent<Button>().onClick.Invoke();

                itemFrame.GetComponent<Button>().onClick.Invoke();
                currentObj.GetComponent<SoundObject>().blankStage();
                currentObj.GetComponent<SoundObject>().SnapReturn();
                currentObj.GetComponent<SoundObject>().blankCrystals();
                Destroy(currentObj);
                break;
            }

        }
    }

    public void searchCavern(GameObject currentObj)
    {
        for (int i = 0; i < oPanelAccess.transform.childCount; i++)
        {
            itemFrame = oPanelAccess.transform.GetChild(i);
            itemImage = itemFrame.Find("ItemSprite").gameObject;


            //print("This stuff" + itemImage.GetComponent<Image>().sprite.name);
            //print("GameObject name is " + currentObj.name);

            matchHitObj = Regex.Match(currentObj.name, itemImage.GetComponent<Image>().sprite.name);

            if (matchHitObj.Success)
            {
                isDragging = true;

                oTab.GetComponent<Button>().onClick.Invoke();

                itemFrame.GetComponent<Button>().onClick.Invoke();
                currentObj.GetComponent<SoundObject>().blankStage();
                currentObj.GetComponent<SoundObject>().SnapReturn();
                currentObj.GetComponent<SoundObject>().blankCrystals();
                Destroy(currentObj);
                break;
            }

        }
    }


    public void startDragging()
    {
        if (isDragging == true)
        {
            es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            itemFrame.GetComponent<Drag>().OnDrag(new PointerEventData(es));
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false; 
                itemFrame.GetComponent<Drag>().OnEndDrag(new PointerEventData(es));
                //dropObjSound.GetComponents<AudioSource>()[0].Play();

            }
        }
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
                    hitObjName = hit.collider.gameObject.name;
                    //objAreaName = hit.collider.transform.parent.name;

                    // Checks if hit object name matches any Hub Object names
                    for(int i = 0; i < hubObjs.transform.childCount && !foundObj; i++){
                        parentObj = hubObjs.transform.GetChild(i);
                        parentWithHitObj = Regex.Match(hitObjName, parentObj.name);
                        //print("found obj loop " + foundObj);
                        if (parentWithHitObj.Success){
                            foundObj = true;
                            searchHub(hit.collider.gameObject);

                        }
                    }
                    //print("FOUND OBJ IS " + foundObj);
                    //Checks if hit object name matches any Forest Object names
                    for(int j = 0; j < forestObjs.transform.childCount; j++)
                    {
                        parentObj = forestObjs.transform.GetChild(j);
                        parentWithHitObj = Regex.Match(hitObjName, parentObj.name);

                        if (parentWithHitObj.Success)
                        {
                          
                            searchForest(hit.collider.gameObject);

                        }
                    }
                    for (int x = 0; x < denObjs.transform.childCount && !foundObj; x++)
                    {
                        parentObj = denObjs.transform.GetChild(x);
                        parentWithHitObj = Regex.Match(hitObjName, parentObj.name);

                        if (parentWithHitObj.Success)
                        {
                            foundObj = true;
                            searchDen(hit.collider.gameObject);

                        }
                    }
                    for (int y = 0; y < cavernObjs.transform.childCount && !foundObj; y++)
                    {
                        parentObj = cavernObjs.transform.GetChild(y);
                        parentWithHitObj = Regex.Match(hitObjName, parentObj.name);

                        if (parentWithHitObj.Success)
                        {
                            foundObj = true;
                            searchCavern(hit.collider.gameObject);

                        }
                    }
                    foundObj = false; 
                }
            }

        }
 
        
    }

    public void setupHPanel()
    {
        Transform currentPanel = null;
        Match itemsHeld; 
        for(int i =0; i< cTabs.transform.childCount; i++){
            for(int j= 0; j< cTabs.transform.GetChild(i).childCount; j++){
                currentPanel = cTabs.transform.GetChild(i).GetChild(j);
                //print("currentPanel " + currentPanel);
                itemsHeld = Regex.Match(currentPanel.name, "ItemsHeld");

                if (itemsHeld.Success){
                    currentPanel.gameObject.SetActive(false);
                }
            }
            

        }
        hPanelAccess.SetActive(true);
        tabSwitchSound.GetComponents<AudioSource>()[0].Play();


    }

    public void setupSPanel()
    {
        Transform currentPanel = null;
        Match itemsHeld;
        for (int i = 0; i < cTabs.transform.childCount; i++)
        {
            for (int j = 0; j < cTabs.transform.GetChild(i).childCount; j++)
            {
                currentPanel = cTabs.transform.GetChild(i).GetChild(j);
                //print("currentPanel " + currentPanel);
                itemsHeld = Regex.Match(currentPanel.name, "ItemsHeld");

                if (itemsHeld.Success)
                {
                    currentPanel.gameObject.SetActive(false);
                }
            }


        }
        sPanelAccess.SetActive(true);
        tabSwitchSound.GetComponents<AudioSource>()[0].Play();
    }

    public void setupBDPanel()
    {
        Transform currentPanel = null;
        Match itemsHeld;
        for (int i = 0; i < cTabs.transform.childCount; i++)
        {
            for (int j = 0; j < cTabs.transform.GetChild(i).childCount; j++)
            {
                currentPanel = cTabs.transform.GetChild(i).GetChild(j);
                //print("currentPanel " + currentPanel);
                itemsHeld = Regex.Match(currentPanel.name, "ItemsHeld");

                if (itemsHeld.Success)
                {
                    currentPanel.gameObject.SetActive(false);
                }
            }


        }
        bdPanelAccess.SetActive(true);
        tabSwitchSound.GetComponents<AudioSource>()[0].Play();
    }

    public void setupOPanel()
    {
        Transform currentPanel = null;
        Match itemsHeld;
        for (int i = 0; i < cTabs.transform.childCount; i++)
        {
            for (int j = 0; j < cTabs.transform.GetChild(i).childCount; j++)
            {
                currentPanel = cTabs.transform.GetChild(i).GetChild(j);
                //print("currentPanel " + currentPanel);
                itemsHeld = Regex.Match(currentPanel.name, "ItemsHeld");

                if (itemsHeld.Success)
                {
                    currentPanel.gameObject.SetActive(false);
                }
            }


        }
        oPanelAccess.SetActive(true);
        tabSwitchSound.GetComponents<AudioSource>()[0].Play();
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
                        dropObjSound.GetComponents<AudioSource>()[0].Play();
                        


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
                            dropObjSound.GetComponents<AudioSource>()[0].Play();
                            

                    }

                }
            }
        }
    }

    public bool checkError()
    {
        RaycastHit hit;
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mouseRay, out hit, stageDistance))
        {
            Match matchStage = Regex.Match(hit.collider.tag, stagePattern);
            Match matchSound = Regex.Match(hit.collider.tag, soundPattern);
            if (matchStage.Success)
            {
                    Transform stageObjectTransform = hit.collider.gameObject.transform;
                    Ray stageRay = new Ray(stageObjectTransform.position, Vector3.up);
                    if (Physics.Raycast(stageRay, out hit, soundDistance))
                    {
                        matchSound = Regex.Match(hit.collider.tag, soundPattern);
                        //if (matchSound.Success) Destroy(hit.collider.gameObject);
                    }


                    /*GameObject placedObject = Instantiate(itemDrop, new Vector3(stageObjectTransform.position.x, stageObjectTransform.position.y + spawnHeight, stageObjectTransform.position.z), stageObjectTransform.rotation);
                    placedObject.SetActive(true);
                    dropObjSound.GetComponents<AudioSource>()[0].Play();*/
                    return true;


                
            }
            else if (matchSound.Success)
            {
                    GameObject hitSoundObject = hit.collider.gameObject;
                    Ray stageRay = new Ray(hitSoundObject.transform.position, Vector3.down);
                    if (Physics.Raycast(stageRay, out hit, soundDistance))
                    {
                        matchSound = Regex.Match(hit.collider.tag, stagePattern);
                        //if (matchSound.Success) Destroy(hitSoundObject);
                        Transform stageObjectTransform = hit.collider.gameObject.transform;

                        /*GameObject placedObject = Instantiate(itemDrop, new Vector3(stageObjectTransform.position.x, stageObjectTransform.position.y + spawnHeight, stageObjectTransform.position.z), stageObjectTransform.rotation);
                        placedObject.SetActive(true);
                        dropObjSound.GetComponents<AudioSource>()[0].Play();*/
                        return true;

                    }

                
            }
        }
        return false;
    }
}