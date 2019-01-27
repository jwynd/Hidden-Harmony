using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSound : MonoBehaviour
{
    public float distance = 10.0f;
    private bool held = false;
    private GameObject sound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray pickRay = new Ray(this.transform.position, this.transfrom.position.forward);
        if(Input.GetKeyDown(KeyCode.E) && !held){
            if(Physics.Raycast(pickRay, out hit, distance)){
                if(hit.collider.tag == "SoundObj"){
                    sound = hit.transfrom.gameObject;
                    sound.transfrom.parent = this.transfrom;
                    held = true;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.E) && held){
            held = false;
            sound.transfrom.parent = null;
        }
    }
}
