using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XX_GraphConnectorDisplay : MonoBehaviour
{
	public UnityEngine.UI.Image image;

	private RectTransform _imageRT;
	private Transform _imageTransform;

	private XX_GraphViewPanel _graphViewPanel;

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

	public XX_GraphPointDisplay previousPt = null;
	public XX_GraphPointDisplay nextPt = null;

	private void Awake( )
	{
		cachedTransform = transform;
		cachedRT = GetComponent<RectTransform>( );
		_imageRT = image.GetComponent<RectTransform>( );
		_imageTransform = image.transform;
	}

	public void Init( XX_GraphViewPanel gvp, int num )
	{
		_graphViewPanel = gvp;
		cachedTransform.SetParent( _graphViewPanel.linesContainer);
		gameObject.name = "Connector_" + num.ToString( );
	}

	public void SetColour( Color c )
	{
		image.color = c;
	}

	public void UpdateDisplay()
	{
		if (previousPt != null && nextPt != null)
		{
			Vector2 pos0 = previousPt.cachedRT.anchoredPosition;
			Vector2 pos1 = nextPt.cachedRT.anchoredPosition;

			float length = (pos0 - pos1).magnitude;
			float width = _graphViewPanel.graphDisplaySettings.lineWidth;

			cachedRT.anchoredPosition = 0.5f * (pos0 + pos1);
//			cachedTransform.localScale = Vector3.one;
			_imageRT.sizeDelta = new Vector2( length, width );
			_imageTransform.localEulerAngles = new Vector3( 0f, 0f, Mathf.Rad2Deg * Mathf.Atan2( pos1.y - pos0.y, pos1.x - pos0.x ) );
		}
	}
}
