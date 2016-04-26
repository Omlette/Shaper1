using UnityEngine;
using UnityEngine.UI;
using Shaper.Calculations;

namespace Shaper.UI
{
	/// <summary>
	/// Responsible for showing currently remaining time
	/// </summary>
	public class AttemptsDisplayUI : MonoBehaviour
	{
		[SerializeField]
		private Text associatedText;
		public Player player;


		void Start ()
		{
			player = GameManager.Instance.player;
		}

		void FixedUpdate ()
		{
			if ( !player )
				return;
			
			associatedText.text = player.AttemptsLeft == -1 ? "∞" : player.AttemptsLeft.ToString ( );
		}
	}
}

