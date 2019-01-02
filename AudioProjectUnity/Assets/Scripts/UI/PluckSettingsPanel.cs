using UnityEngine;
using System.Collections.Generic;

namespace RJWS.Audio.UI
{
	public class PluckSettingsPanel : MonoBehaviour
	{
		private GuitarSettings _guitarSettings;
		private PluckSettings _pluckSettings;
		private GuitarSettingsPanel _settingsPanel;

		public UnityEngine.UI.InputField gammaInputField;
		public UnityEngine.UI.InputField volRangeMinInputField;
		public UnityEngine.UI.InputField speedRangeMinInputField;
		public UnityEngine.UI.InputField volRangeMaxInputField;
		public UnityEngine.UI.InputField speedRangeMaxInputField;

		public UnityEngine.UI.Toggle useEnterToggle;
		public UnityEngine.UI.Toggle useExitToggle;

		public System.Action _onSettingsChangedAction;

		public void Init( GuitarSettingsPanel gsp, PluckSettings psp, GuitarSettings gs, System.Action osa)
		{
			_settingsPanel = gsp;
			_guitarSettings = gs;
			_pluckSettings = psp;

			_onSettingsChangedAction = osa;

			gameObject.SetActive( true );

			SetGammaText( );
			SetVolumeMinText( );
			SetVolumeMaxText( );
			SetSpeedMinText( );
			SetSpeedMaxText( );
	
			if (useEnterToggle.isOn != _pluckSettings.useEnter)
			{
				_pluckSettings.useEnter = !_pluckSettings.useEnter;
				useEnterToggle.isOn = !useEnterToggle.isOn;
            }
			if (useExitToggle.isOn != _pluckSettings.useExit)
			{
				_pluckSettings.useExit = !_pluckSettings.useExit;
				useExitToggle.isOn = !useExitToggle.isOn;
			}
		}

		public bool debugMe = true;

		public void OnUseEnterToggleChanged()
		{
			_pluckSettings.useEnter = !_pluckSettings.useEnter;
			Debug.LogFormat( "Changing useEnter to {0}", _pluckSettings.useEnter );
		}

		public void OnUseExitToggleChanged( )
		{
			_pluckSettings.useExit = !_pluckSettings.useExit;
			Debug.LogFormat( "Changing useExit to {0}", _pluckSettings.useEnter );
		}

		private void SetGammaText()
		{
			gammaInputField.text = _pluckSettings.gamma.ToString();
		}

		public void OnGammaTextEndEdit()
		{
			string s = gammaInputField.text;
			float f;
			if (float.TryParse(s, out f))
			{
				if (!Mathf.Approximately(f, _pluckSettings.gamma))
				{
					if (f <= Core.Audio.AudioConsts.MAX_PLUCK_GAMMA && f >= -1f * Core.Audio.AudioConsts.MAX_PLUCK_GAMMA)
					{
						Debug.LogFormat( "Changing gamma from {0} to {1}", _pluckSettings.gamma, f );
						_pluckSettings.gamma = f;
						if (_onSettingsChangedAction != null)
						{
							_onSettingsChangedAction( );
						}
					}
					else
					{
						Debug.LogWarningFormat( "Attenuation out of range: {0}", f );
					}
				}
			}
			else
			{
				Debug.LogWarningFormat( "Couldn't get float from '{0}'", s );
			}
			SetGammaText( );
		}

		private void SetVolumeMinText( )
		{
			volRangeMinInputField.text = _pluckSettings.volumeRange.x.ToString( );
		}

		public void OnVolumeMinTextEndEdit( )
		{
			string s = volRangeMinInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (!Mathf.Approximately( f, _pluckSettings.volumeRange.x))
				{
					if (f <= _pluckSettings.volumeRange.y && f >= 0f)
					{
						Debug.LogFormat( "Changing volRangeMin from {0} to {1}", _pluckSettings.volumeRange.x, f );
						_pluckSettings.volumeRange.x = f;
						if (_onSettingsChangedAction != null)
						{
							_onSettingsChangedAction( );
						}
					}
					else
					{
						Debug.LogWarningFormat( "VolMin out of range: {0}", f );
					}
				}
			}
			else
			{
				Debug.LogWarningFormat( "Couldn't get float from '{0}'", s );
			}
			SetVolumeMinText( );
		}

		private void SetVolumeMaxText( )
		{
			volRangeMaxInputField.text = _pluckSettings.volumeRange.y.ToString( );
		}

		public void OnVolumeMaxTextEndEdit( )
		{
			string s = volRangeMaxInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (!Mathf.Approximately( f, _pluckSettings.volumeRange.y ))
				{
					if (f >= _pluckSettings.volumeRange.x && f < Core.Audio.AudioConsts.MAX_VOLUME)
					{
						Debug.LogFormat( "Changing volRangeMax from {0} to {1}", _pluckSettings.volumeRange.y, f );
						_pluckSettings.volumeRange.y = f;
						if (_onSettingsChangedAction != null)
						{
							_onSettingsChangedAction( );
						}
					}
					else
					{
						Debug.LogWarningFormat( "VolMax out of range: {0}", f );
					}
				}
			}
			else
			{
				Debug.LogWarningFormat( "Couldn't get float from '{0}'", s );
			}
			SetVolumeMaxText( );
		}


		private void SetSpeedMinText( )
		{
			speedRangeMinInputField.text = _pluckSettings.speedRange.x.ToString( );
		}

		public void OnSpeedMinTextEndEdit( )
		{
			string s = speedRangeMinInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (!Mathf.Approximately( f, _pluckSettings.speedRange.x ))
				{
					if (f <= _pluckSettings.speedRange.y && f >= 0f)
					{
						Debug.LogFormat( "Changing speedRangeMin from {0} to {1}", _pluckSettings.speedRange.x, f );
						_pluckSettings.speedRange.x = f;
						if (_onSettingsChangedAction != null)
						{
							_onSettingsChangedAction( );
						}
					}
					else
					{
						Debug.LogWarningFormat( "SpeedMin out of range: {0}", f );
					}
				}
			}
			else
			{
				Debug.LogWarningFormat( "Couldn't get float from '{0}'", s );
			}
			SetSpeedMinText( );
		}

		private void SetSpeedMaxText( )
		{
			speedRangeMaxInputField.text = _pluckSettings.speedRange.y.ToString( );
		}

		public void OnSpeedMaxTextEndEdit( )
		{
			string s = speedRangeMaxInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (!Mathf.Approximately( f, _pluckSettings.speedRange.y ))
				{
					if (f >= _pluckSettings.speedRange.x && f < 1000f)
					{
						Debug.LogFormat( "Changing speedRangeMax from {0} to {1}", _pluckSettings.speedRange.y, f );
						_pluckSettings.speedRange.y = f;
						if (_onSettingsChangedAction != null)
						{
							_onSettingsChangedAction( );
						}
					}
					else
					{
						Debug.LogWarningFormat( "SpeedMax out of range: {0}", f );
					}
				}
			}
			else
			{
				Debug.LogWarningFormat( "Couldn't get float from '{0}'", s );
			}
			SetSpeedMaxText( );
		}


		public void HandleDoneButton()
		{
			gameObject.SetActive( false );
		}
	}
}

