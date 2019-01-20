using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonControl : MonoBehaviour {
    // Based on code from Stephen Barr

    public enum RotationAxis{
        MouseX = 1,
        MouseY = 2
    }

    public bool isActive = true;

    public RotationAxis axes = RotationAxis.MouseX;

    public float minVertical = -45.0f; //Minimum angle of vertical movement
    public float maxVertical = 45.0f; //Maximum angle of vertical movement

    public float sensHorizontal = 10.0f;
    public float sensVertical = 10.0f;

    public float rotationX = 0;

    // Update is called once per frame
    void Update () {
        /*
        if(Input.GetKeyDown(KeyCode.Return)) {
            isActive = !isActive;
        }
        */

        if (axes == RotationAxis.MouseX && isActive) {
            transform.Rotate (0, Input.GetAxis ("Mouse X") * sensHorizontal, 0);
        } else if (axes == RotationAxis.MouseY && isActive) {
            rotationX -= Input.GetAxis ("Mouse Y") * sensVertical;
            rotationX = Mathf.Clamp (rotationX, minVertical, maxVertical); //Clamps the vertical angle within the min and max limits (45 degrees)

            float rotationY = transform.localEulerAngles.y;

    		transform.localEulerAngles = new Vector3 (rotationX, rotationY, 0);
        }
    }
}
