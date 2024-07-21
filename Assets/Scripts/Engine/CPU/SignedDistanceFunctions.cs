using UnityEngine;
public class SignedDistanceFunctions
{
	public static float SdfCircle(Vector3 point, Vector3 center, float radius)
	{
		return Vector3.Distance(point, center) - radius;
	}
	
	public static float SdfBox(Vector3 point, Vector3 center, float width, float height)
	{
		float dx = Mathf.Abs(point.x - center.x) - (width / 2);
		float dz = Mathf.Abs(point.z - center.z) - (height / 2);

		float maxDist = Mathf.Max(dx, dz);
		return maxDist;
	}
	
	public static float SdfEquilateralTriangle(Vector3 point, Vector3 center, float radius)
	{
		float k = Mathf.Sqrt(3f);  // Square root of 3

		// Transform the point to local coordinates centered around the equilateral triangle
		Vector3 local = point - center;  // Translate point to local coordinates relative to center
		local.x = Mathf.Abs(local.x) - radius;  // Shift x-coordinate by radius
		local.z = local.z + radius / k;  // Shift z-coordinate by radius/k

		// Check if the local point is above the equilateral triangle's top boundary
		if (local.x + k * local.z > 0f)
		{
			// Calculate the closest point on the equilateral triangle's boundary
			local = new Vector3(local.x - k * local.z, 0, -k * local.x - local.z) / 2f;
		}

		// Clamp the x-coordinate within the range of the equilateral triangle's base
		local.x -= Mathf.Clamp(local.x, -2f * radius, 0f);

		// Compute the signed distance to the equilateral triangle
		return -local.magnitude * Mathf.Sign(local.z);
	}
	
	public static float SdfPentagon(Vector3 point, Vector3 center, in float r )
	{
		Vector3 kConst = new Vector3(0.809016994f,0.587785252f,0.726542528f);
		Vector3 local = point - center;
		
		local.x = Mathf.Abs(local.x);
		local -= 2f*Mathf.Min(Vector3.Dot(new Vector3(-kConst.x, 0,kConst.y),local),0f)* new Vector3(-kConst.x,0,kConst.y);
		local -= 2f*Mathf.Min(Vector3.Dot(new Vector3( kConst.x, 0,kConst.y),local),0f)*new Vector3( kConst.x, 0, kConst.y);
		local -= new Vector3(Mathf.Clamp(local.x,-r*kConst.z,r*kConst.z),r);    
		return local.magnitude*Mathf.Sign(local.y);
	}
	
	/// <summary>
	/// Method for finding the signed distance of a point from a shape, using the SDF corresponding to the shape's "type" property
	/// </summary>
	/// <param name="shape"> The shape to check distance from the point with </param>
	/// <param name="point"> The point to check distance from the shape with </param>
	/// <returns></returns>
	public static float GetShapeDistance(Shape shape, Vector3 point)
	{
		switch (shape.type)
		{
			case ShapeType.Circle:
				return SignedDistanceFunctions.SdfCircle(point, shape.transform.position, shape.scale.x);
			case ShapeType.Box:
				return SignedDistanceFunctions.SdfBox(point, shape.transform.position, shape.scale.x, shape.scale.y);
			case ShapeType.EquilateralTriangle:
				return SignedDistanceFunctions.SdfEquilateralTriangle(point, shape.transform.position, shape.scale.x);
		}
		return 0;
	}

}