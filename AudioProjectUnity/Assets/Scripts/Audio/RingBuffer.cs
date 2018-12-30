using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RJWS.Core.Audio
{
	public class RingBuffer<T>
	{
		private int _capacity;
		public int Capacity
		{
			get { return _capacity; }
			set
			{
				if (value != _capacity)
				{
					List<T> newList = new List<T>( );
					int nQueued = 0;
					while (!IsEmpty && nQueued < value)
					{
						newList.Add( Dequeue( ) );
						nQueued++;
					}

					_numQueued = nQueued;
					
					if (value > _buffer.Length)
					{
						_buffer = new T[value];
					}
					System.Array.Copy( newList.ToArray( ), _buffer, _numQueued );
					_capacity = value;
					_first = new CyclicIndex( _capacity );
					_last = new CyclicIndex( _capacity, _numQueued );
				}
			}
		}

		private T[] _buffer;

		private CyclicIndex _first;
		private CyclicIndex _last;

		private int _numQueued;
		public int NumQueued
		{
			get { return _numQueued; }
		}

		public RingBuffer(int c)
		{
			_capacity = c;
			_buffer = new T[_capacity];
			_numQueued = 0;
			_first = new CyclicIndex( _capacity );
			_last = new CyclicIndex( _capacity );
		}

		public RingBuffer(T[] b)
		{
			_capacity = b.Length;
			_buffer = new T[_capacity];
			_numQueued = 0;
			System.Array.Copy( b, _buffer, _capacity );
			_first = new CyclicIndex( _capacity, 0 );
			_last = new CyclicIndex( _capacity, 0 );
		}

		public bool IsEmpty
		{
			get { return NumQueued == 0; } 
		}

		public bool IsFull
		{
			get { return NumQueued >= _capacity; } 
		}

		public void Enqueue(T d)
		{
			if (IsFull)
			{
				throw new System.Exception( "Can't Enqueue into full buffer" );
			}
			_buffer[_last.Value] = d;
			_last.Increment( );
			_numQueued++;
		}

		virtual public T Dequeue()
		{
			T result = Peek( );
			_first.Increment( );
			_numQueued--;
			return result;
		}

		public T Peek()
		{
			if (IsEmpty)
			{
				throw new System.Exception( "Can't Peek at empty buffer" );
			}
			return _buffer[_last.Value];
		}

		public void Fill(T d)
		{
			while (!IsFull)
			{
				Enqueue( d );
			}
		}

		public void Clear()
		{
			_first = new CyclicIndex( _capacity );
			_last = new CyclicIndex( _capacity );
			_numQueued = 0;
		}
	}

}
