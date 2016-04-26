using UnityEngine;
using System.Collections;
using Shaper.Drawing;
using Shaper.Calculations;
using System.Collections.Generic;
using System.Linq;

namespace Shaper
{
	public class PicturesComparation : MonoBehaviour
	{

		#region Delegates and Events

		public delegate void  ComparationDecisionMade ( bool decision );


		/// <summary>
		/// Occurs as soon as our decision about shapes similarity is made!
		/// </summary>
		public event ComparationDecisionMade OnComparationDecisionMade;
		//observer one love :3

		#endregion


		#region Fields

		/// <summary>
		/// Associated player
		/// </summary>
		private Player player;

		/// <summary>
		/// An initial shape, drawn by our player
		/// </summary>
		private DrawnShape masterpiece;


		/// <summary>
		/// Is adjusted at runtime further
		/// </summary>
		private DrawnTaskShape taskShape;


		/// <summary>
		/// A rotation value of radius-vector.
		/// Having overlayed two shapes, we use this for further comparation decision
		/// </summary>
		public float rotation = 0;

		/// <summary>
		/// Our start point of radius-vector (center of circle)
		/// </summary>
		Vector3 vFrom = Vector3.zero;

		/// <summary>
		/// Destination for our radius-vector
		/// </summary>
		Vector3 vTo = Vector3.zero;

		//Vector2 questScreenPosition = Vector2.zero;

		/// <summary>
		/// Some criteria for controlling a proximity of comparation
		/// </summary>
		public int checkingProximity = 10;


		/// <summary>
		/// Two collections that will contain an intersection information (points )between radius-vector and shapes
		/// </summary>
		private List<Vector3> taskIntersections = new List<Vector3> ( ), myIntersections = new List<Vector3> ( );


		//additional comaring criterias
		public int comparationStep = 10;

		//a minimal value, needed to consider our shapes similar
		public int levityFactor = 8;

		/// <summary>
		/// A maximal COUNT of possible difference in points set (number) on each radius-vector state (rotation)
		/// </summary>
		private int maxPointsCountDifference;

		/// <summary>
		/// How many inequalities in points set have we got finally
		/// </summary>
		int pointsDifferenceHappened = 0;

		/// <summary>
		/// Maximal deviation value of our masterpiece's details we can allow to be different:) 
		/// </summary>
		private float maxDeviationValue;

		/// <summary>
		/// The max deviations count.
		/// </summary>
		private int maxDeviationsCount;

		/// <summary>
		/// A number of this significant deviations occured in our case 
		/// </summary>
		int greatDeviations = 0;


		[SerializeField]
		/// <summary>
		/// A flag that tracks a moment to start corparation (shapes are overlayed)
		/// </summary>
		/// <value>The masterpiece.</value>
		private bool comparationMade;

		//outer stuff goes here

		public DrawnShape Masterpiece{ get { return masterpiece; } }

		public DrawnTaskShape TaskShape{ get { return taskShape; } set { taskShape = value; } }

		public List<Vector3> TaskIntersections { get { return taskIntersections; } }

		public List<Vector3> MyIntersections { get { return myIntersections; } }

		public Vector3 VTo { get { return vTo; } }

		public Vector3 VFrom { get { return vFrom; } }



		#endregion



		IEnumerator Start ()
		{
			OnComparationDecisionMade += NotifyComparationDecisionMade;

			//just give a second for some loadinh details.
			//test version > will be definately removed
			yield return new WaitForSeconds ( 1 );

			player = GameManager.Instance.player;
			Drawable playerDrawable = player.GetDrawable ( );

			if ( !(playerDrawable is DrawnShape) )
				yield return null;

			((DrawnShape)playerDrawable).OnShapeDrawn += HandleFinishingDrawing;
			((DrawnShape)playerDrawable).OnDrawingStarted += Reset;
		}

