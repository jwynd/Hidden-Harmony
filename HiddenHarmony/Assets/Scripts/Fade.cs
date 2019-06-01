using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Fade : MonoBehaviour
{
    [Tooltip("This changes the defauld fade color, it can be overwriten in a function")]
    [SerializeField] private Color color = Color.black;

    private const float fadeTime = 1.0f;

    private Image image;
    private RectTransform rt;
    private bool fadingIn = false;
    private bool fadingOut = false;
    private float interpolationVal;
    private float fTime;
    private bool fadeSet;

    /*void OnValidate(){
        if(this.transform.parent.gameObject != GameObject.Find("GameplayObjects/Canvas")){
            Debug.LogError("Fade.cs must be placed on a child of Canvas: Current Parent "+this.transform.parent.gameObject.name);
        }
    }*/
    // Start is called before the first frame update
    void Start(){
        fTime = fadeTime;
        image = GetComponent<Image>();
        image.color = new Color(color.r, color.g, color.b, 0.0f);
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.I)) FadeIn();
        if(Input.GetKeyDown(KeyCode.O)) FadeOut();
        if(fadingOut){
            interpolationVal += Time.deltaTime/fTime;
            image.color = new Color(color.r, color.g, color.b, Mathf.Lerp(0.0f, 1.0f, Mathf.Clamp(interpolationVal, 0.0f, 1.0f)));
            if(image.color.a >= 0.99f) fadingOut = false;
        } else if(fadingIn){
            interpolationVal += Time.deltaTime/fTime;
            image.color = new Color(color.r, color.g, color.b, Mathf.Lerp(1.0f, 0.0f, Mathf.Clamp(interpolationVal, 0.0f, 1.0f)));
            if(image.color.a <= 0.01f) fadingIn = false;
        } else if(!fadeSet) {
            image.color = new Color(color.r, color.g, color.b, 0.0f);
        }
        if(image.color.a == 0.0f){
            this.transform.SetAsFirstSibling();
        } else {
            this.transform.SetAsLastSibling();
        }
        //if(IsFading()) Debug.Log(interpolationVal);
    }

    public void SetFade(float alpha){
        image.color = new Color(color.r, color.g, color.b, alpha);
        fadeSet = true;
    }

    public void UnsetFade(){
        fadeSet = false;
    }

    public void FadeOut(float f = fadeTime){
        if(!IsFading()){ 
            fadingOut = true;
            interpolationVal = 0.0f;
            fTime = f;
        }
    }

    public void FadeIn(float f = fadeTime){
        if(!IsFading()){
            fadingIn = true;
            interpolationVal = 0.0f;
            fTime = f;
        }
    }

    public bool IsFading(){
        return fadingIn || fadingOut;
    }
}
