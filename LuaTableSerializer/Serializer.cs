using System;
using System.Collections;
using System.Linq;

namespace LuaTableSerializer
{
    internal class Serializer
    {
        internal static object ConvertType(object item, int nesting = 1, int index = 0)
        {
            return item switch
            {
                bool value => value.ToString().ToLower(),
                int value => value,
                float value => value,
                double value => value,
                string value => EscapeString(value),
                IList value => ListToLua(value, nesting),
                IDictionary value => DictToLua(value, nesting),
                _ => TryToLuaString(item, index)
            };
        }

        private static object TryToLuaString(object item, int index)
        {

                var method = item.GetType().GetMethod("ToLuaString");
                if (method is null)
                   throw new MissingMethodException($"{item} has no accessible ToLuaString method"); 
                if (method.GetParameters().Count() > 0)
                    return method.Invoke(item, new object[] { index });
                return method.Invoke(item, null);

        }

        private static string ConvertKey(object key) => key switch
        {
            string => $"[\"{key}\"]",
            int => $"[{key}]",
            _ => throw new ArgumentOutOfRangeException(nameof(key), $"Not expected key Type value: {key}")
        };

        private static string DictToLua<T>(T data, int nesting) where T : IDictionary
        {
            var str = "{";
            foreach (DictionaryEntry item in data)
            {
                if (item.Value is null)
                    continue;
                str += $"\n{Utils.GetNesting(nesting)}{ConvertKey(item.Key)} = {ConvertType(item.Value, nesting + 1)},";
            }
            return $"{str}\n{Utils.GetNesting(nesting)}}}";
        }

        private static string ListToLua<T>(T data, int nesting) where T : IList
        {
            var str = "{";
            var index = 1;
            foreach (var item in data)
            {
                str += $"\n{Utils.GetNesting(nesting)}{ConvertKey(index)} = {ConvertType(item, nesting + 1, index)},";
                index++;
            }
            return $"{str}\n{Utils.GetNesting(nesting)}}}";
        }

        private static string EscapeString(string data)
        {
            var value = data
                .Replace("\\", "\\\\")
                .Replace("\t", "\\t")
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\"", "\\\"")
                .Replace("\'", "\\\'")
                .Replace("[", "\\[")
                .Replace("]", "\\]");

            return $"\"{value}\"";
        }
    }
}