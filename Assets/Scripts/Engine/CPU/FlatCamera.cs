using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlatCamera : MonoBehaviour
{
	[Header("Camera")]
	[Range(0f, 179f)]
	[SerializeField] float fovAngle;
	[SerializeField] Color backgroundColor;
	[SerializeField] float fogDensity;
	[SerializeField] Color fogColor;
	float farLimit;
	
	[Header("Ray Marching")]
	[SerializeField] int numRays;
	[SerializeField] int stepNumMax;
	[SerializeField] float stepLength = 1;
	
	[Header("Visualization")]
	[SerializeField] bool visualizeRayMarcher;
	[SerializeField] float pointRadius;
	[SerializeField] bool visualizeFog;
	
	public Texture2D targetTexture { get; private set; }
	List<Shape> allShapes;
	
	private void Awake() 
	{
		//Initialize screen texture, camera distance limit, and scene shapes list
		targetTexture = new Texture2D(numRays + 1, 1)
		{
			filterMode = FilterMode.Point
		};
		farLimit = stepNumMax*stepLength;
		allShapes = new List<Shape>();
	}
	
	private void Start() 
	{
		FindAllShapes();
	}
	
	public void Update()
	{
		float stepAngle = fovAngle*2f / numRays;
		float curAngle = -fovAngle;
		for (int i = 0; i <= numRays; i++)
		{
			//ray direction is calculated by finding how far right the vector's end point should move based on the angle (tan(angle/90)*farLimit) 
			Vector3 rayDirection = (farLimit*transform.forward + transform.right*Mathf.Rad2Deg*Mathf.Tan(Mathf.Deg2Rad*curAngle/90)*farLimit).normalized;
			for (int stepNum = 0; stepNum < stepNumMax; stepNum++)
			{
				//the point to check for shape intersections with
				Vector3 point = transform.position + rayDirection * stepNum * stepLength;
				
				//if point is in any shape, color the pixel corresponding to the current ray with that shape's color
				if(IsPointInShape(point, out Color shapeCol))
				{
					targetTexture.SetPixel(i, 0, Color.Lerp(shapeCol, fogColor, stepNum*fogDensity));
					break;
				}
				
				//if it's the last step and we haven't broken out yet, pixel should be baackground color
				if(stepNum == stepNumMax-1)
				{
					targetTexture.SetPixel(i, 0, backgroundColor);
				}
			}
			curAngle += stepAngle;
		}
		targetTexture.Apply();
	}
	
	public void FindAllShapes()
	{
		allShapes = FindObjectsOfType<Shape>().ToList();
	}
	
	/// <summary>
	/// Method for detecting if a given Vector3 is inside any shape in the scene, using SDFs
	/// </summary>
	/// <param name="point"> The point to check for shape intersections with </param>
	/// <param name="color"> The color of the shape intersected with, black if none </param>
	/// <returns> "true" if the "point" parameter was detected as inside any shape in the scene, "false" if otherwise</returns>
	
	private bool IsPointInShape(Vector3 point, out Color color)
	{
		foreach (var shape in allShapes)
		{
			if(SignedDistanceFunctions.GetShapeDistance(shape, point) < 0)
			{
				color = shape.color;
				return true;
			}
		}
		color = Color.black;
		return false;
	}
	private void OnDrawGizmos() 
	{
		farLimit = stepNumMax*stepLength;
		
		Gizmos.color = Color.white;
		Vector3 farLimitRight = transform.position + farLimit*transform.forward + transform.right*Mathf.Rad2Deg*Mathf.Tan(Mathf.Deg2Rad*fovAngle/90)*farLimit;
		Vector3 farLimitLeft = transform.position + farLimit*transform.forward - transform.right*Mathf.Rad2Deg*Mathf.Tan(Mathf.Deg2Rad*fovAngle/90)*farLimit;
		Gizmos.DrawLine(transform.position, farLimitRight);
		Gizmos.DrawLine(transform.position, farLimitLeft);
		Gizmos.DrawLine(farLimitRight, farLimitLeft);
		
		if(!visualizeRayMarcher) return;
		
		float stepAngle = fovAngle*2f / numRays;
		float curAngle = -fovAngle;
		
		for (int i = 0; i <= numRays; i++)
		{
			Vector3 rayDirection = (farLimit*transform.forward + transform.right*Mathf.Rad2Deg*Mathf.Tan(Mathf.Deg2Rad*curAngle/90)*farLimit).normalized;
			for (int stepNum = 0; stepNum < stepNumMax; stepNum++)
			{
				Vector3 point = transform.position + rayDirection * stepNum * stepLength;
				if(IsPointInShape(point, out Color shapeCol))
				{
					Gizmos.color = shapeCol;
				}
				else
				{
					if(visualizeFog)
					{
						Gizmos.color = Color.Lerp(Color.white, fogColor, stepNum*fogDensity);
					}
					else
					{
						Gizmos.color = backgroundColor;
					}
				}
				Gizmos.DrawSphere(point, pointRadius);
			}
			curAngle += stepAngle;
		}
	}
}
