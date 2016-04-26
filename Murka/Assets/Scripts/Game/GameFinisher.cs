using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Shaper
{
	public class GameFinisher : MonoBehaviour
	{
		/// <summary>
		/// Time in seconds to wait before translating to a 'begin again' 
		/// </summary>
		public byte secondsOffset = 3;

		[SerializeField]
		Player player;
		[SerializeField]
		TimeStream timeStream;
		[SerializeField]
		RoundsOrganizer organizer;

		// Use this for initialization
		void Start ()
		{
			player.OnAttemptSpent += (int left ) => StartCoroutine ( FinishGame ( ) );
			timeStream.OnTimeExpired += () => StartCoroutine ( FinishGame ( ) );
			organizer.OnGameFinished += (int points, int curRound ) => StartCoroutine ( FinishGame ( ) );
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		/// <summary>
		/// Finishs the game.
		/// </summary>
		IEnumerator FinishGame ()
		{
			yield return new WaitForSeconds ( secondsOffset );
			SceneManager.LoadScene ( 0 /*0 - always stays for start scene*/ );
		}
	}
}