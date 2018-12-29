using UnityEngine;
using System.Collections;
using RJWS.Core.DebugDescribable;

public class SceneControllerWaveTestScene: SceneController_Base 
{
	#region inspector hooks


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
	}

	override protected void PostAwake()
	{
	}

#endregion SceneController_Base

}
