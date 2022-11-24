using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Katniss
{
	public sealed class Dictionary<K, T> : IEnumerable<KeyValuePair<K, T>>
	{
		int[] Bucket = new int[32];
		Entry[] Entries = new Entry[32];			// 0번 인덱스는 비워둠

		private int Capacity = 32;
		public int Count { private set; get; } = 0;
		
		public T this[K key]
		{
            set
            {
				Add(key, value);
			}
			get
            {
				return Entries[0].value;
			}
		}

		private int GetHashIdx(int hash)
		{
			return (Math.Abs(hash)) % 31 + 1;		// Range : 1 ~ 31
		}
		/*
		public bool ContainsKey(K key)
		{
			int hash = key.GetHashCode();
			int idx = GetHashIdx(hash);
			int idx;
            while (true)
            {
				idx = Bucket[hash_idx];
            }
			return false;
		}

		
		public bool ContainsValue(T value)
		{
			foreach(KeyValuePair<K, T> item in this)
            {
				if (EqualityComparer<T>.Default.Equals(item.Value, value))
					return true;
            }
			return false;
		}

		public bool TryGetValue(K key, out T result)
		{
			int hash = key.GetHashCode();
			int idx = GetHashIdx(hash);
			if (EqualityComparer<K>.Default.Equals(Entries[idx].key, key))
            {
				result = Entries[idx].value;
				return true;
            }
			else result = default;
            return false; //오타
		}
		*/

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public IEnumerator<KeyValuePair<K, T>> GetEnumerator()
		{
			for (int i = 1; i < 32; i++)
			{
				if (Bucket[i] != 0)
                {
					int idx = Bucket[i];
                    while (idx!=0)
                    {
						yield return new KeyValuePair<K, T>(Entries[idx].key, Entries[idx].value);
						idx = Entries[idx].next;
                    }
				}
				i++;
			}
		}

		public bool Add(K key, T value)
		{
			// 언제 용량을 늘려야 하는지 고민 필요
			if (Count == Capacity - 1)
				ExpandCapacity();

			int hash = key.GetHashCode();
			int hash_idx = GetHashIdx(hash);

			int idx;
			int pre;

			if (Bucket[hash_idx] == 0)
				idx = hash_idx;
			else idx = Bucket[hash_idx];

			while (true) {
				if (Entries[idx].hashCode == hash)
				{
					Debug.Log($"{key}는 이미 존재하는 key입니다.");
					return false;
				}
				else if (Entries[idx].next != 0)
                {
					idx = Entries[idx].next;
					continue;
				}
                else {
					pre = idx;
					while (Entries[idx].hashCode != 0)
                    {
						idx++;
						if (idx == Capacity) idx = idx - Capacity + 1;
                    }

					Entries[idx] = new Entry(hash, 0, key, value);

					if (Bucket[hash_idx] == 0)
						Bucket[hash_idx] = idx;

					if (pre != idx)
						Entries[pre].next = idx;

					Count++;

					//Debug.Log($"{idx} {Entries[idx].key} {Entries[idx].value}");

					return true;
				}
			}
		}
		/*
		public bool Remove(K key)
		{
			int hash = key.GetHashCode();
			int idx = GetHashIdx(hash);
			Entries[idx] = default(Entry);
			//next문제 발생 -> pre로 이전 entry 인덱스 저장

			return true;
		}

		public void Clear()
		{
			Array.Clear(Entries, 0, Capacity);
			//bucket clear
			Count = 0;
		}
		*/

		void ExpandCapacity()
		{
			if (Capacity > Int32.MaxValue / 2)
				throw new Exception("Dictionary의 크기가 허용된 범위를 초과했습니다.");

			Capacity *= 2;

			int[] NewBucket = new int[Capacity];
			Entry[] NewEntries = new Entry[Capacity];
			Rehash(NewBucket, NewEntries);
			Bucket = NewBucket;
			Entries = NewEntries;
		}

		void Rehash(int[] newBucket, Entry[] newEntries)
        {
			int i = 0;
			int _count = 0;
			//while (_count < Count && i < Capacity)
			//{
			//	if (!EqualityComparer<K>.Default.Equals(Bucket[i].key, default(K)))
			//	{
			//		_count++;
			//	}
			//	i++;
			//}
		}

		struct Entry
		{
			public int hashCode;
			public int next;
			public K key;
			public T value;

			public Entry(int _hashCode, int _next, K _key, T _value)
            {
				hashCode = _hashCode;
				next = _next;
				key = _key;
				value = _value;
				// destroy 함수
            }
		}
	}
}