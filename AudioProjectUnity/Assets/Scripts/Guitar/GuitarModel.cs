using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Audio
{
	public class GuitarModel : MonoBehaviour
	{ 
		private List<AudioStringBehaviour> _strings;

		public AudioStringBehaviour stringPrefab;

		private void Awake()
		{
		} 
		
		public void Init(List<float> freqs)
		{
			if (_strings != null)
			{
				for (int i = 0; i < _strings.Count; i++)
				{
					if (_strings[i] != null)
					{
						GameObject.Destroy( _strings[i].gameObject );
					}
				}
				_strings.Clear( );
			}
			for (int i = 0; i < freqs.Count; i++)
			{
				AudioStringBehaviour newString = (Instantiate( stringPrefab.gameObject ) as GameObject).GetComponent<AudioStringBehaviour>( );
				newString.transform.SetParent( this.transform );
				newString.gameObject.name = "String_" + i;
				newString.SetFrequency(freqs[i]);

				_strings.Add( newString );
			}
		}
	}
}
