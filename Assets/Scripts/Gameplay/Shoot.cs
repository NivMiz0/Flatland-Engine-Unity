using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shoot : MonoBehaviour
{
	[SerializeField] SceneManager sceneManager;
	[SerializeField] GameObject bullet;
	[SerializeField] Transform shotOrigin;
		 
	// Update is called once per frame
	void Update()
	{
		if(Input.GetButtonDown("Fire1"))
		{
			Bullet b = sceneManager.CreateShape(bullet, shotOrigin.position, Quaternion.identity).GetComponent<Bullet>();
			b.sceneManager = sceneManager;
			b.direction = transform.forward;
		}
	}
}
