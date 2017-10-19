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

		private void AddPoint(float x, bool andSort = false)
		{
			GraphPoint pt = new GraphPoint( x, _generator );
			points.Add( pt );
			if (andSort)
			{
				SortPoints( );
			}
		}

		private void AddPointAndSort( float x)
		{
			AddPoint( x, true );
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
