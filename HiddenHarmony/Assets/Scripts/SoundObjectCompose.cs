using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


public class SoundObjectCompose : MonoBehaviour{
    public GameObject itemDrop;
    private string stagePattern = "StageObj";
    private string soundPattern = "SoundObj";
    private float stageDistance = 1000;
    private float soundDistance = 5;
    private bool deleteMode = false;
    private float spawnHeight = 2;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        checkStage();
    }

    public void activateDeleteMode() {
        this.deleteMode = true;
    }

    public void setSoundObject(GameObject soundObject){
        this.deleteMode = false;
        this.itemDrop = soundObject;
    }

    public void checkStage(){
        RaycastHit hit;
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(mouseRay, out hit, stageDistance)){
            Match matchStage = Regex.Match(hit.collider.tag, stagePattern);
            Match matchSound = Regex.Match(hit.collider.tag, soundPattern);
            if(matchStage.Success) {
                if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) && itemDrop != null) {
                    Transform stageObjectTransform = hit.collider.gameObject.transform;
                    Ray stageRay = new Ray(stageObjectTransform.position, Vector3.up);
                    if(Physics.Raycast(stageRay, out hit, soundDistance)) {
                        matchSound = Regex.Match(hit.collider.tag, soundPattern);
                        if (matchSound.Success) Destroy(hit.collider.gameObject);
                    }

                    if(!deleteMode){
                        GameObject placedObject = Instantiate(itemDrop, new Vector3(stageObjectTransform.position.x, stageObjectTransform.position.y + spawnHeight, stageObjectTransform.position.z), stageObjectTransform.rotation);
                        placedObject.SetActive(true);
                    }

                }
            }
            else if(matchSound.Success)
            {
                if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) && itemDrop != null) {
                    GameObject hitSoundObject = hit.collider.gameObject;
                    Ray stageRay = new Ray(hitSoundObject.transform.position, Vector3.down);
                    if(Physics.Raycast(stageRay, out hit, soundDistance)) {
                        matchSound = Regex.Match(hit.collider.tag, stagePattern);
                        if (matchSound.Success) Destroy(hitSoundObject);
                        Transform stageObjectTransform = hit.collider.gameObject.transform;
                        if(!deleteMode){
                            GameObject placedObject = Instantiate(itemDrop, new Vector3(stageObjectTransform.position.x, stageObjectTransform.position.y + spawnHeight, stageObjectTransform.position.z), stageObjectTransform.rotation);
                            placedObject.SetActive(true);
                        }
                    }

                }
            }
        }
    }

}