using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractScript : MonoBehaviour
{
    public float interactDistance = 5.0f;

    private AudioSource[] getAudio;
    private GameObject intMsg;
    private Transform camera;
    private Transform player;
    private GameObject currentInteractable;
    private GameObject reticle;
    private Vector3 reticleTransformOrigin;
    private Vector3 reticleTransformGrow;
    private float reticleSizeGrow;
    private float reticleChangeRate;
    private Color tempColor;

    void Start()
    {
        intMsg = GameObject.Find("InteractMessageController");
        player = GameObject.Find("Player").transform;
        camera = GameObject.Find("Player/MainCamera").transform;
        reticle = GameObject.Find("GameplayObjects/Canvas/Reticle");
        reticleSizeGrow = 2f;
        reticleTransformOrigin = reticle.transform.localScale;
        reticleTransformGrow = reticleTransformOrigin * reticleSizeGrow;
        reticleChangeRate = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray pickRay = new Ray(camera.position, camera.forward);
      
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(pickRay, out hit, interactDistance))
            {
                if (hit.collider.tag == "Interactable")
                {
                    currentInteractable = hit.collider.gameObject;
                    if (currentInteractable.GetComponent<Animator>() != null)
                    {
                        currentInteractable.GetComponent<Animator>().SetBool("interacting", true);
                        if (currentInteractable.GetComponent<AudioSource>() != null)
                        {
                            getAudio = currentInteractable.GetComponents<AudioSource>();
                            int clipPick = Random.Range(0, getAudio.Length);
                            getAudio[clipPick].Play();


                        }
                    }
                    else
                    {
                        if (currentInteractable.GetComponent<AudioSource>() != null)
                        {
                            getAudio = currentInteractable.GetComponents<AudioSource>();
                            int clipPick = Random.Range(0, getAudio.Length);
                            getAudio[clipPick].Play();
                        }
                    }
                }
            }
        }
        else
        {
            if (Physics.Raycast(pickRay, out hit, interactDistance))
            {
                if (hit.collider.tag == "Interactable"){
                    InteractableReticle();
                    //intMsg.GetComponent<InteractMessage>().ShowInteractMessage("Press 'E' to interact");
                }
                else if(hit.collider.tag == "SoundObj" && Physics.Raycast(pickRay, out hit, player.GetComponent<InventoryAdd>().interactDistance)){
                    if(!hit.collider.gameObject.GetComponent<SoundObject>().OnStage()){
                        InteractableReticle();
                    }
                }
                else{
                    StopInteractableReticle();
                }
            }
            else{
                StopInteractableReticle();
            }
        }
    }

    private void InteractableReticle(){
        tempColor = reticle.GetComponent<Image>().color;
        reticle.transform.localScale = Vector3.MoveTowards(reticle.transform.localScale, reticleTransformGrow, reticleChangeRate);
        tempColor.a = Mathf.MoveTowards(tempColor.a, 1f, reticleChangeRate);
        reticle.GetComponent<Image>().color = tempColor;
    }

    private void StopInteractableReticle(){
        tempColor = reticle.GetComponent<Image>().color;
        reticle.transform.localScale = Vector3.MoveTowards(reticle.transform.localScale, reticleTransformOrigin, reticleChangeRate);
        tempColor.a = Mathf.MoveTowards(tempColor.a, 0.5f, reticleChangeRate);
        reticle.GetComponent<Image>().color = tempColor;
    }
}