using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphPointDisplay: MonoBehaviour
{
	private GraphPanel _graphPanel;

	private RectTransform _cachedRT;
	public RectTransform cachedRT
	{
		get
		{
			if (_cachedRT == null)
			{
				_cachedRT = GetComponent<RectTransform>( );
			}
			return _cachedRT;
		}
		private set
		{
			_cachedRT = value;
		}
	}

	private Transform _cachedTransform;
	public Transform cachedTransform
	{
		get
		{
			if (_cachedTransform == null)
			{
				_cachedTransform = GetComponent<Transform>( );
			}
			return _cachedTransform;
		}
		private set
		{
			_cachedTransform = value;
		}
	}

	private bool _isDirty;
	public void SetDirty()
	{
		_isDirty = true;
	}

	RJWS.Grph.GraphPoint _graphPoint = null;

	private void Awake()
	{
		
	}

	public void Init(GraphPanel panel, RJWS.Grph.GraphPoint gpt)
	{
		_graphPanel = panel;
		_graphPoint = gpt;

		RegisterListeners( );
		UpdateDisplay( );
	}

	private void OnDestroy()
	{
		DeregisterListeners( );
	}

	private void RegisterListeners()
	{
		DeregisterListeners( );
		_graphPoint.onPtchanged += HandleGraphPointChanged;
	}

	private void DeregisterListeners( )
	{
		_graphPoint.onPtchanged -= HandleGraphPointChanged;
	}

	private void HandleGraphPointChanged( RJWS.Grph.GraphPoint gpt)
	{
		SetDirty( );
	}

	public void Update()
	{
		if (_isDirty)
		{
			UpdateDisplay( );
			_isDirty = false;
		}
	}

	private void UpdateDisplay()
	{
		cachedRT.anchoredPosition = _graphPanel.GetLocationForPoint( _graphPoint.position );
	}
}
