using UnityEngine;

namespace RJWS.Audio.UI
{
	public class AudioStringPanel : MonoBehaviour
	{
		public UnityEngine.UI.Text frequencyText;

		private System.Action<AudioStringBehaviour> _settingsAction;

        public AudioStringBehaviour audioStringBehavuour
		{
			get;
			private set;
		}

		public void Init(AudioStringBehaviour asb, System.Action<AudioStringBehaviour> sa)
		{
			audioStringBehavuour = asb;
			SetFrequencyText( );
			_settingsAction = sa;
			gameObject.SetActive( true );
		}

		private void SetFrequencyText()
		{
			frequencyText.text = audioStringBehavuour.Frequency.ToString( );
		}

		public void HandleSettingsButton()
		{
			if (_settingsAction != null)
			{
				_settingsAction( audioStringBehavuour );
			}
			else
			{
				Debug.LogErrorFormat( this, "No settings button action" );
			}
		}

		public void HandlePluckButton()
		{
			audioStringBehavuour.Pluck( );
		}

		private void OnEnable()
		{
			audioStringBehavuour.onChangedAction -= HandleAudioStringChanged;
			audioStringBehavuour.onChangedAction += HandleAudioStringChanged;
		}

		private void OnDisable( )
		{
			audioStringBehavuour.onChangedAction -= HandleAudioStringChanged;
		}

		private void HandleAudioStringChanged(AudioStringBehaviour asb)
		{
			if (asb != audioStringBehavuour)
			{
				Debug.LogError( "Mismatch" );
			}
			else
			{
				SetFrequencyText( );
			}
		}
	}

}
