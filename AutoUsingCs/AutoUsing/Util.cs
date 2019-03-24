using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace AutoUsing
{
    public static class Util
    {
        /// <summary>
        ///     Indicates whether the specified string is null or an System.String.Empty string.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>true if the value parameter is null or an empty string; otherwise, false.</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        ///     Returns a `camelCase` representation of the specified
        ///     value.
        /// 
        /// Remarks:
        ///     I implemented this method to aid in converting c# pascal case convetions
        ///     to typescript friendly camelCase. It's not used currently though.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCamelCase(this string value)
        {
            string result = string.Empty;

            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                char p = i == 0 ? char.MinValue : value[i - 1];

                result += ((p is ' ') || p is char.MinValue) ? $"{char.ToLower(c)}" : $"{c}";
            }

            return result;
        }

        public static string GetParentDir(string dir)
        {
            return Path.GetFullPath(Path.Combine(dir, @"..\"));
        }

        public static List<MethodInfo> GetExtensionMethods(this Type @class)
        {
            return @class.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(method => method.IsDefined(typeof(ExtensionAttribute), false)).ToList();
        }

        public static bool IsStatic(this Type @class)
        {
            return @class.IsAbstract && @class.IsSealed;
        }

        /// <summary>
        ///     Extension method info.
        /// </summary>
        /// <param name="method "></param>
        /// <returns>The class the method is extending</returns>
        public static Type GetExtendedClass(this MethodInfo method)
        {
            return method.GetParameters()[0].ParameterType;
        }

        public static Dictionary<K, V> ToDictionary<K, V>(this IEnumerable<KeyValuePair<K, V>> map)
        {
            return map.ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        /// <summary>
        ///     Removes the tilde (`) that sometimes appears at the end of class names.
        ///     For example List`1 => List
        /// </summary>
        public static string NoTilde(this string str)
        {
            if (str == null) return null;
            if (str.Length < 2) return str;

            var possibleTilde = str[str.Length - 2];

            if (possibleTilde == '`') return str.Substring(0, str.Length - 2);

            if (str.Length < 3) return str;

            possibleTilde = str[str.Length - 3];

            if (possibleTilde == '`') return str.Substring(0, str.Length - 3);

            return str;
        }


        /// <summary>
        /// Converts a path with environment variables into an actual path,
        ///  for example "${UserProfile}/.nuget/amar" => "C:/users/natan/.nuget/amar"
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ParseEnvironmentVariables(this string path)
        {
            var directories = path.Split(Path.DirectorySeparatorChar);
            var parsed = directories.Select(dir =>
            {
                if (dir.StartsWith("$"))
                {
                    var variable = dir.Substring(2, dir.Length - 3);
                    var result = Environment.GetEnvironmentVariable(variable);
                    return result;
                }
                else return dir;
            });
            var joined = string.Join(Path.DirectorySeparatorChar, parsed);

            return joined;
        }


        /// <summary>
        /// Checks if a file at a location is not being used by another process and therefore it is accessible.
        /// </summary>
        public static bool FileIsAccessible(string filename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    return inputStream.Length > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void WaitForFileToBeAccesible(string filename)
        {
            //This will lock the execution until the file is ready
            while (!FileIsAccessible(filename));
        }

        public static string ToIndentedJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}