using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IEnumerableExtension {

	
	//usage: var seq = new List<int> { 1, 3, 12, 19, 33 };
	//       var transformed = seq.Scan(((state, item) => state + item), 0).Skip(1);
	public static void Do<T>(this IEnumerable<T> input, Action<T> next) {
		var enumerator = input.GetEnumerator();
		while (enumerator.MoveNext())
		{
			next(enumerator.Current);
		}
	}
	
	//usage: var seq = new List<int> { 1, 3, 12, 19, 33 };
	//       var transformed = seq.Scan(((state, item) => state + item), 0).Skip(1);
	public static IEnumerable<U> Scan<T, U>(this IEnumerable<T> input, Func<U, T, U> next, U state) {
		yield return state;
		foreach(var item in input) {
			state = next(state, item);
			yield return state;
		}
	}
	
	//usage: IEnumerable<int> seq = new List<int> { 1, 3, 12, 19, 33 };
	//		 IEnumerable<int> transformed = seq.Scan((state, item) => state + item);
	public static IEnumerable<T> Scan<T>(this IEnumerable<T> Input, Func<T, T, T> Accumulator)
	{
		using (var enumerator = Input.GetEnumerator())
		{
			if (!enumerator.MoveNext())
				yield break;
			T state = enumerator.Current;
			yield return state;
			while (enumerator.MoveNext())
			{
				state = Accumulator(state, enumerator.Current);
				yield return state;
			}
		}
	}
	
	public static IEnumerable<T> Flatten<T>(this T[,] items) {
		for (var i = 0; i < items.GetLength(0); i++)
		for (var j = 0; j < items.GetLength(1); j++)
			yield return items[i, j];
	}
}
