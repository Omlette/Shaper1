using UnityEngine;
using System.Collections;
using Shaper.Drawing;
using System;
using Shaper.Calculations;
using System.Linq;

namespace Shaper
{
	/// <summary>
	/// Class responsible for measuring time during rounds
	/// </summary>
	public class TimeStream : MonoBehaviour
	{
		#region Events and Delegates

		public delegate void RoundTiming ();

		/// <summary>
		/// Occurs when time expires.
		/// </summary>
		public event RoundTiming OnTimeExpired;

		/// <summary>
		/// Occurs when time becomes prolonged.
		/// </summary>
		public event RoundTiming OnTimeProlonged;

		/// <summary>
		/// Occurs as time stream stops.
		/// </summary>
		public event RoundTiming OnTimeStreamStopped;

		#endregion


		#region Variables and Properties

		[SerializeField]
		/// <summary>
		/// The base time for the current round. [MILLISECONDS]
		/// </summary>
		public float originTimePerRound;
		/// <summary>
		/// The time magnitude, diemension
		/// </summary>
		public float timeStep = 100;

		[Range ( 0, 100 )]
		/// <summary>
		/// The percantage value of per-round time cutting off
		/// [PERCENTAGE]
		/// </summary>
		public float timeClippingPerc = 5.0f;

		[Range ( 0, 5 )]
		/// <summary>
		/// An additional percent wich stacks with above one to cut off given time as rounds being won
		/// Max percentage value is 5. +Clamping
		/// [PERCENTAGE]
		/// </summary>
		public float perRoundPerc = 5.0f;

		[SerializeField]
		/// <summary>
		/// Nmber of won rounds in succession
		/// </summary>
		private int combo;

		[SerializeField]
		private PicturesComparation comparator;

		private DrawnTaskShape currentShape;

		[SerializeField]
		/// <summary>
		/// Time left to complete a task
		/// </summary>
		private float timeLeft;

		[SerializeField]
		Player player;
		[SerializeField]
		RoundsOrganizer organizer;


		public float TimeLeft{ get { return timeLeft; } }

		#endregion


		void Awake ()
		{
			timeLeft = originTimePerRound;
			perRoundPerc = Mathf.Clamp ( perRoundPerc, 0, timeClippingPerc - 1 );
			InvokeRepeating ( "TimeHandler", 0, BaseCalculations.ConvertMillisecondsToSeconds ( timeStep ) );

			//as game finishes, we stop our timer
			organizer.OnGameFinished += (int points, int curRound ) => Stop ( );
		}

		void Start ()
		{
			//current drawing
			//correctly redrawing


			comparator = GetComponent<PicturesComparation> ( );
			player = GameManager.Instance.player;

			comparator.OnComparationDecisionMade += ((bool decision ) => {
				//correctly drawn case

				if ( !decision || timeLeft <= 0 )
					return;

				currentShape = GameManager.Instance.player.currentShape;
				//Zero is taken into consideration specially to allow player choose not to reduce time per llevel
				originTimePerRound -= CalcClipping ( );
				timeLeft = originTimePerRound;
				if ( OnTimeProlonged != null )
					OnTimeProlonged ( );
			});
		}

		//Subracting time every timeStep
		void TimeHandler ()
		{
			timeLeft -= Mathf.Clamp ( timeLeft, 0, timeStep );

			if ( timeLeft == 0 ) {
				if ( OnTimeExpired != null )
					OnTimeExpired ( );
			}
		}

		/// <summary>
		/// Calculates the clipping of time given, depending on our level.
		/// </summary>
		/// <returns>The clipping.</returns>
		float CalcClipping ()
		{
			//here, we can assume that:
			//GameManager.Instance.taskShapes.ElementAt(player.currentShape) = combo. So we have:

			//with clipping included
			float newOrigin = Mathf.Clamp ( timeLeft, 0, originTimePerRound * (timeClippingPerc / 100) );

			return newOrigin;
		}

		/// <summary>
		/// Resets and stops time stream
		/// </summary>
		public void Stop ()
		{
			timeLeft = 0;
			CancelInvoke ( "TimeHandler" );

			if ( OnTimeStreamStopped != null )
				OnTimeStreamStopped ( );
		}
	}
}