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



	public sealed class LinkedList<T> : IEnumerable<T>
	{
		// Double-ended Linked List
		public int Count { private set; get; } = 0;
		LinkedListNode<T> head = null;

		public LinkedListNode<T> First
		{
            get
            {
				if (head == null)
					throw new Exception("LinkedList에 유효한 node가 없습니다.");

				return head;
            }
		}

		public LinkedListNode<T> Last
		{
			get
			{
				if (head == null)
					throw new Exception("LinkedList에 유효한 node가 없습니다.");

				LinkedListNode<T> node = head;
				while (node.next != null)
				{
					node = node.next;
				}
				return node;
			}
		}


		public bool Contains(T value)
		{
			LinkedListNode<T> node = head;
			while (node != null)
            {
				if (EqualityComparer<T>.Default.Equals(node.value, value)) return true;
				node = node.next;
            }
			return false;
		}

		public LinkedListNode<T> Find(T value)
		{
			LinkedListNode<T> node = head;
			while (node != null)
			{
				if (EqualityComparer<T>.Default.Equals(node.value, value)) return node;
				node = node.next;
			}
			Debug.Log("값을 찾을 수 없습니다.");
			return null;
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public IEnumerator<T> GetEnumerator()
		{
			LinkedListNode<T> node = head;
			while (node != null)
			{
				yield return node.value;
				node = node.next;
			}
		}


		public LinkedListNode<T> AddFirst(T value)
		{
			return AddBefore(head, value);
		}

		public LinkedListNode<T> AddLast(T value)
		{
			return AddAfter(Last, value);
		}


		public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
		{
			// node == null이면 새로운 head
			LinkedListNode<T> newNode = new LinkedListNode<T>(value);

			if (Count != 0)
			{
				newNode.next = node;
				newNode.prev = node.prev;
				if (node.prev != null) node.prev.next = newNode;
				node.prev = newNode;

				if (node == head) head = newNode;
			}
			else head = newNode;

			Count++;

			return newNode;
		}

		public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
		{
			// node == null이면 새로운 head
			LinkedListNode<T> newNode = new LinkedListNode<T>(value);

			if (Count != 0)
			{
				newNode.prev = node;
				newNode.next = node.next;
				if (node.next != null) node.next.prev = newNode;
				node.next = newNode;
			}
			else head = newNode;

			Count++;

			return newNode;
		}

		public bool Remove(T value)
		{
			LinkedListNode<T> node = Find(value);

			if (node!=null)
            {
				Remove(node);
				return true;
            }
			return false;
		}

		public void Remove(LinkedListNode<T> node)
		{
			if (node == null)
				throw new Exception("node가 null입니다.");

			if (head == node) head = node.next;

			if (node.next != null) node.next.prev = node.prev;
			if (node.prev != null) node.prev.next = node.next;

			node.distroy();
		}

		public void RemoveFirst()
		{
			Remove(head);
		}
		public void RemoveLast()
		{
			Remove(Last);
		}

		public void Clear()
		{
			LinkedListNode<T> node = head;
			LinkedListNode<T> nextNode;
			while (node != null)
			{
				nextNode = node.next;
				node.distroy();
				node = nextNode;
			}

			head = null;
			Count = 0;
		}
	}

	public class LinkedListNode<T>
	{
		public T value { get; set; }
		public LinkedListNode<T> next { get; set; }
		public LinkedListNode<T> prev { get; set; }

		public LinkedListNode(T _value){
			value = _value;
		}

		public void distroy()
        {
			value = default(T);
			next = null;
			prev = null;
        }
	}

	public class Nov21th : StudyBase
	{
		protected override void OnLog()
		{
			// List
			var aList = new List<int>();

			aList.Add(2);
			// 2
			aList.LogValues();

			aList.Insert(0, 1);
			// 1, 2
			aList.LogValues();

			aList.Add(4);
			aList.Insert(aList.Count - 1, 3);
			// 1, 2, 3, 4
			aList.LogValues();

			aList.Remove(2);
			aList.RemoveAt(0);
			// 4
			Log(aList[aList.Count - 1]);


			// Linked List
			var lList = new LinkedList<string>();

			lList.AddFirst("My name is");
			lList.AddLast("AlphaGo");
			lList.AddLast("Hi");
			// My name is, AlphaGo, Hi
			lList.LogValues();

			lList.Remove("Hi");
			lList.AddFirst("Hello");
			// Hello, My name is, AlphaGo
			lList.LogValues();

			lList.RemoveFirst();
			lList.AddLast("I'm glad to meet you");
			// My name is, AlphaGo, I'm glad to meet you
			lList.LogValues();
		}
	}
}