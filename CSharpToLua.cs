using System.Collections;

namespace CSharpeToLua
{
    public class CSharpToLua
    {
        public static string ToLuaTableString(object data) => (string)ConvertType(data);

        private static string DictToLua<T>(T data, int nesting) where T : IDictionary
        {
            var str = "{";
            foreach (DictionaryEntry item in data)
            {
                if (item.Value is null)
                    continue;
                str += $"\n{GetNesting(nesting)}{ConvertKey(item.Key)} = {ConvertType(item.Value, nesting + 1)},";
            }
            return $"{str}\n{GetNesting(nesting)}}}";
        }

        private static string ListToLua<T>(T data, int nesting) where T : IList
        {
            var str = "{";
            var index = 1;
            foreach (var item in data)
            {
                str += $"\n{GetNesting(nesting)}{ConvertKey(index)} = {ConvertType(item, nesting + 1)},";
                index++;
            }
            return $"{str}\n{GetNesting(nesting)}}}";
        }

        private static object ConvertType(object item, int nesting = 1)
        {
            return item switch
            {
                string value => $"\"{value}\"",
                int value => value,
                float value => value,
                double value => value,
                IList value => ListToLua(value, nesting),
                IDictionary value => DictToLua(value, nesting),
                _ => throw new ArgumentOutOfRangeException(nameof(item), $"Not expected value Type value: {item}")
            };
        }


        private static string ConvertKey(object key) => key switch
        {
            string => $"[\"{key}\"]",
            int => $"[{key}]",
            _ => throw new ArgumentOutOfRangeException(nameof(key), $"Not expected key Type value: {key}")
        };

        private static string GetNesting(int nesting) => new string('\t', nesting);

    }
}