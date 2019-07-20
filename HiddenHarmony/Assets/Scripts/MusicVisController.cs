using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVisController : MonoBehaviour
{
    [Header("Materials and Cutoffs")]
    [SerializeField] private Material[] mvMaterials = new Material[3];
    [SerializeField] private int[] cutoffs = new int[3];
    [Header("Information Provided by Other Code (here for testing)")]
    // # of sound objects in play per area (hb = 0, sw = 1, bd = 2, oc = 3)
    [SerializeField] private int[] objsPerArea = new int[4];
    [Header("Area Colors")]
    [SerializeField] private Color[] hbColors = new Color[3];
    [SerializeField] private Color[] swColors = new Color[3];
    [SerializeField] private Color[] bdColors = new Color[3];
    [SerializeField] private Color[] ocColors = new Color[3];
    private float[] buckets;
    private Color[] activeColors = new Color[3];

    // throw and error if the number of cutoffs and materials are not equal
    void OnValidate(){
        if(cutoffs.Length != mvMaterials.Length){
            Debug.LogError("The number of cutoffs and MV_Materials must be equal.");
        }
    }

    void Start()
    {
        // assign an array to buckets
        buckets = new float[cutoffs.Length];
        for(int i = 0; i<buckets.Length; i++){
            buckets[i] = 0;
        }

        // default to Hub Colors
        // clone vs CopyTo?  ????
        activeColors = (Color[])hbColors.Clone();
    }

    void Update()
    {
        // FOR TESTING -- WILL REPLACE LATER
        if(Input.GetKeyDown("m")){
            Debug.Log("Checking colors ...");
            UpdateActiveColors();
        }

        RunVisualizer();
        
    }

    // MUSIC VISUALIZER FUNCTIONS
    // RunVisualizer -- general update, runs every frame.
    //  reads audio spectrum data, computes the amount of 
    //  noise at each set of frequencies, and updates the 
    //  material's color intensity based on the noise intensity
    void RunVisualizer()
    {
        // buckets organized from low to hifreq
        for(int i = 0; i<buckets.Length; i++){
            // reset the bucket
            buckets[i] = 0;

            // go through this bucket's section of cutoff data
            //  and sum it
            for(int j = i==0?0:cutoffs[i-1]; j<cutoffs[i]; j++){
                buckets[i] += (W_AudioPeer.spectrumData[j]);
            }

            // make sure it is in range of 0-1
            Mathf.Clamp(buckets[i], 0, 1);
        }

        for(int i = 0; i < mvMaterials.Length; i++){
            Color lerpCol = Color.Lerp(Color.black, activeColors[i], buckets[i]);
            mvMaterials[i].SetColor("_Color", lerpCol);
        }
    }

    // UpdateActiveColors -- Compares the # of sound objs from each
    //  area to determine which colors to display.
    void UpdateActiveColors()
    {

        // SET UP
        // get sum -- is there no default Sum() method???
        int sumObjs = 0;
        for(int i = 0; i < objsPerArea.Length; i++){
            sumObjs += objsPerArea[i];
        }

        // if no objects are in play, set Hub Colors and return
        if(sumObjs <= 0){
            activeColors = hbColors;
            Debug.Log("No Colors");
            return;
        }

        // TAG SORT
        // create an array of tags for Tag Sorting
        int[] tag = new int[4];
        for(int i = 0; i < 4; i++){
            tag[i] = i;
        }
        
        // Tag Sort the sound objects
        for(int i = 0; i < objsPerArea.Length; i++){
            for(int j = i+1; j < objsPerArea.Length; j++){
                if (objsPerArea[tag[i]] < objsPerArea[tag[j]]){
                    int temp = tag[i];
                    tag[i] = tag[j];
                    tag[j] = temp;
                }
            }
        }

        // make color decisions based on which areas are in play and are dominant
        // dominant objects gets lower frequencies
        int[] colorTags = new int[] {0,0,0};

        // check to see if only objs from one area are in play
        if(objsPerArea[tag[0]] == sumObjs){
            colorTags[0] = tag[0];
            colorTags[1] = tag[0];
            colorTags[2] = tag[0];

            Debug.Log("only one dominant");
            SetColors(colorTags);
            return;
        }

        // check to see if only two are dominant
        // check to see if only objs from one area are in play
        if(objsPerArea[tag[0]] + objsPerArea[tag[1]] == sumObjs){
            colorTags[0] = tag[0];
            colorTags[1] = tag[0];
            colorTags[2] = tag[1];

            Debug.Log("two dominant");
            SetColors(colorTags);
            return;
        }

        // else, display top 3 colors
        colorTags[0] = tag[0];
        colorTags[1] = tag[1];
        colorTags[2] = tag[2];
        Debug.Log("two dominant");
        SetColors(colorTags);
    }

    // sets the colors in active colors based on tags
    // 0 = hub, 1 = subwoofer, 2 = belldeer, 3 = orcastra
    // if i could figure out...how to put...shit in a list or 2D array...
    // .......i will not have this problem
    void SetColors(int[] colorTags)
    {
        for(int i = 0; i<colorTags.Length; i++){
            if(colorTags[i]==0){
                activeColors[i] = hbColors[i];
            }
            else if (colorTags[i]==1){
                activeColors[i] = swColors[i];
            }
            else if (colorTags[i]==2){
                activeColors[i] = bdColors[i];
            }
            else {
                activeColors[i] = ocColors[i];
            }
        }
    }
}
