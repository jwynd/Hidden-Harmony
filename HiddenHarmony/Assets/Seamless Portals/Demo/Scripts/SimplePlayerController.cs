using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour {

	public float MovementSpeed;
	public float MouseSensitivity;
	public Rigidbody rb;
	public GameObject cam;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Escape))
			Cursor.lockState = CursorLockMode.None;
		else
			Cursor.lockState = CursorLockMode.Locked;

		if (Cursor.lockState == CursorLockMode.Locked)
			cam.transform.eulerAngles += new Vector3(-MouseSensitivity * Input.GetAxis("Mouse Y"), MouseSensitivity * Input.GetAxis("Mouse X"), 0.0f);

		Vector3 forward = cam.transform.forward;
		forward.y = 0;
		forward = forward.normalized;

		Vector3 right = cam.transform.right;
		right.y = 0;
		right = right.normalized;

		if (Input.GetKey(KeyCode.W))
			rb.MovePosition(transform.position + forward * MovementSpeed);
		else if (Input.GetKey(KeyCode.S))
			rb.MovePosition(transform.position - forward * MovementSpeed);

		if (Input.GetKey(KeyCode.A))
			rb.MovePosition(transform.position - right * MovementSpeed);
		else if (Input.GetKey(KeyCode.D))
			rb.MovePosition(transform.position + right * MovementSpeed);
	}
	
	public void PortalCameraCorrect(Transform target)
	{
		transform.position = target.position;
		cam.transform.rotation = target.rotation;
	}
}