		/// <summary>
		/// Invokes as soon we finished with drawing our shape. Time to start handling our picture
		/// </summary>
		void HandleFinishingDrawing ( DrawnShape shapeOutput )
		{
			taskShape = GameManager.Instance.player.currentShape;

			masterpiece = shapeOutput;

			//			List<Vector3> apppedLines = new List<Vector3> ( );
			//			int lastlyAdded = 0;
			//
			//			int angles = 0;
			//
			//			for ( int i = 0; i < pointsList.Count; i++ ) {
			//
			//				if ( i % 10 == 0 ) {
			//					apppedLines.Add ( pointsList [i] - pointsList [lastlyAdded] );
			//
			//					lastlyAdded = i;
			//				}
			//			}
			//			
			//			for ( int i = 0, j = 1; i < apppedLines.Count && j < apppedLines.Count; i++, j++ ) {
			//
			//				if ( Mathf.Abs ( Vector3.Dot ( apppedLines [i].normalized, apppedLines [j].normalized ) ) > 0.1f ) {
			//					angles++;
			//				}
			//			}
			//			

			DeriveScales ( );

		}


		void Update ()
		{
			if ( masterpiece )
				HandleFinishingDrawing ( masterpiece );

			if ( !masterpiece || !taskShape || comparationMade )
				return;

			//We cannot do this by subscribing DrawnShape.OnShapeDrawn as we need to perfom translating
			if ( masterpiece.centerPoint == taskShape.centerPoint ) {
				comparationMade = true;
				FinalComparation ( );
			}
		}


		/// <summary>
		/// Now, we match our shape with task's one so that two our picture's sizes becomes identical.
		/// We lay our picture onto another one in order to continue comparation.
		/// </summary>
		void DeriveScales ()
		{
			if ( taskShape == null )
				return;

			if ( !taskShape )
				return;


			if ( masterpiece.shapeRadius > 0 ) {
				//Debug.Log( "ga " + questShape.GetComponent<LineRenderer>().bounds.size.magnitude / line.bounds.size.magnitude );
				//line.useWorldSpace = false;


				//let's calculate needed compression so that it feets task's one
				float xCompression = taskShape.GetComponent<LineRenderer> ( ).bounds.size.x / masterpiece.lineRenderer.bounds.size.x;
				float yCompression = taskShape.GetComponent<LineRenderer> ( ).bounds.size.y / masterpiece.lineRenderer.bounds.size.y;
				//transform.localScale = new Vector3 ( xCompression, yCompression, 1 );
				//see?


				//apply scaling having taken into account a position offset
				for ( int i = 0; i < masterpiece.pointsList.Count; i++ ) {

					Vector3 compression = new Vector3 ( masterpiece.pointsList [i].x * xCompression,
						                      masterpiece.pointsList [i].y * yCompression, 0 );


					masterpiece.pointsList [i] = compression;
					masterpiece.lineRenderer.SetPosition ( i, compression );
				}


				//apply position to entirely overlay two shapes
				for ( int i = 0; i < masterpiece.pointsList.Count; i++ ) {

					Vector3 newPosition = taskShape.centerPoint + (masterpiece.pointsList [i] - masterpiece.centerPoint);

					masterpiece.lineRenderer.SetPosition ( i, newPosition );
					masterpiece.pointsList [i] = newPosition;
				}
			}

			//Debug.Log( "final: " + line.bounds.size );
		}

	
		/// <summary>
		/// Let's start rotation our radius-vector to gether some divergences among our shapes.
		/// </summary>
		/// <returns><c>true</c>, if comparation was finaled, <c>false</c> otherwise.</returns>
		public bool FinalComparation ()
		{
			//if ( vFrom == Vector3.zero || vTo == Vector3.zero )
			//return false; //not adjusted yet
			float similarity = 0;

			//init some calculating variables before we start to liken shapes
			maxDeviationsCount = 2; //fo every 3 lines we allow one great deviation
			maxPointsCountDifference = (masterpiece.pointsList.Count / masterpiece.GetLines ( ).Count ( )) * comparationStep * 2;
			maxDeviationValue = taskShape.shapeRadius / 8;


			vFrom = masterpiece.centerPoint;                                    
			vTo = (masterpiece.centerPoint * masterpiece.shapeRadius);
			Line inter;
			inter.origin = vFrom;
			inter.endPoint = vTo;

			Line[] questLines = taskShape.GetLines ( );
			Line[] lines = masterpiece.GetLines ( );

			//checkingProximity = Mathf.Clamp ( checkingProximity, 5, 36 );

			//'launch' a radius-vector to start finding differences between two overlayed shapes
			for ( rotation = 0; rotation < 360; rotation += comparationStep ) {


				inter.endPoint = BaseCalculations.RotateVector2D ( inter.endPoint, rotation );//actual rotating


				//get all lines which are intersected by our 'radius-vector'
				taskIntersections = new List<Vector3> ( );
				Line[] intersectable = questLines.ToList ( ).FindAll ( l => BaseCalculations.IntersectionBetweenTwoLines ( ref taskIntersections, inter.origin, inter.endPoint - inter.origin, l.origin, l.endPoint - l.origin, inter, l, taskShape.lineWidth ) ).ToArray ( );


				myIntersections = new List<Vector3> ( );
				Line[] myIntersectable = lines.ToList ( ).FindAll ( ln => BaseCalculations.IntersectionBetweenTwoLines ( ref myIntersections, inter.origin, inter.endPoint - inter.origin, ln.origin, ln.endPoint - ln.origin, inter, ln, taskShape.lineWidth ) ).ToArray ( );


				similarity += CountSimilarity ( );

				Debug.Log ( pointsDifferenceHappened );

				//take into account a few things:
				if ( pointsDifferenceHappened >= maxPointsCountDifference ) {
					similarity = -1;
					break;
				}

				if ( greatDeviations >= maxDeviationsCount ) {
					similarity = -1;
					break;
				}
			}
				
			//if(similarity <= 15.5f)

			//Debug.Log ( "SIMILARITY value: " + similarity ); //final one

			if ( similarity <= levityFactor && similarity >= 0.3f )
				OnComparationDecisionMade ( true );
			else
				OnComparationDecisionMade ( false );

			//OnComparationDecisionMade(similar_____);

			//Reset ( );//reset values after comparation

			return false;		
		}

