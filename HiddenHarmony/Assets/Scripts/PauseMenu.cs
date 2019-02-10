using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // below is tutorial suggestion, replace if wrong
    private GameObject pauseMenuUI;
    private GameObject player;
    private GameObject camera;
    private bool isPaused;

    void Awake(){
        player = GameObject.Find("Player");
        camera = GameObject.Find("Player/MainCamera");
        pauseMenuUI = GameObject.Find("Canvas/PauseMenuMain");
        if(pauseMenuUI == null) throw new System.ArgumentException("PauseMenuMain not found");
        DeactivateMenu();
        isPaused = false;
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            isPaused = !isPaused;
            
        }

        if(isPaused){
            ActivateMenu();
        }
        else{
            DeactivateMenu();
        }
    }

    private void ActivateMenu(){
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<FirstPersonControl>().enabled = false;
        player.GetComponent<Pickup>().enabled = false;
        camera.GetComponent<FirstPersonControl>().enabled = false;
        pauseMenuUI.SetActive(true);
    }

    public void DeactivateMenu(){
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<FirstPersonControl>().enabled = true;
        player.GetComponent<Pickup>().enabled = true;
        camera.GetComponent<FirstPersonControl>().enabled = true;
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }

    public void MainMenu(){
        SceneManager.LoadScene("Demo");
    }

    public void Quit(){
        Application.Quit();
    }
}
