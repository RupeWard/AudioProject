using UnityEngine;
using System.Collections;

using RJWS.UI.Scrollable;
using RJWS.Core.DebugDescribable;

public class SceneControllerTestScene : SceneController_Base
{
	public RectTransform canvasRT;
	public RectTransform permanentButtonsRT;

	public NewPeriodicWaveFormOverlay newPeriodicWaveFormOverlay;

	private GraphPanel _graphPanel;
	public GameObject graphPanelPrefab;

	override public SceneManager.EScene Scene( )
	{
		return SceneManager.EScene.TestScene;
	}

	//	static private readonly bool DEBUG_LOCAL = false;

	private ScrollablePanel _scrollablePanel;
	public ScrollablePanelSettings scrollablePanelSettings;

	protected override void PostStart( )
	{
		RefreshScrollablePanel( );
		newPeriodicWaveFormOverlay.Close( );
	}

	private void RefreshScrollablePanel()
	{
		if (_scrollablePanel != null)
		{
			GameObject.Destroy( _scrollablePanel.gameObject );
		}
		GameObject graphPanelPrefab = Resources.Load<GameObject>( "Prefabs/UI/ScrollablePanel" );

		_scrollablePanel = GameObject.Instantiate( graphPanelPrefab ).GetComponent<ScrollablePanel>( );
		_scrollablePanel.cachedRT.SetParent( canvasRT );
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

		_scrollablePanel.cachedRT.sizeDelta = new Vector2( canvasRT.rect.width - permanentButtonsRT.rect.width, canvasRT.rect.height );
		if (scrollablePanelSettings != null)
		{
			_scrollablePanel.settings = scrollablePanelSettings.Clone< ScrollablePanelSettings >();
		}
		else
		{
			scrollablePanelSettings = _scrollablePanel.settings.Clone<ScrollablePanelSettings>( );
		}
		_scrollablePanel.Init( );

	}

	protected override void PostAwake( )
	{
		newPeriodicWaveFormOverlay.onOkButton += NewWaveformCreated;
	}

	public void QuitScene()
	{
		Debug.Log( "Leaving scene" );
		SceneManager.Instance.SwitchScene( SceneManager.EScene.DevSetup);
	}

	public void HandleRefreshButton()
	{
		RefreshScrollablePanel( );
	}

	public void HandleNewWaveFormButton()
	{
		Debug.Log( "Press" );

		newPeriodicWaveFormOverlay.Init( "WF" );

	}

	public void NewWaveformCreated( RJWS.Audio.PeriodicWaveFormGenerator wfg )
	{
		if (wfg != null)
		{
			Debug.Log( wfg.DebugDescribe( ) );
		}
		if (_graphPanel == null)
		{
			_graphPanel = GameObject.Instantiate( graphPanelPrefab ).GetComponent< GraphPanel>();
			_graphPanel.Init( _scrollablePanel);
			_graphPanel.xRange = new Vector2( 0f, wfg.waveLengthSecs );
			_graphPanel.yRange = wfg.GetValueRange( );
			_graphPanel.DrawDefaultAxes( );
		}
	}
}
