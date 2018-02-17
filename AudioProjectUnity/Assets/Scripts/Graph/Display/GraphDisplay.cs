using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.Extensions;
using RJWS.Core.DebugDescribable;

namespace RJWS.Graph.Display
{
	public class GraphDisplay//: MonoBehaviour
	{
		private static readonly bool DEBUG_LOCAL = true;

		/*
		public void SetDirty( )
		{
			_displayPosDirty = true;
			_displayScaleDirty = true;
		}

		public bool IsDirty
		{
			get
			{
				return _displayScaleDirty || _displayPosDirty;
			}
		}
		*/

		public GraphDisplaySettings graphDisplaySettings
		{
			get;
			private set;
		}

		public GameObject graphPointPrefab;
		public GameObject graphConnectorPrefab;

		private List<GraphPointDisplay> _fractionalGraphPtDisplays = new List<GraphPointDisplay>( );
		private List<GraphPointDisplay> _sampleGraphPtDisplays = new List<GraphPointDisplay>( );
		private List<GraphConnectorDisplay> _graphConnectorDisplays = new List<GraphConnectorDisplay>( );

		private List<GraphPointDisplay> _allGraphPtDisplays = new List<GraphPointDisplay>( );

		private int _numFractionalPointsBACKING = 0;
		public int numFractionalPoints
		{
			get { return _numFractionalPointsBACKING; }
			set
			{
				if (value != _numFractionalPointsBACKING)
				{
					_numFractionalPointsBACKING = value;
					//HandleNumPointsChanged( );
				}
			}
		}

		private int _numSampledPointsBACKING = 0;
		public int numSampledPoints
		{
			get { return _numSampledPointsBACKING; }
			set
			{
				if (value != _numSampledPointsBACKING)
				{
					_numSampledPointsBACKING = value;
					//HandleNumPointsChanged( );
				}
			}
		}

		public int totalNumPoints
		{
			get
			{
				return numSampledPoints + numFractionalPoints;
			}
		}

		private RJWS.Grph.AbstractGraphGenerator _graphGenerator;

		public void ChangeGraph( RJWS.Audio.AbstractWaveFormGenerator graphGenerator, int nFractionalPoints, int nSampledPoints )
		{
			// TODO scaling of axis labels. vertical axes labels too big after verrtical reduction (but then static), & vv.
			if (graphGenerator == null)
			{
				Debug.LogError( "NULL graphGenerator in GD.ChangeGraph" );
				return;
			}
			if (DEBUG_LOCAL)
			{
				Debug.Log( "GD.ChangeGraph to " + graphGenerator.DebugDescribe( ) );
			}
			for (int i = 0; i < _sampleGraphPtDisplays.Count; i++)
			{
				_sampleGraphPtDisplays[i].gameObject.SetActive( false );
			}

			numFractionalPoints = nFractionalPoints;
			numSampledPoints = nSampledPoints;

			_graphGenerator = graphGenerator;
		}

		/*
		private void HandleNumPointsChanged( )
		{
			while (_fractionalGraphPtDisplays.Count < numFractionalPoints)
			{
				GraphPointDisplay newPoint = GameObject.Instantiate( _graphPanel.graphPointPrefab ).GetComponent<GraphPointDisplay>( );
				_fractionalGraphPtDisplays.Add( newPoint );
				newPoint.Init( this, "FR_" + (_fractionalGraphPtDisplays.Count - 1).ToString( ), GraphPointDisplay.EPtType.Fractional );
				newPoint.SetColour( graphDisplaySettings.fractionalPointColour );
			}
			while (_fractionalGraphPtDisplays.Count > numFractionalPoints)
			{
				GameObject.Destroy( _fractionalGraphPtDisplays[0].gameObject );
				_fractionalGraphPtDisplays.RemoveAt( 0 );
			}

			while (_sampleGraphPtDisplays.Count < numSampledPoints)
			{
				GraphPointDisplay newPoint = GameObject.Instantiate( graphPointPrefab ).GetComponent<GraphPointDisplay>( );
				_sampleGraphPtDisplays.Add( newPoint );
				newPoint.Init( this, "SA_" + (_sampleGraphPtDisplays.Count - 1).ToString( ), GraphPointDisplay.EPtType.Sampled );
				newPoint.SetColour( graphDisplaySettings.samplePointColour );
				newPoint.gameObject.SetActive( false );
			}
			while (_sampleGraphPtDisplays.Count > numSampledPoints)
			{
				GameObject.Destroy( _sampleGraphPtDisplays[0].gameObject );
				_sampleGraphPtDisplays.RemoveAt( 0 );
			}

			while (_graphConnectorDisplays.Count < totalNumPoints - 1)
			{
				GraphConnectorDisplay newConnector = GameObject.Instantiate( graphConnectorPrefab ).GetComponent<GraphConnectorDisplay>( );
				_graphConnectorDisplays.Add( newConnector );
				newConnector.gameObject.SetActive( false );
			}
			while (_graphConnectorDisplays.Count > totalNumPoints - 1)
			{
				GameObject.Destroy( _graphConnectorDisplays[0].gameObject );
				_graphConnectorDisplays.RemoveAt( 0 );
			}

		}
		*/
		System.Text.StringBuilder debugsb = new System.Text.StringBuilder( );

