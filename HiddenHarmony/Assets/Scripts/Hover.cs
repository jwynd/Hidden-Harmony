using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField][Range(0.0f, 10.0f)] private float bobDegree = 1.0f;
    [SerializeField][Range(0.0f, 10.0f)] private float bobSpeed = 1.0f;
    [SerializeField] private bool startPositionCenter = false;

    private float x;
    private float y;
    private float z;
    // Start is called before the first frame update
    void Start()
    {
        x = this.transform.localPosition.x;
        y = this.transform.localPosition.y;
        z = this.transform.localPosition.z;
        y = startPositionCenter?y:y+bobDegree;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = new Vector3(x, y + bobDegree*Mathf.Sin(Time.time*bobSpeed), z);
    }
}
