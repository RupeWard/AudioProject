using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RJWS.UI.Scrollable;
using RJWS.Core.DebugDescribable;
using RJWS.Core.Extensions;

using RJWS.Graph;
using RJWS.Graph.Display;

public class SceneControllerGraphTestScene : SceneController_Base
{
	public GraphDisplaySettings graphDisplaySettings = new GraphDisplaySettings( );
	public GraphPanelDisplaySettings graphPanelDisplaySettings = new GraphPanelDisplaySettings( );

	public RectTransform canvasRT;
	public RectTransform permanentButtonsRT;

	public NewPeriodicWaveFormOverlay newPeriodicWaveFormOverlay;

	private GraphPanelDisplay _graphViewPanel;
	public GameObject graphViewPanelPrefab;

	public AudioClip[] testAudioClips = new AudioClip[2];

	private RJWS.UI.FileSystemFileChooser _fileChooser;

	public Canvas mainCanvas;

	override public SceneManager.EScene Scene( )
	{
		return SceneManager.EScene.GraphTestScene;
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
//		permanentButtonsRT.sizeDelta = new Vector2( Mathf.Max( permanentButtonsRT.sizeDelta.x, RJWS.AppManager.Instance.minClickablePixels ), permanentButtonsRT.sizeDelta.y );

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

		_graphViewPanel = null;
	}

	private string _wavInputFolderPath;
	const string WAVINPUTFILEPATH_KEY = "WAVINPUTPATH";
	private static readonly bool DEBUG_LOCAL = true;
	public GameObject fileChooserPrefab;

    protected override void PostAwake( )
	{
		newPeriodicWaveFormOverlay.onOkButton += NewWaveformCreated;

		_wavInputFolderPath = PlayerPrefs.GetString( WAVINPUTFILEPATH_KEY );
		if (_wavInputFolderPath.Length > 0)
		{
			if (DEBUG_LOCAL)
			{
				Debug.Log( "Loaded input folder path (static) from player prefs = '" + _wavInputFolderPath + "'" );
			}
		}
		else
		{
#if UNITY_ANDROID || UNITY_IPHONE
			_wavInputFolderPath = Application.persistentDataPath;
#endif
			if (DEBUG_LOCAL)
			{
				Debug.Log( "No input folder path (static) in player prefs, defaulting to: '" + _wavInputFolderPath + "'" );
			}
		}

		_fileChooser = (GameObject.Instantiate( fileChooserPrefab )).GetComponent<RJWS.UI.FileSystemFileChooser>( );
		_fileChooser.gameObject.SetActive( false );
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
		newPeriodicWaveFormOverlay.Init( "WF" );

	}
	
	public void HandleLoadWavButton()
	{
		mainCanvas.gameObject.SetActive( false );
		_fileChooser.rect = new Rect( 50, 50, Screen.width - 100, Screen.height - 100 );
		_fileChooser.Open("Select wav file", _wavInputFolderPath, "wav", HandleWavFileChosen);
	}

	private static readonly bool DEBUG_IO = true;

	private IEnumerator LoadWavToGraphPanel(string filepath)
	{
		System.IO.FileInfo fileInfo = new System.IO.FileInfo( filepath );
		if (DEBUG_IO)
		{
			Debug.Log( "File " + filepath + " exists = " + fileInfo.Exists );
		}

		RJWS.Core.Audio.AudioLoader audioLoader = new RJWS.Core.Audio.AudioLoader( filepath );
		Debug.Log( audioLoader );
		AudioClip audioClip = AudioClip.Create( "testSound", audioLoader.SampleCount, 1, audioLoader.Frequency, false );
		if (audioClip != null)
		{
			audioClip.SetData( audioLoader.LeftChannel, 0 );
			yield return null;

			string fp = filepath.Replace( '\\', '/' );
			_wavInputFolderPath = fp.Substring( 0, fp.LastIndexOf( "/" ) );
			PlayerPrefs.SetString( WAVINPUTFILEPATH_KEY, _wavInputFolderPath );
			PlayerPrefs.Save( );
			if (DEBUG_IO)
			{
				Debug.Log( "Saved path: " + fp );
			}
			if (audioClip.length == 0f)
			{
				Debug.LogError( "Zero length testaudioclip loaded from " + filepath);
				yield break;
			}
			if (audioClip.samples== 0)
			{
				Debug.LogError( "Zero samples in testaudioclip loaded from " + filepath );
				yield break;
			}
			LoadAudioClipToNewPanel( audioClip );
		}
		else
		{
			Debug.LogError( "Created clip is null" );
		}
	}

	public void HandleWavFileChosen(string filepath, string filename)
	{
		mainCanvas.gameObject.SetActive( true );

		if (!string.IsNullOrEmpty(filepath))
		{
			Debug.Log( "Wav chosen: '" + filepath + "'" );

			StartCoroutine( LoadWavToGraphPanel( filepath ) );
		}
		else
		{
			Debug.Log( "No file chosen" );
		}
	}

	public void HandleLoadWaveformButton( int n )
	{
		if (n >= testAudioClips.Length)
		{
			Debug.LogError( "Only " + testAudioClips.Length + " testAudioClips, not " + n );
			return;
		}
		AudioClip testAudioClip = testAudioClips[n];
		if (testAudioClip == null)
		{
			Debug.LogError( "Null testaudioclip " + n + " of " + testAudioClips.Length );
			return;
		}
		LoadAudioClipToNewPanel( testAudioClip );
	}

	public void LoadAudioClipToNewPanel(AudioClip clip )
	{
		float lengthSecs = clip.length;
		Debug.Log( "Clip frequency = " + clip.frequency+", L="+lengthSecs );
		int nSamples = Mathf.FloorToInt( clip.frequency * lengthSecs);

		float[] buffer = new float[nSamples];

		if (clip.GetData( buffer, 0 ))
		{
			RJWS.Audio.WaveFormGenerator_Sampled wfg = new RJWS.Audio.WaveFormGenerator_Sampled( "WAV", buffer, (double)1 / clip.frequency );
			if (wfg.numSamples == 0)
			{
				Debug.LogError( "WFG has 0 samples!" );
				return;
			}

			RefreshScrollablePanel( );

			if (_graphViewPanel == null)
			{
				_graphViewPanel = GameObject.Instantiate( graphViewPanelPrefab ).GetComponent<GraphPanelDisplay>( );
				_graphViewPanel.Init( _scrollablePanel, graphPanelDisplaySettings );
			}

			Vector2 xRange = new Vector2( 0f, clip.length );
			_graphViewPanel.SetRanges( xRange, wfg.GetValueRange( ) );
			_graphViewPanel.AddGraph( wfg, graphDisplaySettings);
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

		RefreshScrollablePanel( );

		if (_graphViewPanel == null)
		{
			_graphViewPanel = GameObject.Instantiate( graphViewPanelPrefab ).GetComponent< GraphPanelDisplay>();
			_graphViewPanel.Init( _scrollablePanel, graphPanelDisplaySettings);
		}

		Vector2 xRange = new Vector2( 0f, (float)(wfg.waveLengthSecs * numPeriods) );

		_graphViewPanel.SetRanges( xRange, wfg.GetValueRange( ) );
		_graphViewPanel.AddGraph( wfg, graphDisplaySettings);

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
