using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Shaper.Drawing;


namespace Shaper
{
	public class GameManager : Manager
	{

		/// <summary>
		/// All possible drawing modes. Outer initialization
		/// </summary>
		public List<Drawable> drawableModes;

		//player
		//current shape task
		//other

		public PicturesComparation picturesComparator;

		void Awake ()
		{

			singleton = GetComponent<GameManager> ( );

			LoadAllTaskShapes ( );
		}


	}
}