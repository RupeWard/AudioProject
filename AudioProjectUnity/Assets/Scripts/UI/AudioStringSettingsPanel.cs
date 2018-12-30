using UnityEngine;

namespace RJWS.Audio.UI
{
	public class AudioStringSettingsPanel : MonoBehaviour
	{
		private AudioStringBehaviour _audioStringBehaviour;

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

		public bool debugMe = true;

		public UnityEngine.UI.InputField freqInputField;
		public UnityEngine.UI.InputField attenInputField;
		public UnityEngine.UI.InputField lowPassCutOffFreqInputField;
		public UnityEngine.UI.Toggle reverbToggle;

		private System.Action<AudioStringBehaviour> _onDoneAction;

		private void SetFreqInputText( )
		{
			freqInputField.text = _audioStringBehaviour.Frequency.ToString( );
		}

		public void HandleFreqEndEdit( )
		{
			string s = freqInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (f < RJWS.Audio.AudioString.MIN_FREQ)
				{
					Debug.LogErrorFormat( this, "Frequency out of range: {0} < {1}", f, RJWS.Audio.AudioString.MIN_FREQ );
				}
				else
				{
					_audioStringBehaviour.SetFrequency( f );
					if (debugMe)
					{
						Debug.LogFormat( "Frequency changed to {0}", _audioStringBehaviour.Frequency);
					}
				}
			}
			else
			{
				Debug.LogErrorFormat( this, "Couldn't get Frequency from: {0} ", s );
			}
			SetFreqInputText( );
		}

		private void SetAttenInputText( )
		{
			attenInputField.text = _audioStringBehaviour.Attenuation.ToString( );
		}

		public void HandleAttenEndEdit( )
		{
			string s = attenInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (f < RJWS.Core.Audio.KSRingBufferF.MIN_ATTENUATION || f >= 1f)
				{
					Debug.LogErrorFormat( this, "Attenuation out of range: {0} < {1}", f, RJWS.Core.Audio.KSRingBufferF.MIN_ATTENUATION );
				}
				else
				{
					_audioStringBehaviour.SetAttenuation( f );
					if (debugMe)
					{
						Debug.LogFormat( "Attenuation changed to {0}", _audioStringBehaviour.Attenuation );
					}
				}
			}
			else
			{
				Debug.LogErrorFormat( this, "Couldn't get Attenuation from: {0} ", s );
			}
			SetAttenInputText( );
		}

		private void SetLowPassFreqCutOffInputText( )
		{
			lowPassCutOffFreqInputField.text = _audioStringBehaviour.LowPassCutOffFrequency.ToString( );
		}

		public void HandleLowPassFreqEndEdit( )
		{
			string s = lowPassCutOffFreqInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (f <= RJWS.Audio.AudioString.MIN_LOWPASSFREQ)
				{
					Debug.LogErrorFormat( this, "LowPass Frequency out of range: {0} < {1}", f, RJWS.Audio.AudioString.MIN_LOWPASSFREQ );
				}
				else
				{
					_audioStringBehaviour.SetLowPassFrequency( f );
					if (debugMe)
					{
						Debug.LogFormat( "LowPass Frequency changed to {0}", _audioStringBehaviour.LowPassCutOffFrequency );
					}
				}
			}
			else
			{
				Debug.LogErrorFormat( this, "Couldn't get LowPass Frequency from: {0} ", s );
			}
			SetLowPassFreqCutOffInputText( );
		}

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

		public void HandleReverbToggleChanged( bool v )
		{
			_audioStringBehaviour.UseReverb(reverbToggle.isOn);
		}

		public void HandleDoneButton()
		{
			if (_onDoneAction != null)
			{
				_onDoneAction( _audioStringBehaviour);
			}
			gameObject.SetActive( false );
		}
	}
}

