using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Grph
{
	public class Graph
	{
		private AbstractGraphGenerator _generator;

		public List<GraphPoint> points
		{
			get;
			private set;
		}

		private void SortPoints( )
		{
			points.Sort( GraphPoint.ComparerX.up );
		}

		private bool AddPoint(float x)
		{
			bool success = false;

			GraphPoint pt = new GraphPoint( x, _generator );
			for (int index = 0; index < points.Count; index++)
			{
				if (Mathf.Approximately(x, points[index].x))
				{
					Debug.LogWarning( "Already have a point with x = " + x );
					return false;
				}
				if (points[index].x > x )
				{
					// insert here
					points.Insert( index, pt );
					success = true;
				}
			}
			if (!success)
			{
				points.Add( pt );
				success = true;
			}
			return success;
		}

		
		public Graph( AbstractGraphGenerator gen, Vector2 xRange, int numPoints )
		{
			points = new List<GraphPoint>( );
			_generator = gen;
			Generate( gen, xRange, numPoints );
		}

		private void Generate( AbstractGraphGenerator gen, Vector2 xRange, int numPoints)
		{
			float currentX = xRange.x;
			AddPoint( currentX );

			for (int i = 1; i < (numPoints-1); i++)
			{
				currentX = Mathf.Lerp( xRange.x, xRange.y, (float)i / (numPoints - 1));
				AddPoint( currentX );
			}
			currentX = xRange.y;
			AddPoint( currentX );
		}

}
}
