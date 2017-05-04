using UnityEngine;
using System.Collections;

public class SceneControllerTestScene : SceneController_Base
{
	override public SceneManager.EScene Scene( )
	{
		return SceneManager.EScene.TestScene;
	}

//	static private readonly bool DEBUG_LOCAL = false;


	// Use this for initialization
	void Start( )
	{
	}

	protected override void PostAwake( )
	{
	}

	public void QuitScene()
	{
		SceneManager.Instance.SwitchScene( SceneManager.EScene.DevSetup);
	}
}
