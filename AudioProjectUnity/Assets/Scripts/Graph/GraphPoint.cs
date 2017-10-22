using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using RJWS.Core.DebugDescribable;

namespace RJWS.Grph 
{
	[System.Serializable]
	public class GraphPoint : IDebugDescribable
	{
		[SerializeField]
		private Vector2 _position;

		public Vector2 position
		{
			get { return _position; }
			set
			{
				if (value != _position)
				{
					_position = value;
					HandlePointChanged( );
				}
			}
		}

		public float x
		{
			get { return _position.x; }
			set
			{
				if (value != _position.x)
				{
					_position.x = value;
					HandlePointChanged( );
				}
			}
		}

		public float y
		{
			get { return _position.y; }
			set
			{
				if (value != _position.y)
				{
					_position.y = value;
					HandlePointChanged( );
				}
			}
		}

		private void HandlePointChanged( )
		{
			if (onPtchanged != null)
			{
				onPtchanged( this );
			}
		}

		public System.Action<GraphPoint> onPtchanged;

		public GraphPoint( float x, AbstractGraphGenerator gen)
		{
			InitPos(x, gen.GetYForX( x ));
		}

		public GraphPoint( float x, float y)
		{
			InitPos( x, y );
		}

		private void InitPos(float x, float y)
		{
			_position.x = x;
			_position.y = y;
		}

		public void DebugDescribe(System.Text.StringBuilder sb)
		{
			sb.Append( "[GP: " ).Append( _position.x ).Append(", ").Append(_position.y ).Append( "]" );
		}

		#region comparers

		public class ComparerX : IComparer<GraphPoint>
		{
			private int _direction = 1;

			private ComparerX( int d )
			{
				_direction = d;
			}

			static private ComparerX _up = null;

			static public ComparerX up
			{
				get
				{
					if (_up == null)
					{
						_up = new ComparerX( 1 );
					}
					return _up;
				}
			}

			static private ComparerX _down = null;

			static public ComparerX down
			{
				get
				{
					if (_down == null)
					{
						_down = new ComparerX( -1 );
					}
					return _down;
				}
			}

			public int Compare( GraphPoint a, GraphPoint b )
			{
				if (a == null)
				{
					if (b == null)
					{
						return 0;
					}
					else
					{
						return -1 * _direction;
					}
				}
				else
				{
					// If a is not null...
					//
					if (b == null)
					// ...and b is null, a is greater.
					{
						return 1 * _direction;
					}
					else
					{
						// ...and b is not null, compare the 
						// x values of the two strings.
						//
						if (a.x > b.x)
						{
							return 1 * _direction;
						}
						else if (a.x < b.x)
						{
							return -1 * _direction;
						}
						return 0;
					}
				}
			}
		}

		public class ComparerY : IComparer<GraphPoint>
		{
			private int _direction = 1;

			private ComparerY( int d )
			{
				_direction = d;
			}

			static private ComparerY _up = null;

			static public ComparerY up
			{
				get
				{
					if (_up == null)
					{
						_up = new ComparerY( 1 );
					}
					return _up;
				}
			}

			static private ComparerY _down = null;

			static public ComparerY down
			{
				get
				{
					if (_down == null)
					{
						_down = new ComparerY( -1 );
					}
					return _down;
				}
			}

			public int Compare( GraphPoint a, GraphPoint b )
			{
				if (a == null)
				{
					if (b == null)
					{
						return 0;
					}
					else
					{
						return -1 * _direction;
					}
				}
				else
				{
					// If a is not null...
					//
					if (b == null)
					// ...and b is null, a is greater.
					{
						return 1 * _direction;
					}
					else
					{
						// ...and b is not null, compare the 
						// y values of the two strings.
						//
						if (a.y > b.y)
						{
							return 1 * _direction;
						}
						else if (a.y < b.y)
						{
							return -1 * _direction;
						}
						return 0;
					}
				}
			}
		}

		#endregion comparers
	}


}
