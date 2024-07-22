using UnityEditor;
using UnityEngine;

public enum ShapeType { Circle, Box, EquilateralTriangle }
public class Shape : MonoBehaviour
{
	[field: SerializeField] public ShapeType type { get; private set; }
	[field: SerializeField] public Color color { get; private set; }
	[Tooltip("For circle and equilateral triangle shapes, the x value is read as the desired radius. For box type shapes, both x and y values are used as the desired box dimensions")]
	[field: SerializeField] public Vector2 scale { get; private set; }

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Handles.color = color;
		Gizmos.color = color;
		switch (type)
		{
			case ShapeType.Circle:
				Handles.DrawSolidDisc(transform.position, new Vector3(0, 1, 0), scale.x);
				break;
			case ShapeType.Box:
				Vector3 point1 = transform.position + transform.forward * scale.y / 2 + transform.right * scale.x / 2;
				Vector3 point2 = transform.position - transform.forward * scale.y / 2 + transform.right * scale.x / 2;
				Vector3 point3 = transform.position - transform.forward * scale.y / 2 - transform.right * scale.x / 2;
				Vector3 point4 = transform.position + transform.forward * scale.y / 2 - transform.right * scale.x / 2;
				Handles.DrawAAConvexPolygon(point1, point2, point3, point4);
				break;
			case ShapeType.EquilateralTriangle:
				point1 = transform.position + transform.forward * scale.x;
				point2 = transform.position - transform.forward * scale.x / 2 + transform.right * 0.86602f * scale.x;
				point3 = transform.position - transform.forward * scale.x / 2 - transform.right * 0.86602f * scale.x;
				Handles.DrawAAConvexPolygon(point1, point2, point3);
				break;
		}
	}
#endif
}
