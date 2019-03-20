using System;
using System.Collections.Generic;

namespace Utils
{
    public static class Extension
    {
        //public object this[string propertyName]
        //{
        //    get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
        //    set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        //}

        public static int KeyValue<T>(this T t, string propertyName)
        {
            return (int) t.GetType().GetProperty(propertyName).GetValue(t, null);
        }

        public static int KeyValue<T>(this T t)
        {
            return KeyValue<T>(t, "Id");
        }

        public static Type GetItemType<T>(this IEnumerable<T> enumerable)
        {
            return typeof(T);
        }
    }
}