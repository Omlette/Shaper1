using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Shaper.Drawing;
using Shaper.Calculations;

namespace Shaper.Drawing
{
	public partial class DrawnShape : Drawable
	{

		#region Events and Delegates

		/// <summary>
		/// Occurs as soon as our shape has been drawn
		/// </summary>
		public event DrawingFinisedHandler OnShapeDrawn;
		public event DrawingHandler OnDrawingShape;
		public event DrawingHandler OnDrawingStarted;



		#endregion

		protected Vector3 defaultPosition = Vector3.zero;

		protected bool isMousePressed;
		protected Vector3 mousePos;

		public DrawnTaskShape questShape;


		Vector3 vFrom = Vector3.zero;
		Vector3 vTo = Vector3.zero;

		//[SerializeField]
		public Color drawingColor;

		//int


		public int rotation = 0;
		//    -----------------------------------

		void Awake ()
		{
			Initialize ( );
		}


		public override void AwakeFunc ()
		{//awake
		}


		protected override void Initialize ()
		{
			// Create line renderer component and set its property
			lineRenderer = gameObject.GetComponent<LineRenderer> ( );
			defaultPosition = transform.position;
			//lineRenderer.material = new Material ( Shader.Find ( "Particles/Additive" ) );
			lineRenderer.SetVertexCount ( 0 );
			lineRenderer.SetWidth ( 0.1f, 0.1f );
			SetColor ( drawingColor, drawingColor );
			lineRenderer.useWorldSpace = false;    
			isMousePressed = false;
			pointsList = new List<Vector3> ( );
			questShape = Manager.Instance.player.currentShape;


			//        renderer.material.SetTextureOffset(
		}

		public override void OnEnable ()
		{
			Initialize ( );
		}


		/// <summary>
		/// Drawing stuff goes here
		/// </summary>
		public override void UpdateFunc ()
		{ 
			if ( !questShape ) {
				Debug.LogError ( "questShape is empty" );
				return;
			}


			if ( questShape.enabledW == true )
				return;

			// If mouse button down, remove old line and set it's colour to green
			if ( Input.GetMouseButtonDown ( 0 ) ) {
				Clear ( );
				isMousePressed = true;


				if ( OnDrawingStarted != null )
					OnDrawingStarted ( );
			}
			if ( Input.GetMouseButtonUp ( 0 ) ) {

				if ( OnShapeDrawn != null )
					OnShapeDrawn ( this );

				isMousePressed = false;
			}


			// Drawing line when mouse is moving(presses)
			if ( isMousePressed ) {
				mousePos = Camera.main.ScreenToWorldPoint ( Input.mousePosition );
				mousePos.z = 0;
				if ( !pointsList.Contains ( mousePos ) ) {
					pointsList.Add ( mousePos );
					lineRenderer.SetVertexCount ( pointsList.Count );
					lineRenderer.SetPosition ( pointsList.Count - 1, (Vector3)pointsList [pointsList.Count - 1] );
				}

				if ( OnDrawingShape != null )
					OnDrawingShape ( );
			}

			UpdateBounds ( );

		}


		public override void LateUpdateFunc ()
		{
			return;
		}


		/// <summary>
		/// Unused approach
		/// Checks whether two lines collide
		/// </summary>
		/// <returns><c>true</c>, if lines collide was ared, <c>false</c> otherwise.</returns>
		protected virtual bool DoLinesCollide ()//unused nw
		{
			if ( pointsList.Count < 2 )
				return false;
		
			int totalLines = pointsList.Count - 1;
			Line[] lines = new Line[totalLines];

			if ( totalLines > 1 ) {
				
				for ( int i = 0; i < totalLines; i++ ) {
					lines [i].origin = (Vector3)pointsList [i];
					lines [i].endPoint = (Vector3)pointsList [i + 1];
				}
			}

			for ( int i = 0; i < totalLines - 1; i++ ) {
				
				Line currentLine;
				currentLine.origin = (Vector3)pointsList [pointsList.Count - 2];
				currentLine.endPoint = (Vector3)pointsList [pointsList.Count - 1];
				if ( BaseCalculations.AreLinesIntersect ( lines [i], currentLine ) )
					return true;
			}
			return false;
		}



		//!!!Just an old approach
		//finds vertexes
		//		private bool IsNewShapeVertex ()
		//		{
		//
		//			if ( pointsList.Count < 2 )
		//				return false;
		//
		//			int TotalLines = pointsList.Count - 1;
		//			Line[] lines = new Line[TotalLines];
		//			Line lastLine = new Line ( );
		//
		//
		//			if ( TotalLines > 1 ) {
		//				for ( int i = 0; i < TotalLines; i++ ) {
		//					lines [i].origin = (Vector3)pointsList [i];
		//					lines [i].endPoint = (Vector3)pointsList [i + 1];
		//				}
		//			}
		//
		//
		//			Vector3 one = (lines.Last ( ).endPoint - lines.Last ( ).origin).normalized;
		//			Vector3 two = (lastLine.endPoint - lastLine.origin).normalized;
		//			Vector2 res = two - one;
		//
		//
		//
		//
		//
		//
		//
		//			return false;
		//
		//
		//
		//			//return false;
		//		}

		public override void Clear ()
		{
			transform.localScale = new Vector3 ( 1, 1, 1 );
			transform.position = defaultPosition;

			lineRenderer.useWorldSpace = false;
			lineRenderer.SetVertexCount ( 0 );
			pointsList.RemoveRange ( 0, pointsList.Count );
			SetColor ( drawingColor, drawingColor );
		}

		/// <summary>
		/// Recieves an array of lines wich are contracted from our points list
		/// </summary>
		/// <returns>The lines.</returns>
		public virtual Line[] GetLines ()
		{
			int TotalLines = pointsList.Count - 1;
			if ( TotalLines < 2 )
				return new Line[]{ };


			Line[] lines = new Line[TotalLines];

			if ( TotalLines > 1 ) {
				for ( int i = 0; i < TotalLines; i++ ) {
					lines [i].origin = pointsList [i];
					lines [i].endPoint = pointsList [i + 1];
				}
			}



			return lines;

		}

		public virtual void SetColor ( Color col )
		{
			lineRenderer.SetColors ( col, col );
		}

		public virtual void SetColor ( Color colStart, Color colEnd )
		{
			lineRenderer.SetColors ( colStart, colEnd );
		}
	}
}