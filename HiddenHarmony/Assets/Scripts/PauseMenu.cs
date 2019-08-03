using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // below is tutorial suggestion, replace if wrong
    private string mainMenuScene;
    private GameObject pauseMenuUI;
    private GameObject optionsMenuUI;
    private GameObject player;
    private GameObject camera;
    private bool isPaused;
    private bool inOptions = false;
    private GameObject menuCamera; // use this to determine if while loop should be running
    private GameObject hTab;
    private GameObject sTab;
    private GameObject bdTab;
    private GameObject oTab;
    private bool hState = false;
    private bool bdState = false;
    private bool sState = false;
    private bool oState = false;
    private GameObject hInventory;
    private GameObject sInventory;
    private GameObject bdInventory;
    private GameObject oInventory; 
    private GameObject reticle;
    private AudioSource cmtAudio;
    private ComposeModeTransition cmt;

    void Awake(){
        player = GameObject.Find("Player");
        camera = GameObject.Find("Player/MainCamera");
        pauseMenuUI = GameObject.Find("Canvas/PauseMenuMain");
        optionsMenuUI = GameObject.Find("Canvas/OptionsMenuMain");
        menuCamera = GameObject.Find("MenuCamera");
        hInventory = GameObject.Find("Canvas/CTabs/HTabs/HItemsHeld");
        sInventory = GameObject.Find("Canvas/CTabs/STabs/SItemsHeld");
        bdInventory = GameObject.Find("Canvas/CTabs/BDTabs/BDItemsHeld");
        oInventory = GameObject.Find("Canvas/CTabs/OTabs/OItemsHeld");

        hTab = GameObject.Find("Canvas/CTabs/HTabs/HTab");
        sTab = GameObject.Find("Canvas/CTabs/STabs/STab");
        bdTab = GameObject.Find("Canvas/CTabs/BDTabs/BDTab");
        oTab = GameObject.Find("Canvas/CTabs/OTabs/OTab");
        reticle = GameObject.Find("Reticle");
        mainMenuScene = SceneManager.GetActiveScene().name;
        if(pauseMenuUI == null) throw new System.ArgumentException("PauseMenuMain not found");
        cmt = GameObject.Find("GameplayObjects/CameraChange").GetComponent<ComposeModeTransition>();
        cmtAudio = GameObject.Find("GameplayObjects/CameraChange").GetComponents<AudioSource>()[2];
        cmtAudio.enabled = false;
        DeactivateMenu();
        hInventory.SetActive(false);
        hTab.SetActive(false);
        sTab.SetActive(false);
        bdTab.SetActive(false);
        oTab.SetActive(false);
        optionsMenuUI.SetActive(false);
        isPaused = false;
    }

    void Update(){
        if(menuCamera == null){
            if(Input.GetKeyDown(KeyCode.Escape)){
                if(inOptions){
                    OptionsReturn();
                } else {
                    isPaused = !isPaused;
                    if(isPaused){
                        checkTabs();
                        ActivateMenu();
                    }
                    else{        
                        DeactivateMenu();
                        activateTabs();
                    }
                }
            }
        }
    }

    private void ActivateMenu(){
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<FirstPersonControl>().enabled = false;
        player.GetComponent<InventoryAdd>().enabled = false;
        player.transform.Find("GlideParticles").GetComponent<ParticleSystem>().Pause();
        player.transform.Find("Audio/GlideAudio").GetComponent<AudioSource>().Pause();
        camera.GetComponent<FirstPersonControl>().enabled = false;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        cmtAudio.enabled = false;
        hTab.SetActive(false);
        sTab.SetActive(false);
        bdTab.SetActive(false);
        oTab.SetActive(false); 
        hInventory.SetActive(false);
        sInventory.SetActive(false);
        bdInventory.SetActive(false);
        oInventory.SetActive(false);
        reticle.SetActive(false);
    }

    public void DeactivateMenu(){
        // print("DeactivateMenu");
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<FirstPersonControl>().enabled = true;
        player.GetComponent<InventoryAdd>().enabled = true;
        player.transform.Find("GlideParticles").GetComponent<ParticleSystem>().Play();
        player.transform.Find("Audio/GlideAudio").GetComponent<AudioSource>().Play();
        camera.GetComponent<FirstPersonControl>().enabled = !cmt.Compose();
        pauseMenuUI.SetActive(false);

        cmtAudio.enabled = true;
        hTab.SetActive(true);
        bdTab.SetActive(true);
        sTab.SetActive(true);
        oTab.SetActive(true);
        hInventory.SetActive(true);
        sInventory.SetActive(false);
        bdInventory.SetActive(false);
        oInventory.SetActive(false);
        Cursor.visible = cmt.Compose();
        isPaused = false;
        reticle.SetActive(true);
    }

    public void checkTabs()
    {
         hState = hTab.activeSelf;
         //print("hState" + hState);
         bdState = bdTab.activeSelf;
         //print("bdState " + bdState);
         sState = sTab.activeSelf;
         oState = oTab.activeSelf;
    }
    
    public void activateTabs()
    {
        /*if(hState == true){
            hTab.SetActive(true);
        }
        else{
            hTab.SetActive(false);
        }
        if(bdState == true){
            bdTab.SetActive(true);
        }
        else{
            bdTab.SetActive(false);
        }
        if(sState == true){
            sTab.SetActive(true);
        } else{
            sTab.SetActive(false);
        }
        if(oState == true){
            oTab.SetActive(true);
        } else{
            oTab.SetActive(false);
        }*/

        hTab.SetActive(true);
        bdTab.SetActive(true);
        sTab.SetActive(true);
        oTab.SetActive(true);
    }

    public void Resume(){
        DeactivateMenu();
    }

    public void MainMenu(){
        player.GetComponent<InventoryAdd>().enabled = true;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Quit(){
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void Options(){
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
        inOptions = true;
    }

    public void OptionsReturn(){
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        inOptions = false;
    }


    public void PlayHover(){
        GetComponents<AudioSource>()[0].Play();
    }

    public void PlayClick(){
        GetComponents<AudioSource>()[1].Play();
    }
}
