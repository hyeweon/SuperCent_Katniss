using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Katniss
{
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