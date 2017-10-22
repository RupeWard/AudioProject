using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RJWS.Core.DebugDescribable;

public class GraphPointDisplay: MonoBehaviour, IDebugDescribable
{
	private static int s_nextid = 0;

	private GraphPanel _graphPanel;

	public System.Action onSetDirty;

	public RJWS.UI.ObjectGrabber objectGrabber;

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

		objectGrabber.onMovementAction += HandleMovement;
		objectGrabber.onXMovementAction += HandleXMovement;
		objectGrabber.onYMovementAction += HandleYMovement;
		objectGrabber.onDoubleClickAction += HandleGrabberDoubleClick;
		objectGrabber.onActivateAction += HandleGrabberActivated;

		RegisterListeners( );
		UpdateDisplay( );
	}

	public void HandleClick( )
	{
		//			Debug.Log( "SBE "+Time.time + " Click on " + transform.GetPathInHierarchy( ) );
		if (!objectGrabber.isActivated)
		{
			if (RJWS.UI.ObjectGrabManager.Instance.HandleGrabRequest( objectGrabber ))
			{
				Debug.Log( "Grabbed " + this.DebugDescribe( ) );
			}
		}
	}

	public void HandleMovement( Vector2 v)
	{
		Debug.Log( "Move point " + v + this.DebugDescribe( ) );
	}

	public void HandleXMovement( float delta )
	{
		Debug.Log( "Move X point " + delta + this.DebugDescribe( ) );
	}

	public void HandleYMovement( float delta )
	{
		Debug.Log( "Move Y point "+delta + this.DebugDescribe( ) );
	}

	public void HandleGrabberClick( )
	{
		Debug.Log( "Clicked point " + this.DebugDescribe( ) );
	}

	public void HandleGrabberDoubleClick( )
	{
		Debug.Log( "Double Clicked point " + this.DebugDescribe( ) );
	}

	public void HandleGrabberActivated( bool b)
	{
		if (b)
		{
			Debug.Log( "Activated point " + this.DebugDescribe( ) );
		}
		else
		{
			Debug.Log( "Dectivated point " + this.DebugDescribe( ) );
		}
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
