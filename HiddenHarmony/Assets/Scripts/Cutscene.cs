using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;


public class Cutscene : MonoBehaviour
{
    [Tooltip("These are the points that the camera will look at, it will switch to the next point after the time in delay has pased")]
    [SerializeField] private Transform[] lookAt;
    [Tooltip("The camera will wait this long before looking at the next target as specified by \"Look At\"")]
    [SerializeField] private float[] delays;
    [Tooltip("These will be created at the delay minus the \"Appearance Offset\"")]
    [SerializeField] private GameObject[] toEnable;
    [Tooltip("Game Objects are enabled at delay minus this")]
    [SerializeField] private float appearanceOffset;
    [Tooltip("This is the Path Creator for the path that you want the camera to follow")]
    [SerializeField] private PathCreator pathCreator;

    private GameObject mainCamera;
    private int index = 0;
    private float timer = 0;
    private bool runSequence = true;

    void OnValidate(){
        if(appearanceOffset < 0.0f) appearanceOffset = 0.0f;
    }


    void Awake()
    {
        if(lookAt.Length != delays.Length || delays.Length != toEnable.Length){
            #if UNITY_EDITOR
            throw new System.ArgumentException("All arrays must be of identical length");
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }

    void Start(){
        mainCamera = GameObject.Find("MainCamera");
        //mainCamera.GetComponent<PathFollower>().enabled = true;
        Fadeout(Color.black); //Fadeout camera
        // Camera will snap to position when the pathCreator is enabled
        pathCreator.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(runSequence){// if the sequence has not run to completion
            
            if(index < lookAt.Length){
                //Main Portion
                mainCamera.transform.LookAt(lookAt[index]);
                if(timer > delays[index] - appearanceOffset){
                    Fadeout(Color.white); // this should happen a certain number of seconds before the object appears and only once
                    toEnable[index].SetActive(true);
                }
            } else {
                runSequence = false;
                Destroy(pathCreator);
                GameObject.Find("CameraChange").GetComponent<ComposeModeTransition>().composeImmediate();
            }
            timer += Time.deltaTime;// Increment the time
            if(timer > delays[index]) index++;
        }
        // Destroy(pathCreator); //This runs at the end of the script
    }

    private void Fadeout(Color fadeColor){
        // Add fadeout as a post processing effect
        throw new System.ArgumentException("Fadeout not written");
    }
}
