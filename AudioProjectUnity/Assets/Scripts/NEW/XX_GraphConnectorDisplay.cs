using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XX_GraphConnectorDisplay : MonoBehaviour
{
	public UnityEngine.UI.Image image;

	public RectTransform cachedRT
	{
		get;
		private set;
	}

	public XX_GraphPointDisplay previousPt = null;
	public XX_GraphPointDisplay nextPt = null;

	private void Awake( )
	{
		cachedRT = GetComponent<RectTransform>( );
	}

	public void SetColour( Color c )
	{
		image.color = c;
	}


}
