using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float raycastRadius = 0.5f;
    [SerializeField] private float raycastDistance = 1.0f;
    private bool occupied;
    private bool hitDetect;

    public bool IsOccupied(){
        return occupied;
    }

    // Update is called once per frame
    void Update(){
        // make 5 raycast hits
        RaycastHit hit;
        Ray[] stageRay = new Ray[5];
        stageRay[0] = new Ray(this.transform.position, Vector3.up);
        stageRay[1] = new Ray(new Vector3(this.transform.position.x + raycastRadius, this.transform.position.y, this.transform.position.z), Vector3.up);
        stageRay[2] = new Ray(new Vector3(this.transform.position.x - raycastRadius, this.transform.position.y, this.transform.position.z), Vector3.up);
        stageRay[3] = new Ray(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + raycastRadius), Vector3.up);
        stageRay[4] = new Ray(new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - raycastRadius), Vector3.up);

        for(int i = 0; i < 5; ++i){
            if(Physics.Raycast(stageRay[i], out hit, raycastDistance)){
                if(hit.collider.tag == "SoundObj"){
                    occupied = true;
                    break;
                }
                else{
                    occupied = false;
                }
            }
            else{
                occupied = false;
            }
        }
        /*if(Physics.Raycast(stageRay, out hit, 1.0f)){

        /*if(Physics.SphereCast(this.transform.position, 5.0f, Vector3.up, out hit, 3.0f)){
            if(hit.collider.tag == "SoundObj"){
                occupied = true;
            }
            else{
                occupied = false;
            }
        }
        else{
            occupied = false;
        }*/
    }
}
