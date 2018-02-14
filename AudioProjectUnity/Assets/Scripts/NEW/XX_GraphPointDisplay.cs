using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XX_GraphPointDisplay : MonoBehaviour
{
	private static readonly bool DEBUG_LOCAL = false;

	private XX_GraphViewPanel _graphViewPanel;

	public UnityEngine.UI.Image image;
	public float size = 1;

	public enum EPtType
	{
		Fractional,
		Sampled
	}

	public EPtType PtType
	{
		get;
		private set;
	}

	public class PtXComparer : IComparer< XX_GraphPointDisplay>
	{
		public int Compare( XX_GraphPointDisplay pt0, XX_GraphPointDisplay pt1)
		{
			float x0 = pt0.Value.x;
			float x1 = pt1.Value.x;
			return (int)Mathf.Sign( x0 - x1 );
		}

	}

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

	public void Init(XX_GraphViewPanel gvp, string n, EPtType t)
	{
		_graphViewPanel = gvp;
		PtType = t;
		cachedTransform.SetParent( _graphViewPanel.pointsContainer );
		gameObject.name = "Point_" + n;
	}

	public void HandleScaling( Vector2 screenFraction )
	{
		image.transform.localScale = new Vector2( size * screenFraction.x, size * screenFraction.y );
	}
}
