using System.Collections.Generic;
using System.Linq;

namespace UnityIoC
{
    public partial class Context
    {
        private static Dictionary<string, object> _objectNames = new Dictionary<string, object>();
        private static Dictionary<string, HashSet<object>> ObjectTags = new Dictionary<string, HashSet<object>>();

        public static void BindByName(object obj, string name)
        {
            _objectNames[name] = obj;
        }

        public static void BindByTags(object obj, params string[] tags)
        {
            for (var i = 0; i < tags.Length; i++)
            {
                ObjectTags[tags[i]].Add(obj);
            }
        }

        public static T ResolveByName<T>(string name)
        {
            var obj = _objectNames[name];
            if (obj.GetType().IsAssignableFrom(typeof(T)))
            {
                return (T) obj;
            }
            return default(T);
        }

        public static HashSet<object> ResolveByTag(string tag)
        {
            return ObjectTags[tag];
        }

        public static HashSet<object> ResolveByTags<T>(params string[] tags)
        {
            var output = new HashSet<object>();
            for (var i = tags.Length - 1; i >= 0; i--)
            {
                foreach (var o in ObjectTags[tags[i]])
                {
                    output.Add(o);
                }
            }
            return output;
        }

        public static T ResolveByTag<T>(params string[] tags)
        {
            for (var i = tags.Length - 1; i >= 0; i--)
            {
                foreach (var o in ObjectTags[tags[i]])
                {
                    if (o is T)
                    {
                        return (T) o;
                    }
                }
            }
            return default(T);
        }
        
        public static IEnumerable<T> ResolveManyByTags<T>(params string[] tags)
        {
            List<T> t = new List<T>();
            for (var i = tags.Length - 1; i >= 0; i--)
            {
                foreach (var o in ObjectTags[tags[i]])
                {
                    if (o is T)
                    {
                        t.Add((T) o);
                    }
                }
            }
            return t;
        }
    }
}