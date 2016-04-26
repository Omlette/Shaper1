using UnityEngine;
using System.Collections;

namespace Shaper.Drawing
{
	// Structure for line points
	public struct Line
	{
		/// <summary>
		/// The beginning for our lines
		/// </summary>
		public Vector3 origin;

		/// <summary>
		/// The end point.
		/// </summary>
		public Vector3 endPoint;
	};
}