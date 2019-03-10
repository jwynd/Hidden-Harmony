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

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        RaycastHit hit1;
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.Log("general test");
        if(Physics.Raycast(mouseRay, out hit1, stageDistance)){
            //Debug.Log("hit1");
            Match match = Regex.Match(hit1.collider.tag, stagePattern);
            if(match.Success) {
                //Debug.Log("match success");
                if (Input.GetMouseButtonDown(0) && itemDrop != null) {
                    Transform hit1ObjectTransform = hit1.collider.gameObject.transform;
			        Ray stageRay = new Ray(hit1ObjectTransform.position, Vector3.up);
			        RaycastHit hit2;
                	match = Regex.Match(hit1.collider.tag, soundPattern);
                	if(Physics.Raycast(stageRay, out hit2, soundDistance)) {
                		Destroy(hit2.collider.gameObject);
                	}

                    GameObject placedObject;
                    placedObject = Instantiate(itemDrop, new Vector3(hit1ObjectTransform.position.x, hit1ObjectTransform.position.y + 2, hit1ObjectTransform.position.z), hit1ObjectTransform.rotation);

                }
            }
        }
    }

    public void setSoundObject(GameObject soundObject){
        this.itemDrop = soundObject;
    }

}