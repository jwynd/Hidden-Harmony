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
    [SerializeField] private GameObject itemButton;

    void Awake(){
        //itemPanel = GameObject.Find("Canvas/ItemsHeld");

    }

    void Start(){
        soundSFX = GameObject.Find("SoundObjSFX/PickupSound").GetComponent<AudioSource>();
        intMsg = GameObject.Find("InteractMessageController");
        player = GameObject.Find("Player").transform;
        camera = GameObject.Find("Player/MainCamera").transform;
        holdPosition = GameObject.Find("Player/MainCamera/HoldPosition").transform;
    }

    // Update is called once per frame
    void Update(){
        RaycastHit hit;
        Ray pickRay = new Ray(camera.position, camera.forward);
        if(Input.GetKeyDown(KeyCode.E)){
            if(Physics.Raycast(pickRay, out hit, interactDistance)){
                if(hit.collider.tag == "SoundObj"){
                    if(!hit.collider.gameObject.GetComponent<SoundObject>().OnStage()){
                        itemPanel = GameObject.Find("Canvas/ItemsHeld");
                        newButton = Instantiate(itemButton, itemPanel.transform);
                        newButton.GetComponent<Button>().onClick.AddListener(() => itemPanel.GetComponent<SoundObjectCompose>().setSoundObject(hit.collider.gameObject));
                        itemSprite = newButton.transform.Find("ItemSprite").gameObject;
                        print("\n\n\n!!!hit.collider.gameObject.name = "+hit.collider.gameObject.name+"\n\n\n");
                        itemSprite.GetComponent<Image>().sprite = Resources.Load<Sprite>(hit.collider.gameObject.name);
                        print("itemSprite"+itemSprite);
                        hit.collider.gameObject.SetActive(false);
                        soundSFX.Play();
                    }
                }
            }
        }
        else {
            intMsg.GetComponent<InteractMessage>().HideInteractMessage();
            if(Physics.Raycast(pickRay, out hit, interactDistance)){
                if(hit.collider.tag == "SoundObj"){
                    if(!hit.collider.gameObject.GetComponent<SoundObject>().OnStage()){
                        intMsg.GetComponent<InteractMessage>().ShowInteractMessage("Press 'E' to pick up");
                    }
                }
            }
        }
    }
}
