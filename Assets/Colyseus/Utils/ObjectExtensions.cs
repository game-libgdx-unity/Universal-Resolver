﻿using System.Collections.Generic;

namespace Colyseus.Utils
{
    public static class ObjectExtensions
    {
        public static T ToObject<T>(object source) where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in (IDictionary<string, object>)source) {
                someObjectType
                         .GetProperty(item.Key)
                         .SetValue(someObject, item.Value, null);
            }

            return someObject;
        }
    }
}
