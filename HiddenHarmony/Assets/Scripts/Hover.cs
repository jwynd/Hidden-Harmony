using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField][Range(0.0f, 1.0f)] private float bobDegree = 0.1f;
    [SerializeField][Range(0.0f, 5.0f)] private float bobSpeed = 3.0f;
    [SerializeField] private bool randomizeSpeed = false;
    [Tooltip("This field is only used if the above is checked")]
    [SerializeField][Range(0.0f, 1.0f)] private float speedRange = 0.5f;
    [SerializeField] private bool randomizeSinPosition = false;
    [Tooltip("This field is only used if the above is checked")]
    [SerializeField][Range(0.0f, 3.14f)] private float sinOffsetRange = 1.0f;
    [SerializeField] private bool startPositionCenter = false;

    private float x;
    private float y;
    private float z;
    private float speedOffset = 0.0f;
    private float sinOffset = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        x = this.transform.localPosition.x;
        y = this.transform.localPosition.y;
        z = this.transform.localPosition.z;
        y = startPositionCenter?y:y+bobDegree;
        if(randomizeSpeed){
            speedOffset = Random.Range(-speedRange, speedRange);
        }
        if(randomizeSinPosition){
            sinOffset = Random.Range(-sinOffsetRange, sinOffsetRange);
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = new Vector3(x, y + bobDegree*Mathf.Sin(Time.time*(bobSpeed+speedOffset)+sinOffset), z);
    }
}
