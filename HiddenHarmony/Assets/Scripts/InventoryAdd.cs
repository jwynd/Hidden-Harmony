using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryAdd : MonoBehaviour
{
    [HideInInspector] public bool held = false;
    public float interactDistance = 5.0f;

    private GameObject intMsg;
    private GameObject currentObject;
    private AudioSource soundSFX;
    private Transform camera;
    private Transform player;
    private Transform holdPosition;
    private GameObject itemPanel;
    private GameObject newButton;
    private GameObject itemSprite;
    private GameObject hTab;
    private GameObject sTab;
    private GameObject bdTab;
    private GameObject oTab;
    private GameObject hItemPanel;
    private GameObject sItemPanel;
    private GameObject bdItemPanel;
    private GameObject oItemPanel;
    private GameObject currentItem;
    private string parentName;
    private GameObject controller; 
    [SerializeField] private GameObject itemButton; // itemButton refers to ItemFrame Prefab

    private GameObject errorSprite;
    private GameObject enlargedSprite;


    void Awake(){
        //itemPanel = GameObject.Find("Canvas/ItemsHeld");

    }

    void Start(){
        soundSFX = GameObject.Find("SoundObjSFX/PickupSound").GetComponent<AudioSource>();
        intMsg = GameObject.Find("InteractMessageController");
        player = GameObject.Find("Player").transform;
        camera = GameObject.Find("Player/MainCamera").transform;
        holdPosition = GameObject.Find("Player/MainCamera/HoldPosition").transform;
        controller = GameObject.Find("Canvas/Controllers/ComposeObjectController");
        hTab = GameObject.Find("Canvas/CTabs/HTabs/HTab");
        sTab = GameObject.Find("Canvas/CTabs/STabs/STab");
        bdTab = GameObject.Find("Canvas/CTabs/BDTabs/BDTab");
        oTab = GameObject.Find("Canvas/CTabs/OTabs/OTab");
        hItemPanel = GameObject.Find("Canvas/CTabs/HTabs/HItemsHeld");
        bdItemPanel = GameObject.Find("Canvas/CTabs/BDTabs/BDItemsHeld");
        sItemPanel = GameObject.Find("Canvas/CTabs/STabs/SItemsHeld");
        oItemPanel = GameObject.Find("Canvas/CTabs/OTabs/OItemsHeld");
    }

    void createButtonInTab(GameObject tab, GameObject hitItem)
    {
        errorSprite = itemButton.transform.GetChild(1).gameObject;
        errorSprite.SetActive(false);
        newButton = Instantiate(itemButton, tab.transform);
        newButton.GetComponent<Button>().onClick.AddListener(() => controller.GetComponent<SoundObjectCompose>().setSoundObject(hitItem));
        itemSprite = newButton.transform.Find("ItemSprite").gameObject;
        itemSprite.GetComponent<Image>().sprite = Resources.Load<Sprite>(hitItem.name);
        hitItem.SetActive(false);
        soundSFX.Play();
    }


    // Update is called once per frame
    void Update(){
        RaycastHit hit;
        Ray pickRay = new Ray(camera.position, camera.forward);
        if(Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)){
            if(Physics.Raycast(pickRay, out hit, interactDistance)){
                if(hit.collider.tag == "SoundObj"){
                    if(!hit.collider.gameObject.GetComponent<SoundObject>().OnStage()){
                        //itemPanel = GameObject.Find("Canvas/ItemsHeld");
                        
                        if(hit.collider.gameObject.GetComponent<SoundObject>().toDestroyOnPickup != null){
                            Destroy(hit.collider.gameObject.GetComponent<SoundObject>().toDestroyOnPickup);
                        }
                        parentName = hit.collider.transform.parent.name;
                        if(parentName == "HubSoundObjs")
                        {
                            //hTab.SetActive(true);
                            hItemPanel.SetActive(true);
                            sItemPanel.SetActive(false);
                            bdItemPanel.SetActive(false);
                            oItemPanel.SetActive(false);
                            currentItem = hit.collider.gameObject;
                            itemButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("hub_slot");
                            enlargedSprite = itemButton.transform.GetChild(0).gameObject;
                            enlargedSprite.GetComponent<Image>().sprite = Resources.Load<Sprite>("hub_slot");
                            createButtonInTab(hItemPanel, currentItem);
                        }
                        else if(parentName == "DenSoundObjs")
                        {

                            //sTab.SetActive(true);
                            hItemPanel.SetActive(false);
                            sItemPanel.SetActive(true);
                            bdItemPanel.SetActive(false);
                            oItemPanel.SetActive(false);
                            itemButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("cave_slot");
                            enlargedSprite = itemButton.transform.GetChild(0).gameObject;
                            enlargedSprite.GetComponent<Image>().sprite = Resources.Load<Sprite>("cave_slot");
                            currentItem = hit.collider.gameObject;
                            createButtonInTab(sItemPanel, currentItem);
        
                        } else if(parentName == "ForestSoundObjs")
                        {

                            //bdTab.SetActive(true);
                            hItemPanel.SetActive(false);
                            sItemPanel.SetActive(false);
                            bdItemPanel.SetActive(true);
                            oItemPanel.SetActive(false);
                            itemButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("forest_slot");
                            enlargedSprite = itemButton.transform.GetChild(0).gameObject;
                            enlargedSprite.GetComponent<Image>().sprite = Resources.Load<Sprite>("forest_slot");
                            currentItem = hit.collider.gameObject;
                            createButtonInTab(bdItemPanel, currentItem);
                        } else if(parentName == "CavernSoundObjs")
                        {

                            //oTab.SetActive(true);

                            hItemPanel.SetActive(false);
                            sItemPanel.SetActive(false);
                            bdItemPanel.SetActive(false);
                            oItemPanel.SetActive(true);
                            itemButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("ocean_slot");
                            enlargedSprite = itemButton.transform.GetChild(0).gameObject;
                            enlargedSprite.GetComponent<Image>().sprite = Resources.Load<Sprite>("ocean_slot");
                            currentItem = hit.collider.gameObject;
                            createButtonInTab(oItemPanel, currentItem);
                        }
                        
                    }
                }
            }
        }
        else {
            intMsg.GetComponent<InteractMessage>().HideInteractMessage();
            if(Physics.Raycast(pickRay, out hit, interactDistance)){
                if(hit.collider.tag == "SoundObj"){
                    if(!hit.collider.gameObject.GetComponent<SoundObject>().OnStage()){
                        //intMsg.GetComponent<InteractMessage>().ShowInteractMessage("Press 'E' to pick up");
                    }
                }
            }
        }
    }
}
