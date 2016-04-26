using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Shaper
{
	public abstract class ShapeBase : MonoBehaviour
	{

		/// <summary>
		/// The associated game object.
		/// </summary>
		protected LineRenderer associatedShapeObject;
		public float lineWidth = 0.1f;
		public float shapeRadius = 0;


		/// <summary>
		/// The center point of shape.
		/// </summary>
		public Vector3 centerPoint = Vector3.zero;


		/// <summary>
		/// A collection of vetex points wich is our shape is based on.
		/// </summary>
		public List<Vector3> pointsList;



		public LineRenderer GetPicture ()
		{
			return associatedShapeObject ?? null;
		}
	}
}