using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Katniss
{
    public sealed class Queue<T> : IEnumerable<T>
    {
        // Circular Queue
        int front = 0;
        int rear = 0;                       // 마지막 아이템의 인덱스 + 1

        public T[] Items = new T[4];

        public int Capacity = 4;
        public int Count { private set; get; } = 0;

        private int Front
        {
            get { return front; }
            set { front = value % Capacity; }
        }

        private int Rear
        {
            get { return rear; }
            set { rear = value % Capacity; }
        }

        public bool Contains(T value)
        {
            if (Count == 0) return false;

            int i = Front;
            do
            {
                if (EqualityComparer<T>.Default.Equals(Items[i], value)) return true;
                //i++;
                //if (i >= Capacity) i -= Capacity;
                i = (i + 1) % Capacity;
            }
            while (i != Rear);

            return false;
        }

        public T Peek()
        {
            if (Count == 0) throw new Exception("빈 Queue입니다.");

            return Items[Front];
		}

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<T> GetEnumerator()
        {
            if (Count != 0)
            {
                int i = Front;
                do
                {
                    yield return Items[i];
                    i = (i + 1) % Capacity;
                }
                while (i != Rear);
            }
        }


        public void Enqueue(T value)
        {
            if (Count == Capacity) ExpandCapacity();

            Items[Rear] = value;
            Rear++;
            Count++;
        }

        public T Dequeue()
        {
            if (Count == 0) throw new Exception("빈 Queue입니다.");

            T tmp = Items[Front];
            Items[Front] = default(T);
            Front++;
            Count--;

            return tmp;
        }

        public void Clear()
        {
            Array.Clear(Items, 0, Count);
            Front = 0;
            Rear = 0;
            Count = 0;
        }

        public void ExpandCapacity()
        {
            if (Capacity > Int32.MaxValue / 2)
                throw new Exception("List의 크기가 허용된 범위를 초과했습니다.");

            Capacity *= 2;

            T[] NewItems = new T[Capacity];
            Array.Copy(Items, Front, NewItems, 0, Count - Front);
            if (Front != 0) Array.Copy(Items, 0, NewItems, Count - Front, Front);
            Items = NewItems;
            Front = 0;
            Rear = Count;
        }
    }

    public class MyQueue : StudyBase
    {
        protected override void OnLog()
        {
            var queue = new Queue<string>();

            queue.Enqueue("1stJob");
            queue.Enqueue("2ndJob");
            // 1stJob
            Log(queue.Peek());

            queue.Enqueue("3rdJob");
            var str = queue.Dequeue();
            // 1stJob;
            Log(str);

            queue.Enqueue("4thJob");
            // 2ndJob, 3rdJob, 4thJob
            queue.LogValues();
        }
    }
}