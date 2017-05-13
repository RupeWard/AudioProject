using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS
{
	public enum ELowHigh
	{
		Low,
		High,
		None
	}

	public enum EOrthoDirection
	{
		Horizontal,
		Vertical
	}

	static class EnumsHelpers
	{
		public static EOrthoDirection OrthogonalDirection( this EOrthoDirection dirn )
		{
			if (dirn == EOrthoDirection.Horizontal)
			{
				return EOrthoDirection.Vertical;
			}
			else if (dirn == EOrthoDirection.Vertical)
			{
				return EOrthoDirection.Horizontal;
			}
			else
			{
				Debug.LogError( "No OrthogonalDirection for " + dirn );
				return dirn;
			}
		}
	}

	public class Enums
	{

	}

}
