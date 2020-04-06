using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTransitions : MonoBehaviour
{
    [Tooltip("This is the distance at which fog starts to fade in")]
    public float effectiveRange = 1.0f;
    [Range(0.0f, 0.5f)]
    public float maximumDensity = 0.07f;
    public Color fogColor = Color.red;
    private GameObject player;
    private BoxCollider bc;
    private bool inTrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        bc = this.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!inTrigger){
            float dist = Vector3.Distance(bc.ClosestPoint(player.transform.position), player.transform.position);
            if(dist < effectiveRange){
                RenderSettings.fog = true;
                RenderSettings.fogMode = FogMode.ExponentialSquared;
                RenderSettings.fogColor = fogColor;
                RenderSettings.fogDensity = Mathf.Lerp(0.0f, maximumDensity, (effectiveRange-dist)/effectiveRange);
            } else {
                RenderSettings.fog = false;
            }
        }
        //Debug.Log("Distance from plane to player is " + PointToPlaneDistance());
    }

    private void OnTriggerEnter (Collider other){
        if(other.gameObject == player){
            inTrigger = true;
        }
    }

    private void OnTriggerExit (Collider other){
        if(other.gameObject == player){
            inTrigger = false;
        }
    }

    // http://paulbourke.net/geometry/pointlineplane/
    // private float PointToPlaneDistance()
    // {
    //     float A = this.transform.forward.x;
    //     float B = this.transform.forward.y;
    //     float C = this.transform.forward.z;
    //     float D;
    //     float X = player.transform.position.x;
    //     float Y = player.transform.position.y;
    //     float Z = player.transform.position.z;

    //     // calculate D
    //     D = (A*this.transform.position.x + B*this.transform.position.y + C*this.transform.position.z)*-1;

    //     float tempVal = (A*X + B*Y + C*Z + D)/(Mathf.Sqrt(A*A + B*B + C*C));

    //     // return absolute value of tempval
    //     if(tempVal < 0){
    //       return tempVal*-1;
    //     } else {
    //       return tempVal;
    //     }
    // }
}
