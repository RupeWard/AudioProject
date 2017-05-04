using UnityEngine;
using System.Collections;
using RJWS.Graph;

public class SceneControllerTestScene : SceneController_Base
{
	public RectTransform canvasRT;
	public RectTransform permanentButtonsRT;

	override public SceneManager.EScene Scene( )
	{
		return SceneManager.EScene.TestScene;
	}

//	static private readonly bool DEBUG_LOCAL = false;

	void Start( )
	{
		GameObject graphPanelPrefab = Resources.Load<GameObject>( "Graph/Prefabs/GraphPanel" );

		GameObject graphPanelGO = GameObject.Instantiate( graphPanelPrefab );
		GraphPanel graphPanel = graphPanelGO.GetComponent<GraphPanel>( );
		graphPanel.cachedRT.SetParent( canvasRT );
		graphPanel.cachedRT.sizeDelta = new Vector2( canvasRT.rect.width - permanentButtonsRT.rect.width, canvasRT.rect.height );
		graphPanel.Init( );
	}

	protected override void PostAwake( )
	{
	}

	public void QuitScene()
	{
		SceneManager.Instance.SwitchScene( SceneManager.EScene.DevSetup);
	}
}
