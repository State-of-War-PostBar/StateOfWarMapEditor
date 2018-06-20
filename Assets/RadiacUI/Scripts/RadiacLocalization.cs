using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace RadiacUI
{
    /// <summary>
    /// Only for text support.
    /// The localization support for other elements, such as textures, are managed out of the Radiac UI context.
    /// </summary>
    public static class LocalizationSupport
    {
        public static Dictionary<string, string> transfer = new Dictionary<string, string>();
        
        static Regex matcherEq = new Regex(@"\$e\$", RegexOptions.Compiled);
        static Regex matcherLb = new Regex(@"\$n\$", RegexOptions.Compiled);
        static Regex matcherCs = new Regex(@"\$h\$", RegexOptions.Compiled);
        
        public static void LoadLocalizationFile()
        {
            var file = Resources.Load("Localization") as TextAsset;
            
            if(file == null) throw new InvalidOperationException("Localization config file not found!");
            
            // Hard code format.
            // Syntax :
            // character # for single line comment.
            // no multiple comments yet.
            // use ini file format, keys and entries.
            // equals sign is seperator. Spaces around equals sign are considered as part of strings.
            // use $e$ for equal sign of text.
            // use $n$ for line break.
            // use $h$ for charactor #.
            
            string[] lines = file.text.Replace("\r", "").Split('\n');
            int curLine = 0;
            foreach(var line in lines)
            {
                curLine++;
                int index = line.IndexOf('#');
                if(index == -1)
                    index = line.Length;
                var s = line.Substring(0, index);
                if(s == null || s == "" || s.Length == 0)
                {
                    var lineWithoutSpace = s.Replace(" ", "").Replace("\t", "");
                    if(lineWithoutSpace.Length != 0)
                    {
                        throw new InvalidOperationException("Cannot find '=' at line " + curLine);
                    }
                    continue;
                }
                else
                {
                    var parts = s.Split('=');
                    if(parts.Length > 2)
                    {
                            throw new InvalidOperationException("Too many equals signs at line " + curLine);
                    }
                    
                    for(int i=0; i<2; i++)
                    {
                        parts[i] = matcherLb.Replace(parts[i], "\n");
                        parts[i] = matcherEq.Replace(parts[i], "=");
                        parts[i] = matcherCs.Replace(parts[i], "#");
                    }
                    
                    transfer.Add(parts[0], parts[1]);
                }
            }
        }
        
        public static string GetLocalizedString(string source)
        {
            var src = source.Split('$');
            StringBuilder sb = new StringBuilder();
            
            for(int i=0; i<src.Length; i++)
            {
                string s = src[i];
                if((i & 1) == 1)
                {
                    if(transfer.ContainsKey(s))
                    {
                        s = transfer[s];
                    }
                }
                
                sb.Append(s);
            }
            
            return sb.ToString();
        }
    }
    
    
    
}
