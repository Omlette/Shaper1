using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Shaper.UI
{
	public class GameResultUI : MonoBehaviour
	{
		public Text text, pointsText;

		/// <summary>
		/// The color of our victory lettering
		/// </summary>
		public Color victoryColor;
		/// <summary>
		/// The color of our loss lettering
		/// </summary>
		public Color lossColor;

		[SerializeField]
		private Player player;

		[SerializeField]
		private TimeStream timeStream;

		[SerializeField]
		private RoundsOrganizer organizer;


		private enum GameResult
		{
			NotDecided,
			Victory,
			Loss
		}

		// Use this for initialization
		void Start ()
		{
			//begin handling!!!

			GameResult result = GameResult.NotDecided;

			SetSubcription ( );
		}

		void SetSubcription ( bool subscribe = true )
		{
			if ( subscribe ) {
				player.OnAttemptSpent += AttemptsSpent;
				timeStream.OnTimeExpired += TimeExpired;
				organizer.OnGameFinished += GameFinished;
			} else {
				player.OnAttemptSpent -= AttemptsSpent;
				timeStream.OnTimeExpired -= TimeExpired;
				organizer.OnGameFinished -= GameFinished;
			}	
		}

		void AttemptsSpent ( int left )
		{
			if ( left == 0 ) {
				Display ( GameResult.Loss, player.CurrentPoints );
			}
		}

		void TimeExpired ()
		{
			Display ( GameResult.Loss, player.CurrentPoints );
		}

		void GameFinished ( int points, int curRound )
		{
			Display ( GameResult.Victory, player.CurrentPoints );
		}

		void Display ( GameResult result, int points = -1 )
		{

			switch ( result ) {
			case GameResult.Victory:
				text.color = victoryColor;
				text.text = "VICTORY";
				break;
			case GameResult.Loss:
				text.color = lossColor;
				text.text = "LOSS";
				break;

			default:
				//leave empty in extra cases
				text.text = "";
				break;
			}

			pointsText.text = "score: " + player.CurrentPoints.ToString ( );

			//finally unscribe
			SetSubcription ( false );
		}

		void Update ()
		{
		}
	}
}
