using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Shaper
{
	/// <summary>
	/// Launches a game
	/// </summary>
	public class GameStarter : MonoBehaviour
	{
		public delegate void TimeTickingHandler ( int secondsLeft );

		public event TimeTickingHandler OnTimeTick;


		/// <summary>
		/// Time offset value to start our game
		/// </summary>
		public int originTimeToStart = 3;

		/// <summary>
		/// Time left before our game starts
		/// </summary>
		private int timeToStart;


		//called outside, somewhere from UI
		public void Begin ()
		{
			timeToStart = originTimeToStart;
			//start our countdown timer
			InvokeRepeating ( "Tick", 0, originTimeToStart );
		}


		/// <summary>
		/// Every second reduces time left to begin
		/// </summary>
		void Tick ()
		{
			//very simple approach
			timeToStart--;
			if ( OnTimeTick != null )
				OnTimeTick ( timeToStart );


			if ( timeToStart == 0 ) {//game starts
				StartGame ( );
				CancelInvoke ( "Tick" );
			}
		}

		public void StartGame ()
		{
			SceneManager.LoadScene ( "Game" );
		}
	}
}