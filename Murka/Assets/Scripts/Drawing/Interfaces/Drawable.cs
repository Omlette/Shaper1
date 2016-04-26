using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Shaper.Drawing
{
	[RequireComponent ( typeof(LineRenderer) )]
	/// <summary>
	/// A very base class for all possible ways shapes that can be drawn [by LineRederer]
	/// </summary>
	public abstract class Drawable : MonoBehaviour
	{
		#region Delegates and Events

		public delegate void DrawingHandler ();

		public delegate void DrawingFinisedHandler ( DrawnShape drawnShape );


		#endregion


		#region Variables and Properties



		/// <summary>
		/// The associated game object.
		/// </summary>
		public LineRenderer lineRenderer;
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


		#endregion

		#region Methods

		public abstract void AwakeFunc ();

		//public abstract void Start ();

		public abstract void UpdateFunc ();

		//public abstract void FixedUpdate ();

		public abstract void LateUpdateFunc ();

		//independent func
		public abstract void OnEnable ();


		protected abstract void Initialize ();



		protected virtual void UpdateBounds ()
		{
			if ( !lineRenderer || pointsList.Count <= 0 )
				return;

			centerPoint = lineRenderer.bounds.center;
			shapeRadius = lineRenderer.bounds.extents.magnitude;
		}

		public abstract void Clear ();

		#endregion
	}
}

