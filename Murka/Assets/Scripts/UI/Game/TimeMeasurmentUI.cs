using UnityEngine;
using UnityEngine.UI;
using Shaper.Calculations;

namespace Shaper.UI
{
	/// <summary>
	/// Responsible for showing currently remaining time
	/// </summary>
	public class TimeMeasurmentUI : MonoBehaviour
	{
		[SerializeField]
		private Text associatedText;

		[SerializeField]
		private Text pointsLabel;

		public TimeStream timeStream;

		/// <summary>
		/// Color of 'expired' title label
		/// </summary>
		public Color expiredColor;

		/// <summary>
		/// Flag that controls whether to take timeLeft from stream
		/// </summary>
		private bool showFlag;

		void Start ()
		{
			showFlag = true;

			timeStream.OnTimeStreamStopped += (() => {
				showFlag = false;
				associatedText.text = "";
			});



			timeStream.OnTimeExpired += (() => {
				showFlag = false;
				associatedText.color = expiredColor;
				associatedText.text = "Expired";
			});

		}

		void FixedUpdate ()
		{
			if ( !showFlag )
				return;
			
			associatedText.text = string.Format ( "{0:##.##}", Calculations.BaseCalculations.ConvertMillisecondsToSeconds
				( timeStream.TimeLeft ).ToString ( ) );
		}
	}
}

