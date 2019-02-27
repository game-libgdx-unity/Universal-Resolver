using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ArrayExtensions {

	
	public static T[] RemoveAt<T>(this T[] source, int index)
	{
		T[] dest = new T[source.Length - 1];
		if( index > 0 )
			Array.Copy(source, 0, dest, 0, index);

		if( index < source.Length - 1 )
			Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

		return dest;
	}
	
	public static bool IsEqual<T>(this T[] a, T[] b)
	{
		return (a.Length == b.Length && a.Intersect(b).Count() == a.Length);
	}
	
	public static IEnumerable<T> Flatten<T>(this T[,] items) {
		for (int i = 0; i < items.GetLength(0); i++)
		for (int j = 0; j < items.GetLength(1); j++)
			yield return items[i, j];
	}
}
