using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Katniss
{
	public sealed class BST<T> : IEnumerable<T>
	{
		BSTNode<T> root;

		public int Count { private set; get; } = 0;
		public BSTNode<T> Root { private set; get; } = null;
		public IComparer<T> Comparer = Comparer<T>.Default;

		public bool Contains(T value)
		{
			if (Count == 0) return false;

			BSTNode<T> currNode = root;
			while (true)
			{
				int comp = Comparer.Compare(currNode.value, value);
				switch (comp)
				{
					case var _ when comp == 0:
						return true;

					case var _ when comp > 0:
						if (currNode.left != default)
							currNode = currNode.left;
						else
							return false;
						break;

					case var _ when comp < 0:
						if (currNode.right != default)
							currNode = currNode.right;
						else
							return false;
						break;
				}
			}
		}

		public BSTNode<T> Find(T value)
		{
			if (Count == 0) throw new System.Exception("존재하지 않는 value입니다.");

			BSTNode<T> currNode = root;
			while (true)
			{
				int comp = Comparer.Compare(currNode.value, value);
				switch (comp)
				{
					case var _ when comp == 0:
						return currNode;

					case var _ when comp > 0:
						if (currNode.left != default)
							currNode = currNode.left;
						else
							throw new System.Exception("존재하지 않는 value입니다.");
						break;

					case var _ when comp < 0:
						if (currNode.right != default)
							currNode = currNode.right;
						else
							throw new System.Exception("존재하지 않는 value입니다.");
						break;
				}
			}
		}


		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public IEnumerator<T> GetEnumerator()
		{
			return root.GetEnumerator();
		}

		public IEnumerator<T> GetOverlaps(T min, T max)
		{
			return root.GetOverlaps(min, max);
		}

		public BSTNode<T> Insert(T value)
		{
            if (Count == 0)
            {
				root = new BSTNode<T>(value, default);
				Count++;
				return root;
            }

			BSTNode<T> currNode = root;
			while (true)
            {
				int comp = Comparer.Compare(currNode.value, value);
				switch (comp)
                {
					case var _ when comp == 0:
						throw new System.Exception("중복된 값입니다.");

					case var _ when comp > 0:
						if (currNode.left != default)
							currNode = currNode.left;
                        else
                        {
							currNode.left = new BSTNode<T>(value, currNode);
							Count++;
							return currNode.left;
						}
						break;

					case var _ when comp < 0:
						if (currNode.right != default)
							currNode = currNode.right;
						else
						{
							currNode.right = new BSTNode<T>(value, currNode);
							Count++;
							return currNode.right;
						}
						break;
				}
            }
		}

		public bool Remove(T value)
		{
			if (Count == 0) return false;

			BSTNode<T> currNode = root;
			while (true)
			{
				int comp = Comparer.Compare(currNode.value, value);
				switch (comp)
				{
					case var _ when comp == 0:
						Remove(currNode);
						return true;

					case var _ when comp > 0:
						if (currNode.left != default)
							currNode = currNode.left;
						else
							return false;
						break;

					case var _ when comp < 0:
						if (currNode.right != default)
							currNode = currNode.right;
						else
							return false;
						break;
				}
			}
		}
		
		public void Remove(BSTNode<T> node)
		{
			Count--;

            if (node.right == default)
            {
				if (node.left != default)
				{
					node.value = node.left.value;
					node.right = node.left.right;
					node.left = node.left.left;
				}
                else
                {
					if (node.parent.left == node)
						node.parent.left = default;
					else if (node.parent.right == node)
						node.parent.right = default;
					else node.destroy();
				}
			}
            else
            {
				BSTNode<T> old_left = node.left;
				node.value = node.right.value;
				node.left = node.right.left;
				node.right = node.right.right;
				if (old_left != default)
                {
					while (true)
					{
						int comp = Comparer.Compare(node.value, old_left.value);
						switch (comp)
						{
							case var _ when comp > 0:
								if (node.left != default)
									node = node.left;
                                else
                                {
									node.left = old_left;
									return;
								}
								break;

							case var _ when comp < 0:
								if (node.right != default)
									node = node.right;
								else
								{
									node.right = old_left;
									return;
								}
								break;
						}
					}
				}
			}
		}

		public void Clear()
		{
			root.destroy();
			Count = 0;
		}
	}

	public class BSTNode<T> : IEnumerable<T>
	{
		public T value;
		public BSTNode<T> parent;
		public BSTNode<T> left;
		public BSTNode<T> right;

		public BSTNode(T _value, BSTNode<T> _parent)
        {
			value = _value;
			parent = _parent;
        }

		public void destroy()
        {
			value = default;
			parent = default;
			left = default;
			right = default;
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public IEnumerator<T> GetEnumerator()
		{
			if (left != default)
                foreach (T _value in left)
                    yield return _value;

            yield return value;

			if (right != default)
                foreach (T _value in right)
                    yield return _value;
        }

		public IEnumerator<T> GetOverlaps(T min, T max)
		{
			if (left != default && Comparer<T>.Default.Compare(value, min) > 0)
            {
				IEnumerator enumerator = left.GetOverlaps(min, max);
				while (enumerator.MoveNext())
				{
					yield return (T)enumerator.Current;
				}
			}

			if (Comparer<T>.Default.Compare(value, min) >= 0
				&& Comparer<T>.Default.Compare(value, max) <= 0)
				yield return value;

			if (right != default && Comparer<T>.Default.Compare(value, max) < 0)
			{
				IEnumerator enumerator = right.GetOverlaps(min, max);
				while (enumerator.MoveNext())
				{
					yield return (T)enumerator.Current;
				}
			}
		}
	}
}