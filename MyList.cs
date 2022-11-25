using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Katniss
{
	public class MyList : StudyBase
	{
		protected override void OnLog()
		{
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
		}
	}
}