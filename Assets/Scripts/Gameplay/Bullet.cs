using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[HideInInspector]
	public Vector3 direction;
	Vector3 target;
	[SerializeField] private float speed;
	[SerializeField] private float smoothness;
	
	[HideInInspector]
	public SceneManager sceneManager;
	
	void Update()
	{
		target = transform.position + direction * speed;
		transform.position = Vector3.Lerp(transform.position, target, smoothness * Time.deltaTime);
	}
	
	public void Collide(Shape s)
	{
		if(s.GetComponent<Destructible>() != null)
		{
			sceneManager.DestroyShape(s.gameObject);
		}
		sceneManager.DestroyShape(gameObject);
	}
}
