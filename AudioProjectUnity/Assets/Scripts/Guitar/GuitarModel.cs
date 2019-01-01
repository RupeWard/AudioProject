using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Audio
{
	public class GuitarModel : MonoBehaviour
	{ 
		private List<AudioStringBehaviour> _strings;

		public AudioStringBehaviour stringPrefab;

		public GuitarSettings guitarSettings;

		private void Awake()
		{
			GuitarSettings.LoadDefaultsIfNUll( ref guitarSettings );
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
			else
			{
				_strings = new List<AudioStringBehaviour>( );
			}
			for (int i = 0; i < freqs.Count; i++)
			{
				AudioStringBehaviour newString = (Instantiate( stringPrefab.gameObject ) as GameObject).GetComponent<AudioStringBehaviour>( );
				newString.transform.SetParent( this.transform );
				newString.gameObject.name = "String_" + i;
				newString.SetFrequency(freqs[i]);
				newString.SetZeroThreshold( guitarSettings.zeroThreshold );
				newString.UseReverb( false );
				_strings.Add( newString );
			}
		}

		public int NumStrings
		{
			get { return _strings.Count; }
		}

		public AudioStringBehaviour GetString(int stringNum)
		{
			if (stringNum < 0 || stringNum >= _strings.Count)
			{
				throw new System.Exception( string.Format( "stringNum OOR: {0}, (0,{2})", stringNum, _strings.Count - 1 ) );
			}
			return _strings[stringNum];
		}
	}
}
