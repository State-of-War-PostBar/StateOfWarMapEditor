using UnityEngine;
using System;
using System.Collections.Generic;

namespace RadiacUI
{
    // ================================================================================================================
    // ================================================================================================================
    // ================================================================================================================
    
    internal static class RadiacFunctional
    {
        public static To[] Map<From, To>(From[] src, Func<From, To> f)
        {
            To[] res = new To[src.Length];
            for(int i=0; i<=src.Length; i++) res[i] = f(src[i]); 
            return res;
        }
        
        public static SetTo Map<From, To, SetFrom, SetTo>(IEnumerable<From> src, Func<From, To> f)
            where SetTo : ICollection<To>, new()
        {
            SetTo res = new SetTo();
            foreach(var i in src) res.Add(f(i));
            return res;
        }
        
        public static int Count<T>(IEnumerable<T> src, Predicate<T> f)
        {
            int c = 0;
            foreach(var i in src) if(f(i)) c++;
            return c;
        }
        
        public static RetType Filter<T, RetType>(IEnumerable<T> src, Predicate<T> f)
            where RetType : ICollection<T>, new()
        {
            RetType res = new RetType();
            foreach(var i in src) if(f(i)) res.Add(i);
            return res;
        }
        
        public static Target ToCollection<T, Target>(this IEnumerable<T> src)
            where Target : ICollection<T>, new()
        {
            Target res = new Target();
            foreach(var i in src) res.Add(i);
            return res;
        }
        
