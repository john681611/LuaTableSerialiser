using System.Collections.Generic;

namespace LuaTableSerializer
{
    public class LuaSerializer
    {
        public static string Serialize(object data) => $"{Serializer.ConvertType(data)}";

        public static Dictionary<object, object> Deserialize(string data) => Deserializer.ToDict(data);
    }
}