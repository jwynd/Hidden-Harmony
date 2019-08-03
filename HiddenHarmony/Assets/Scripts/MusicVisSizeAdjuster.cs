using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVisSizeAdjuster : MonoBehaviour
{
    
    [SerializeField]
    private int bucket = 0;
    [SerializeField]
    private Vector3 axisControl = new Vector3(1,1,1);

    // reference to object's original scale
    private Vector3 oScale;

    // Start is called before the first frame update
    void Start()
    {
        oScale = transform.localScale;
    }

    // use LateUpdate so that mv_Buckets has already been calculated
    void LateUpdate()
    {
        transform.localScale = oScale + axisControl*MusicVisController.mv_Buckets[bucket];
    }

    // return objects to original size after running
    void OnApplicationQuit()
    {
        transform.localScale = oScale;
    }
}
