using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // below is tutorial suggestion, replace if wrong
    private string mainMenuScene;
    private GameObject pauseMenuUI;
    private GameObject player;
    private GameObject camera;
    private bool isPaused;
    private GameObject menuCamera; // use this to determine if while loop should be running
    private GameObject hTab;
    private GameObject sTab;
    private GameObject bdTab;
    private GameObject oTab;
    private GameObject hInventory;
    private GameObject sInventory;
    private GameObject bdInventory;
    private GameObject oInventory; 
    private GameObject reticle;
    private ComposeModeTransition cmt;

    void Awake(){
        player = GameObject.Find("Player");
        camera = GameObject.Find("Player/MainCamera");
        pauseMenuUI = GameObject.Find("Canvas/PauseMenuMain");
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
        DeactivateMenu();
        hInventory.SetActive(false);
        isPaused = false;
    }

    void Update(){
        if(menuCamera == null){
            if(Input.GetKeyDown(KeyCode.Escape)){
                isPaused = !isPaused;
                if(isPaused){
                    ActivateMenu();
                }
                else{
                    DeactivateMenu();
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
        player.transform.Find("AudioSource").GetComponent<AudioSource>().Pause();
        camera.GetComponent<FirstPersonControl>().enabled = false;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);

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
        player.transform.Find("AudioSource").GetComponent<AudioSource>().Play();
        camera.GetComponent<FirstPersonControl>().enabled = !cmt.Compose();
        pauseMenuUI.SetActive(false);

        hTab.SetActive(false);
        sTab.SetActive(false);
        bdTab.SetActive(false);
        oTab.SetActive(false);
        hInventory.SetActive(false);
        sInventory.SetActive(false);
        bdInventory.SetActive(false);
        oInventory.SetActive(false);
        Cursor.visible = cmt.Compose();
        isPaused = false;
        reticle.SetActive(true);
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

    public void PlayHover(){
        GetComponents<AudioSource>()[0].Play();
    }

    public void PlayClick(){
        GetComponents<AudioSource>()[1].Play();
    }
}
