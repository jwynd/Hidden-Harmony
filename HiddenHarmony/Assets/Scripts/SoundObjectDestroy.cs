using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions; // used for Regex Matching

public class SoundObjectDestroy : MonoBehaviour
{
    private string pattern = "StageObj";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider soundObjectCollide){
        Match match = Regex.Match(soundObjectCollide.tag, pattern);
        if(match.Success){
            soundObjectCollide.gameObject.transform.position = soundObjectCollide.gameObject.GetComponent<SoundObject>().origin;
        }
    }
}
