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
	
	public static T[] Remove<T>(this T[] source, T obj)
	{
		var index = Array.IndexOf(source, obj);

		if (index != -1)
		{
			RemoveAt(source, index);
		}

		return source;
	}
	public static T[] Where<T>(this T[] source, Predicate<T> func)
	{
		return Array.FindAll(source, func);
	}
	
	public static bool IsEqual<T>(this T[] a, T[] b)
	{
		return (a.Length == b.Length && a.Intersect(b).Count() == a.Length);
	}
}