		public void NotifyComparationDecisionMade ( bool dec )
		{
			
		}

		void Reset ()
		{
			rotation = 0;//reset rotation
			pointsDifferenceHappened = 0;
			greatDeviations = 0;
			myIntersections.Clear ( );
			taskIntersections.Clear ( );
			comparationMade = false;
			masterpiece = null;
		}

		/// <summary>
		/// Counts the similarity of two shapes by measuring distances between radius-vector intersections.
		/// </summary>
		/// <returns>The similarity.</returns>
		private float CountSimilarity ()
		{
			float result = 0;

		
			for ( int i = 0; i < taskIntersections.Count; i++ ) {
				
				if ( taskIntersections.Count > 0 && taskIntersections.Count == myIntersections.Count ) {
					//if()
					float distance = Vector3.Distance ( taskIntersections.First ( ),
						                 myIntersections.First ( ) );

					//for ( int j = 0; j < taskIntersections.Count; j++ ) {

					if ( taskIntersections.Count == myIntersections.Count ) {

						//pair
						for ( int j = 0; j < taskIntersections.Count; j++ ) {

							Vector3 myFirstInter = myIntersections [j];
							Vector3 accordingInter = taskIntersections.OrderBy ( v => Vector3.Distance (
								                         v, myFirstInter ) ).First ( );

							distance = Vector3.Distance ( myFirstInter, accordingInter );

							//
							if ( distance > 0.4f )
								result += 1.4f;

							if ( distance > 0.58f )
								result += 6.5f;
						}
					}


					result += distance < maxDeviationValue ? distance : distance * 1.25f;



				} else {
					//one-way intersections
					if ( (taskIntersections.Count <= 0 && myIntersections.Count > 0) ||
					     (myIntersections.Count <= 0 && taskIntersections.Count > 0) ) {
						Debug.Log ( "points different one-way" );
						pointsDifferenceHappened += 2;//double
					} else
						pointsDifferenceHappened++;
				}
			}



			//USABLE CODE HERE
			//OnComparationDecisionMade ( result <= 8.0f && result > 0 );

			return result;
		}
	}
}