using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Shaper.UI
{
	public class GameStarterUI : MonoBehaviour
	{
		[SerializeField]
		private GameStarter starter;

		[SerializeField]
		//associated text
		private Text text;

		void Start ()
		{
			text.text = "prepare...";

			starter.OnTimeTick += (int secondsLeft ) => {
				text.gameObject.SetActive ( true );
				text.text = text.text.Remove ( text.text.Length - 1 );
			};
		}
	}
}