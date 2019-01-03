using UnityEngine;
using System.Collections.Generic;

namespace RJWS.Audio.UI
{
	public class PluckSettingsPanel : MonoBehaviour
	{
		private GuitarSettings _guitarSettings;
		private PluckSettings _pluckSettings;
		private GuitarSettingsPanel _settingsPanel;

		public UnityEngine.UI.InputField strumGammaInputField;
		public UnityEngine.UI.InputField pluckGammaInputField;

		public UnityEngine.UI.InputField strumVolRangeMinInputField;
		public UnityEngine.UI.InputField strumVolRangeMaxInputField;

		public UnityEngine.UI.InputField pluckVolRangeMinInputField;
		public UnityEngine.UI.InputField pluckVolRangeMaxInputField;

		public UnityEngine.UI.InputField speedRangeMinInputField;
		public UnityEngine.UI.InputField speedRangeMaxInputField;

		public UnityEngine.UI.InputField durationRangeMinInputField;
		public UnityEngine.UI.InputField durationRangeMaxInputField;

		public UnityEngine.UI.InputField slideFractionInputField;
		public UnityEngine.UI.InputField slideMinInputField;

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

			SetStrumGammaText( );
			SetStrumVolMinText( );
			SetStrumVolMaxText( );

			SetPluckGammaText( );
			SetPluckVolMinText( );
			SetPluckVolMaxText( );

			SetSpeedMinText( );
			SetSpeedMaxText( );
			SetDurationMinText( );
			SetDurationMaxText( );

			SetSlideMinText( );
			SetSlideFractionText( );

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

		private void SetStrumGammaText()
		{
			strumGammaInputField.text = _pluckSettings.strumGamma.ToString();
		}

		public void OnStrumGammaTextEndEdit( )
		{
			string s = strumGammaInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (!Mathf.Approximately( f, _pluckSettings.strumGamma ))
				{
					if (f <= Core.Audio.AudioConsts.MAX_PLUCK_GAMMA && f >= -1f * Core.Audio.AudioConsts.MAX_PLUCK_GAMMA)
					{
						Debug.LogFormat( "Changing strum gamma from {0} to {1}", _pluckSettings.strumGamma, f );
						_pluckSettings.strumGamma = f;
						/*
						if (_onSettingsChangedAction != null)
						{
							_onSettingsChangedAction( );
						}
						*/
					}
					else
					{
						Debug.LogWarningFormat( "Strum gamma out of range: {0}", f );
					}
				}
			}
			else
			{
				Debug.LogWarningFormat( "Couldn't get float from '{0}'", s );
			}
			SetStrumGammaText( );
		}

		private void SetSlideFractionText( )
		{
			slideFractionInputField.text = _pluckSettings.slideVolumeIncreaseFraction.ToString( );
		}

		public void OnSlideFractionTextEndEdit()
		{
			string s = slideFractionInputField.text;
			float f;
			if (float.TryParse(s, out f))
			{
				if (!Mathf.Approximately(f, _pluckSettings.slideVolumeIncreaseFraction))
				{
					if (f <= 1f && f >= 0f)
					{
						Debug.LogFormat( "Changing slide fraction from {0} to {1}", _pluckSettings.slideVolumeIncreaseFraction, f );
						_pluckSettings.slideVolumeIncreaseFraction= f;
						/*
						if (_onSettingsChangedAction != null)
						{
							_onSettingsChangedAction( );
						}
						*/
					}
					else
					{
						Debug.LogWarningFormat( "Slide vol fraction out of range: {0}", f );
					}
				}
			}
			else
			{
				Debug.LogWarningFormat( "Couldn't get float from '{0}'", s );
			}
			SetSlideFractionText( );
		}

		private void SetSlideMinText( )
		{
			slideMinInputField.text = _pluckSettings.slideVolumeIncreasedMin.ToString( );
		}

