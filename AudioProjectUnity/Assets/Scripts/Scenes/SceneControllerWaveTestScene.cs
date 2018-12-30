using UnityEngine;
using System.Collections;
using RJWS.Core.DebugDescribable;

public class SceneControllerWaveTestScene: SceneController_Base 
{
	#region inspector hooks
	public bool debugMe = true;

	public UnityEngine.UI.InputField freqInputField;
	public float startingFrequency = (float)RJWS.Core.Audio.AudioConsts.CONCERT_A;
	private float _frequency;

	public UnityEngine.UI.InputField attenInputField;
	public float startingAttenuation = (float)RJWS.Core.Audio.KSRingBufferF.DEFAULT_GUITAR_ATTENUATION;
	private float _attenuation;

	private const float MIN_FREQ = 100f;

	public RJWS.Audio.AudioStringBehaviour audioStringBehaviour;

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
	}

	override protected void PostAwake()
	{
		_frequency = startingFrequency;
		_attenuation = startingAttenuation;
	}

	#endregion SceneController_Base


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
			if (f < MIN_FREQ)
			{
				Debug.LogErrorFormat( this, "Frequency out of range: {0} < {1}", f, MIN_FREQ );
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
			if (f < RJWS.Core.Audio.KSRingBufferF.MIN_ATTENUATION || f >= 1f)
			{
				Debug.LogErrorFormat( this, "Attenuation out of range: {0} < {1}", f, RJWS.Core.Audio.KSRingBufferF.MIN_ATTENUATION );
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

	public void HandlePluckButton()
	{
		audioStringBehaviour.Pluck( _frequency, _attenuation );
	}

	public void HandleResetButton()
	{
		_attenuation = RJWS.Core.Audio.KSRingBufferF.DEFAULT_GUITAR_ATTENUATION;
		SetAttenInputText( );
	}
}
