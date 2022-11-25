using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Katniss
{
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
            if (Count == 0)
            {
				head = new LinkedListNode<T>(value, this);
				Count++;
				return head;
			}
			return AddBefore(head, value);
		}

		public LinkedListNode<T> AddLast(T value)
		{
			if (Count == 0) return AddFirst(value);
			return AddAfter(Last, value);
		}

		public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
		{
			if (node == null) throw new Exception("node가 null입니다.");
			if (this != node.list) throw new Exception("이 LinkedList에 포함된 node가 아닙니다.");

			LinkedListNode<T> newNode = new LinkedListNode<T>(value, this);

			newNode.next = node;
			newNode.prev = node.prev;
			if (node.prev != null) node.prev.next = newNode;
			node.prev = newNode;

			if (node == head) head = newNode;

			Count++;

			return newNode;
		}

		public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
		{
			if (node == null) throw new Exception("node가 null입니다.");
			if (this != node.list) throw new Exception("이 LinkedList에 포함된 node가 아닙니다.");

			LinkedListNode<T> newNode = new LinkedListNode<T>(value, this);

			newNode.prev = node;
			newNode.next = node.next;
			if (node.next != null) node.next.prev = newNode;
			node.next = newNode;

			Count++;

			return newNode;
		}

		public bool Remove(T value)
		{
			LinkedListNode<T> node = Find(value);

			if (node != null)
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
		public LinkedList<T> list { get; set; }

		public LinkedListNode(T _value, LinkedList<T> _list)
		{
			value = _value;
			list = _list;
		}

		public void distroy()
		{
			value = default(T);
			next = null;
			prev = null;
		}
	}
}