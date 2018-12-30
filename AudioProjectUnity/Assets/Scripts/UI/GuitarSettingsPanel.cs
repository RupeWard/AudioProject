using UnityEngine;

namespace RJWS.Audio.UI
{
	public class GuitarSettingsPanel : MonoBehaviour
	{
		public void Init()
		{
			gameObject.SetActive( true );
		}
		/*
		public void Init(AudioStringBehaviour audios, System.Action<AudioStringBehaviour> oda)
		{
			_audioStringBehaviour = audios;
			_onDoneAction = oda;
			SetFreqInputText( );
			SetAttenInputText( );
			SetLowPassFreqCutOffInputText( );
			if (reverbToggle.isOn != _audioStringBehaviour.UsingReverb)
			{
				_audioStringBehaviour.UseReverb( !_audioStringBehaviour.UsingReverb );
				reverbToggle.isOn = !reverbToggle.isOn;
			}
			gameObject.SetActive( true );
		}
		*/

		public bool debugMe = true;


		/*
		public void HandleResetButton( )
		{
			_attenuation = RJWS.Core.Audio.KSRingBufferF.DEFAULT_GUITAR_ATTENUATION;
			SetAttenInputText( );

			_lowPassCutOffreq = RJWS.Audio.AudioStringBehaviour.DEFAULT_LOWPASSFREQ;
			SetLowPassFreqCutOffInputText( );

			_audioStringBehaviour.SetLowPassFrequency( _lowPassCutOffreq );
		}
		*/
		public void HandleDoneButton()
		{
			/*
			if (_onDoneAction != null)
			{
				_onDoneAction( _audioStringBehaviour);
			}
			*/
			gameObject.SetActive( false );
		}
	}
}

