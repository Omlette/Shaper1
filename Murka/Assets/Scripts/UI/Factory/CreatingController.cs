using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Shaper.Drawing;
using Shaper.Factory;
using System.Collections.Generic;
using System.Linq;

namespace Shaper.UI
{
	/// <summary>
	/// This class is responsible for controlling state of drawing new task shapes (edit, cancel, etc...)
	/// </summary>
	public class CreatingController : MonoBehaviour
	{
		#region Singleton

		private static CreatingController singleton;

		public static CreatingController Instance{ get { return singleton; } }

		#endregion


		public TaskShapeCreator shapesCreator;

		[SerializeField]
		private List</*ICanvasElement*/ Selectable> canvasElements;

		void Awake ()
		{
			singleton = GetComponent<CreatingController> ( );

			//by default
			ActivateDrawingUI ( false );

			shapesCreator.OnDrawingStarted += ((shape ) => {
			
				ActivateDrawingUI ( true );
			}
			);

			shapesCreator.OnDrawingCanceled += ((shape ) => {

				ActivateDrawingUI ( false );
			}
			);

			shapesCreator.OnDrawingFinished += ((shape ) => {

				ActivateDrawingUI ( false );
			}
			);

			//buttonCancel.onClick.AddListener ( () =>  );

		}

		public void ActivateDrawingUI ( bool val )
		{
			canvasElements.ForEach ( elem => elem.gameObject.SetActive ( val ) );
		}
	}
}