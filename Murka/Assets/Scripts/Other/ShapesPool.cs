using UnityEngine;
using System;
using System.Collections.Generic;
using Shaper.Drawing;
using System.Linq;
using System.Collections;

namespace Shaper
{
	//should be a generic one, yet we have no time :(
	public class ShapesPool/*<T>*/ : MonoBehaviour/*, IEnumerable*/ /*where T : DrawnShape*/
	{
		public delegate void PoolPrepared ( List<DrawnTaskShape> pool );

		public event PoolPrepared OnPoolPrepared;


		public List<DrawnTaskShape> taskShapesPool;




		/// <summary>
		/// Currently visible (activated) shape in pool
		/// </summary>
		/// <returns>The shape.</returns>
		public DrawnTaskShape ActivatedShape{ get { return taskShapesPool.First ( s => s.isActiveAndEnabled ); } }


		/*public ShapesPool this [ int index ] {
			get{ return taskShapesPool [index]; }
			protected set { taskShapesPool [index] = value; }
		}*/


		void Start ()
		{
			OnPoolPrepared += NotifyPoolPrepared;

			Manager.Instance.taskShapes.ForEach ( s => {
				
				if ( s.tag != ShapesSetting.UNUSED_TAG ) {
					GameObject g = Instantiate ( s.gameObject,
						               transform.position, Quaternion.identity ) as GameObject;

					DrawnTaskShape shapeComp = g.GetComponent<DrawnTaskShape> ( );
					PushnPlace ( shapeComp );
					SetPoolActive ( false );
				}
			} );


			OnPoolPrepared ( taskShapesPool );
		}

		public void NotifyPoolPrepared ( List<DrawnTaskShape> pool )
		{

			//Debug.Log ( "prepared - ok! " + pool.Count );
		}

		public void Push ( DrawnTaskShape shape )
		{
			taskShapesPool.Add ( shape );
			shape.transform.parent = transform;
			shape.transform.localPosition = Vector3.zero;
			SetShapePosition ( shape );
			shape.gameObject.SetActive ( false );
		}

		public void PushnPlace ( DrawnTaskShape shape )
		{
			Push ( shape );
			SetPoolActive ( false );
			shape.gameObject.SetActive ( true );
		}

		/// <summary>
		/// Enables/disables an entire pool
		/// </summary>
		public void SetPoolActive ( bool val )
		{

			taskShapesPool.ForEach ( t => t.gameObject.SetActive ( val ) );
		}

	

		public void Activate ( DrawnTaskShape shape )
		{
			if ( !taskShapesPool.Contains ( shape ) )
				return;

			SetPoolActive ( false );
			//100
			shape.gameObject.SetActive ( true );
		}


		private void SetShapePosition ( DrawnTaskShape shape )
		{
			shape.transform.position = transform.position - shape.centerPoint;
		}


		/// <summary>
		/// Walks throught pool with a view to send some updates outward if needed.
		/// Observers can INDIRECTLY receive currently last version of this pool
		/// </summary>
		public void GetPoolUpdates ()
		{

			OnPoolPrepared ( taskShapesPool );
		}



		/// <summary>
		/// Remove the specified shape.
		/// </summary>
		/// <param name="shape">Shape.</param>
		public void Remove ( DrawnTaskShape shape )
		{
			taskShapesPool.Remove ( shape );
			shape.gameObject.tag = ShapesSetting.UNUSED_TAG;//
			Destroy ( shape.gameObject );
		}
	}
}