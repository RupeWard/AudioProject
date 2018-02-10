using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.UI
{
	public class PanelTurner : MonoBehaviour
	{
		public RectTransform turnedRT;
		public RectTransform containerRT;


		private void LateUpdate()
		{
			turnedRT.sizeDelta = new Vector2( Mathf.Abs(containerRT.rect.height), Mathf.Abs( containerRT.rect.width) );
		}

	}

}
