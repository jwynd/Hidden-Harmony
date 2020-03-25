using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateVertical : MonoBehaviour
{
    // Rotates along a horizontal axis, making the object appear to rotate vertically
    public float speed = 70f;


    void Update()
    {
        transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
}
