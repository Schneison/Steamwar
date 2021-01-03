using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Steamwar.Utils
{
    public class DebugUtil
    {
        public static string BuildString(object[] objs)
        {
            string str = "";
            if (objs.Length > 0)
            {
                str = objs[0] != null ? objs[0].ToString() : "null";
                for (int index = 1; index < objs.Length; ++index)
                {
                    object obj = objs[index];
                    str = str + " " + (obj != null ? obj.ToString() : "null");
                }
            }
            return str;
        }

        public static void LogErrorArgs(params object[] objs) => Debug.LogError(BuildString(objs));

        public static void LogErrorArgs(UnityEngine.Object context, params object[] objs) => Debug.LogError(BuildString(objs), context);

        public static void LogException(UnityEngine.Object context, string errorMessage, Exception e)
        {
            LogErrorArgs(context, errorMessage, ("\n" + e.ToString()));
        }
    }
}
