using UnityEngine;
using System.Collections;


namespace Shaper.Commands
{
	/// <summary>
	/// Base interface for all the commands
	/// </summary>
	public abstract class Command : MonoBehaviour
	{
		/// <summary>
		/// Receiver target
		/// </summary>
		public Player player;


		public void SetPlayer ( Player playerTarget )
		{
			//lazy initializetion
			if ( player == null )
				player = playerTarget;
		}

		//a very base 'entry point' for execution every commands
		public abstract void Execute ();
	}
}
