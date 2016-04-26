using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Shaper.UI
{
	public class TaskShapeRoundTitle : MonoBehaviour
	{
		[SerializeField]
		private Player player;

		[SerializeField]
		private RoundsOrganizer organizer;

		private Text text;

		void Awake ()
		{
			text = GetComponent<Text> ( );

			if ( !player )
				return;
			
			organizer.OnRoundProgress += ((int points, int curRound ) => {
				text.text = player.currentShape.shapeTitle;
			});
		}
	}
}