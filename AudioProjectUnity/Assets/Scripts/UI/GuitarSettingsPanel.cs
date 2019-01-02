using UnityEngine;
using System.Collections.Generic;

namespace RJWS.Audio.UI
{
	public class GuitarSettingsPanel : MonoBehaviour
	{
		private GuitarSettings _settings;
		private PluckSettings _pluckSettings;

		public UnityEngine.UI.InputField attenuationInputField;
		public UnityEngine.UI.Toggle useReverbToggle;
		public UnityEngine.UI.Dropdown pluckerTypeDropDown;
		public UnityEngine.UI.InputField colliderSizeInputField;

		public System.Action _onSettingsChangedAction;
		public PluckSettingsPanel pluckSettingsPanel;
		public GameObject pluckSettingsButton;

		private Dictionary<EPluckerType, string> _dropdownTextMap = new Dictionary<EPluckerType, string>( )
		{
			{ EPluckerType.BasicDrag, "B-Drag" },
			{ EPluckerType.BasicStrum, "B-Strum" },
			{ EPluckerType.BasicUp, "B-Up" }
		};

		public void Init( GuitarSettings gs, PluckSettings ps, System.Action osa)
		{
			_settings = gs;
			_pluckSettings = ps;

			_onSettingsChangedAction = osa;

			pluckSettingsButton.SetActive( _settings.pluckerType == EPluckerType.BasicDrag );

			gameObject.SetActive( true );

			SetAttenuationText( );
			SetColliderSizeText( );
			if (useReverbToggle.isOn != _settings.useReverb)
			{
				_settings.useReverb = !_settings.useReverb;
				useReverbToggle.isOn = !useReverbToggle.isOn;
			}
			
			List<UnityEngine.UI.Dropdown.OptionData> options = new List<UnityEngine.UI.Dropdown.OptionData>( );

			int index = 0;
			int dropDownIndex = -1;
			foreach (KeyValuePair<EPluckerType, string> kvp in  _dropdownTextMap)
			{
				options.Add( new UnityEngine.UI.Dropdown.OptionData( kvp.Value, null ) );
				if (kvp.Key == _settings.pluckerType)
				{
					dropDownIndex = index;
				}
				index++;
			}
			pluckerTypeDropDown.options = options;

			if (dropDownIndex == -1)
			{
				Debug.LogErrorFormat( "Couldn;t find in dropdown: {0} => {1}", _settings.pluckerType, _dropdownTextMap[_settings.pluckerType] );
			}
			else
			{
				pluckerTypeDropDown.value = dropDownIndex;
			}
		}

		public bool debugMe = true;

		private void Awake()
		{
			if (pluckSettingsPanel == null)
			{
				pluckSettingsPanel = GameObject.FindObjectOfType<PluckSettingsPanel>( );
			}
			if (pluckSettingsPanel == null)
			{
				throw new System.Exception( "No pluckSettingsPanel" );
			}
			pluckSettingsPanel.gameObject.SetActive( false );
		}

		private void SetAttenuationText()
		{
			attenuationInputField.text = _settings.attenuation.ToString();
		}

		public void OnAttenuationTextEndEdit()
		{
			string s = attenuationInputField.text;
			float f;
			if (float.TryParse(s, out f))
			{
				if (!Mathf.Approximately(f, _settings.attenuation))
				{
					if (f < 1f && f > Core.Audio.AudioConsts.MIN_GUITAR_ATTENUATION)
					{
						Debug.LogFormat( "Changing attenuation from {0} to {1}", _settings.attenuation, f );
						_settings.attenuation = f;
						DoOnSettingsChangeAction( );
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

		private void SetColliderSizeText( )
		{
			colliderSizeInputField.text = _settings.stringColliderSize.ToString( );
		}

		public void OnColliderSizeTextEndEdit( )
		{
			string s = colliderSizeInputField.text;
			float f;
			if (float.TryParse( s, out f ))
			{
				if (!Mathf.Approximately( f, _settings.stringColliderSize))
				{
					if (f <= 1f && f > 0f)
					{
						Debug.LogFormat( "Changing collider size from {0} to {1}", _settings.stringColliderSize, f );
						_settings.stringColliderSize = f;
						DoOnSettingsChangeAction( );
					}
					else
					{
						Debug.LogWarningFormat( "Collider size out of range: {0}", f );
					}
				}
			}
			else
			{
				Debug.LogWarningFormat( "Couldn't get float from '{0}'", s );
			}
			SetColliderSizeText( );
		}


		public void OnReverbToggleChanged()
		{
			_settings.useReverb = !_settings.useReverb;
			Debug.LogFormat( "Changing user reverb from {0} to {1}", !_settings.useReverb, _settings.useReverb );
			DoOnSettingsChangeAction( );
		}

		public void OnPluckerTypeDropDownChanged(int i)
		{
			bool changed = false;

			string pt = pluckerTypeDropDown.options[pluckerTypeDropDown.value].text;
			if (pt == _dropdownTextMap[EPluckerType.BasicDrag])
			{
				if (_settings.pluckerType != EPluckerType.BasicDrag)
				{
					_settings.pluckerType = EPluckerType.BasicDrag;
					changed = true;
				}
			}
			else if (pt == _dropdownTextMap[EPluckerType.BasicUp])
			{
				if (_settings.pluckerType != EPluckerType.BasicUp)
				{
					_settings.pluckerType = EPluckerType.BasicUp;
					changed = true;
				}
			}
			else if (pt == _dropdownTextMap[EPluckerType.BasicStrum])
			{
				if (_settings.pluckerType != EPluckerType.BasicStrum)
				{
					_settings.pluckerType = EPluckerType.BasicStrum;
					changed = true;
				}
			}
			if (changed)
			{
				DoOnSettingsChangeAction( );
				Debug.LogFormat( "Changed plucker type to {0}", _settings.pluckerType );
			}
		}

		private void DoOnSettingsChangeAction()
		{
			if (_onSettingsChangedAction != null)
			{
				_onSettingsChangedAction( );
			}
			pluckSettingsButton.SetActive( _settings.pluckerType == EPluckerType.BasicDrag );
		}

		public void HandlePluckSettingsButton()
		{
			pluckSettingsPanel.Init( this, _pluckSettings, _settings,  DoOnSettingsChangeAction);
		}

		public void HandleDoneButton()
		{
			gameObject.SetActive( false );
		}
	}
}

