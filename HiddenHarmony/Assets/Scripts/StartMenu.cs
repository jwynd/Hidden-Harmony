using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private float fadeTime = 2.0f;
    [SerializeField] private float movementSpeed = 10.0f;
    [SerializeField] private PostProcessProfile playerPostProcessingProfile;

    private GameObject player;
    private GameObject startMenuUI;
    private GameObject menuCamera;
    private GameObject mainCamera;
    private GameObject fade;
    private float timer = 0.0f;
    private bool fading = false;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
        startMenuUI = GameObject.Find("Canvas/StartMenuMain");
        menuCamera = GameObject.Find("MenuCamera");
        mainCamera = GameObject.Find("Player/MainCamera");
        fade = GameObject.Find("MenuCamera/Fade");
        Cursor.visible = true;
        player.SetActive(false);
        
    }

    void Update() {
        if(fading){
            

            fade.GetComponent<MeshRenderer>().material.color = 
                Color.Lerp(fade.GetComponent<MeshRenderer>().material.color,
                           Color.black, fadeTime * Time.deltaTime);
            timer += Time.deltaTime;
            menuCamera.transform.position = 
                Vector3.MoveTowards(menuCamera.transform.position, fade.transform.position, movementSpeed*Time.deltaTime);

            if(timer > fadeTime){
                // do this after everything has fade
                mainCamera.GetComponent<Camera>().enabled = true;
                mainCamera.GetComponent<FirstPersonControl>().enabled = true;
                mainCamera.GetComponent<AudioListener>().enabled = true;
                mainCamera.GetComponent<PostProcessVolume>().enabled = true;
                mainCamera.GetComponent<PostProcessVolume>().profile = playerPostProcessingProfile;
                Destroy(fade);
                Destroy(menuCamera);
                player.SetActive(true);
                GameObject.Find("PauseMenuController").GetComponent<PauseMenu>().DeactivateMenu();
                fading = false;
            }
        }


    }

    public void StartButton(){
        startMenuUI.SetActive(false);
        Cursor.visible = false;
        fading = true;
    }

}
