using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderFlatCameraOutput : MonoBehaviour
{
	[SerializeField] RawImage renderTexture;
	[SerializeField] FlatCamera flatCam;
	
	private void Start() 
	{
		renderTexture.texture = flatCam.targetTexture;
	}
}