		private Dictionary<RJWS.EOrthoDirection, bool> _directionFlags = null;
		private void ResetDirectionFlags( bool b )
		{
			if (_directionFlags == null)
			{
				_directionFlags = new Dictionary<RJWS.EOrthoDirection, bool>( );
			}
			_directionFlags.Clear( );
			_directionFlags.Add( RJWS.EOrthoDirection.Horizontal, b );
			_directionFlags.Add( RJWS.EOrthoDirection.Vertical, b );
		}

		private Rect viewValuesRect
		{
			get
			{
				return new Rect( firstX, firstY, (lastX - firstX), (lastY - firstY) );
			}
		}

		public void UpdateDisplay( )
		{
			if (_graphGenerator == null)
			{
				Debug.LogError( "NULL graphGenerator" );
				return;
			}
			if (DEBUG_LOCAL)
			{
				debugsb.Length = 0;
			}

			//if (_displayScaleDirty)
			{
				for (int i = 0; i < numFractionalPoints; i++)
				{
					_fractionalGraphPtDisplays[i].HandleScaling( _graphPanel.displayScaleFractionReadonly );
				}
				for (int i = 0; i < numSampledPoints - 1; i++)
				{
					_sampleGraphPtDisplays[i].HandleScaling( _graphPanel.displayScaleFractionReadonly );
				}
				//_displayScaleDirty = false;
			}
			//if (_displayPosDirty)
			{
				if (DEBUG_LOCAL)
				{
					debugsb.Append( "\n- pos dirty = " + _graphPanel.displayPosOfFirstEdgeFractionReadonly + ", first/last = " + firstX + " / " + lastX );
				}

				int numSamplePtsDisplayed = 0;

				// TODO do this with plymorphism
				RJWS.Audio.WaveFormGenerator_Sampled sampledWFG = _graphGenerator as RJWS.Audio.WaveFormGenerator_Sampled;
				if (sampledWFG != null)
				{
					int nSamplePts = sampledWFG.GetSampleXsInInterval( firstXD, lastXD - firstXD, _debugSamples );

					int step = 1;
					if (nSamplePts > numSampledPoints)
					{
						step = Mathf.CeilToInt( (float)nSamplePts / numSampledPoints );
					}

					int samplePtIndex = 0;
					double samplex = firstXD;
					int samplePtDisplayIndex = 0;

					if (DEBUG_LOCAL)
					{
						debugsb.Append( "\nStepping through samples, firstXD = " ).Append( firstXD ).Append( " last = " ).Append( lastXD );
					}

					while (samplePtDisplayIndex < _sampleGraphPtDisplays.Count && samplePtIndex < nSamplePts && samplex <= lastXD)
					{
						samplex = _debugSamples[samplePtIndex];
						if (samplex < lastXD)
						{
							float sampleY = _graphGenerator.GetYForX( samplex );
							_sampleGraphPtDisplays[samplePtDisplayIndex].gameObject.SetActive( true );
							_sampleGraphPtDisplays[samplePtDisplayIndex].Value = new Vector2( (float)samplex, sampleY );
							samplePtIndex += step;
							samplePtDisplayIndex += 1;
						}
						else
						{
							if (DEBUG_LOCAL)
							{
								debugsb.Append( "\nERROR - samplex = " ).Append( samplex ).Append( " for index " ).Append( samplePtIndex );
							}
						}
					}
					numSamplePtsDisplayed = samplePtDisplayIndex;
					while (samplePtDisplayIndex < _sampleGraphPtDisplays.Count)
					{
						_sampleGraphPtDisplays[samplePtDisplayIndex].gameObject.SetActive( false );
						samplePtDisplayIndex++;
					}

					if (DEBUG_LOCAL)
					{
						Debug.Log( "N=" + nSamplePts + ", step " + step + " gives " + numSamplePtsDisplayed + " displayed" );
						debugsb.Append( "\nN=" + nSamplePts + ", step " + step + " gives " + numSamplePtsDisplayed + " displayed" );
						for (int i = 0; i < numSamplePtsDisplayed; i++)
						{
							debugsb.Append( _debugSamples[i] ).Append( ", " );
						}
						Debug.Log( debugsb.ToString( ) );
					}
				}
				else
				{
					if (DEBUG_LOCAL)
					{
						Debug.Log( "No sampled points" );
					}
				}
				double xstepD = (lastXD - firstXD) / (numFractionalPoints - 1);
				if (DEBUG_LOCAL)
				{
					debugsb.Append( "\n- xstep = " + xstepD );
				}
				for (int i = 0; i < numFractionalPoints; i++)
				{
					double x = firstXD + xstepD * i;
					float y = _graphGenerator.GetYForX( x );
					_fractionalGraphPtDisplays[i].Value = new Vector2( (float)x, y );
					/*
					if (i > 0)
					{
						_graphConnectorDisplays[i-1].UpdateDisplay( );
					}
					*/
				}

				// Now put them all together
				_allGraphPtDisplays.Clear( );

				for (int i = 0; i < _fractionalGraphPtDisplays.Count; i++)
				{
					_allGraphPtDisplays.Add( _fractionalGraphPtDisplays[i] );
				}
				if (numSamplePtsDisplayed > 0)
				{
					for (int i = 0; i < numSamplePtsDisplayed; i++)
					{
						_allGraphPtDisplays.Add( _sampleGraphPtDisplays[i] );
					}
				}

				_allGraphPtDisplays.Sort( new GraphPointDisplay.PtXComparer( ) );
				if (DEBUG_LOCAL)
				{
					debugsb.Append( "\nMerged " ).Append( numFractionalPoints ).Append( " fractional and " ).Append( numSamplePtsDisplayed ).Append( " sampled making " ).Append( _allGraphPtDisplays.Count );
					for (int i = 0; i < _allGraphPtDisplays.Count; i++)
					{
						debugsb.Append( "\n " ).Append( i ).Append( _allGraphPtDisplays[i].Value.x.ToString( "G4" ) );
					}
				}

				int pointIndex = 0;
				for (pointIndex = 0; pointIndex < _allGraphPtDisplays.Count; pointIndex++)
				{
					if (pointIndex == 0)
					{
						_allGraphPtDisplays[pointIndex].previousPt = null;
					}
					else
					{
						_allGraphPtDisplays[pointIndex].previousPt = _allGraphPtDisplays[pointIndex - 1];
					}
					if (pointIndex == _allGraphPtDisplays.Count - 1)
					{
						_allGraphPtDisplays[pointIndex].nextPt = null;
					}
					else
					{
						_allGraphPtDisplays[pointIndex].nextPt = _allGraphPtDisplays[pointIndex + 1];
					}
					if (pointIndex < _allGraphPtDisplays.Count - 1)
					{
						if (!_graphConnectorDisplays[pointIndex].gameObject.activeSelf)
						{
							_graphConnectorDisplays[pointIndex].gameObject.SetActive( true );
						}
			//			_graphConnectorDisplays[pointIndex].Init( this, pointIndex );
						_graphConnectorDisplays[pointIndex].previousPt = _allGraphPtDisplays[pointIndex];
						_graphConnectorDisplays[pointIndex].nextPt = _allGraphPtDisplays[pointIndex + 1];
						_graphConnectorDisplays[pointIndex].UpdateDisplay( );

						if (sampledWFG != null)
						{
							double prevXD = (double)_allGraphPtDisplays[pointIndex].Value.x;
							double nextXD = (double)_allGraphPtDisplays[pointIndex + 1].Value.x;
							int nSamplePts = sampledWFG.GetSampleXsInInterval( prevXD, nextXD - prevXD, _debugSamples );
							if (_allGraphPtDisplays[pointIndex].PtType == GraphPointDisplay.EPtType.Sampled)
							{
								nSamplePts--;
							}
							if (_allGraphPtDisplays[pointIndex + 1].PtType == GraphPointDisplay.EPtType.Sampled)
							{
								nSamplePts--;
							}
							Color col = (nSamplePts > 0) ? (graphDisplaySettings.sampleHidingConnectorColor) : (graphDisplaySettings.pureConnectorColour);
							_graphConnectorDisplays[pointIndex].SetColour( col );
							//					Debug.Log( "N=" + nSamplePts+" col="+col );
						}
					}
				}
				int connectorIndex = pointIndex;
				for (; connectorIndex < _graphConnectorDisplays.Count; connectorIndex++)
				{
					if (_graphConnectorDisplays[connectorIndex].gameObject.activeSelf)
					{
						_graphConnectorDisplays[connectorIndex].gameObject.SetActive( false );
					}
				}

			}
			if (DEBUG_LOCAL)
			{
				if (debugsb.Length > 0)
				{
					Debug.Log( debugsb.ToString( ) );
				}
			}

		}

