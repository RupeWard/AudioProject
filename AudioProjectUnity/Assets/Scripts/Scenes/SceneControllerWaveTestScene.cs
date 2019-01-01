using UnityEngine;
using System.Collections;
using RJWS.Core.DebugDescribable;

public class SceneControllerWaveTestScene: SceneController_Base 
{
	#region inspector hooks
	public bool debugMe = true;

	public UnityEngine.UI.InputField freqInputField;
	public float startingFrequency = RJWS.Core.Audio.AudioConsts.CONCERT_A;
	private float _frequency;

	public UnityEngine.UI.InputField attenInputField;
	public float startingAttenuation = (float)RJWS.Core.Audio.AudioConsts.DEFAULT_GUITAR_ATTENUATION;
	private float _attenuation;

	public UnityEngine.UI.InputField lowPassCutOffFreqInputField;
	public float startingLowPassCutOffFreq = (float)RJWS.Audio.AudioStringBehaviour.DEFAULT_LOWPASSFREQ;
	private float _lowPassCutOffreq;

	private bool _useReverb = false;
	public UnityEngine.UI.Toggle reverbToggle;

	public RJWS.Audio.UI.AudioStringSettingsPanel stringSettingsPanel;

	public RectTransform stringPanelsHolder;
	public GameObject stringPanelPrefab;
	 
	private RJWS.Audio.AudioStringBehaviour[] _audioStringBehaviours;

	#endregion inspector hooks

	#region event handlers

	public void HandleQuitButtonPressed( )
	{
		SceneManager.Instance.SwitchScene( SceneManager.EScene.DevSetup);
	}

	#endregion event handlers

	#region SceneController_Base

	override public SceneManager.EScene Scene ()
	{
		return SceneManager.EScene.WaveTestScene;
	}

	override protected void PostStart()
	{
		SetFreqInputText( );
		SetAttenInputText( );
		SetLowPassFreqCutOffInputText( );
		if (reverbToggle.isOn != _useReverb)
		{
			_useReverb = !_useReverb;
			reverbToggle.isOn = !reverbToggle.isOn;
		}
		for (int i = 0; i < _audioStringBehaviours.Length; i++)
		{
			_audioStringBehaviours[i].UseReverb( _useReverb );
			if (i < RJWS.Core.Audio.AudioConsts.s_standardGuitarTuning.Count)
			{
				_audioStringBehaviours[i].SetFrequency( RJWS.Core.Audio.AudioConsts.s_standardGuitarTuning[i] );
			}

			GameObject go = GameObject.Instantiate( stringPanelPrefab );
			go.GetComponent<RectTransform>( ).SetParent( stringPanelsHolder );
			go.transform.localScale = Vector3.one;
			RJWS.Audio.UI.AudioStringPanel stringPanel = go.GetComponent<RJWS.Audio.UI.AudioStringPanel>( );
			stringPanel.Init( _audioStringBehaviours[i], OpenStringSettingsPanel );
		}
	}

	override protected void PostAwake()
	{
		_audioStringBehaviours = transform.GetComponentsInChildren<RJWS.Audio.AudioStringBehaviour>( );
		_frequency = startingFrequency;
		_attenuation = startingAttenuation;
		_lowPassCutOffreq = startingLowPassCutOffFreq;

		stringSettingsPanel.gameObject.SetActive( false );
	}

	#endregion SceneController_Base

	public void OpenStringPanel(RJWS.Audio.UI.AudioStringPanel asp)
	{
		stringSettingsPanel.Init( asp.audioStringBehavuour, OnStringSettingsChanged );
	}

	public void OpenStringSettingsPanel(RJWS.Audio.AudioStringBehaviour asb)
	{
		stringSettingsPanel.Init( asb, OnStringSettingsChanged );
	}

	private void OnStringSettingsChanged(RJWS.Audio.AudioStringBehaviour asb)
	{
		Debug.Log( "String settings changed" );
	}

	private void SetFreqInputText()
	{
		freqInputField.text = _frequency.ToString( );
	}

	public void HandleFreqEndEdit()
	{
		string s = freqInputField.text;
		float f;
		if (float.TryParse(s, out f))
		{
			if (f < RJWS.Audio.AudioString.MIN_FREQ)
			{
				Debug.LogErrorFormat( this, "Frequency out of range: {0} < {1}", f, RJWS.Audio.AudioString.MIN_FREQ );
			}
			else
			{
				_frequency = f;
				if (debugMe)
				{
					Debug.LogFormat( "Frequency changed to {0}", _frequency );
				}
			}
		}
		else
		{
			Debug.LogErrorFormat( this, "Couldn't get Frequency from: {0} ", s);
		}
		SetFreqInputText( );
	}

	private void SetAttenInputText( )
	{
		attenInputField.text = _attenuation.ToString( );
	}

	public void HandleAttenEndEdit( )
	{
		string s = attenInputField.text;
		float f;
		if (float.TryParse( s, out f ))
		{
			if (f < RJWS.Core.Audio.AudioConsts.MIN_GUITAR_ATTENUATION || f >= 1f)
			{
				Debug.LogErrorFormat( this, "Attenuation out of range: {0} < {1}", f, RJWS.Core.Audio.AudioConsts.MIN_GUITAR_ATTENUATION );
			}
			else
			{
				_attenuation = f;
				if (debugMe)
				{
					Debug.LogFormat( "Attenuation changed to {0}", _attenuation);
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
		lowPassCutOffFreqInputField.text = _lowPassCutOffreq.ToString( );
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
				_lowPassCutOffreq = f;
				_audioStringBehaviours[0].SetLowPassFrequency( _lowPassCutOffreq );
				if (debugMe)
				{
					Debug.LogFormat( "LowPass Frequency changed to {0}", _frequency );
				}
			}
		}
		else
		{
			Debug.LogErrorFormat( this, "Couldn't get LowPass Frequency from: {0} ", s );
		}
		SetLowPassFreqCutOffInputText( );
	}


	public void HandlePluckButton()
	{
		_audioStringBehaviours[0].Pluck(1f);
	}

	public void HandleResetButton()
	{
		_attenuation = RJWS.Core.Audio.AudioConsts.DEFAULT_GUITAR_ATTENUATION;
		SetAttenInputText( );

		_lowPassCutOffreq = RJWS.Audio.AudioStringBehaviour.DEFAULT_LOWPASSFREQ;
		SetLowPassFreqCutOffInputText( );

		_audioStringBehaviours[0].SetLowPassFrequency( _lowPassCutOffreq );
	}

	public void HandleReverbToggleChanged(bool v)
	{
		_useReverb = reverbToggle.isOn;
		_audioStringBehaviours[0].UseReverb( _useReverb );
	}
}
