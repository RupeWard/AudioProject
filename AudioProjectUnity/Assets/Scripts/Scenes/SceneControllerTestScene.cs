using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RJWS.UI.Scrollable;
using RJWS.Core.DebugDescribable;
using RJWS.Core.Extensions;

using RJWS.Graph;
using RJWS.Graph.Display;

public class SceneControllerTestScene : SceneController_Base
{
	public GraphDisplaySettings graphDisplaySettings = new GraphDisplaySettings( );

	public RectTransform canvasRT;
	public RectTransform permanentButtonsRT;

	public NewPeriodicWaveFormOverlay newPeriodicWaveFormOverlay;

	private GraphViewPanel _graphViewPanel;
	public GameObject graphViewPanelPrefab;

	public AudioClip[] testAudioClips = new AudioClip[2];

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

	public void HandleLoadWaveformButton(int n)
	{
		if (n >= testAudioClips.Length)
		{
			Debug.LogError( "Only " + testAudioClips.Length + " testAudioClips, not "+n );
			return;
		}
		AudioClip testAudioClip = testAudioClips[n];
		if (testAudioClip == null)
		{
			Debug.LogError( "Null testaudioclip " +n+" of "+ testAudioClips.Length );
			return;
		}
		float lengthSecs = testAudioClip.length;
		Debug.Log( "Clip frequency = " + testAudioClip.frequency+", L="+lengthSecs );
		int nSamples = Mathf.FloorToInt( testAudioClip.frequency * lengthSecs);

		float[] buffer = new float[nSamples];

		if (testAudioClip.GetData( buffer, 0 ))
		{
			RJWS.Audio.WaveFormGenerator_Sampled wfg = new RJWS.Audio.WaveFormGenerator_Sampled( "WAV", buffer, (double)1 / testAudioClip.frequency );
			if (_graphViewPanel == null)
			{
				_graphViewPanel = GameObject.Instantiate( graphViewPanelPrefab ).GetComponent<GraphViewPanel>( );
				_graphViewPanel.Init( _scrollablePanel, graphDisplaySettings );
			}

			Vector2 xRange = new Vector2( 0f, testAudioClip.length );
			_graphViewPanel.ChangeGraph( wfg, nFractionalPerWavelength, nSampledPerWavelength, xRange );
			_graphViewPanel.AddAxes(
				new List<AxisDefn>( )
				{
					// Fixed only (with auto)

					AxisDefn.CreateFixed( RJWS.EOrthoDirection.Vertical, xRange.x ),
					AxisDefn.CreateFixed( RJWS.EOrthoDirection.Vertical, xRange.y ),
					AxisDefn.CreateFixed( RJWS.EOrthoDirection.Vertical, xRange.MidPoint() ),
					AxisDefn.CreateFixed( RJWS.EOrthoDirection.Horizontal, 0f ),
					AxisDefn.CreateFixed( RJWS.EOrthoDirection.Horizontal, 0.9f * wfg.GetValueRange().y ),

					/* 
					// fractional only (no auto)

					XX_AxisDefn.CreateFractional( RJWS.EOrthoDirection.Vertical, 0f ),
					XX_AxisDefn.CreateFractional( RJWS.EOrthoDirection.Vertical, 0.75f ),
					XX_AxisDefn.CreateFractional( RJWS.EOrthoDirection.Vertical, 1f ),
					XX_AxisDefn.CreateFractional( RJWS.EOrthoDirection.Horizontal, 0f ),
					XX_AxisDefn.CreateFractional( RJWS.EOrthoDirection.Horizontal, .5f ),
                    XX_AxisDefn.CreateFractional( RJWS.EOrthoDirection.Horizontal, 1f )
					*/
				}
			);


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

	public int nFractionalPerWavelength = 32;
	public int nSampledPerWavelength = 20;

	public void NewWaveformCreated( RJWS.Audio.PeriodicWaveFormGenerator wfg, int numPeriods )
	{
		if (wfg != null)
		{
			Debug.Log( wfg.DebugDescribe( ) );
		}
		if (_graphViewPanel == null)
		{
			_graphViewPanel = GameObject.Instantiate( graphViewPanelPrefab ).GetComponent< GraphViewPanel>();
			_graphViewPanel.Init( _scrollablePanel, graphDisplaySettings);
		}

		Vector2 xRange = new Vector2( 0f, (float)(wfg.waveLengthSecs * numPeriods) );

		_graphViewPanel.ChangeGraph( wfg, nFractionalPerWavelength, nSampledPerWavelength, xRange);

		_graphViewPanel.AddAxes(
			new List< AxisDefn>()
			{
				AxisDefn.CreateFixed( RJWS.EOrthoDirection.Vertical, xRange.x ),
				AxisDefn.CreateFixed( RJWS.EOrthoDirection.Vertical, xRange.y ),
				AxisDefn.CreateFixed( RJWS.EOrthoDirection.Vertical, xRange.MidPoint() ),
//				XX_AxisDefn.CreateFractional( RJWS.EOrthoDirection.Vertical, 0.5f ),
				AxisDefn.CreateFixed( RJWS.EOrthoDirection.Horizontal, 0f ),
//                XX_AxisDefn.CreateFixed( RJWS.EOrthoDirection.Horizontal, 0.75f ),
//                XX_AxisDefn.CreateFractional( RJWS.EOrthoDirection.Horizontal, 0.2f )
			}
			);

		
	}
}
