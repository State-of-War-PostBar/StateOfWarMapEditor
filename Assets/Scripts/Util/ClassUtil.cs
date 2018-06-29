using System;
using System.Reflection;

namespace MapEditor
{
    public static class ObjectExt
    {
        public static object Duplicate(this object x)
        {
            var type = x.GetType();
            object r = Activator.CreateInstance(type);
            foreach(var i in type.GetFields())
            {
                i.SetValue(r, i.GetValue(x));
            }
            return r;
        }
        
        public static void Assign(this object x, object other)
        {
            var type = x.GetType();
            foreach(var i in type.GetFields())
            {
                i.SetValue(x, i.GetValue(other));
            }
        }
    }
}
