using RJWS.Core.DebugDescribable;
using UnityEngine;

namespace RJWS.Core.Data
{
	public class CyclicIndex : RJWS.Core.DebugDescribable.IDebugDescribable
	{
		private int _capacity;

		private int _value;
		public int Value
		{
			get { return _value; }
			private set
			{
				_value = value;
				ClampValue( );
			}
		}

		public CyclicIndex( int cap, int v = 0 )
		{
			_capacity = cap;
			_value = 0;
		}

		private CyclicIndex(CyclicIndex other)
		{
			_capacity = other._capacity;
			_value = other._value;
		}

		public CyclicIndex Clone()
		{
			CyclicIndex result = new CyclicIndex( this );
			return result;
		}

		public CyclicIndex Increment( int i = 1 )
		{
			if (i == 0)
			{
				Debug.LogErrorFormat( "CyclicIndex.Increment called with 0" );
			}
			Value += i;
			return this;
		}

		public CyclicIndex Decrement( int i = 1 )
		{
			if (i == 0)
			{
				Debug.LogErrorFormat( "CyclicIndex.Decrement called with 0" );
			}
			Value -= i;
			return this;
		}

		private void ClampValue( )
		{
			while (_value >= _capacity)
			{
				_value -= _capacity;
			}
			while (_value < 0)
			{
				_value += _capacity;
			}
		}

		public int DistanceAfter(CyclicIndex other)
		{
			return Distance( this, other );
		}

		public int DistanceBefore(CyclicIndex other)
		{
			return Distance( other, this );
		}

		public static int Distance( CyclicIndex first, CyclicIndex second )
		{
			if (first._capacity != second._capacity)
			{
				throw new System.Exception( "Can't find Distance when CyclicIndex's have different capacities: eg " + first.DebugDescribe( ) + " & " + second.DebugDescribe( ) );
			}
			int v = second.Value;
			while (v < first.Value)
			{
				v += second._capacity;
			}

			return v - first.Value;
		}

		void IDebugDescribable.DebugDescribe( System.Text.StringBuilder sb )
		{
			sb.Append( _value ).Append( "(" ).Append( _capacity ).Append( ")" );
		}
	}


}
