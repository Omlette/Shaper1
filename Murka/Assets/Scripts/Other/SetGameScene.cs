using UnityEngine;
using System.Collections;
using System.Linq;

namespace Shaper
{
	public class SetGameScene : MonoBehaviour, IDrawingChangeable
	{
		void Start ()
		{
			SetPlayerDrawingMode ( );
		}


		/// <summary>
		/// Sets the player drawing mode to completing tasks
		/// </summary>
		public void SetPlayerDrawingMode ()
		{
			GameManager m = (GameManager)GameManager.Instance;

			m.player.SetDrawable ( m.drawableModes.Last ( d => d is Drawing.DrawnShape ) );
		}
	}
}