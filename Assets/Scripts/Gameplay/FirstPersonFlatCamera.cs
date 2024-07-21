using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonFlatCamera : MonoBehaviour
{
	[SerializeField] float sensitivity;
	float rot;
	
	private void Start() 
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	
	void Update()
	{
		rot += sensitivity * Input.GetAxisRaw("Mouse X");
		
		transform.rotation = Quaternion.AngleAxis(rot, Vector3.up); 
	}
}
