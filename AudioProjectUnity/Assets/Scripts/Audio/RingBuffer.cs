using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.Data
{
	public class RingBuffer
	{
		private int _capacity;
		public int Capacity
		{
			get { return _capacity; }
		}

		private double[] _buffer;

		private CyclicIndex _first;
		private CyclicIndex _last;

		public RingBuffer(int c)
		{
			_capacity = c;
			_buffer = new double[_capacity];
			_first = new CyclicIndex( _capacity );
			_last = new CyclicIndex( _capacity );
		}

		public int Size
		{ 
			get { return _first.DistanceAfter(_last);  } 
		}

		public bool IsEmpty
		{
			get { return Size == 0; } 
		}

		public bool IsFull
		{
			get { return Size >= _capacity; } 
		}

		public void Enqueue(double d)
		{
			if (IsFull)
			{
				throw new System.Exception( "Can't Enqueue into full buffer" );
			}
			_buffer[_last.Value] = d;
			_last.Increment( );
		}

		public double Dequeue()
		{
			double result = Peek( );
			_first.Increment( );
			return result;
		}

		public double Peek()
		{
			if (IsEmpty)
			{
				throw new System.Exception( "Can't Peek at empty buffer" );
			}
			return _buffer[_last.Value];
		}
	}

}
