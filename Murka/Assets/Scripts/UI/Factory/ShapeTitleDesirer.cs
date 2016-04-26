using UnityEngine;
using UnityEngine.UI;
using Shaper.Factory;
using Shaper.Drawing;

namespace Shaper.UI
{
	public class ShapeTitleDesirer : MonoBehaviour
	{
		public InputField titleField;
		public TaskShapeCreator creator;
		public ShowTaskShapes representer;

		void Awake ()
		{
			
			titleField.onEndEdit.AddListener ( (string title ) => creator.desiredTitle = title );

			creator.OnDrawingFinished += ((DrawnTaskShape shape ) => representer.buttonsSet.FindLast ( btn => btn.GetComponent<TaskShapeButton> ( ).associatedShape == shape ).transform.GetComponentInChildren<Text> ( ).text = shape.shapeTitle);
		}
	}
}

