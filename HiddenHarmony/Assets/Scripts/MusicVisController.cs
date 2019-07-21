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
    private Color[] activeColors;
    private List<Color[]> areaColors = new List<Color[]>();
    private Count count;

    // throw and error if the number of cutoffs and materials are not equal
    void OnValidate(){
        if(cutoffs.Length != mvMaterials.Length){
            Debug.LogError("The number of cutoffs and Mv Materials must be equal.");
        }
    }

    void Start()
    {
        // get a reference to count
        count = GameObject.Find("GameplayObjects/Count").GetComponent<Count>();

        // assign an array to buckets
        buckets = new float[cutoffs.Length];
        for(int i = 0; i<buckets.Length; i++){
            buckets[i] = 0;
        }

        // default to Hub Colors
        //activeColors = (Color[])hbColors.Clone();
        activeColors = new Color[3]{Color.gray, Color.gray, Color.gray};

        // put the area color arrays into the list
        areaColors.Add(hbColors);
        areaColors.Add(swColors);
        areaColors.Add(bdColors);
        areaColors.Add(ocColors);

    }

    void Update()
    {
        // FOR TESTING -- WILL REPLACE LATER
        if(Input.GetKeyDown("m")){
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
            activeColors[i].a = buckets[i];
            mvMaterials[i].SetColor("_Color", activeColors[i]);
        }
    }

    // UpdateActiveColors -- Compares the # of sound objs from each
    //  area to determine which colors to display.
    void UpdateActiveColors()
    {
        UpdateObjsPerArea();
        
        // SET UP
        // get sum -- is there no default Sum() method???
        int sumObjs = 0;
        for(int i = 0; i < objsPerArea.Length; i++){
            sumObjs += objsPerArea[i];
        }

        // if no objects are in play, set Hub Colors and return
        if(sumObjs <= 0){
            activeColors = new Color[3]{Color.gray, Color.gray, Color.gray};
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

        
        // DETERMINE COLORS
        // 1.
        // check to see if only objs from one area are in play
        if(objsPerArea[tag[0]] == sumObjs){
            //activeColors = areaColors[tag[0]];
            activeColors = (Color[])(areaColors[tag[0]]).Clone();
            return;
        }

        // 2.
        // check to see if only two are dominant
        if(objsPerArea[tag[0]] + objsPerArea[tag[1]] == sumObjs){
            activeColors[0] = (areaColors[tag[0]])[0];
            activeColors[1] = (areaColors[tag[0]])[1];
            activeColors[2] = (areaColors[tag[1]])[0];
            return;
        }
        
        // 3.
        // else, display top 3 colors
        activeColors[0] = (areaColors[tag[0]])[0];
        activeColors[1] = (areaColors[tag[1]])[0];
        activeColors[2] = (areaColors[tag[2]])[0];
    }


    void UpdateObjsPerArea()
    {
        objsPerArea[0] = count.ActiveHub();
        objsPerArea[1] = count.ActiveDen();
        objsPerArea[2] = count.ActiveForest();
        objsPerArea[3] = count.ActiveCavern();
    }


}
