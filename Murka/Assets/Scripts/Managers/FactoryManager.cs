using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Shaper.Drawing;
using System.Linq;

namespace Shaper
{
	public partial class FactoryManager : Manager
	{
		public Drawing.DrawnTaskShape drawnTaskShape;


		void Awake ()
		{
			singleton = GetComponent<FactoryManager> ( );

			LoadAllTaskShapes ( );
		}
	}
}