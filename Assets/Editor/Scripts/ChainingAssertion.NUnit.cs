/*--------------------------------------------------------------------------
 * Chaining Assertion
 * ver 1.7.1.0 (Apr. 29th, 2013)
 *
 * created and maintained by neuecc <ils@neue.cc - @neuecc on Twitter>
 * licensed under Microsoft Public License(Ms-PL)
 * http://chainingassertion.codeplex.com/
 *--------------------------------------------------------------------------*/

/* -- Tutorial --
 * | at first, include this file on NUnit Project.
 * 
 * | three example, "Is" overloads.
 * 
 * // This same as Assert.AreEqual(25, Math.Pow(5, 2))
 * Math.Pow(5, 2).Is(25);
 * 
 * // This same as Assert.IsTrue("foobar".StartsWith("foo") && "foobar".EndWith("bar"))
 * "foobar".Is(s => s.StartsWith("foo") && s.EndsWith("bar"));
 * 
 * // This same as CollectionAssert.AreEqual(Enumerable.Range(1,5), new[]{1, 2, 3, 4, 5})
 * Enumerable.Range(1, 5).Is(1, 2, 3, 4, 5);
 * 
 * | CollectionAssert
 * | if you want to use CollectionAssert Methods then use Linq to Objects and Is
 * 
 * var array = new[] { 1, 3, 7, 8 };
 * array.Count().Is(4);
 * array.Contains(8).IsTrue(); // IsTrue() == Is(true)
 * array.All(i => i < 5).IsFalse(); // IsFalse() == Is(false)
 * array.Any().Is(true);
 * new int[] { }.Any().Is(false);   // IsEmpty
 * array.OrderBy(x => x).Is(array); // IsOrdered
 *
 * | Other Assertions
 * 
 * // Null Assertions
 * Object obj = null;
 * obj.IsNull();             // Assert.IsNull(obj)
 * new Object().IsNotNull(); // Assert.IsNotNull(obj)
 *
 * // Not Assertion
 * "foobar".IsNot("fooooooo"); // Assert.AreNotEqual
 * new[] { "a", "z", "x" }.IsNot("a", "x", "z"); /// CollectionAssert.AreNotEqual
 *
 * // ReferenceEqual Assertion
 * var tuple = Tuple.Create("foo");
 * tuple.IsSameReferenceAs(tuple); // Assert.AreSame
 * tuple.IsNotSameReferenceAs(Tuple.Create("foo")); // Assert.AreNotSame
 *
 * // Type Assertion
 * "foobar".IsInstanceOf<string>(); // Assert.IsInstanceOf
 * (999).IsNotInstanceOf<double>(); // Assert.IsNotInstanceOf
 * 
 * | Advanced Collection Assertion
 * 
 * var lower = new[] { "a", "b", "c" };
 * var upper = new[] { "A", "B", "C" };
 *
 * // Comparer CollectionAssert, use IEqualityComparer<T> or Func<T,T,bool> delegate
 * lower.Is(upper, StringComparer.InvariantCultureIgnoreCase);
 * lower.Is(upper, (x, y) => x.ToUpper() == y.ToUpper());
 *
 * // or you can use Linq to Objects - SequenceEqual
 * lower.SequenceEqual(upper, StringComparer.InvariantCultureIgnoreCase).Is(true);
 * 
 * | StructuralEqual
 * 
 * class MyClass
 * {
 *     public int IntProp { get; set; }
 *     public string StrField;
 * }
 * 
 * var mc1 = new MyClass() { IntProp = 10, StrField = "foo" };
 * var mc2 = new MyClass() { IntProp = 10, StrField = "foo" };
 * 
 * mc1.IsStructuralEqual(mc2); // deep recursive value equality compare
 * 
 * mc1.IntProp = 20;
 * mc1.IsNotStructuralEqual(mc2);
 * 
 * | DynamicAccessor
 * 
 * // AsDynamic convert to "dynamic" that can call private method/property/field/indexer.
 * 
 * // a class and private field/property/method.
 * public class PrivateMock
 * {
 *     private string privateField = "homu";
 * 
 *     private string PrivateProperty
 *     {
 *         get { return privateField + privateField; }
 *         set { privateField = value; }
 *     }
 * 
 *     private string PrivateMethod(int count)
 *     {
 *         return string.Join("", Enumerable.Repeat(privateField, count));
 *     }
 * }
 * 
 * // call private property.
 * var actual = new PrivateMock().AsDynamic().PrivateProperty;
 * Assert.AreEqual("homuhomu", actual);
 * 
 * // dynamic can't invoke extension methods.
 * // if you want to invoke "Is" then cast type.
 * (new PrivateMock().AsDynamic().PrivateMethod(3) as string).Is("homuhomuhomu");
 * 
 * // set value
 * var mock = new PrivateMock().AsDynamic();
 * mock.PrivateProperty = "mogumogu";
 * (mock.privateField as string).Is("mogumogu");
 * 
 * -- more details see project home --*/

