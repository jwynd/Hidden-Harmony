using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnce : MonoBehaviour
{
    [SerializeField] private GameObject[] deleteObjects;

    private bool eq = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        eq = this.GetComponent<SoundObject>().onStage;
        if(eq){
            // print("Creating subWooferObjects");
            foreach(GameObject obj in deleteObjects){
                if(obj.activeSelf) obj.SetActive(false);
            }
        }
    }
}
