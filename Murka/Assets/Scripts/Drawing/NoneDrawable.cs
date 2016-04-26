using UnityEngine;
using System.Collections;

namespace Shaper
{
	public class NoneDrawable : Drawing.Drawable
	{
		private static NoneDrawable singleton;

		public static NoneDrawable Instance{ get { return singleton; } }

		public override void AwakeFunc ()
		{
			singleton = this;
		}


		public override void OnEnable ()
		{
			Debug.Log ( "not drawing applied " );
		}

		protected override void Initialize ()
		{
			return;
		}


		public override void UpdateFunc ()
		{
			return;
		}

		public override void LateUpdateFunc ()
		{
			return;
		}

		public override void Clear ()
		{
			return;
		}
	}
}