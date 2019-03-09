using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


public class SoundObjectCompose : MonoBehaviour{
    public GameObject itemDrop;
    private string pattern = "StageObj";
    private float checkDistance = 1000;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        RaycastHit hit;
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.Log("general test");
        if(Physics.Raycast(mouseRay, out hit, checkDistance)){
            //Debug.Log("hit");
            Match match = Regex.Match(hit.collider.tag, pattern);
            if(match.Success) {
                //Debug.Log("match success");
                if (Input.GetMouseButtonDown(0) && itemDrop != null) {
                    GameObject placedObject;
                    Transform hitObjectTransform = hit.collider.gameObject.transform;
                    placedObject = Instantiate(itemDrop, new Vector3(hitObjectTransform.position.x, hitObjectTransform.position.y + 2, hitObjectTransform.position.z), hitObjectTransform.rotation);

                }
            }
        }
    }

    public void setSoundObject(GameObject soundObject){
        this.itemDrop = soundObject;
    }

}