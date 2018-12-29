using UnityEngine;
using System.Collections;
using RJWS.Core.DebugDescribable;

public class SceneControllerWaveTestScene: SceneController_Base 
{
	#region inspector hooks

	public UnityEngine.UI.InputField freqInputField;
	public float startingFrequency = (float)RJWS.Core.Audio.AudioConsts.CONCERT_A;
	private float _frequency;

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
	}

	override protected void PostAwake()
	{
		_frequency = startingFrequency;
	}

	#endregion SceneController_Base

	const float MIN_FREQ = 100f;

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
			if (_frequency < MIN_FREQ)
			{
				Debug.LogErrorFormat( this, "Frequency out of range: {0} < {1}", f, MIN_FREQ );
			}
			else
			{
				_frequency = f;
			}
		}
		else
		{
			Debug.LogErrorFormat( this, "Couldn't get Frequency from: {0} ", s);
		}
		SetFreqInputText( );
	}

	public void HandlePluckButton()
	{
		audioStringBehaviour.Pluck( _frequency );
	}
}
