using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

[System.Serializable]
public class TextBox : MonoBehaviour{

    public float currentBlock;

    private Transform player;
    private Transform textObject;
    private TextMesh textMesh;
    private GameObject nextIndicator;
    [SerializeField] private string displayedText;
    private RaycastHit hit;
    private Ray pickRay;
    public List<string> textBlocks;
    private Match matchHitObj;
    private string matchText;
    private PauseMenu pauseMenu;

    // Start is called before the first frame update
    void Start(){
        if(textBlocks == null){
            textBlocks = new List<string>();
        }
        textObject = this.transform.Find("Text");
        textObject.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Text");
        textMesh = textObject.GetComponent<TextMesh>();
        nextIndicator = this.transform.Find("Next Text Indicator Sprite").gameObject;
        player = GameObject.Find("Player").transform;
        matchText = "Dialogue Prompt";
        pauseMenu = GameObject.Find("PauseMenuController").GetComponent<PauseMenu>();

        currentBlock = 0;
        if(textBlocks.Count < 1){
            AddText("(Block " + (textBlocks.Count + 1) + ")\nEnter Text Here:");
        }
        textMesh.text = textBlocks[0];
    }

    // Update is called once per frame
    void Update(){
        if(this.gameObject.activeSelf && Input.GetMouseButtonDown(0) && !pauseMenu.GetPaused()){
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
                this.transform.parent.transform.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            this.transform.gameObject.SetActive(false);
        }
        textMesh.text = textBlocks[(int)currentBlock];
        displayedText = textMesh.text;
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

}
