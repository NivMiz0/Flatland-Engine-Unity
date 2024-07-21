using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlatCamera))]
public class FlatCameraInspector : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		FlatCamera cam = (FlatCamera)target;
		if(GUILayout.Button("Refresh Shape List"))
		{
			cam.FindAllShapes();
		}
	}
}
