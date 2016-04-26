using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Shaper.Drawing;
using Shaper.Calculations;
using System.Linq;

namespace Shaper.Explore
{
	//working class; something like laboratory to discover some behaviour
	[RequireComponent ( typeof(PicturesComparation) )]
	public class ShapesComparationGizmos : MonoBehaviour
	{
		public PicturesComparation objective;

		DrawnShape playerShape;
		DrawnTaskShape taskShape;

		private List<Vector3> taskIntersections = new List<Vector3> ( ), myIntersections = new List<Vector3> ( );

		Vector3 vTo = Vector3.zero, vFrom = Vector3.zero;

		[SerializeField]
		//just for debug. We are testing distance between points of shape's intersections
		float pairDistance = 0;
		 

		/// <summary>
		/// Connects two points, declared above. 'Radius-vector'
		/// </summary>
		Line inter;

		//again

		void OnEnable ()
		{
			/*/SetMe ( );*/
		}


		/// <summary>
		/// Sets observing components
		/// </summary>
		void SetMe ()
		{
			objective = GetComponent<PicturesComparation> ( );
		}

		void OnDrawGizmosSelected ()
		{
			//update
			playerShape = objective.Masterpiece;
			taskShape = objective.TaskShape;

			if ( !objective )
				return;



			if ( !playerShape || !objective || !taskShape )
				SetMe ( );
			

			if ( !playerShape || !objective || !taskShape )
				return;


			if ( !playerShape.lineRenderer && playerShape.shapeRadius > 0 )
				return;


			if ( taskShape.lineRenderer == null || taskShape.shapeRadius <= 0 )
				return;

			//finding centres, radius and so on
			vFrom = playerShape.centerPoint;                                    
			vTo = (playerShape.centerPoint * playerShape.shapeRadius);
			inter.origin = vFrom;
			inter.endPoint = vTo;
			inter.endPoint = BaseCalculations.RotateVector2D ( inter.endPoint, objective.rotation );



			RenderPlayerShapeSphere ( );


			#region Area of studing Intersections


			Line[] questLines = taskShape.GetLines ( );
			taskIntersections = new List<Vector3> ( );
			Line[] intersectable = questLines.ToList ( ).FindAll ( l => BaseCalculations.IntersectionBetweenTwoLines ( ref taskIntersections, inter.origin, inter.endPoint - inter.origin, l.origin, l.endPoint - l.origin, inter, l, taskShape.lineWidth ) ).ToArray ( );
			intersectable.ToList ( ).ForEach ( ible => {
				Gizmos.DrawLine ( ible.origin, ible.endPoint );
			} );

			taskIntersections.ForEach ( section => {
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere ( section, 0.18f );
			} );



			Line[] myLines = playerShape.GetLines ( );
			myIntersections = new List<Vector3> ( );
			Line[] myIntersectable = myLines.ToList ( ).FindAll ( ln => BaseCalculations.IntersectionBetweenTwoLines ( ref myIntersections, inter.origin, inter.endPoint - inter.origin, ln.origin, ln.endPoint - ln.origin, inter, ln, taskShape.lineWidth ) ).ToArray ( );
			myIntersectable.ToList ( ).ForEach ( ible => {
				Gizmos.DrawLine ( ible.origin, ible.endPoint );
			} );

			myIntersections.ForEach ( section => {
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere ( section, 0.18f );
			} );


			Gizmos.color = Color.green;
			//pair intersection points will give us a true deviation
			if ( taskIntersections.Count == myIntersections.Count ) {

				for ( int i = 0; i < taskIntersections.Count; i++ ) {

					Vector3 myFirstInter = myIntersections [i];
					Vector3 accordingInter = taskIntersections.OrderBy ( v => Vector3.Distance (
						                         v, myFirstInter ) ).First ( );
					pairDistance = Vector3.Distance ( myFirstInter, accordingInter );
					Gizmos.DrawLine ( myFirstInter, accordingInter );
				}
			}
			Gizmos.color = Color.blue;

			#endregion


			RenderTaskShapeSphere ( );
		}


		/// <summary>
		/// Just draws the lines.
		/// </summary>
		/// <param name="lines">Lines.</param>
		void DrawLines ( Line[] lines )
		{
			for ( int i = 0; i < lines.Length; i++ ) {

				if ( i % 2 == 0 )
					Gizmos.color = Color.blue;
				else
					Gizmos.color = Color.white;

				Gizmos.DrawLine ( lines [i].origin, 
					lines [i].endPoint );

			}
		}


		void RenderTaskShapeSphere ()
		{
			Gizmos.color = Color.magenta;


			Gizmos.DrawWireSphere ( taskShape.centerPoint, taskShape.shapeRadius );


			Gizmos.color = Color.red;

			Line[] taskLines = taskShape.GetLines ( );

			for ( int i = 0; i < taskLines.Length; i++ ) {

				Gizmos.DrawLine ( taskLines [i].origin, 
					taskLines [i].endPoint );
			}
		}


		void RenderPlayerShapeSphere ()
		{
			Gizmos.color = Color.cyan;

			Gizmos.DrawWireSphere ( playerShape.centerPoint, playerShape.shapeRadius );

			Gizmos.color = Color.yellow;

		

			Gizmos.DrawWireSphere ( inter.origin, 0.2f );
			Gizmos.DrawWireSphere ( inter.endPoint, 0.2f );
			Gizmos.DrawLine ( inter.origin, inter.endPoint );

			Gizmos.color = Color.blue;


			Line[] lines = playerShape.GetLines ( );
			DrawLines ( lines );
		}
	}
}