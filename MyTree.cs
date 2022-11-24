using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Katniss
{
	public class MyTree : StudyBase
	{
		protected override void OnLog()
		{
			var bTree = new BST<int>();

			bTree.Insert(10);
			bTree.Insert(5);
			bTree.Insert(9);
			bTree.Insert(15);
			// 5 9 10 15
			bTree.LogValues();

			bTree.Insert(2);
			bTree.Remove(9);
			bTree.Insert(7);
			// 2 5 7 10 15
			bTree.LogValues();

			// 5 7 10
			bTree.GetOverlaps(5, 10).LogValues();
		}
	}
}