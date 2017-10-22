﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.DebugDescribable;

public class GraphPointDisplay: MonoBehaviour, IDebugDescribable
{
	private static int s_nextid = 0;

	private GraphPanel _graphPanel;

	public System.Action onSetDirty;

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
		if (onSetDirty != null)
		{
			onSetDirty( );
		}
	}

	RJWS.Grph.GraphPoint _graphPoint = null;

	public int id
	{
		get;
		private set;
	}

	private void Awake()
	{
		id = s_nextid;
		s_nextid++;
		gameObject.name = "GPT_" + id.ToString( "000000" );
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

	public void DebugDescribe( System.Text.StringBuilder sb )
	{
		sb.Append( "[" ).Append(id.ToString("000000")).Append(" ");
		_graphPoint.DebugDescribe( sb );
		sb.Append( "]" );
	}

#if UNITY_EDITOR
	[ContextMenu( "DebugDescribe" )]
	private void DebugDescribeMenuItem( )
	{
		Debug.Log( this.DebugDescribe( ) );
	}
#endif


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
