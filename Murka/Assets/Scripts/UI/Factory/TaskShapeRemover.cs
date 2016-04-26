using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Shaper.Drawing;
using Shaper.UI;
using Shaper;
using Shaper.Factory;

//UI class
public class TaskShapeRemover : MonoBehaviour
{
	//outer-adjusting
	public ShapesPool shapesPool;
	private Button button;

	void Awake ()
	{
		shapesPool = /*Will be certainly changed if i don't forget :) */ GameObject.Find ( "ShapesPool" ).GetComponentInParent<ShapesPool> ( );
		button = transform.parent.GetComponent<Button> ( );
		GetComponent<Button> ( ).onClick.AddListener ( () =>
			RemoveTaskShape ( /*send a prefab*/ FactoryManager.Instance.taskShapes.FindLast ( shape => shape.shapeTitle == button.GetComponent<TaskShapeButton> ( ).associatedShape.shapeTitle ) ) );
	
	}


	public void RemoveTaskShape ( DrawnTaskShape shape )
	{
		//deactivate this for future and if it's not already deleted
		if ( shape )
			shape.gameObject.tag = ShapesSetting.UNUSED_TAG;

		shapesPool.Remove ( button.GetComponent<TaskShapeButton> ( ).associatedShape );

		//and just like a puzzle this button will be a single one wich is interactable.
		//so being removed button now, is our being created one. Let's just shut down creator's UI now
		CreatingController.Instance.ActivateDrawingUI ( false );

		shapesPool.GetPoolUpdates ( );
	}
}
