using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Shape))]
public class ShapeCollider : MonoBehaviour
{
	SceneManager sceneManager;
	[SerializeField] UnityEvent<Shape> collision;
	
	private void Start() 
	{
		sceneManager = FindObjectOfType<SceneManager>();
	}
	
	void Update()
	{
		foreach (Shape shape in sceneManager.sceneShapes)
		{
			if(shape == GetComponent<Shape>()) continue;
			if(SignedDistanceFunctions.GetShapeDistance(shape, transform.position) <= 0)
			{
				collision.Invoke(shape);
				break;
			}
		}
	}
}
