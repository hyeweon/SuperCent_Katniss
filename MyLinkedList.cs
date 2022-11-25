using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Katniss
{
	public class MyLinkedList : StudyBase
	{
		protected override void OnLog()
		{
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