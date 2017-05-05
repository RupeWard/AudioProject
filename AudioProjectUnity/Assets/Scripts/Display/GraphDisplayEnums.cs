using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Graph
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

	static class GraphDisplayEnumsHelpers
	{
		public static EOrthoDirection OtherDirection(this EOrthoDirection dirn)
		{
			if (dirn == EOrthoDirection.Horizontal)
			{
				return EOrthoDirection.Vertical;
			}
			else
			{
				return EOrthoDirection.Horizontal;
			}
		}
	}
}
