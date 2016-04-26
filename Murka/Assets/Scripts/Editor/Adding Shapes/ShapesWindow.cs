using UnityEngine;
using UnityEditor;
using System.Collections;


namespace Shaper.Editor
{
	public partial class ShapesWindow : EditorWindow
	{
		/// <summary>
		/// Default size for window
		/// </summary>
		public Vector2 windowDefaultSize = new Vector2 ( 225, 400 );


		[MenuItem ( "Shapes/Inspect" )]
		/// <summary>
	/// Opens current window
	/// </summary>
	static void CreateWindow ()
		{
			// Get existing open window or if none, make a new one:
			ShapesWindow window =
				(ShapesWindow)EditorWindow.GetWindow ( typeof(ShapesWindow) );
		}


		void OnEnable ()
		{
			ConfigureWindow ( );

		}

		/// <summary>
		/// Configures current window's properties
		/// </summary>
		private void ConfigureWindow ()
		{
			minSize = windowDefaultSize;
		}

		void OnGUI ()
		{
			for ( int i = 0; i < 10; i++ )
				GUI.Label ( new Rect ( 0, 0 + i * 30, 100, 100 ), "shape icon!" );


			Handles.BeginGUI ( );
			Handles.color = Color.red;
			Handles.DrawLine ( new Vector3 ( 0, 0 ), new Vector3 ( 300, 300 ) );

			Handles.EndGUI ( );
		}
	}
}
