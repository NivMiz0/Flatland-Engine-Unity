using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatPlayerMovement : MonoBehaviour
{
	[SerializeField] float speed;
	[SerializeField] FirstPersonFlatCamera cam;
	
	Vector3 direction;
	
	void Update()
	{
		transform.eulerAngles = cam.transform.eulerAngles;
		cam.transform.position = transform.position;
		
		direction.x = Input.GetAxisRaw("Horizontal");
		direction.z = Input.GetAxisRaw("Vertical");
	}
	
	void FixedUpdate()
	{
		transform.Translate(direction.normalized*speed);
	}
	
	//Push the player out of the shape it's collided with
	public void PushPlayerOut()
	{
		transform.Translate(-direction.normalized*speed);
	}
}
