using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XX_GraphView : MonoBehaviour
{
	/*
	public XX_GraphViewPanel graphPanel
	{
		get;
		private set;
	}

	private bool _isDirty = false;
	public void SetDirty()
	{
		_isDirty = true;
	}

	public GameObject graphPointPrefab;

	private List<XX_GraphPointDisplay> _graphPtDisplays = new List<XX_GraphPointDisplay>( );

	private int _numPointsBACKING = 0;
	public int numPoints
	{
		get { return _numPointsBACKING; }
		set
		{
			if (value != _numPointsBACKING)
			{
				_numPointsBACKING = value;
				HandleNumPointsChanged( );
			}
		}
	}

	private RJWS.Grph.AbstractGraphGenerator _graphGenerator;

	private void Awake()
	{
		graphPanel = GetComponent<XX_GraphViewPanel>();
	}

	private void Init(RJWS.Grph.AbstractGraphGenerator graphGenerator, int n)
	{
		numPoints = n;
		_graphGenerator = graphGenerator;
	}

	private void HandleNumPointsChanged()
	{
		while (_graphPtDisplays.Count < numPoints)
		{
			XX_GraphPointDisplay newPoint = Instantiate( graphPointPrefab ).GetComponent<XX_GraphPointDisplay>( );
			_graphPtDisplays.Add( newPoint );
			SetDirty( );
		}
		while (_graphPtDisplays.Count > numPoints)
		{
			GameObject.Destroy( _graphPtDisplays[0].gameObject );
			_graphPtDisplays.RemoveAt( 0 );
			SetDirty( );
		}
		for (int i = 0; i < _graphPtDisplays.Count; i++)
		{
			_graphPtDisplays[i].Init( this, i );
			if (i == 0)
			{
				_graphPtDisplays[i].previousPt = null;
			}
			else
			{
				_graphPtDisplays[i].previousPt = _graphPtDisplays[i - 1]; 
			}
			if (i == _graphPtDisplays.Count -1)
			{
				_graphPtDisplays[i].nextPt = null;
			}
			else
			{
				_graphPtDisplays[i].nextPt = _graphPtDisplays[i+1];
			}
		}
	}

	private void LateUpdate()
	{

	}
	*/
}
