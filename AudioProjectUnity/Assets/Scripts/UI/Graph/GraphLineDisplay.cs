using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.DebugDescribable;

public class GraphLineDisplay: MonoBehaviour, IDebugDescribable
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

	GraphPointDisplay _graphPointDisplay0 = null;
	GraphPointDisplay _graphPointDisplay1 = null;

	private int _id;

	private void Awake()
	{
	}

	public void Init(GraphPanel panel, GraphPointDisplay gpd0, GraphPointDisplay gpd1)
	{
		_graphPointDisplay0 = gpd0;
		_graphPointDisplay1 = gpd1;
		_graphPanel = panel;

		gameObject.name = idStr;

		RegisterListeners( );
		UpdateDisplay( );
	}

	private string idStr
	{
		get
		{
			return "GL_" + _graphPointDisplay0.id + "_" + _graphPointDisplay1.id;
        }
	}
	private void OnDestroy()
	{
		DeregisterListeners( );
	}

	private void RegisterListeners()
	{
		DeregisterListeners( );
		_graphPointDisplay0.onSetDirty += SetDirty;
		_graphPointDisplay1.onSetDirty += SetDirty;
	}

	private void DeregisterListeners( )
	{
		_graphPointDisplay0.onSetDirty -= SetDirty;
		_graphPointDisplay1.onSetDirty -= SetDirty;
	}

	public void DebugDescribe( System.Text.StringBuilder sb )
	{
		sb.Append( "[" ).Append(idStr).Append(" ");
		_graphPointDisplay0.DebugDescribe( sb );
		sb.Append( " " );
		_graphPointDisplay1.DebugDescribe( sb );
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
		Vector2 pos0 = _graphPointDisplay0.cachedRT.anchoredPosition;
		Vector2 pos1 = _graphPointDisplay1.cachedRT.anchoredPosition;

		float length = (pos0 - pos1).magnitude;
		float width = GraphDisplayManager.Instance.lineWidth;

		cachedRT.anchoredPosition = 0.5f * (pos0 + pos1);
		cachedTransform.localScale = Vector3.one;
		cachedRT.sizeDelta= new Vector2(length, width);
		cachedTransform.localEulerAngles = new Vector3( 0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(pos1.y-pos0.y, pos1.x - pos0.x));
	}
}
