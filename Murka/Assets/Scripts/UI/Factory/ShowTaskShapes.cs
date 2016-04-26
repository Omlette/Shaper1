using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Shaper.Drawing;
using System.Linq;
using Shaper.Factory;

namespace Shaper.UI
{
	public class ShowTaskShapes : MonoBehaviour
	{
		public List<DrawnTaskShape> allTasks = new List<DrawnTaskShape> ( );

		public RectTransform scrollRectContent;

		[SerializeField]
		private GameObject shapesButton;

		public List<Button> buttonsSet = new List<Button> ( );

		//link to pool
		public ShapesPool shapesPool;
		public TaskShapeCreator creator;


		void Awake ()
		{
			shapesPool.OnPoolPrepared += CreateButtons;
			// We cannot select shapes while drawing
			creator.OnDrawingStarted += ((shape ) => {
			
				buttonsSet.ForEach ( btn => {
					btn.interactable = false;
					//remove button for each button. just underline this
					btn.GetComponentInChildren<TaskShapeRemover> ( ).GetComponent<Button> ( ).interactable = false;
				} );

			});
				
			creator.OnDrawingCanceled += ((shape ) => {
				buttonsSet.ForEach ( (btn ) => { 

					if ( btn.GetComponent<TaskShapeButton> ( ).associatedShape == shape ) {
						buttonsSet.Remove ( btn );
						Destroy ( btn.gameObject );
					}
				} );

				buttonsSet.ForEach ( btn => {
					btn.interactable = true;
					btn.GetComponentInChildren<TaskShapeRemover> ( ).GetComponent<Button> ( ).interactable = true;
				} );//we also want to activate back our buttons
			});

			creator.OnDrawingFinished += ((shape ) => {
				buttonsSet.ForEach ( btn => {
					btn.interactable = true;
					btn.GetComponentInChildren<TaskShapeRemover> ( ).GetComponent<Button> ( ).interactable = false;
				} );//we also want to activate back our buttons
			});

		}

		// Use this for initialization
		void Start ()
		{
			if ( !scrollRectContent )
				return;
			
			allTasks = ((FactoryManager)FactoryManager.Instance).taskShapes;
		}


		/// <summary>
		/// Creates the buttons to represent a collection of currently existing shapes
		/// </summary>
		public void CreateButtons ( List<DrawnTaskShape> shapesPoolElements )
		{
			List<Button> unexistingButtons = buttonsSet.FindAll ( b => !(shapesPool.taskShapesPool.Exists ( elem => elem.shapeTitle == b.GetComponent<TaskShapeButton> ( ).associatedShape.shapeTitle )) );

			if ( unexistingButtons.Count > 0 ) {
				buttonsSet.ForEach ( b => {
					if ( unexistingButtons.Contains ( b ) ) {
						buttonsSet.Remove ( b );
						Destroy ( b.gameObject );
					}
				} );
			}


			shapesPoolElements.ForEach ( t => {
				//no time left to create a pools

				//deleted ones
				if ( t.gameObject.tag == ShapesSetting.UNUSED_TAG )
					return;


				//if already exists => perfomance
				if ( buttonsSet.Exists ( val => val.GetComponent<TaskShapeButton> ( ).associatedShape == t ) )
					return;


				//actual creating of new btn
				GameObject btnGO = Instantiate ( shapesButton ) as GameObject;
				Button btn = btnGO.GetComponent<Button> ( );
				RectTransform btnTransform = btn.GetComponent<RectTransform> ( );
				btn.GetComponent<TaskShapeButton> ( ).associatedShape = t;

				btnTransform.parent = scrollRectContent;
				btnTransform.localScale = new Vector3 ( 1, 1, 1 );

				Text btnText = btn.transform.GetComponentInChildren<Text> ( );
				btnText.text = (t.shapeTitle == string.Empty ? t.name : t.shapeTitle);

				string cloneStr = "(Clone)"; //removing excess words for title

				btnText.text = btnText.text.Contains ( "(Clone)" ) ? btnText.text.Replace ( cloneStr, "" ) : btnText.text;

				btn.onClick.AddListener ( () => OpenTaskShape ( t ) );


				buttonsSet.Add ( btn );
			} );

		}


		public void RepaintButtons ()
		{
			shapesPool.GetPoolUpdates ( );

		}

		void OpenTaskShape ( DrawnTaskShape shape )
		{
			shapesPool.Activate ( shape );

		}
	}
}
