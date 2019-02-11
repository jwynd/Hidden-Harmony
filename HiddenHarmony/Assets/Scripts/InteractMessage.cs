using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InteractMessage : MonoBehaviour
{
    private GameObject interactMessage;
    private Text interactText;
    private string defaultText;
    // Start is called before the first frame update
    void Start()
    {
        interactMessage = GameObject.Find("Canvas/InteractMessage");
        interactText = interactMessage.GetComponent<Text>();
        defaultText = interactText.text;
        interactMessage.SetActive(false);
    }

    public void ShowInteractMessage(){
        interactMessage.SetActive(true);
    }

    public void ShowInteractMessage(string msg){
        interactText.text = msg;
        interactMessage.SetActive(true);
    }

    public void HideInteractMessage(){
        interactText.text = defaultText;
        interactMessage.SetActive(false);
    }

    public void ChangeDefaultMessage(string msg){
        interactText.text = msg;
        defaultText = msg;
    }

}
