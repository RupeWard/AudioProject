using UnityEngine;
using System.Collections.Generic;

namespace RJWS.Audio.UI
{
	public class PluckSettingsPanel : MonoBehaviour
	{
		private GuitarSettings _guitarSettings;
		private PluckSettings _pluckSettings;
		private GuitarSettingsPanel _settingsPanel;

		public UnityEngine.UI.InputField attenuationInputField;
		public UnityEngine.UI.Toggle useReverbToggle;
		public UnityEngine.UI.Dropdown pluckerTypeDropDown;

		public System.Action _onSettingsChangedAction;

		private Dictionary<EPluckerType, string> _dropdownTextMap = new Dictionary<EPluckerType, string>( )
		{
			{ EPluckerType.BasicDrag, "B-Drag" },
			{ EPluckerType.BasicUp, "B-Up" }
		};

		public void Init( GuitarSettingsPanel gsp, PluckSettings psp, GuitarSettings gs, System.Action osa)
		{
			_settingsPanel = gsp;
			_guitarSettings = gs;
			_onSettingsChangedAction = osa;

			gameObject.SetActive( true );

			SetAttenuationText( );
			if (useReverbToggle.isOn != _guitarSettings.useReverb)
			{
				_guitarSettings.useReverb = !_guitarSettings.useReverb;
				useReverbToggle.isOn = !useReverbToggle.isOn;
			}
			
			List<UnityEngine.UI.Dropdown.OptionData> options = new List<UnityEngine.UI.Dropdown.OptionData>( );

			int index = 0;
			int dropDownIndex = -1;
			foreach (KeyValuePair<EPluckerType, string> kvp in  _dropdownTextMap)
			{
				options.Add( new UnityEngine.UI.Dropdown.OptionData( kvp.Value, null ) );
				if (kvp.Key == _guitarSettings.pluckerType)
				{
					dropDownIndex = index;
				}
				index++;
			}
			pluckerTypeDropDown.options = options;

			if (dropDownIndex == -1)
			{
				Debug.LogErrorFormat( "Couldn;t find in dropdown: {0} => {1}", _guitarSettings.pluckerType, _dropdownTextMap[_guitarSettings.pluckerType] );
			}
			else
			{
				pluckerTypeDropDown.value = dropDownIndex;
			}
		}

		public bool debugMe = true;

		private void SetAttenuationText()
		{
			attenuationInputField.text = _guitarSettings.attenuation.ToString();
		}

		public void OnAttenuationTextEndEdit()
		{
			string s = attenuationInputField.text;
			float f;
			if (float.TryParse(s, out f))
			{
				if (!Mathf.Approximately(f, _guitarSettings.attenuation))
				{
					if (f < 1f && f > Core.Audio.AudioConsts.MIN_GUITAR_ATTENUATION)
					{
						Debug.LogFormat( "Changing attenuation from {0} to {1}", _guitarSettings.attenuation, f );
						_guitarSettings.attenuation = f;
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
			SetAttenuationText( );
		}

		public void OnReverbToggleChanged()
		{
			_guitarSettings.useReverb = !_guitarSettings.useReverb;
			Debug.LogFormat( "Changing user reverb from {0} to {1}", !_guitarSettings.useReverb, _guitarSettings.useReverb );
			if (_onSettingsChangedAction != null)
			{
				_onSettingsChangedAction( );
			}
		}

		public void OnPluckerTypeDropDownChanged(int i)
		{
			bool changed = false;

			string pt = pluckerTypeDropDown.options[pluckerTypeDropDown.value].text;
			if (pt == _dropdownTextMap[EPluckerType.BasicDrag])
			{
				if (_guitarSettings.pluckerType != EPluckerType.BasicDrag)
				{
					_guitarSettings.pluckerType = EPluckerType.BasicDrag;
					changed = true;
				}
			}
			else if (pt == _dropdownTextMap[EPluckerType.BasicUp])
			{
				if (_guitarSettings.pluckerType != EPluckerType.BasicUp)
				{
					_guitarSettings.pluckerType = EPluckerType.BasicUp;
					changed = true;
				}
			}
			if (changed)
			{
				if (_onSettingsChangedAction != null)
				{
					_onSettingsChangedAction( );
				}
				Debug.LogFormat( "Changed plucker type to {0}", _guitarSettings.pluckerType );
			}
		}

		public void HandleDoneButton()
		{
			gameObject.SetActive( false );
		}
	}
}

