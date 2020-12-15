﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class TextBoxUI : MonoBehaviour{

    public float currentBlock;

    private Transform player;
    private Transform textObject;
    private TextMeshProUGUI textMesh;
    private GameObject nextIndicator;
    [SerializeField] private string displayedText;
    private RaycastHit hit;
    private Ray pickRay;
    public List<string> textBlocks;
    private Match matchHitObj;
    private string matchText;
    private PauseMenu pauseMenu;

    private float wrapWidth = 4.2f;
    private TextSize ts;
    private Image sprite;

    [Tooltip("While true, clicking anywhere will progress text (usually keep true for gameplay and false for cutscenes, etc.)")]
    public bool progressByClick = true;
    [Tooltip("Plays function at specified index")]
    public bool playFunction = false;
    [Tooltip("Plays function after text has ended")]
    public bool playFunctionAtEnd = false;
    [Tooltip("Must be between 0 and the index of the last dialogue box")]
    public float functionAtIndex = 0;
    public UnityEvent functionPlayed;
    public UnityEvent functionPlayedAtEnd;

    // Start is called before the first frame update
    void Awake(){
        if(textBlocks == null){
            textBlocks = new List<string>();
        }
        textObject = this.transform.Find("Text");
        //textObject.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Text");
        textMesh = textObject.GetComponent<TextMeshProUGUI>();
        nextIndicator = this.transform.Find("Next Text Indicator Sprite").gameObject;
        player = GameObject.Find("Player").transform;
        matchText = "Dialogue Prompt";
        pauseMenu = GameObject.Find("PauseMenuController").GetComponent<PauseMenu>();

        // Text wrapping initialization
        //ts = new TextSize(textMesh);
        sprite = transform.GetChild(0).GetComponent<Image>();
        //wrapWidth = sprite.bounds.size.x  - (sprite.bounds.size.x * 0.15f);

        currentBlock = 0;
        if(textBlocks.Count < 1){
            AddText("(Block " + (textBlocks.Count + 1) + ")\nEnter Text Here:");
        }
        textMesh.text = textBlocks[0];

        //ts.FitToWidth(wrapWidth); // Wrap text
    }

    // Update is called once per frame
    void Update(){
        if(this.gameObject.activeSelf && Input.GetMouseButtonDown(0) && !pauseMenu.GetPaused() && progressByClick){
            CycleText();
        }
    }

    public void CycleText(){
        currentBlock++;
        if(currentBlock == textBlocks.Count - 1){
            nextIndicator.SetActive(false);
        }
        if(currentBlock >= textBlocks.Count){
            currentBlock = 0;
            nextIndicator.SetActive(true);

            matchHitObj = Regex.Match(this.transform.parent.name, matchText);
            if (matchHitObj.Success){
                this.transform.parent.transform.gameObject.GetComponent<Image>().enabled = true;
            }

            if(playFunctionAtEnd){
                if(functionPlayedAtEnd == null){
                    print("functionPlayedAtEnd is empty");
                }
                else{
                    functionPlayedAtEnd.Invoke();
                }
            }

            this.transform.gameObject.SetActive(false);
        }
        if(playFunction && currentBlock == functionAtIndex){
            if(functionPlayed == null){
                print("functionPlayed is empty");
            }
            else{
                functionPlayed.Invoke();
            }
        }


        textMesh.text = textBlocks[(int)currentBlock];
        displayedText = textMesh.text;
        // Text Wrapping
        //ts.FitToWidth(wrapWidth);
    }

    public void AddText(string insertText){
        textBlocks.Add(insertText);
    }

    public void RemoveText(){
        if(textBlocks.Count > 1){
            textBlocks.RemoveAt(textBlocks.Count - 1);
        }
    }

    public void ActivateTextBox(){
        this.gameObject.SetActive(true);
    }

    public void ClickActive(){
        this.progressByClick = !this.progressByClick;
    }

    public void ClickActive(bool canClick){
        this.progressByClick = canClick;
    }

    public void ClearText(){
        while(textBlocks.Count > 1){
            this.RemoveText();
        }
    }

    public void EditTextAtIndex(string text, int index){
        textBlocks[index] = text;
    }

    public void EditDisplayText(string text){
        textMesh.text = text;
        displayedText = textMesh.text;
    }

    public void EditFirstText(string text){
        EditTextAtIndex(text, 0);
        EditDisplayText(text);
    }

}