using System;
using System.Collections.Generic;
namespace MapEditor
{
    public static partial class Ext
    {
        public static T ToEnum<T>(this string value)
        {
            return (T) Enum.Parse(typeof(T), value);
        }
    }
    
}
