using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Katniss
{
	public sealed class List<T> : IEnumerable<T>
	{
		private int Capacity = 2;

		public T[] Items = new T[2];

		public int Count { private set; get; } = 0;

		public T this[int index]
		{
			set
			{
				if (index >= Count || index < 0) throw new Exception("범위를 벗어난 인덱스입니다.");
				Items[index] = value;
			}
			get
			{
				if (index >= Count || index < 0) throw new Exception("범위를 벗어난 인덱스입니다.");
				return Items[index];
			}
		}


		public bool Contains(T value)
		{
			for (int i = 0; i < Count; i++)
			{
				if (EqualityComparer<T>.Default.Equals(Items[i], value)) return true;
			}
			return false;
		}

		public int IndexOf(T value)
		{
			for (int i = 0; i < Count; i++)
			{
				if (EqualityComparer<T>.Default.Equals(Items[i], value))
				{
					return i;
				}
			}
			Debug.Log("값을 찾을 수 없습니다.");
			return -1;
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public IEnumerator<T> GetEnumerator()
		{
			int i = 0;
			while (i < Count)
			{
				yield return Items[i];
				i++;
			}
		}

		public void Add(T value)
		{
			if (Count == Capacity)
			{
				ExpandCapacity();
			}

			Items[Count] = value;
			Count++;
		}

		public void Insert(int index, T value)
		{
			if (index >= Count)
				throw new Exception("범위를 벗어난 인덱스입니다.");

			if (Count == Capacity)
			{
				ExpandCapacity();
			}

			Array.Copy(Items, index, Items, index + 1, Count - index);
			Items[index] = value;
			Count++;
		}

		public bool Remove(T value)
		{
			int index = IndexOf(value);
			if (index < 0) return false;

			RemoveAt(index);
			return true;
		}

		public void RemoveAt(int index)
		{
			if (index >= Count || index < 0) throw new Exception("범위를 벗어난 인덱스입니다.");

			Count--;
			Array.Copy(Items, index + 1, Items, index, Count - index);
			Items[Count] = default(T);
		}

		public void Clear()
		{
			Array.Clear(Items, 0, Count);
			Count = 0;
		}

		public void ExpandCapacity()
		{
			if (Capacity > Int32.MaxValue / 2)
				throw new Exception("List의 크기가 허용된 범위를 초과했습니다.");

			Capacity *= 2;

			T[] NewItems = new T[Capacity];
			Array.Copy(Items, 0, NewItems, 0, Count);
			Items = NewItems;
		}
	}
}