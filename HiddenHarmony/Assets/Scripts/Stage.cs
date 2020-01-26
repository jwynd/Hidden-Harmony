using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public int[] halfBeats;
    public int[] pitches;
    [HideInInspector] public GameObject SoundObject = null;
    [SerializeField][Tooltip("Places an offset on when the crystal lights up. Must be between 0 and the number of notes - 1")]
    private int crystalOffset;
    [HideInInspector] public GameObject[] crystals;
    [HideInInspector] public Vector3[] crystalScales;
    [Tooltip("The outline material")]
    [SerializeField]private Material outline;
    [Tooltip("The not outlined material")]
    [SerializeField]private Material noOutline;
    [Tooltip("Game object that will be outlined")]
    [SerializeField]private GameObject toOutline;
    private Renderer rend;
    

    void Start(){
        if(halfBeats.Length != pitches.Length) throw new System.ArgumentException("Beats and pitches must be of equal length");
        foreach(float halfBeat in halfBeats){
            if (halfBeat <= 0) throw new System.ArgumentException("All beats must be greater than zero");
        }
        int[] temp = new int[pitches.Length];
        for(int i = 0; i < pitches.Length; i++){
            temp[i] = pitches[i];
        }
        for(int i = 0; i < pitches.Length; i++){
            if(i == pitches.Length-1){pitches[i]=temp[0];}
            else{pitches[i] = temp[i+1];}
        }
        /*foreach(int pitch in pitches){
            if(pitch > 7 || pitch < 0) throw new System.ArgumentException("All pitches must be between 0 and 7 inclusive");
        }*/
        int index;
        crystals = new GameObject[halfBeats.Length];
        crystalScales = new Vector3[crystals.Length];
        for(int i = 0; i < halfBeats.Length; ++i){
            index = i + crystalOffset;
            index = index % halfBeats.Length;
            crystals[index] = this.transform.parent.GetChild(0).GetChild(0).GetChild(i).gameObject;
            crystalScales[index] = new Vector3(crystals[index].transform.localScale.x, crystals[index].transform.localScale.y, crystals[index].transform.localScale.z);
        }
        rend = toOutline.GetComponent<Renderer>();
    }

    public bool IsOccupied(){
        return this.transform.childCount == 0;
    }
    public void Outline(bool o){
        if(o){
            rend.material = outline;
        } else {
            rend.material = noOutline;
        }
    }
}
