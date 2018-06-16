// #define RADIAC_DEBUG

using UnityEngine;

namespace RadiacUI
{
    internal static class Log
    {
        public static void AddLog(object s)
        {
            #if RADIAC_DEBUG
            Debug.Log(s);
            #endif
        }
        
        public static void AddLogFormat(string s, params object[] args)
        {
            #if RADIAC_DEBUG
            Debug.LogFormat(s, args);
            #endif
        }
        
        public static void AddWarning(object s)
        {
            Debug.LogWarning(s);
        }
        
        public static void AddWarningFormat(string s, params object[] args)
        {
            Debug.LogWarningFormat(s, args);
        }
    }
    
    
}
