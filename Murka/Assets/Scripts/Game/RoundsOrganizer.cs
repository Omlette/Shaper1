using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Shaper.Drawing;

namespace Shaper
{
	public class RoundsOrganizer : MonoBehaviour
	{
		public delegate void RoundsCompletingHandler ( int points,int curRound );

		public event RoundsCompletingHandler OnGameFinished;
		public event RoundsCompletingHandler OnRoundProgress;


		/// <summary>
		/// An associated pool of all shapes (rounds) which will be being given
		/// </summary>
		public ShapesPool poolOfShapes;

		//lays here
		public PicturesComparation comparator;
		Drawing.DrawnTaskShape firstShape;

		[SerializeField]
		//current round counter
		private int currentRound;

		/// <summary>
		/// Gets the current round.
		/// </summary>
		/// <value>The current round.</value>
		public int CurrentRound{ get { return currentRound; } }

		[SerializeField]
		private Player player;

		void Awake ()
		{
			OnGameFinished += NotifyGameFinished;

			/// <summary>
			/// Launches game by enabling first figure to draw and adjusts it to our player class
			/// </summary>
			/// <param name="pool">Pool.</param>
			poolOfShapes.OnPoolPrepared += (pool ) => {
				Go ( pool.First ( ) );
				firstShape = pool.First ( );
				firstShape.gameObject.SetActive ( true );
			};

			//Completed round began after points had been adjusted, not after correctly redrawing!!!
			player.OnPointsAdded += ((points ) => {
				
				//last+1
				int idx = poolOfShapes.taskShapesPool.IndexOf (
					          poolOfShapes.taskShapesPool.Last ( s => s.gameObject.activeSelf ) ) + 1;
				if ( idx >= poolOfShapes.taskShapesPool.Count ) {
					OnGameFinished ( player.CurrentPoints, currentRound );//Great!
					return;
				}
					
				//current task shape round
				DrawnTaskShape taskShape = poolOfShapes.taskShapesPool.ElementAt ( idx );
					
				//need
				Go ( taskShape );
				firstShape.gameObject.SetActive ( false );
			});
		}


		public void NotifyGameFinished ( int points, int cRound )
		{
			
		}


		// Use this for initialization
		void Start ()
		{
			//if ( !player )
			//	player = Manager.Instance.player;
		}
	
		// Update is called once per frame
		void Update ()
		{
			ActivateFirst ( );
		}


		private void ActivateFirst (/*bool val*/)
		{
			if ( !firstShape.gameObject.activeSelf && Manager.Instance.player.currentShape == firstShape )
				firstShape.gameObject.SetActive ( true );
		}

		/// <summary>
		/// Prepares everything to go to a next level
		/// </summary>
		/// <param name="shape">Shape.</param>
		void Go ( DrawnTaskShape shape )
		{
			poolOfShapes.Activate ( shape );
			//have to adjust it to our player to let him know comparation target
			player.currentShape = shape;
			player.GetDrawable ( ).Clear ( );
			currentRound++;

			if ( OnRoundProgress != null )
				OnRoundProgress ( player.CurrentPoints, currentRound );
		}
	}
}
