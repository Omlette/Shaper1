using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shaper.Drawing;
using System;

namespace Shaper.Calculations
{
	/// <summary>
	/// Class is responsible for storing some Mathematical and physical tools
	/// </summary>
	public class BaseCalculations : MonoBehaviour
	{
		/// <summary>
		/// Checks wether two lines intersect and returns intersection point by reference
		/// </summary>
		/// <returns><c>true</c>, if two lines intersect, <c>false</c> otherwise.</returns>
		/// <param name="intersections">Intersections.</param>
		/// <param name="linePoint1">Line point1.</param>
		/// <param name="lineVec1">Line vec1.</param>
		/// <param name="linePoint2">Line point2.</param>
		/// <param name="lineVec2">Line vec2.</param>
		/// <param name="lineOne">Line one.</param>
		/// <param name="lineTwo">Line two.</param>
		public static bool IntersectionBetweenTwoLines ( ref List<Vector3> intersections, Vector3 linePoint1, Vector3 lineVec1,
		                                                 Vector3 linePoint2, Vector3 lineVec2, Line lineOne, Line lineTwo, float lineTwoWidth )
		{
			Vector3 intersection = Vector3.zero; //found intersection
			float lineWidthError = lineTwoWidth / 8; //muliply intersections on one line

			//List<Vector3> intersectionTmp = new List<Vector3> ( intersactions );
			//intersactions = new List<Vector3> ( intersectionTmp );

			Vector3 lineVec3 = linePoint2 - linePoint1;
			Vector3 crossVec1and2 = Vector3.Cross ( lineVec1, lineVec2 );
			Vector3 crossVec3and2 = Vector3.Cross ( lineVec3, lineVec2 );

			float planarFactor = Vector3.Dot ( lineVec3, crossVec1and2 );

			//Lines are not coplanar. Take into account rounding errors.
			if ( (planarFactor >= 0.00001f) || (planarFactor <= -0.00001f) ) {

				return false;
			}

			//Note: sqrMagnitude does x*x+y*y+z*z on the input vector.
			float s = Vector3.Dot ( crossVec3and2, crossVec1and2 ) / crossVec1and2.sqrMagnitude;

			if ( (s >= 0.0f) && (s <= 1.0f) ) {

				if ( IsPointOnLine ( lineTwo, linePoint1 + (lineVec1 * s) ) ) {
					intersection = linePoint1 + (lineVec1 * s);

					if ( !intersections.Contains ( intersection )
					     && !intersections.Exists ( p => Vector3.Distance ( p, intersection ) <= lineTwoWidth + lineWidthError ) ) {
						intersections.Add ( intersection );
					}
				}

				return true;
			} else {
				return false;  
				//myIntersections.Add ( intersection );
			}
		}


		/// <summary>
		/// Wether two lines intersect
		/// </summary>
		/// <returns><c>true</c>, if lines intersect was ared, <c>false</c> otherwise.</returns>
		/// <param name="L1">L1.</param>
		/// <param name="L2">L2.</param>
		public static bool AreLinesIntersect ( Line L1, Line L2 )
		{
			if ( SamePoints ( L1.origin, L2.origin ) ||
			     SamePoints ( L1.origin, L2.endPoint ) ||
			     SamePoints ( L1.endPoint, L2.origin ) ||
			     SamePoints ( L1.endPoint, L2.endPoint ) )
				return false;

			return((Mathf.Max ( L1.origin.x, L1.endPoint.x ) >= Mathf.Min ( L2.origin.x, L2.endPoint.x )) &&
			(Mathf.Max ( L2.origin.x, L2.endPoint.x ) >= Mathf.Min ( L1.origin.x, L1.endPoint.x )) &&
			(Mathf.Max ( L1.origin.y, L1.endPoint.y ) >= Mathf.Min ( L2.origin.y, L2.endPoint.y )) &&
			(Mathf.Max ( L2.origin.y, L2.endPoint.y ) >= Mathf.Min ( L1.origin.y, L1.endPoint.y ))
			);
		}


		/// <summary>
		/// Following method checks whether given two points are same or not
		/// </summary>
		public static bool SamePoints ( Vector3 pointA, Vector3 pointB )
		{
			return (pointA.x == pointB.x && pointA.y == pointB.y);
		}


		/// <summary>
		/// Perfoms a 2D vector rotation
		/// </summary>
		/// <returns>The vector2 d.</returns>
		/// <param name="oldDirection">Old direction.</param>
		/// <param name="angle">Angle.</param>
		public static Vector3 RotateVector2D ( Vector3 oldDirection, float angle )
		{
			float newX = Mathf.Cos ( angle * Mathf.Deg2Rad ) * (oldDirection.x) - Mathf.Sin ( angle * Mathf.Deg2Rad ) * (oldDirection.y);   
			float newY = Mathf.Sin ( angle * Mathf.Deg2Rad ) * (oldDirection.x) + Mathf.Cos ( angle * Mathf.Deg2Rad ) * (oldDirection.y);        
			float newZ = oldDirection.z;    
			return new Vector3 ( newX, newY, newZ );   
		}


		/// <summary>
		/// Determines if our point belongs to given line
		/// </summary>
		/// <returns><c>true</c> if is point on line the specified line point; otherwise, <c>false</c>.</returns>
		/// <param name="line">Line.</param>
		/// <param name="point">Point.</param>
		public static bool IsPointOnLine ( Line line, Vector3 point )
		{
			if ( Mathf.Abs ( Vector3.Distance ( line.origin, point ) + Vector3.Distance ( line.endPoint, point ) -
			     Vector3.Distance ( line.origin, line.endPoint ) ) <= 0.01f /*0.0000002384f*/ )
				return true; // C is on the line.
			return false;    // C is not on the line.
		}


		public static float ConvertSecondsToMilliseconds ( float seconds )
		{
			return (float)TimeSpan.FromSeconds ( (double)seconds ).TotalMilliseconds;
		}

		public static float ConvertMillisecondsToSeconds ( float milliseconds )
		{
			return (float)TimeSpan.FromMilliseconds ( (double)milliseconds ).TotalSeconds;
		}
	}
}