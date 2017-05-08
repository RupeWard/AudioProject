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

	protected override void PostStart( )
	{
		GameObject graphPanelPrefab = Resources.Load<GameObject>( "Graph/Prefabs/GraphPanel" );

		GameObject graphPanelGO = GameObject.Instantiate( graphPanelPrefab );
		GraphPanel graphPanel = graphPanelGO.GetComponent<GraphPanel>( );
		graphPanel.cachedRT.SetParent( canvasRT );
		permanentButtonsRT.sizeDelta = new Vector2( Mathf.Max( permanentButtonsRT.sizeDelta.x, RJWS.AppManager.Instance.minClickablePixels ), permanentButtonsRT.sizeDelta.y );

		foreach (Transform t in permanentButtonsRT.transform)
		{
			RectTransform rt = t.GetComponent<RectTransform>( );
			if (rt != null)
			{
				float buttonWidth = permanentButtonsRT.sizeDelta.x - 4f;
                rt.sizeDelta = new Vector2(  buttonWidth, buttonWidth);
			}
		}
		graphPanel.cachedRT.sizeDelta = new Vector2( canvasRT.rect.width - permanentButtonsRT.rect.width, canvasRT.rect.height );
		graphPanel.Init( );
	}

	protected override void PostAwake( )
	{
	}

	public void QuitScene()
	{
		Debug.Log( "Leaving scene" );
		SceneManager.Instance.SwitchScene( SceneManager.EScene.DevSetup);
	}
}
