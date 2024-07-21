using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
	public List<Shape> sceneShapes = new List<Shape>();

	void Start()
	{
		sceneShapes = FindObjectsOfType<Shape>().ToList();
	}

	//Any script that wants to create a new shape in the scene should have a reference to SceneManager and call this script
	public GameObject CreateShape(GameObject obj, Vector3 position, Quaternion rotation)
	{
		GameObject instance = Instantiate(obj, position, rotation);
		sceneShapes.Add(instance.GetComponent<Shape>());
		return instance;
	}
	
	public void DestroyShape(GameObject obj)
	{
		sceneShapes.Remove(obj.GetComponent<Shape>());
		Destroy(obj);
	}
}
