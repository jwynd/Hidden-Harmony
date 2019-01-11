using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffControl : MonoBehaviour
{
    // keyword this refers to the object this script is attatched to, which should only be the staff

    // public variables, editable in the inspector
    public float speed = 1.14f; // controls the speed of the player object
    public Material noteMaterial; // controls what the note looks like;
    public float noteScale = 2.5f; // controls the size of the notes when being placed
    public bool useSpaces = false; // if true, notes are placed in the middle of the spaces, not on the lines
    public AudioClip[] sounds = new AudioClip[6]; // array to hold the audio clips that will be used

    //  Cursor variables
//    public float playerPositionZ;
    public GameObject cursor1;
    public GameObject cursor2;
    public GameObject cursor3;

    // private variables, accessible only in script
    private GameObject player; // references the object controlled by the player
    private GameObject[] Notes = new GameObject[8]; // these will keep references to the note objects
    private int currentAnimal = 0; // holds the index of the currently playing animal
    private bool[] isInCol = new bool[] {false, false, false, false, false, false, false, false}; // this keeps track of the collumns which already have notes
    private float[,] prevNotePosition = new float[2, 8]; // this will hold the x positions of previous notes so that they can be replayed later
    private bool[] wasUsed = new bool[] {false, false, false, false, false, false, false, false};
    private bool isComplete = false; // this will become true when the player is placing new sounds for the second pass
    private AudioSource[] aS;        // Audio Source reference, used to play, pause, and manage the audio

    // these hold the indeces of sounnds with locations of current top middle and bottom
    private int top = 0;
    private int middle = 1;
    private int bottom = 2;

    // below is a set of private constant variables to make script modification easier
    private const float vertMove = 6.666666666666f; // constant variable to control player motion
    private const float staffEdge = 18.0f;          // location of the edge of the staff
    private float[] colEdge = new float[] {-13.5f, -9.0f, -4.5f, 0.0f, 4.5f, 9.0f, 13.5f, 18.0f}; // location where notes can be placed
    private float[] noteOffset = new float[] {0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f};// these offset the position of the notes to account for paralax
    private float xscale; // holds the scale on the x axis so that it will remain in proportion regarless of how it is stretched

    // Start is called before the first frame update
    void Start()
    {
        player = this.transform.Find("Player").gameObject;
        if(player == null) NullChild("Player");

        aS = gameObject.GetComponents<AudioSource>();

        xscale = this.transform.localScale.x;

        for(int i = 0; i < colEdge.Length; i++){
            // move position to the notes if spaces are used
            if(useSpaces){
                colEdge[i] = colEdge[i] - 2.25f;
            }
            // adjust position by current scale of the staff
            colEdge[i] = colEdge[i] * xscale;
            // print(colEdge[i]);
        }
        // adjust the speed by the local scale
        speed = speed * xscale;
    }

    // Update is called once per frame
    void Update()
    {
      // constant motion is handled below, notice it is multiplies by speed
       player.transform.localPosition += Vector3.right * speed * Time.deltaTime;
        
        // this is the right-left world wrap
        if(player.transform.localPosition.x > staffEdge) {
            player.transform.localPosition = new Vector3(-staffEdge, player.transform.localPosition.y, player.transform.localPosition.z);
        }
        // This allows the player to move up
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            player.transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z + vertMove);
        }
        // this allow the palyer to move down
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            player.transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, player.transform.localPosition.z - vertMove);
        }

        // this section is the world wrap from top to bottom
        if(player.transform.localPosition.z > vertMove){
            player.transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, -vertMove);
        }

        // this section is the world wrap from bottom to top
        if(player.transform.localPosition.z < -vertMove){
            player.transform.localPosition = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y, vertMove);
        }
        // This should create a not on the screen, I hope
        if(Input.GetKeyDown(KeyCode.Space)){
            // calculate the Position and place note if necessary in collumns 2 through 8
            for(int i = 1; i < 8; i++){
                if(player.transform.localPosition.x*xscale < colEdge[i] && player.transform.localPosition.x*xscale > colEdge[i-1] && !isInCol[i]){
                    createNote(i);
                }
                else if(player.transform.localPosition.x*xscale < colEdge[i] && player.transform.localPosition.x*xscale > colEdge[i-1] && isInCol[i]){
                    if(player.transform.localPosition.z != Notes[i].transform.localPosition.z){
                        Notes[i].transform.localPosition = new Vector3((colEdge[i] + noteOffset[i])/xscale, player.transform.localPosition.y, player.transform.localPosition.z);
                    }
                    else{
                        Destroy(Notes[i]);
                        isInCol[i] = false;
                    }

                }
            }
            
            // calculate the position to place the note and place one if necessary 1st collumn
            if(player.transform.localPosition.x*xscale < colEdge[0] && !isInCol[0]){
                createNote(0);
            }
            else if(player.transform.localPosition.x*xscale < colEdge[0] && isInCol[0]){
                if(player.transform.localPosition.z != Notes[0].transform.localPosition.z){
                    Notes[0].transform.localPosition = new Vector3((colEdge[0] + noteOffset[0])/xscale, player.transform.localPosition.y, player.transform.localPosition.z);
                }
                else{
                    Destroy(Notes[0]);
                    isInCol[0] = false;
                }
            }

        }
        // place audio clip play sounds
        for(int i = 0; i < 8; i++){
            if(player.transform.localPosition.x*xscale > colEdge[i]+noteOffset[i]-0.02f && player.transform.localPosition.x*xscale < colEdge[i]+noteOffset[i]+0.02f && isInCol[i]){
                if(Notes[i].transform.localPosition.z > 1.0f){
                    addAndPlay(sounds[top], 0);
                }
                else if(Notes[i].transform.localPosition.z < -1.0f){
                    addAndPlay(sounds[bottom], 0);
                }
                else{
                    addAndPlay(sounds[middle], 0);
                }
            }
        }

        // Debug.Log(player.transform.position.z);
        // changes cursor to the appropriate sprite
        if(player.transform.localPosition.z > 0) {
            cursor1.GetComponent<Renderer>().enabled = true;
            cursor2.GetComponent<Renderer>().enabled = false;
            cursor3.GetComponent<Renderer>().enabled = false;
        }
        else if (player.transform.localPosition.z < 0) {
            cursor1.GetComponent<Renderer>().enabled = false;
            cursor2.GetComponent<Renderer>().enabled = false;
            cursor3.GetComponent<Renderer>().enabled = true;
        }
        else {
            cursor1.GetComponent<Renderer>().enabled = false;
            cursor2.GetComponent<Renderer>().enabled = true;
            cursor3.GetComponent<Renderer>().enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && !isComplete){
            nextAnimal();
        }

        if(isComplete){
            playBackground();
        }
    }

    // helper function to create notes.
    // takes argument index (i)
    private void createNote(int i){
        Notes[i] = GameObject.CreatePrimitive(PrimitiveType.Cube); // add a note to the array 
        Notes[i].transform.parent = this.transform;
        Notes[i].transform.localPosition = new Vector3((colEdge[i] + noteOffset[i])/xscale, player.transform.localPosition.y, player.transform.localPosition.z);
        Notes[i].transform.localScale = new Vector3(noteScale, noteScale, noteScale); // adjust x position for angle of view
        isInCol[i] = true;
        Notes[i].transform.Rotate(Vector3.up * 180.0f); // rotate the cube upside down
        Notes[i].GetComponent<Renderer>().material = noteMaterial; // apply the material
    }

    private void nextAnimal(){
        // stuff in above if
        for(int i = 0; i < Notes.Length; i++){
            if(isInCol[i]){
                prevNotePosition[currentAnimal, i] = Notes[i].transform.localPosition.z;
            }
            wasUsed[i] = isInCol[i];
            isInCol[i] = false;
            Destroy(Notes[i]);
        }
        top += 3;
        middle += 3;
        bottom += 3;
        isComplete = true;
    }
/*
    private void prevAnimal(){
        // inverse of above function
    }*/

    public void playBackground(){

        for(int i = 0; i < 8; i++){
            if(player.transform.localPosition.x*xscale > colEdge[i]+noteOffset[i]-0.02f && player.transform.localPosition.x*xscale < colEdge[i]+noteOffset[i]+0.02f && wasUsed[i]){
                if(prevNotePosition[Abs(currentAnimal-1), i] > 1.0f){
                    addAndPlay(sounds[0], 1);
                }
                else if(prevNotePosition[Abs(currentAnimal-1), i] < -1.0f){
                    addAndPlay(sounds[2], 1);
                }
                else{
                    addAndPlay(sounds[1], 1);
                }
            }
        }
    }

    public void addAndPlay(AudioClip inputClip, int index){
        aS[index].clip = inputClip;
        if(aS[index].clip != null){
            aS[index].Play();
        }
        else {
            print("no audio clip provided");
        }
    }

    void NullChild(string str){
        throw new System.ArgumentException("Child not found", str);
    }

    // returns the absolute value of the integer x
    private int Abs(int x){
        if(x < 0) return x*-1;
        else return x;
    }
}