		private List<double> _debugSamples = new List<double>( );

		private GraphPanelDisplay _graphPanel;

		public float firstX
		{
			get
			{
				return _graphPanel.firstX;
			}
		}

		public double firstXD
		{
			get
			{
				return _graphPanel.firstXD;
			}
		}


		public float lastX
		{
			get
			{
				return _graphPanel.lastX;
			}
		}

		public double lastXD
		{
			get
			{
				return _graphPanel.lastXD;
			}
		}

		public float firstY
		{
			get
			{
				return _graphPanel.firstY;
			}
		}

		public double firstYD
		{
			get
			{
				return _graphPanel.firstYD;
			}
		}

		public float lastY
		{
			get
			{
				return _graphPanel.lastY;
			}
		}

		public double lastYD
		{
			get
			{
				return _graphPanel.lastYD;
			}
		}

		public GraphDisplaySettings _graphDisplaySettings;
		public void Init( GraphPanelDisplay gpd, GraphDisplaySettings gSettings )
		{
			_graphDisplaySettings = gSettings;
			_graphPanel = gpd;
		}

		static readonly bool DEBUG_SCALE = false;

		private static readonly bool DEBUG_POS = false;


		/*
		public void HandleDisplayScaleChanged(RJWS.EOrthoDirection dirn, float scale)
		{
			scale = 1f / scale;
			if (dirn == RJWS.EOrthoDirection.Horizontal)
			{
				if (scale != _displayScale.x)
				{
					if (DEBUG_LOCAL)
					{
						Debug.Log( "DisplayScale changed : " + dirn + ", " + scale + " from "
							+ _displayScale.x );
					}
					_displayScale.x = scale;
					_displayScaleDirty = true;
				}
				else
				{
					if (DEBUG_LOCAL)
					{
						Debug.Log( "X DisplayScale UNchanged : " + dirn + ", " + _displayScale.x );
					}
				}
			}
			else if (dirn == RJWS.EOrthoDirection.Vertical)
			{
				if (scale != _displayScale.y)
				{
					if (DEBUG_LOCAL)
					{
						Debug.Log( "DisplayScale changed : " + dirn + ", " + scale + " from "
							+ _displayScale.y );
					}
					_displayScale.y = scale;
					_displayScaleDirty = true;
				}
				else
				{
					if (DEBUG_LOCAL)
					{
						Debug.Log( "X DisplayScale UNchanged : " + dirn + ", " + _displayScale.y );
					}
				}
			}
		}
		*/

	}

}

