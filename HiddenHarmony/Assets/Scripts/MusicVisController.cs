using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVisController : MonoBehaviour
{
    [SerializeField] private Material low;
    [SerializeField] private Material mid;
    [SerializeField] private Material hi;
    [SerializeField] private float spectrumScalar = 10;

    private float[] buckets = new float[3];
    [SerializeField] private int[] cutoffs = new int[3];

    void Start()
    {
        for(int i = 0; i<buckets.Length; i++){
            buckets[i] = 0;
        }
    }

    void Update()
    {
    
        for(int i = 0; i<buckets.Length; i++){
            // reset the bucket
            buckets[i] = 0;

            // go through this bucket's section of cutoff data
            //  and sum it
            for(int j = i==0?0:cutoffs[i-1]; j<cutoffs[i]; j++){
                buckets[i] += W_AudioPeer.spectrumData[j];
            }

            // make sure it is in range of 0-1
            Mathf.Clamp(buckets[i], 0, 1);
        }

        // use the informatin from the buckets to lerp material colors
        // lerp low
        Color lerpCol = Color.Lerp(Color.black, Color.red, buckets[0]);
        low.SetColor("_Color", lerpCol);

        // lerp mid
        lerpCol = Color.Lerp(Color.black, Color.green, buckets[1]);
        mid.SetColor("_Color", lerpCol);

        // lerp hi
        lerpCol = Color.Lerp(Color.black, Color.blue, buckets[2]);
        hi.SetColor("_Color", lerpCol);
    }
}
