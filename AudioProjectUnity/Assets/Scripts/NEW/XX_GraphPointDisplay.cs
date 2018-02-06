using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XX_GraphPointDisplay : MonoBehaviour
{
	private static readonly bool DEBUG_LOCAL = false;

	private XX_GraphViewPanel _graphViewPanel;

	public UnityEngine.UI.Image image;
	public float size = 1;

	private Vector2 _value = Vector2.zero;
	public Vector2 Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
			cachedRT.anchoredPosition = _graphViewPanel.GetLocation(_value);
			if (DEBUG_LOCAL)
			{
				Debug.Log( gameObject.name + ": val = (" + _value.x+","+_value.y + "), pos = " + cachedRT.anchoredPosition );
			}
		}
	}

	public RectTransform cachedRT
	{
		get;
		private set;
	}

	public Transform cachedTransform
	{
		get;
		private set;
	}

	public XX_GraphPointDisplay previousPt =  null;
	public XX_GraphPointDisplay nextPt = null;
	public XX_GraphConnectorDisplay previousConnector = null;
	public XX_GraphConnectorDisplay nextConnector = null;

	private float _xValue = float.NaN;
	public float xValue
	{
		get { return _xValue; }
		set
		{
			_xValue = value;
		}
	}

	private void Awake()
	{
		cachedRT = GetComponent<RectTransform>();
		cachedTransform = transform;
	}

	public void SetColour(Color c)
	{
		image.color = c;
	}

	public void Init(XX_GraphViewPanel gvp, int num)
	{
		_graphViewPanel = gvp;
		cachedTransform.SetParent( _graphViewPanel.pointsContainer );
		gameObject.name = "Point_" + num.ToString( );
	}

	public void HandleScaling( Vector2 screenFraction )
	{
		image.transform.localScale = new Vector2( size * screenFraction.x, size * screenFraction.y );
	}
}
