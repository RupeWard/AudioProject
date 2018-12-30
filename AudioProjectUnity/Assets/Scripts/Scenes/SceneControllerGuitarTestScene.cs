using UnityEngine;
using System.Collections;
using RJWS.Core.DebugDescribable;

public class SceneControllerGuitarTestScene: SceneController_Base 
{
	#region inspector hooks
	public bool debugMe = true;

	public RJWS.Audio.UI.AudioStringSettingsPanel stringSettingsPanel;

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
		return SceneManager.EScene.GuitarTestScene;
	}

	override protected void PostStart()
	{
	}

	override protected void PostAwake()
	{
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

}
