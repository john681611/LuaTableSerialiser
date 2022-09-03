using System;
using System.Collections.Generic;
using LuaTableSerializer;

var lst = new List<string>(){"Hey", "You"};
var dict = new Dictionary<string, object>{
    {"Bob", 1},
    {"James", 1.1},
    {"Sid", "Sid"},
    {"list", lst},
    {"dict", new Dictionary<string, int>{
        {"1", 1},
        {"2", 2},
        {"3", 3},
        {"4", 4},
        }
    },
    {"dictNumKey", new Dictionary<int, int>{
        {1, 1},
        {2, 2},
        {3, 3},
        {4, 4},
        }
    },
    {"dictOBJ", new Dictionary<string, object>{
        {"1", 1},
        {"1.5", 1.5},
        {"str", "item"},
        {"lst", lst},
        }
    }
};
var serialisedData = LuaSerializer.Serialize(dict);
Console.WriteLine($"mission = {serialisedData}");
var deserialised  =  LuaSerializer.Deserialize(serialisedData);
Utils.PrintDict(deserialised);
