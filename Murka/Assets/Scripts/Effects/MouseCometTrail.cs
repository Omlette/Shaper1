using UnityEngine;
using System.Collections;
using Shaper.Drawing;
using System.Linq;

namespace Shaper
{
	[RequireComponent ( typeof(TrailRenderer) )]
	// This system should have been accomplished using Strategy pattern at least..
	/// <summary>
	/// Responsible for generating some effect wich is following mouse pointer
	/// </summary>
	public class MouseCometTrail : MonoBehaviour
	{
		[SerializeField]
		private TrailRenderer trail;

		[SerializeField]
		private Player player;


		/// <summary>
		/// Width of trail renderer when we are not drawing currently
		/// </summary>
		public float defaultWidthValue = 0.05f;
		/// <summary>
		/// Width of trail renderer during drawing process
		/// </summary>
		public float drawingWidthValue = 1.0f;

		/// <summary>
		/// Associated player's drawable.
		/// </summary>
		//We will use, certrainly, only DrawnShape one
		DrawnShape playersDrawable;


		void Awake ()
		{
			if ( !trail )
				trail = GetComponent<TrailRenderer> ( );

			SetWidth ( 0.05f );
		}

		// Use this for initialization
		void Start ()
		{
			if ( !player )
				return;

			try {
				playersDrawable = (DrawnShape)player.GetDrawable ( );
			} catch {
				playersDrawable = null;
			}


			playersDrawable.OnDrawingStarted += () => SetWidth ( drawingWidthValue );
			playersDrawable.OnShapeDrawn += (DrawnShape drawnShape ) => SetWidth ( defaultWidthValue );

		}
	
		// Update is called once per frame
		void Update ()
		{
			if ( !player || !trail || !playersDrawable )
				return;

			HandlePosition ( );

		}

		void HandlePosition ()
		{
			Ray r = Camera.main.ScreenPointToRay ( Input.mousePosition );
			Vector3 pos = r.GetPoint ( 10 );
			transform.position = pos;
		}


		/// <summary>
		/// Just sets a trail's width
		/// </summary>
		/// <param name="val">Value.</param>
		void SetWidth ( float val )
		{
			trail.startWidth = val;
			trail.endWidth = val;
		}
	}
}
