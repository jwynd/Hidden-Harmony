using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    private GameObject startMenuUI;
    private GameObject menuCamera;
    private GameObject mainCamera;

    // Start is called before the first frame update
    void Start() {
        startMenuUI = GameObject.Find("Canvas/StartMenuMain");
        menuCamera = GameObject.Find("MenuCamera");
        mainCamera = GameObject.Find("Player/MainCamera");
    }

    public void StartButton(){
        startMenuUI.SetActive(false);
        Destroy(menuCamera);
        mainCamera.GetComponent<Camera>().enabled = true;
        mainCamera.GetComponent<FirstPersonControl>().enabled = true;
        mainCamera.GetComponent<AudioListener>().enabled = true;
    }

}
