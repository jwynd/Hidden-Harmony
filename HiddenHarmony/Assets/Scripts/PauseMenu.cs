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
    private GameObject inventory;
    private GameObject reticle;

    void Awake(){
        player = GameObject.Find("Player");
        camera = GameObject.Find("Player/MainCamera");
        pauseMenuUI = GameObject.Find("Canvas/PauseMenuMain");
        menuCamera = GameObject.Find("MenuCamera");
        inventory = GameObject.Find("ItemsHeld");
        reticle = GameObject.Find("Reticle");
        mainMenuScene = SceneManager.GetActiveScene().name;
        if(pauseMenuUI == null) throw new System.ArgumentException("PauseMenuMain not found");
        DeactivateMenu();
        inventory.SetActive(false);
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
        camera.GetComponent<FirstPersonControl>().enabled = false;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true);
        inventory.SetActive(false);
        reticle.SetActive(false);
    }

    public void DeactivateMenu(){
        // print("DeactivateMenu");
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<FirstPersonControl>().enabled = true;
        player.GetComponent<InventoryAdd>().enabled = true;
        camera.GetComponent<FirstPersonControl>().enabled = true;
        pauseMenuUI.SetActive(false);
        inventory.SetActive(true);
        Cursor.visible = false;
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
        Application.Quit();
    }
}
