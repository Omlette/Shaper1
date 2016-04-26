using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Shaper.Drawing;
using System.Linq;

namespace Shaper
{
	//a very base class for all the managers
	public partial class Manager : MonoBehaviour
	{
		#region Delegates and Events

		public delegate void Initialization ();

		#endregion


		protected static Manager singleton;

		public static Manager Instance{ get { return singleton; } }


		/// <summary>
		/// Current user
		/// </summary>
		public Player player;

		/// <summary>
		/// All the shapes which can be given on task
		/// </summary>
		public List<DrawnTaskShape> taskShapes = new List<DrawnTaskShape> ( );

		void Awake ()
		{
			Initialization initMethods;
		}

		protected virtual void Initializing ()
		{
			//Initialization += LoadAllTaskShapes;
		}

		/// <summary>
		/// Prepares a list of task shapes
		/// </summary>
		protected void LoadAllTaskShapes ()
		{
			taskShapes = Resources.LoadAll<Shaper.Drawing.DrawnTaskShape> ( ShapesPath.SHAPES_PASS ).ToList ( );
		}
	}
}