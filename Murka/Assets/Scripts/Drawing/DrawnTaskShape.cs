using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Shaper.Drawing;

namespace Shaper.Drawing
{
	public partial class DrawnTaskShape : Drawable
	{
		#region Delegates and Events

		//

		#endregion


		#region Variables and Properties

		// Reference to LineRenderer
		protected Vector3 mousePos;
		protected Vector3 startPos;
		// Start position of line
		protected Vector3 endPos;
		// End position of line


		/// <summary>
		/// An importaint variable to track a number of sides (lines) 
		/// </summary>
		protected int sides = 0;

		public bool enabledW = false;


		public string shapeTitle = "";

		/// <summary>
		/// Points award for correct redrawing
		/// </summary>
		public short pointsAward = 1;

		#endregion




		public override void AwakeFunc ()
		{
			Initialize ( );
		}

		public override void OnEnable ()
		{
			Initialize ( );
		}

		protected override void Initialize ()
		{
			lineRenderer = GetComponent<LineRenderer> ( );

			if ( enabledW ) {

				lineRenderer.SetColors ( Color.white, Color.white );

				lineRenderer.SetVertexCount ( 0 );
				pointsList = new List<Vector3> ( );
				sides = 0;
				pointsList.Add ( Vector3.zero );
				lineRenderer.SetWidth ( lineWidth, lineWidth );
				lineRenderer.useWorldSpace = true;
			}
		}

		public override void UpdateFunc ()
		{			
			if ( Input.GetKeyDown ( KeyCode.Q ) ) {
				if ( !enabledW )
					enabledW = true;
			}

			if ( EventSystem.current.IsPointerOverGameObject ( ) )
				return;

			if ( !enabledW )
				return;


			if ( !lineRenderer )
				return;

			// On mouse down new line will be created
			if ( Input.GetMouseButtonDown ( 0 ) ) {
			
				mousePos = Camera.main.ScreenToWorldPoint ( Input.mousePosition );
				mousePos.z = 0;
				//pointsList.Add ( mousePos );


				sides += 2;
				lineRenderer.SetVertexCount ( sides );

				lineRenderer.SetPosition ( (sides - 1) - 1, mousePos );

				try {
					pointsList [(sides - 1) - 1] = mousePos;
				} catch {
					pointsList.Add ( Vector3.zero );
					pointsList [(sides - 1) - 1] = mousePos;

				}

				startPos = mousePos;


			} else if ( Input.GetMouseButtonUp ( 0 ) ) {

				if ( lineRenderer ) {
				
					mousePos = Camera.main.ScreenToWorldPoint ( Input.mousePosition );
					mousePos.z = 0;

					lineRenderer.SetPosition ( sides - 1, mousePos );
					try {
						pointsList [sides - 1] = mousePos;
					} catch {
						pointsList.Add ( Vector3.zero );
						pointsList [sides - 1] = mousePos;

					}

					endPos = mousePos;


					UpdateBounds ( );
				}


			} else if ( Input.GetMouseButton ( 0 ) ) {
			
				mousePos = Camera.main.ScreenToWorldPoint ( Input.mousePosition );
				mousePos.z = 0;


				lineRenderer.SetPosition ( sides - 1, mousePos );
				try {
					pointsList [sides - 1] = mousePos;
				} catch {
					pointsList.Add ( Vector3.zero );
					pointsList [sides - 1] = mousePos;

				}
			} else if ( Input.GetMouseButtonDown ( 1 ) ) {
				Clear ( );
			}

		}

		public void LateUpdate ()
		{
			UpdateBounds ( );
		}

		public override void LateUpdateFunc ()
		{
			//stuff
		}


		/// <summary>
		/// Entirely cleares out our picture
		/// </summary>
		public override void Clear ()
		{
			if ( !lineRenderer )
				return;
			
			sides = 0;
			lineRenderer.SetVertexCount ( 0 );
			pointsList.Clear ( );
		}


		public void DisallowPoint ()
		{


		}


		/// <summary>
		/// Actually gets the points.
		/// </summary>
		/// <returns>The points.</returns>
		public virtual List<Vector3> GetPoints ()
		{
			List<Vector3> coords = new List<Vector3> ( pointsList );


			for ( int i = 0; i < coords.Count; i++ ) {

				float dx = pointsList [i].x + transform.position.x;
				float dy = pointsList [i].y + transform.position.y;

				Vector3 actualCoords = new Vector3 ( dx, dy, 0 );

				coords [i] = actualCoords;
			}
			
			return coords;
		}

		public virtual Line[] GetLines ()
		{

			int TotalLines = GetPoints ( ).Count - 1;
			if ( TotalLines < 2 )
				return new Line[]{ };

			Line[] lines = new Line[TotalLines];

			if ( TotalLines > 1 ) {
				for ( int i = 0; i < TotalLines; i++ ) {
					lines [i].origin = GetPoints ( ) [i];
					lines [i].endPoint = GetPoints ( ) [i + 1];
				}

				return lines;
			}

			return new Line []{ };
		}

		/// <summary>
		/// Creates a brand new blank task shape
		/// </summary>
		public static DrawnTaskShape CreateNewOne ( string shapeTitle = ShapesSetting.DEFAULT_TITLE, bool readyDrawing = false )
		{
			GameObject shapeObject = new GameObject ( shapeTitle );
			DrawnTaskShape shapeComponent;

			//LineRenderer is automatically added from reqComp :)
			shapeObject.AddComponent<DrawnTaskShape> ( );
			shapeComponent = shapeObject.GetComponent<DrawnTaskShape> ( );
			shapeComponent.shapeTitle = shapeTitle;

			//lets init it's default values
			shapeComponent.enabledW = true;
			shapeComponent.Initialize ( );
			shapeComponent.lineWidth = ShapesSetting.DEFAULT_TASK_SHAPE_LINE_THICKNESS;

			return shapeComponent;
		}
	}
}