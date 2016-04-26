using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using Shaper.Drawing;
using System.Linq;

namespace Shaper.Factory
{
	public class TaskShapeCreator : MonoBehaviour
	{
		public delegate void DrawingHandler ( DrawnTaskShape shape );

		public event DrawingHandler OnDrawingStarted;
		public event DrawingHandler OnDrawingCanceled;
		public event DrawingHandler OnDrawingFinished;


		//outer adjusting
		/// <summary>
		/// The shape pool.
		/// </summary>
		public ShapesPool shapePool;


		private DrawnTaskShape blankTaskShape;

		public DrawnTaskShape BlankTaskShape{ get { return blankTaskShape; } }


		public string desiredTitle = "Untitled";

		void Start ()
		{
			//shapePool.OnPoolPrepared += CreateShape;
		}

		/// <summary>
		/// Creates the shape.
		/// </summary>
		public void CreateShape ()
		{
			if ( !shapePool )
				return;

			//Just disallow creating while there is already one being created shape
			if ( shapePool.taskShapesPool.Exists ( s => s.shapeTitle == ShapesSetting.DEFAULT_TITLE ) )
				return;
			
			//create new task shape having set it's capability to be drawn(fomed) now
			blankTaskShape = DrawnTaskShape.CreateNewOne ( ShapesSetting.DEFAULT_TITLE, true );
			shapePool.PushnPlace ( blankTaskShape );
			FactoryManager.Instance.player.SetDrawable ( blankTaskShape );

			if ( OnDrawingStarted != null )
				OnDrawingStarted ( blankTaskShape );
		}


		/// <summary>
		/// Cancel drawing our shape and returning to viewing mode
		/// </summary>
		/// <returns><c>true</c> if this instance cancel ; otherwise, <c>false</c>.</returns>
		public void Cancel ()
		{
			if ( !blankTaskShape )//destroyed?
				return;
			
			Manager.Instance.player.SetDrawable ( NoneDrawable.Instance );


			//for 'smoother' destroying
			blankTaskShape.Clear ( );
			blankTaskShape.enabledW = false;//let's just turn on it's capability to be drawn
			shapePool.taskShapesPool.Remove ( blankTaskShape );
			Destroy ( blankTaskShape.gameObject );

			shapePool.GetPoolUpdates ( );

			if ( OnDrawingCanceled != null )
				OnDrawingCanceled ( blankTaskShape );
		}


		/// <summary>
		/// Saves drawn shape having clicked according button
		/// </summary>
		public void SaveShape ()
		{
			//Object prefab = EditorUtility.CreateEmptyPrefab ( string.Format ( "{0}/{1}.prefab", ShapesPath.SHAPES_PASS, blankTaskShape.name ) );

			if ( !blankTaskShape || !blankTaskShape.lineRenderer )
				return;

			//we don't want to save invisible shapes or event just lines!
			if ( blankTaskShape.pointsList.Count < 3 )
				return;

			Manager.Instance.player.SetDrawable ( NoneDrawable.Instance );

			//stopdrawing
			blankTaskShape.enabledW = false;
			blankTaskShape.lineRenderer.useWorldSpace = false;
			//let's set desired title to shape
			HandleDesiredTitle ( );
			blankTaskShape.shapeTitle = desiredTitle;
			blankTaskShape.gameObject.name = desiredTitle;

			//let's update
			shapePool.GetPoolUpdates ( );

			if ( OnDrawingFinished != null )
				OnDrawingFinished ( blankTaskShape );


			#if UNITY_EDITOR
			//finally we should create a prefab to save this for a future needs
			Object prefab = EditorUtility.CreateEmptyPrefab ( string.Format ( "Assets/Resources/{0}/{1}.prefab", ShapesPath.SHAPES_PASS, desiredTitle ) );
			GameObject actualPrefab = EditorUtility.ReplacePrefab ( blankTaskShape.gameObject, prefab, ReplacePrefabOptions.ReplaceNameBased );
			actualPrefab.GetComponent<DrawnTaskShape> ( ).lineRenderer.material = new Material ( Shader.Find ( "Particles/Additive" ) );

			//create a new connection and parse it to our manager
			//FactoryManager.Instance.taskShapes.Add ( actualPrefab.GetComponent<DrawnTaskShape> ( ) );
			#endif
		}

		/// <summary>
		/// Gets rid of a few misunderstandings
		/// Is called from InputField
		/// </summary>
		/// <returns>The desired title.</returns>
		public void HandleDesiredTitle ()
		{
			desiredTitle = desiredTitle == "" ? ShapesSetting.DEFAULT_TITLE : desiredTitle;

			//we want to have unique titles
			if ( shapePool.taskShapesPool.Exists ( sh => sh.shapeTitle == desiredTitle ) ) {
				desiredTitle += (shapePool.taskShapesPool.Count ( sh => sh.shapeTitle.Contains ( desiredTitle ) ) + 1).ToString ( );
			}
				
			//just remember
			//desiredTitle = result;
		}
	}
}
