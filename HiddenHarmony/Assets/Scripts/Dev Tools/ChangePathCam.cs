using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using PathCreation.Examples;

public class ChangePathCam : MonoBehaviour
{
    public GameObject[] disable;
    public GameObject[] enable;
    public Material skybox;
    public PostProcessProfile ppp; 
    public float cameraSpeed = 4f;
    public bool underwater;

    private GameObject camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("TrailerPaths/CutsceneCam");
    }

    void OnTriggerEnter() {
        foreach(GameObject obj in disable) {
            obj.SetActive(false);
        }
        foreach(GameObject obj in enable) {
            obj.SetActive(true);
        }

        RenderSettings.skybox = skybox;
        RenderSettings.fog = underwater;

        camera.GetComponent<PostProcessVolume>().profile = ppp;
        camera.GetComponent<lookAt>().speed = cameraSpeed;
    }
}
