using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    private GameObject startMenuUI;
    private GameObject menuCamera;
    private GameObject mainCamera;
    private GameObject fade;
    private float fadeTime = 2.0f;
    private float timer = 0.0f;
    private bool fading = false;

    // Start is called before the first frame update
    void Start() {
        startMenuUI = GameObject.Find("Canvas/StartMenuMain");
        menuCamera = GameObject.Find("MenuCamera");
        mainCamera = GameObject.Find("Player/MainCamera");
        fade = GameObject.Find("MenuCamera/Fade");
    }

    void Update() {
        if(fading){
            

            fade.GetComponent<MeshRenderer>().material.color = 
                Color.Lerp(fade.GetComponent<MeshRenderer>().material.color,
                           Color.black, fadeTime * Time.deltaTime);
            timer += Time.deltaTime;

            if(timer > fadeTime){
                // do this after everything has fade
                mainCamera.GetComponent<Camera>().enabled = true;
                mainCamera.GetComponent<FirstPersonControl>().enabled = true;
                mainCamera.GetComponent<AudioListener>().enabled = true;
                Destroy(fade);
                Destroy(menuCamera);
                fading = false;
            }
        }


    }

    public void StartButton(){
        print("start button pressed");
        startMenuUI.SetActive(false);
        fading = true;
    }

}