		public void OnSlideMinTextEndEdit( )
		{
			string s = slideMinInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (!Mathf.Approximately( f, _pluckSettings.slideVolumeIncreasedMin))
				{
					if (f <= 1f && f >= 0f)
					{
						Debug.LogFormat( "Changing slide min from {0} to {1}", _pluckSettings.slideVolumeIncreasedMin, f );
						_pluckSettings.slideVolumeIncreasedMin= f;
						/*
						if (_onSettingsChangedAction != null)
						{
							_onSettingsChangedAction( );
						}
						*/
					}
					else
					{
						Debug.LogWarningFormat( "Slide vol min out of range: {0}", f );
					}
				}
			}
			else
			{
				Debug.LogWarningFormat( "Couldn't get float from '{0}'", s );
			}
			SetSlideMinText( );
		}


		private void SetPluckGammaText( )
		{
			pluckGammaInputField.text = _pluckSettings.pluckGamma.ToString( );
		}

		public void OnPluckGammaTextEndEdit( )
		{
			string s = pluckGammaInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (!Mathf.Approximately( f, _pluckSettings.pluckGamma))
				{
					if (f <= Core.Audio.AudioConsts.MAX_PLUCK_GAMMA && f >= -1f * Core.Audio.AudioConsts.MAX_PLUCK_GAMMA)
					{
						Debug.LogFormat( "Changing pluck gamma from {0} to {1}", _pluckSettings.pluckGamma, f );
						_pluckSettings.pluckGamma= f;
						/*
						if (_onSettingsChangedAction != null)
						{
							_onSettingsChangedAction( );
						}
						*/
					}
					else
					{
						Debug.LogWarningFormat( "Pluck gamma out of range: {0}", f );
					}
				}
			}
			else
			{
				Debug.LogWarningFormat( "Couldn't get float from '{0}'", s );
			}
			SetPluckGammaText( );
		}

		private void SetStrumVolMinText( )
		{
			strumVolRangeMinInputField.text = _pluckSettings.strumVolRange.x.ToString( );
		}

		public void OnStrumVolMinTextEndEdit( )
		{
			string s = strumVolRangeMinInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (!Mathf.Approximately( f, _pluckSettings.strumVolRange.x))
				{
					if (f <= _pluckSettings.strumVolRange.y && f >= 0f)
					{
						Debug.LogFormat( "Changing strum volRangeMin from {0} to {1}", _pluckSettings.strumVolRange.x, f );
						_pluckSettings.strumVolRange.x = f;
						/*
						if (_onSettingsChangedAction != null)
						{
							_onSettingsChangedAction( );
						}
						*/
					}
					else
					{
						Debug.LogWarningFormat( "Strum VolMin out of range: {0}", f );
					}
				}
			}
			else
			{
				Debug.LogWarningFormat( "Couldn't get float from '{0}'", s );
			}
			SetStrumVolMinText( );
		}

		private void SetStrumVolMaxText( )
		{
			strumVolRangeMaxInputField.text = _pluckSettings.strumVolRange.y.ToString( );
		}

		public void OnStrumVolMaxTextEndEdit( )
		{
			string s = strumVolRangeMaxInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (!Mathf.Approximately( f, _pluckSettings.strumVolRange.y ))
				{
					if (f >= _pluckSettings.strumVolRange.x && f < Core.Audio.AudioConsts.MAX_VOLUME)
					{
						Debug.LogFormat( "Changing strum volRangeMax from {0} to {1}", _pluckSettings.strumVolRange.y, f );
						_pluckSettings.strumVolRange.y = f;
						/*
						if (_onSettingsChangedAction != null)
						{
							_onSettingsChangedAction( );
						}
						*/
					}
					else
					{
						Debug.LogWarningFormat( "Strum VolMax out of range: {0}", f );
					}
				}
			}
			else
			{
				Debug.LogWarningFormat( "Couldn't get float from '{0}'", s );
			}
			SetStrumVolMaxText( );
		}

		private void SetPluckVolMinText( )
		{
			pluckVolRangeMinInputField.text = _pluckSettings.pluckVolRange.x.ToString( );
		}

		public void OnPluckVolMinTextEndEdit( )
		{
			string s = pluckVolRangeMinInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (!Mathf.Approximately( f, _pluckSettings.pluckVolRange.x ))
				{
					if (f <= _pluckSettings.pluckVolRange.y && f >= 0f)
					{
						Debug.LogFormat( "Changing pluck volRangeMin from {0} to {1}", _pluckSettings.pluckVolRange.x, f );
						_pluckSettings.pluckVolRange.x = f;
						/*
						if (_onSettingsChangedAction != null)
						{
							_onSettingsChangedAction( );
						}
						*/
					}
					else
					{
						Debug.LogWarningFormat( "Pluck VolMin out of range: {0}", f );
					}
				}
			}
			else
			{
				Debug.LogWarningFormat( "Couldn't get float from '{0}'", s );
			}
			SetPluckVolMinText( );
		}

		private void SetPluckVolMaxText( )
		{
			pluckVolRangeMaxInputField.text = _pluckSettings.pluckVolRange.y.ToString( );
		}

		public void OnPluckVolMaxTextEndEdit( )
		{
			string s = pluckVolRangeMaxInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (!Mathf.Approximately( f, _pluckSettings.pluckVolRange.y ))
				{
					if (f >= _pluckSettings.pluckVolRange.x && f < Core.Audio.AudioConsts.MAX_VOLUME)
					{
						Debug.LogFormat( "Changing pluck volRangeMax from {0} to {1}", _pluckSettings.pluckVolRange.y, f );
						_pluckSettings.pluckVolRange.y = f;
						/*
						if (_onSettingsChangedAction != null)
						{
							_onSettingsChangedAction( );
						}
						*/
					}
					else
					{
						Debug.LogWarningFormat( "Pluck VolMax out of range: {0}", f );
					}
				}
			}
			else
			{
				Debug.LogWarningFormat( "Couldn't get float from '{0}'", s );
			}
			SetPluckVolMaxText( );
		}


		private void SetDurationMinText( )
		{
			durationRangeMinInputField.text = _pluckSettings.durationRange.x.ToString( );
		}

		public void OnDurationMinTextEndEdit( )
		{
			string s = durationRangeMinInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (!Mathf.Approximately( f, _pluckSettings.durationRange.x ))
				{
					if (f <= _pluckSettings.durationRange.y && f > -1f)
					{
						Debug.LogFormat( "Changing durationRangeMin from {0} to {1}", _pluckSettings.durationRange.x, f );
						_pluckSettings.durationRange.x = f;
						/*
						if (_onSettingsChangedAction != null)
						{
							_onSettingsChangedAction( );
						}
						*/
					}
					else
					{
						Debug.LogWarningFormat( "DurationMin out of range: {0}", f );
					}
				}
			}
			else
			{
				Debug.LogWarningFormat( "Couldn't get float from '{0}'", s );
			}
			SetDurationMinText( );
		}

		private void SetDurationMaxText( )
		{
			durationRangeMaxInputField.text = _pluckSettings.durationRange.y.ToString( );
		}

		public void OnDurationMaxTextEndEdit( )
		{
			string s = durationRangeMaxInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (!Mathf.Approximately( f, _pluckSettings.durationRange.y ))
				{
					if (f >= _pluckSettings.durationRange.x && f < 2f)
					{
						Debug.LogFormat( "Changing durationRangeMax from {0} to {1}", _pluckSettings.durationRange.y, f );
						_pluckSettings.durationRange.x = f;
						/*
						if (_onSettingsChangedAction != null)
						{
							_onSettingsChangedAction( );
						}
						*/
					}
					else
					{
						Debug.LogWarningFormat( "DurationMax out of range: {0}", f );
					}
				}
			}
			else
			{
				Debug.LogWarningFormat( "Couldn't get float from '{0}'", s );
			}
			SetDurationMaxText( );
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

