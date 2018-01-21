using UnityEngine;
using System.Collections;

using RJWS.UI.Scrollable;
using RJWS.Core.DebugDescribable;

public class SceneControllerTestScene2 : SceneController_Base
{
	public RectTransform canvasRT;
	public RectTransform permanentButtonsRT;

	public NewPeriodicWaveFormOverlay newPeriodicWaveFormOverlay;

	private XX_GraphViewPanel _graphViewPanel;
	public GameObject graphViewPanelPrefab;

	public AudioClip testAudioClip;

	override public SceneManager.EScene Scene( )
	{
		return SceneManager.EScene.TestScene2;
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
		GameObject scrollablePanelPrefab = Resources.Load<GameObject>( "Prefabs/UI/ScrollablePanel" );

		_scrollablePanel = GameObject.Instantiate( scrollablePanelPrefab ).GetComponent<ScrollablePanel>( );
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
//		Debug.Log( "Press" );

		newPeriodicWaveFormOverlay.Init( "WF" );

	}

	public void HandleLoadWaveformButton()
	{
		float lengthSecs = testAudioClip.length;
		Debug.Log( "Clip frequency = " + testAudioClip.frequency+", L="+lengthSecs );
		int nSamples = Mathf.FloorToInt( testAudioClip.frequency * lengthSecs);

		float[] buffer = new float[nSamples];

		if (testAudioClip.GetData( buffer, 0 ))
		{
			RJWS.Audio.WaveFormGenerator_Sampled wfg = new RJWS.Audio.WaveFormGenerator_Sampled( "WAV", buffer, (double)1 / testAudioClip.frequency );
			if (_graphViewPanel == null)
			{
				_graphViewPanel = GameObject.Instantiate( graphViewPanelPrefab ).GetComponent<XX_GraphViewPanel>( );
				_graphViewPanel.Init( _scrollablePanel );
			}

			Vector2 yRange = wfg.GetValueRange( );
			_graphViewPanel.xRange = new Vector2( 0f, testAudioClip.length );
			yRange.x = yRange.x - 0.1f * yRange.magnitude;
			yRange.y = yRange.y + 0.1f * yRange.magnitude;
			_graphViewPanel.yRange = yRange;

			_graphViewPanel.ChangeGraph( wfg, nPerWavelength );

			/*


			//_graphViewPanel.DrawDefaultAxes( );

			Debug.Log( "Got " + nSamples + " samples with xrange = " + _graphViewPanel.xRange + ", yrange = " + yRange );

//			RJWS.Grph.Graph newGraph = new RJWS.Grph.Graph( buffer, _graphPanel.xRange);
			*/
		}
		else
		{
			Debug.LogError( "Failed to get " + nSamples + " samples" );
		}

	}

	public int nPerWavelength = 32;

	public void NewWaveformCreated( RJWS.Audio.PeriodicWaveFormGenerator wfg, int numPeriods )
	{
		if (wfg != null)
		{
			Debug.Log( wfg.DebugDescribe( ) );
		}
		if (_graphViewPanel == null)
		{
			_graphViewPanel = GameObject.Instantiate( graphViewPanelPrefab ).GetComponent< XX_GraphViewPanel>();
			_graphViewPanel.Init( _scrollablePanel);
		}
		_graphViewPanel.xRange = new Vector2( 0f, wfg.waveLengthSecs * numPeriods );
		Vector2 yRange = wfg.GetValueRange( );
		float yExtra = 0.1f * yRange.magnitude;
		yRange.x = yRange.x - yExtra;
		yRange.y = yRange.y + yExtra;
		_graphViewPanel.yRange = yRange;
		//_graphViewPanel.DrawDefaultAxes( );

//		RJWS.Grph.Graph newGraph = new RJWS.Grph.Graph( wfg, _graphPanel.xRange, numPeriods* nPerWavelength +1 );
		_graphViewPanel.ChangeGraph( wfg, nPerWavelength);
	}
}
