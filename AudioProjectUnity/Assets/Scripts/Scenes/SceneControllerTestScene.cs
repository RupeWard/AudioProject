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

	private GraphPanel _graphPanel;
	public GraphPanelSettings graphSettings;

	protected override void PostStart( )
	{
		RefreshGraphPanel( );
	}

	private void RefreshGraphPanel()
	{
		if (_graphPanel != null)
		{
			GameObject.Destroy( _graphPanel.gameObject );
		}
		GameObject graphPanelPrefab = Resources.Load<GameObject>( "Graph/Prefabs/GraphPanel" );

		_graphPanel = GameObject.Instantiate( graphPanelPrefab ).GetComponent<GraphPanel>( );
		_graphPanel.cachedRT.SetParent( canvasRT );
		permanentButtonsRT.sizeDelta = new Vector2( Mathf.Max( permanentButtonsRT.sizeDelta.x, RJWS.AppManager.Instance.minClickablePixels ), permanentButtonsRT.sizeDelta.y );

		foreach (Transform t in permanentButtonsRT.transform)
		{
			RectTransform rt = t.GetComponent<RectTransform>( );
			if (rt != null)
			{
				float buttonWidth = permanentButtonsRT.sizeDelta.x - 4f;
				rt.sizeDelta = new Vector2( buttonWidth, buttonWidth );
			}
		}

		_graphPanel.cachedRT.sizeDelta = new Vector2( canvasRT.rect.width - permanentButtonsRT.rect.width, canvasRT.rect.height );
		if (graphSettings != null)
		{
			_graphPanel.graphPanelSettings = graphSettings.Clone< GraphPanelSettings >();
		}
		else
		{
			graphSettings = _graphPanel.graphPanelSettings.Clone<GraphPanelSettings>( );
		}
		_graphPanel.Init( );

	}

	protected override void PostAwake( )
	{
	}

	public void QuitScene()
	{
		Debug.Log( "Leaving scene" );
		SceneManager.Instance.SwitchScene( SceneManager.EScene.DevSetup);
	}

	public void HandleRefreshButton()
	{
		RefreshGraphPanel( );
	}
}