        public static List<T> ToList<T>(this IEnumerable<T> src)
        {
            return src.ToCollection<T, List<T>>();
        }
        
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> src)
        {
            return src.ToCollection<T, HashSet<T>>();
        }
    }
    
    // ================================================================================================================
    // ================================================================================================================
    // ================================================================================================================
    
    internal static class RadiacCollection
    {
        public static V ElementAt<K, V>(this IDictionary<K, V> dict, K index)
            where V : new()
        {
            if(dict.ContainsKey(index)) return dict[index];
            V newElement = new V();
            dict.Add(index, newElement);
            return newElement;
        }
    }
    
    // ================================================================================================================
    // ================================================================================================================
    // ================================================================================================================
    
    internal static class RadiacAlgorithm
    {
        public enum DFSStyle
        {
            Preorder,
            Postorder
        }
        
        public static void ForeachChild(Transform t, Action<Transform> f)
        {
            for(int i=0; i<t.childCount; i++) f(t.GetChild(i));
        }
        
        public static void DFSHierarchy(Transform root, Action<Transform> f, DFSStyle style = DFSStyle.Preorder)
        {
            switch(style)
            {
                case DFSStyle.Preorder :
                    DFSPreorder(root, f);
                break;
                case DFSStyle.Postorder :
                    DFSPostorder(root, f);
                break;
                default : throw new ArgumentOutOfRangeException();
            }
        }
        
        static void DFSPreorder(Transform root, Action<Transform> f)
        {
            f(root);
            ForeachChild(root, (x) => DFSPreorder(x, f));
        }
        
        static void DFSPostorder(Transform root, Action<Transform> f)
        {
            ForeachChild(root, (x) => DFSPreorder(x, f));
            f(root);
        }
    }
    
    // ================================================================================================================
    // ================================================================================================================
    // ================================================================================================================
    internal static class RadiacUtility
    {
        public static Rect? Intersect(this Rect rect, Rect other)
        {
            float wr = Mathf.Min(rect.xMax, other.xMax);
            float wl = Mathf.Max(rect.xMin, other.xMin);
            float ht = Mathf.Min(rect.yMax, other.yMin);
            float hb = Mathf.Max(rect.yMin, other.yMin);
            if(wr < wl || ht < hb) return null;
            return new Rect(wl, hb, wr - wl, ht - hb);
        }
        
        public static Rect Transform(this Rect rect, Vector2 pos)
        {
            return new Rect(rect.x + pos.x, rect.y + pos.y, rect.width, rect.height);
        }
        
        public static bool IsInRect(this Vector2 pos, Rect rect)
        {
            return rect.Contains(pos);
        }
        
        public static bool IsInAnyOf(this Vector2 pos, params Rect[] rects)
        {
            foreach(var i in rects) if(pos.IsInRect(i)) return true;
            return false;
        }
        
        public static bool IsInAllOf(this Vector2 pos, params Rect[] rects)
        {
            foreach(var i in rects) if(!pos.IsInRect(i)) return false;
            return true;
        }
        
        public static bool Contains<T>(this IList<T> v, T val) where T : IEquatable<T>
        {
            foreach(var i in v) if(i.Equals(val)) return true;
            return false;
        }
        
        public static void DrawRectangleGizmos(Rect rect, float h, Color c)
        {
            var bottomLeft = new Vector3(rect.xMin, rect.yMin, h);
            var bottomRight = new Vector3(rect.xMax, rect.yMin, h);
            var topLeft = new Vector3(rect.xMin, rect.yMax, h);
            var topRight = new Vector3(rect.xMax, rect.yMax, h);
            Debug.DrawLine(bottomLeft, bottomRight, c);
            Debug.DrawLine(topLeft, topRight, c);
            Debug.DrawLine(bottomLeft, topLeft, c);
            Debug.DrawLine(bottomRight, topRight, c);
        }
        
        public static T FindComponentInParents<T>(this Transform cur) where T : MonoBehaviour
        {
            if(cur == null) return null;
            return _FindComponentInParents<T>(cur.parent);
        }
        
        static T _FindComponentInParents<T>(Transform cur) where T : MonoBehaviour
        {
            if(cur == null) return null;
            var x = cur.GetComponent<T>();
            if(x != null) return x;
            return _FindComponentInParents<T>(cur.parent);
        }
        
        public static T[] FindComponentsInParents<T>(this Transform cur) where T : MonoBehaviour
        {
            if(cur == null) return new T[0];
            List<T> res = new List<T>();
            _FindComponentsInParents<T>(cur.parent, res);
            return res.ToArray();
        }
        
        public static void _FindComponentsInParents<T>(Transform cur, List<T> res) where T : MonoBehaviour
        {
            if(cur == null) return;
            var x = cur.GetComponent<T>();
            if(x != null) res.Add(x);
            _FindComponentsInParents(cur.parent, res);
        }
        
        public static Color SetR(this Color c, float r) => new Color(r, c.g, c.b, c.a);
        public static Color SetG(this Color c, float g) => new Color(c.r, g, c.b, c.a);
        public static Color SetB(this Color c, float b) => new Color(c.r, c.g, b, c.a);
        public static Color SetA(this Color c, float a) => new Color(c.r, c.g, c.b, a);
        
        public static Vector2 SetX(this Vector2 v, float x) => new Vector2(x, v.y);
        public static Vector2 SetY(this Vector2 v, float y) => new Vector2(v.x, y);
        
        public static Vector3 SetX(this Vector3 v, float x) => new Vector3(x, v.y, v.z);
        public static Vector3 SetY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);
        public static Vector3 SetZ(this Vector3 v, float z) => new Vector3(v.x, v.y, z);
        
        public static Vector4 SetX(this Vector4 v, float x) => new Vector4(x, v.y, v.z, v.w);
        public static Vector4 SetY(this Vector4 v, float y) => new Vector4(v.x, y, v.z, v.w);
        public static Vector4 SetZ(this Vector4 v, float z) => new Vector4(v.x, v.y, z, v.w);
        public static Vector4 SetW(this Vector4 v, float w) => new Vector4(v.x, v.y, v.z, w);
        
        public static Vector2 Clamp(this Vector2 v, Vector2 l, Vector2 u)
            => new Vector2(Mathf.Clamp(v.x, l.x, u.x), Mathf.Clamp(v.y, l.y, u.y));
        public static Vector3 Clamp(this Vector3 v, Vector3 l, Vector3 u)
            => new Vector3(Mathf.Clamp(v.x, l.x, u.x), Mathf.Clamp(v.y, l.y, u.y), Mathf.Clamp(v.z, l.z, u.z));
        public static Vector4 Clamp(this Vector4 v, Vector4 l, Vector4 u)
            => new Vector4(Mathf.Clamp(v.x, l.x, u.x), Mathf.Clamp(v.y, l.y, u.y), Mathf.Clamp(v.z, l.z, u.z), Mathf.Clamp(v.w, l.w, u.w));
        public static Color Clamp(this Color v, Color l, Color u)
            => new Color(Mathf.Clamp(v.r, l.r, u.r), Mathf.Clamp(v.g, l.g, u.g), Mathf.Clamp(v.b, l.b, u.b), Mathf.Clamp(v.a, l.a, u.a));
    }
    
    
    
}
