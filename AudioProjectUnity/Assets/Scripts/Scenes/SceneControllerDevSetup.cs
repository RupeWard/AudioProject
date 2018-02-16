using UnityEngine;
using System.Collections;
using RJWS.Core.DebugDescribable;

public class SceneControllerDevSetup: SceneController_Base 
{
	#region inspector hooks

	public UnityEngine.UI.Text versionText;

	#endregion inspector hooks

	#region event handlers

	public void HandleTestButtonPressed( )
	{
		SceneManager.Instance.SwitchScene( SceneManager.EScene.TestScene);
	}

	public void HandleQuitButtonPressed( )
	{
		Application.Quit();
	}

	#endregion event handlers

	#region SceneController_Base

	override public SceneManager.EScene Scene ()
	{
		return SceneManager.EScene.DevSetup;
	}

	override protected void PostStart()
	{
		versionText.text = RJWS.Core.Version.Version.versionNumber.DebugDescribe( );
	}

	override protected void PostAwake()
	{
	}

#endregion SceneController_Base

}
