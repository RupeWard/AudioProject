using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPeriodicWaveFormOverlay : MonoBehaviour
{
	public InputField frequencyInputField;
	public InputField amplitudeInputField;
	public InputField nameInputField;
	public Dropdown typeDropDown;

	public System.Action onCancelButton;
	public System.Action< RJWS.Audio.PeriodicWaveFormGenerator > onOkButton;

	private float _frequencyHz = 440f;
	private float _amplitude = 1f;

	public void Init(string wfName)
	{
		Debug.Log( "Init" );

		gameObject.SetActive( true );

		nameInputField.text = wfName;
		SetFrequencyInputField( );
		SetAmplitudeInputField( );
	}

	public void Close()
	{
		gameObject.SetActive( false );
	}

	private void SetFrequencyInputField()
	{
		frequencyInputField.text = _frequencyHz.ToString( );
	}

	public void OnFrequencyInputFieldEndEdit( string s )
	{
		float f;
		if (float.TryParse( frequencyInputField.text, out f ))
		{
			if (f > 0f)
			{
				_frequencyHz = f;
			}
		}
		SetFrequencyInputField( );
	}

	private void SetAmplitudeInputField( )
	{
		amplitudeInputField.text = _amplitude.ToString( );
	}

	public void OnAmplitudeInputFieldEndEdit( string s )
	{
		float f;
		if (float.TryParse( amplitudeInputField.text, out f ))
		{
			if (f > 0f)
			{
				_amplitude = f;
			}
		}
		SetAmplitudeInputField( );
	}

	public void HandleCancelButton()
	{
		Close( );
	}

	public void OnTypeDropDownValueChanged(string s)
	{
//		Debug.Log( "TypeDropDown changed to '" + s + "' value=" + typeDropDown.value + " s=" + typeDropDown.options[typeDropDown.value].text );
	}

	public void HandleOkButton()
	{
		string wfType = typeDropDown.options[typeDropDown.value].text;
//		Debug.Log( "HandleOkButton type=" + typeDropDown.value + " s=" + typeDropDown.options[typeDropDown.value].text );

		RJWS.Audio.PeriodicWaveFormGenerator newGenerator = null;
		switch (wfType)
		{
			case "Sine":
				{
					newGenerator = new RJWS.Audio.WaveFormGenerator_Sine( nameInputField.text, 1f / _frequencyHz, _amplitude );
					break;
				}
			case "Saw":
				{
					newGenerator = new RJWS.Audio.WaveFormGenerator_Saw( nameInputField.text, 1f / _frequencyHz, _amplitude );
					break;
				}
			default:
				{
					Debug.LogError( "Unhandled type: " + wfType );
					break;
				}
		}
		if (onOkButton != null)
		{
			onOkButton( newGenerator );
		}
		Close( );
	}
}
