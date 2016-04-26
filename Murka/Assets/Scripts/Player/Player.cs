using UnityEngine;
using System.Collections;
using Shaper.Drawing;
using System.Linq;

namespace Shaper
{
	//Player class. There is no need in any kind of abstraction 'cause its too simple class. KISS
	public class Player : MonoBehaviour
	{
		#region Delegates and Events

		public delegate void PointsAddingHandler ( int pointsAmount );

		public delegate void AttemptsSpendHandler ( int left );


		/// <summary>
		/// Occurs when attempt was spent.
		/// </summary>
		public event AttemptsSpendHandler OnAttemptSpent;
		/// <summary>
		/// Occurs when points were added after successfull redrawing.
		/// </summary>
		public event PointsAddingHandler OnPointsAdded;

		#endregion

		#region Variables and Properties

		/*[HideInInspector]*/
		[SerializeField]
		/// <summary>
		/// Currently reached points. 1 Point = 1 Shape correctly redrawn
		/// </summary>
		protected short currentPoints = 0;


		[SerializeField]
		/// <summary>
		/// How much time left to complete current task
		/// </summary>
		protected float time = 0.0f;


		[SerializeField]
		/// <summary>
		/// How many attempts we have to complete current task. -1 stays for infinite value
		/// </summary>
		protected short attemptsLeft = -1;

		[SerializeField]
		protected Drawable drawable;

		public short CurrentPoints{ get { return currentPoints; } set { currentPoints = value; } }

		public float Time{ get { return time; } set { time = value; } }

		public short AttemptsLeft {
			get { return attemptsLeft; }
			set {
				attemptsLeft = value;
				if ( OnAttemptSpent != null )
					OnAttemptSpent ( attemptsLeft );
			}
		}



		/// <summary>
		/// Current shape that is in task to redraw
		/// </summary>
		public DrawnTaskShape currentShape;

		//link to a manager for shorter writing
		private GameManager manager;


		#endregion

		void Awake ()
		{
			if ( drawable )
				drawable.AwakeFunc ( );

			if ( drawable is DrawnShape ) {
				manager = (GameManager)GameManager.Instance;

				//currentShape = GameObject.Find ( "Picture" ).
				GetComponent<DrawnTaskShape> ( );
			}

			if ( !drawable )
				return;
		}

		// Use this for initialization
		void Start ()
		{
			if ( Manager.Instance is GameManager ) {
				((GameManager)GameManager.Instance).picturesComparator.OnComparationDecisionMade += ((r ) => {
					if ( attemptsLeft > 0 ) {
						attemptsLeft--;

						if ( OnAttemptSpent != null )
							OnAttemptSpent ( attemptsLeft );
					}
				});

				((GameManager)GameManager.Instance).picturesComparator.OnComparationDecisionMade += ((r ) => {
					
					if ( currentPoints >= 0 && r ) { //for succesly redrawing
						currentPoints += currentShape.pointsAward;

						if ( OnPointsAdded != null )
							OnPointsAdded ( currentPoints );
					}

					if ( !r )
						drawable.lineRenderer.SetColors ( Color.red, Color.red );
				});
			}
		}
	
		// Update is called once per frame
		void Update ()
		{
			if ( !drawable )
				return;

			drawable.UpdateFunc ( );
//
//
//			if ( Input.GetKeyDown ( KeyCode.A ) )
//				SetDrawable ( manager.drawableModes.First ( m => m is NoneDrawable ) );
//
//			if ( Input.GetKeyDown ( KeyCode.B ) )
//				SetDrawable ( manager.drawableModes.First ( m => m is DrawnShape ) );
		}

		void LateUpdate ()
		{
			
			if ( !drawable )
				return;
			
			drawable.LateUpdateFunc ( );
		}


		void OnEnable ()
		{

		}

		/// <summary>
		/// Dynamically changes a way we can draw or disable this capability
		/// </summary>
		/// <param name="drawable">Drawable.</param>
		public void SetDrawable ( Drawable drawable )
		{
			if ( this.drawable != null )
				this.drawable.gameObject.SetActive ( false );

			if ( drawable == null )
				return;

			this.drawable = drawable;

			drawable.gameObject.SetActive ( true );

			//this.drawable.Awake ( ); //call it's init
			//this.drawable.OnEnableFunc ( );
		
		}

		public Drawable GetDrawable ()
		{

			return drawable ?? null;
		}
	}
}