/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NUnit.Framework
{
    #region Extensions

    [System.Diagnostics.DebuggerStepThroughAttribute]
    public static partial class AssertEx
    {
        /// <summary>Assert.AreEqual, if T is IEnumerable then CollectionAssert.AreEqual</summary>
        public static void Is<T>(this T actual, T expected, string message = "")
        {
            if (typeof(T) != typeof(string) && typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                ((IEnumerable)actual).Cast<object>().Is(((IEnumerable)expected).Cast<object>(), message);
                return;
            }

            Assert.AreEqual(expected, actual, message);
        }

        /// <summary>Assert.IsTrue(predicate(value))</summary>
        public static void Is<T>(this T value, Expression<Func<T, bool>> predicate, string message = "")
        {
            var condition = predicate.Compile().Invoke(value);

            var paramName = predicate.Parameters.First().Name;
            string msg = "";
            try
            {
                var dumper = new ExpressionDumper<T>(value, predicate.Parameters.Single());
                dumper.Visit(predicate);
                var dump = string.Join(", ", dumper.Members.Select(kvp => kvp.Key + " = " + kvp.Value).ToArray());
                msg = string.Format("\r\n{0} = {1}\r\n{2}\r\n{3}{4}",
                    paramName, value, dump, predicate,
                    string.IsNullOrEmpty(message) ? "" : ", " + message);
            }
            catch
            {
                msg = string.Format("{0} = {1}, {2}{3}",
                    paramName, value, predicate,
                    string.IsNullOrEmpty(message) ? "" : ", " + message);
            }

            Assert.IsTrue(condition, msg);
        }

        /// <summary>CollectionAssert.AreEqual</summary>
        public static void Is<T>(this IEnumerable<T> actual, params T[] expected)
        {
            Is(actual, expected.AsEnumerable());
        }

        /// <summary>CollectionAssert.AreEqual</summary>
        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected, string message = "")
        {
            CollectionAssert.AreEqual(expected.ToArray(), actual.ToArray(), message);
        }

        /// <summary>CollectionAssert.AreEqual</summary>
        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected, IEqualityComparer<T> comparer, string message = "")
        {
            Is(actual, expected, comparer.Equals, message);
        }

        /// <summary>CollectionAssert.AreEqual</summary>
        public static void Is<T>(this IEnumerable<T> actual, IEnumerable<T> expected, Func<T, T, bool> equalityComparison, string message = "")
        {
            CollectionAssert.AreEqual(expected.ToArray(), actual.ToArray(), new ComparisonComparer<T>(equalityComparison), message);
        }

        /// <summary>Assert.AreNotEqual, if T is IEnumerable then CollectionAssert.AreNotEqual</summary>
        public static void IsNot<T>(this T actual, T notExpected, string message = "")
        {
            if (typeof(T) != typeof(string) && typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                ((IEnumerable)actual).Cast<object>().IsNot(((IEnumerable)notExpected).Cast<object>(), message);
                return;
            }

            Assert.AreNotEqual(notExpected, actual, message);
        }

        /// <summary>CollectionAssert.AreNotEqual</summary>
        public static void IsNot<T>(this IEnumerable<T> actual, params T[] notExpected)
        {
            IsNot(actual, notExpected.AsEnumerable());
        }

        /// <summary>CollectionAssert.AreNotEqual</summary>
        public static void IsNot<T>(this IEnumerable<T> actual, IEnumerable<T> notExpected, string message = "")
        {
            CollectionAssert.AreNotEqual(notExpected.ToArray(), actual.ToArray(), message);
        }

        /// <summary>CollectionAssert.AreNotEqual</summary>
        public static void IsNot<T>(this IEnumerable<T> actual, IEnumerable<T> notExpected, IEqualityComparer<T> comparer, string message = "")
        {
            IsNot(actual, notExpected, comparer.Equals, message);
        }

        /// <summary>CollectionAssert.AreNotEqual</summary>
        public static void IsNot<T>(this IEnumerable<T> actual, IEnumerable<T> notExpected, Func<T, T, bool> equalityComparison, string message = "")
        {
            CollectionAssert.AreNotEqual(notExpected.ToArray(), actual.ToArray(), new ComparisonComparer<T>(equalityComparison), message);
        }

        /// <summary>Assert.IsNull</summary>
        public static void IsNull<T>(this T value, string message = "")
        {
            Assert.IsNull(value, message);
        }

        /// <summary>Assert.IsNotNull</summary>
        public static void IsNotNull<T>(this T value, string message = "")
        {
            Assert.IsNotNull(value, message);
        }

        /// <summary>Is(true)</summary>
        public static void IsTrue(this bool value, string message = "")
        {
            value.Is(true, message);
        }

        /// <summary>Is(false)</summary>
        public static void IsFalse(this bool value, string message = "")
        {
            value.Is(false, message);
        }

        /// <summary>Assert.AreSame</summary>
        public static void IsSameReferenceAs<T>(this T actual, T expected, string message = "")
        {
            Assert.AreSame(expected, actual, message);
        }

        /// <summary>Assert.AreNotSame</summary>
        public static void IsNotSameReferenceAs<T>(this T actual, T notExpected, string message = "")
        {
            Assert.AreNotSame(notExpected, actual, message);
        }

        /// <summary>Assert.IsInstanceOf</summary>
        public static TExpected IsInstanceOf<TExpected>(this object value, string message = "")
        {
            Assert.IsInstanceOf<TExpected>(value, message);
            return (TExpected)value;
        }

        /// <summary>Assert.IsNotInstanceOf</summary>
        public static void IsNotInstanceOf<TWrong>(this object value, string message = "")
        {
            Assert.IsNotInstanceOf<TWrong>(value, message);
        }

        /// <summary>EqualityComparison to IComparer Converter for CollectionAssert</summary>
        private class ComparisonComparer<T> : IComparer
        {
            readonly Func<T, T, bool> comparison;

            public ComparisonComparer(Func<T, T, bool> comparison)
            {
                this.comparison = comparison;
            }

            public int Compare(object x, object y)
            {
                return (comparison != null)
                    ? comparison((T)x, (T)y) ? 0 : -1
                    : object.Equals(x, y) ? 0 : -1;
            }
        }

        private class ReflectAccessor<T>
        {
            public Func<object> GetValue { get; private set; }
            public Action<object> SetValue { get; private set; }

            public ReflectAccessor(T target, string name)
            {
                var field = typeof(T).GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (field != null)
                {
                    GetValue = () => field.GetValue(target);
                    SetValue = value => field.SetValue(target, value);
                    return;
                }

                var prop = typeof(T).GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (prop != null)
                {
                    GetValue = () => prop.GetValue(target, null);
                    SetValue = value => prop.SetValue(target, value, null);
                    return;
                }

                throw new ArgumentException(string.Format("\"{0}\" not found : Type <{1}>", name, typeof(T).Name));
            }
        }

        #region ExpressionDumper

        private class ExpressionDumper<T> : ExpressionVisitor
        {
            ParameterExpression param;
            T target;

            public Dictionary<string, object> Members { get; private set; }

            public ExpressionDumper(T target, ParameterExpression param)
            {
                this.target = target;
                this.param = param;
                this.Members = new Dictionary<string, object>();
            }

            protected override System.Linq.Expressions.Expression VisitMember(MemberExpression node)
            {
                if (node.Expression == param && !Members.ContainsKey(node.Member.Name))
                {
                    var accessor = new ReflectAccessor<T>(target, node.Member.Name);
                    Members.Add(node.Member.Name, accessor.GetValue());
                }

                return base.VisitMember(node);
            }
        }

        #endregion
    }

    #endregion
}

