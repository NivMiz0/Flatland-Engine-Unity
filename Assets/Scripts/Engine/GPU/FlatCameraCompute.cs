using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class FlatCameraCompute : MonoBehaviour
{
	[Header("Canvas")]
	[SerializeField] ComputeShader computeShader;
	[Tooltip("The height of the display canvas. Display remains 1D even if the height is increased.")]
	[SerializeField] int height;
	[SerializeField] Color emptyCanvasColor;
	RenderTexture renderTexture;

	[Header("Camera")]
	[Range(0, 180f)]
	
	[SerializeField] float fovAngle;
	[SerializeField] Color backgroundColor;
	[SerializeField] float fogDensity;
	[SerializeField] Color fogColor;

	[Header("Ray Marching")]

	[Tooltip("The desired number of rays to cast from the camera - meaning the desired horizontal tresolution of the screen. Should be a multiple of 128. ")]
	[SerializeField] int numRays;
	[SerializeField] float maxSteps;

	[Header("World")]
	[SerializeField] bool allowNonStaticShapes;

	Shape[] allShapes;
	
	void Start()
	{
		allShapes = FindObjectsOfType<Shape>();

		RenderPipelineManager.endContextRendering += OnEndContextRendering;
	}

	void OnEndContextRendering(ScriptableRenderContext context, List<Camera> cameras)
	{
		// Initializiation - should run only once, on the first frame
		if (renderTexture == null)
		{
			Init();
		}

		//Pass to the gpu every randering frame
		computeShader.SetFloat("RenderedHeight", height);
		computeShader.SetVector("POS", transform.position);
		computeShader.SetVector("FWD", transform.forward);
		computeShader.SetVector("RIT", transform.right);

		if (allowNonStaticShapes) SendShapesBuffer();

		//Run the compute shader
		computeShader.Dispatch(0, renderTexture.width / 128, renderTexture.height / 8, 1);

		//Blit the result from the compute shader to the screen
		Graphics.Blit(renderTexture, cameras[0].activeTexture);
	}

	//Initialize the stuff that I only need to pass to the gpu once
	void Init()
	{
		renderTexture = new RenderTexture(numRays, numRays, 24)
		{
			filterMode = FilterMode.Point,
			enableRandomWrite = true
		};
		renderTexture.Create();
		computeShader.SetTexture(0, "Result", renderTexture);

		computeShader.SetFloat("Width", renderTexture.width);
		computeShader.SetFloat("Height", renderTexture.height);
		computeShader.SetVector("EmptyCanvasColor", emptyCanvasColor);

		computeShader.SetFloat("FOVAngle", fovAngle);

		computeShader.SetVector("BackgroundColor", backgroundColor);
		computeShader.SetVector("FogColor", fogColor);
		computeShader.SetFloat("FogDensity", fogDensity);

		computeShader.SetFloat("MaxSteps", maxSteps);

		SendShapesBuffer();
	}

	//Sends the shapes buffer to the gpu
	void SendShapesBuffer()
	{
		allShapes = FindObjectsOfType<Shape>();
		ShapeData[] allShapesData = new ShapeData[allShapes.Length];
		for (int i = 0; i < allShapes.Length; i++)
		{
			Shape curShape = allShapes[i];
			allShapesData[i] = new ShapeData()
			{
				position = curShape.transform.position,
				type = (int)curShape.type,
				scale = curShape.scale,
				color = curShape.color
			};
		}
		ComputeBuffer shapesBuffer = new ComputeBuffer(allShapesData.Length, ShapeData.Size());
		shapesBuffer.SetData(allShapesData);
		computeShader.SetBuffer(0, "SceneShapes", shapesBuffer);
		computeShader.SetFloat("NumSceneShapes", allShapesData.Length);
	}
	void OnDestroy()
	{
		RenderPipelineManager.endContextRendering -= OnEndContextRendering;
	}

	public struct ShapeData
	{
		public Vector3 position;
		public int type;
		//for objects with a radius - scale.x is the radius. for objects with a width and height - scale.x and scale.y
		public Vector2 scale;
		public Color color; //color is a vector4

		//Size of the struct in bytes
		public static int Size()
		{
			return sizeof(float) * 9 + sizeof(int) * 1;
		}
	}
}
