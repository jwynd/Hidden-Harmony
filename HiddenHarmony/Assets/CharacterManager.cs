using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour {

    public Transform[] characterSprites;
    public Transform stagePosition;
    public Transform[] performPositions;
    public int stageScale = 3;
    public int performScale = 1;

    private int totalCharacters = 2;
    private int currentCharacter = -1;

	void Start () {
        foreach(Transform character in characterSprites){
            character.gameObject.SetActive(false);
        }
        NextCharacter();
	}
	
    public void NextCharacter(){
        if (currentCharacter >= characterSprites.Length) //resets currentCharacter to the first character if the Length of the array is reached
        {
        	currentCharacter = -1;
        }
        if(currentCharacter >= 0){
            characterSprites[currentCharacter].localScale = Vector3.one * performScale;
            characterSprites[currentCharacter].position = performPositions[currentCharacter].position;
        }
        ++currentCharacter;
        if (currentCharacter < characterSprites.Length){
            characterSprites[currentCharacter].gameObject.SetActive(true);
            characterSprites[currentCharacter].localScale = Vector3.one * stageScale;
            characterSprites[currentCharacter].position = stagePosition.position;
        }
    }

    public void PrevCharacter(){
        if (currentCharacter < -1) //resets currentCharacter to the first character if the Length of the array is reached
        {
        	currentCharacter = characterSprites.Length;
        }
        if(currentCharacter <= characterSprites.Length){
            characterSprites[currentCharacter].localScale = Vector3.one * performScale;
            characterSprites[currentCharacter].position = performPositions[currentCharacter].position;
        }
        --currentCharacter;
        if (currentCharacter < characterSprites.Length){
            characterSprites[currentCharacter].gameObject.SetActive(true);
            characterSprites[currentCharacter].localScale = Vector3.one * stageScale;
            characterSprites[currentCharacter].position = stagePosition.position;
        }
    }

    private void Update(){
    	//Old Code if(Input.GetButtonDown("Submit") && currentCharacter < totalCharacters){
    	if (Input.GetKeyDown(KeyCode.RightArrow)){
            NextCharacter();
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow)){
        	PrevCharacter();
        }

    }
}
