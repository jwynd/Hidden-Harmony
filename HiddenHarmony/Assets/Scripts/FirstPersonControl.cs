/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonControl : MonoBehaviour
{
    Vector2 mouseLook;
    Vector2 SmoothC;
    public float movement = 3.0f;
    public float smooth = 3.0f;

    GameObject character;

    void Start()
    {
        //Points to parent of this (capsule) 
        character = this.transform.parent.gameObject;
    }
    void Update()
    {
        var m = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        m = Vector2.Scale(m, new Vector2(movement*smooth, movement*smooth));
        SmoothC.x = Mathf.Lerp(SmoothC.x, m.x, 1f / smooth);
        SmoothC.y = Mathf.Lerp(SmoothC.y, m.y, 1f / smooth);
        mouseLook += SmoothC;
        
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
    }
}
*/
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
    Vector2 mouseLook;
    Vector2 SmoothC;
    public float minVertical = -45.0f; //Minimum angle of vertical movement
    public float maxVertical = 45.0f; //Maximum angle of vertical movement

    [Range(0.01f, 1.0f)] public float sensHorizontal = 10.0f;
    [Range(0.01f, 1.0f)] public float sensVertical = 10.0f;
    public float smooth = 2.3f;

    public float rotationX = 0;

    // Update is called once per frame
    void Update () {
        /*
        if(Input.GetKeyDown(KeyCode.Return)) {
            isActive = !isActive;
        }       
        */
        var m = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        m = Vector2.Scale(m, new Vector2(sensHorizontal * smooth, sensVertical * smooth));
        SmoothC.x = Mathf.Lerp(SmoothC.x, m.x, 1f / smooth);
        SmoothC.y = Mathf.Lerp(SmoothC.y, m.y, 1f / smooth);
       // mouseLook += SmoothC;

        if (axes == RotationAxis.MouseX && isActive) {
            transform.Rotate (0, SmoothC.x, 0);
        } else if (axes == RotationAxis.MouseY && isActive) {
            rotationX -= SmoothC.y;
            rotationX = Mathf.Clamp (rotationX, minVertical, maxVertical); //Clamps the vertical angle within the min and max limits (45 degrees)

            float rotationY = transform.localEulerAngles.y;

    		transform.localEulerAngles = new Vector3 (rotationX, rotationY, 0);
        }
    }
}
