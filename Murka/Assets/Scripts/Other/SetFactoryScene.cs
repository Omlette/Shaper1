using UnityEngine;
using System.Collections;
using System.Linq;

namespace Shaper
{
	public class SetFactoryScene : MonoBehaviour, IDrawingChangeable
	{
		void Awake ()
		{
		}

		// Use this for initialization
		void Start ()
		{
			//not now :(
			//SetPlayerDrawingMode ( );
		}


		public void SetPlayerDrawingMode ()
		{
			FactoryManager m = (FactoryManager)FactoryManager.Instance;


			m.player.SetDrawable ( m.drawnTaskShape );
		}
	}
}