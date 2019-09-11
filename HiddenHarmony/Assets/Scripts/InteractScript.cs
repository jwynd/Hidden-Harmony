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
    [SerializeField] GameObject shake;

    [Header("Reticle")]
    [SerializeField][Tooltip("The size of the reticle when selecting")]
    private float reticleSizeGrow;
    private float reticleChangeRate;
    [SerializeField][Tooltip("The Red value of the reticle when selecting")] [Range (0,1)]
    private float reticleSelectR;
    [SerializeField][Tooltip("The Green value of the reticle when selecting")] [Range (0,1)]
    private float reticleSelectG;
    [SerializeField][Tooltip("The Blue value of the reticle when selecting")] [Range (0,1)]
    private float reticleSelectB;
    private Color reticleStartColor;
    private Color tempColor;
    private GameObject textBox;

    void Start()
    {
        intMsg = GameObject.Find("InteractMessageController");
        player = GameObject.Find("Player").transform;
        camera = GameObject.Find("Player/MainCamera").transform;
        reticle = GameObject.Find("GameplayObjects/Canvas/Reticle");
        reticleTransformOrigin = reticle.transform.localScale;
        reticleTransformGrow = reticleTransformOrigin * reticleSizeGrow;
        reticleChangeRate = 0.5f;
        reticleStartColor = reticle.GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        RaycastHit2D hit2D;
        Ray pickRay = new Ray(camera.position, camera.forward);
      
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(pickRay, out hit, interactDistance))
            {
                if (hit.collider.tag == "Interactable")
                {
                    currentInteractable = hit.collider.gameObject;
                    if (currentInteractable.transform.Find("Text Box") != null){
                        textBox = currentInteractable.transform.Find("Text Box").gameObject;
                        if(!textBox.activeSelf){
                            textBox.SetActive(true);
                            textBox.transform.parent.transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                        }
                    }
                    if (currentInteractable.transform.Find("Dialogue Prompt") != null){
                        if (currentInteractable.transform.Find("Dialogue Prompt").Find("Text Box") != null){
                            textBox = currentInteractable.transform.Find("Dialogue Prompt").Find("Text Box").gameObject;
                            if(!textBox.activeSelf){
                                textBox.SetActive(true);
                                textBox.transform.parent.transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                            }
                        }
                    }
                    if (currentInteractable.GetComponent<Animator>() != null)
                    {
                        currentInteractable.GetComponent<Animator>().SetBool("interacting", true);

                        if (currentInteractable.GetComponent<AudioSource>() != null && hit.collider.name == "Orcastra")//Mine
                        {
                            getAudio = currentInteractable.GetComponents<AudioSource>();
                            //currentInteractable.GetComponent<ParticleSystem>().Play();
                            int clipPick = Random.Range(0, getAudio.Length);
                            getAudio[clipPick].Play();
                            Camera C_Shake = shake.GetComponent<Camera>();
                            shake.GetComponent<CameraShake>().ShakeOnce(3, 14, new Vector3(2, 2, 2), C_Shake);///End Mine
                        }
                        else
                            if (currentInteractable.GetComponent<AudioSource>() != null)
                        {
                            getAudio = currentInteractable.GetComponents<AudioSource>();
                            //currentInteractable.GetComponent<ParticleSystem>().Play();
                            int clipPick = Random.Range(0, getAudio.Length);
                            getAudio[clipPick].Play();
                        }
                    }
                    else
                    {
                        if (currentInteractable.GetComponent<AudioSource>() != null && currentInteractable.GetComponent<ParticleSystem>() != null)
                        {
                            getAudio = currentInteractable.GetComponents<AudioSource>();
                            currentInteractable.GetComponent<ParticleSystem>().Play();
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
        tempColor.r = Mathf.MoveTowards(tempColor.a, reticleSelectR, reticleChangeRate);
        tempColor.g = Mathf.MoveTowards(tempColor.g, reticleSelectG, reticleChangeRate);
        tempColor.b = Mathf.MoveTowards(tempColor.b, reticleSelectB, reticleChangeRate);
        reticle.GetComponent<Image>().color = tempColor;
    }

    private void StopInteractableReticle(){
        tempColor = reticle.GetComponent<Image>().color;
        reticle.transform.localScale = Vector3.MoveTowards(reticle.transform.localScale, reticleTransformOrigin, reticleChangeRate);
        tempColor.a = Mathf.MoveTowards(tempColor.a, reticleStartColor.a, reticleChangeRate);
        tempColor.r = Mathf.MoveTowards(tempColor.a, reticleStartColor.r, reticleChangeRate);
        tempColor.g = Mathf.MoveTowards(tempColor.g, reticleStartColor.g, reticleChangeRate);
        tempColor.b = Mathf.MoveTowards(tempColor.b, reticleStartColor.b, reticleChangeRate);
        reticle.GetComponent<Image>().color = tempColor;
    }
}