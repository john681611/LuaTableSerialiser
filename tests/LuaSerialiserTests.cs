using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace LuaTableSerialiser.Tests;

public class LuaSerialiserTests
{

    private readonly static List<string> _testList = new(){"Hey", "You"};
    private readonly static Dictionary<string, object> _testDict = new()
    {
        {"Bob", 1},
        {"James", 1.1},
        {"Sid", "Sid"},
        {"list", _testList},
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
            {"lst", _testList},
            }
        }
    };

    [Fact]
public void Serialise_NestedObject_ReturnString()
    {
        var result = LuaSerialiser.Serialize(_testDict);
        var formatResult = result.Replace("\t","").Replace("\n", "");
        formatResult.Should().Be(@"{[""Bob""] = 1,[""James""] = 1.1,[""Sid""] = ""Sid"",[""list""] = {[1] = ""Hey"",[2] = ""You"",},[""dict""] = {[""1""] = 1,[""2""] = 2,[""3""] = 3,[""4""] = 4,},[""dictNumKey""] = {[1] = 1,[2] = 2,[3] = 3,[4] = 4,},[""dictOBJ""] = {[""1""] = 1,[""1.5""] = 1.5,[""str""] = ""item"",[""lst""] = {[1] = ""Hey"",[2] = ""You"",},},}");
    }

    [Fact]
    public void Deserialize_NestedObject_ReturnsDictionary()
    {
        var result =  LuaSerialiser.Deserialize(@"{
        [""Bob""] = 1,
        [""James""] = 1.1,
        [""Sid""] = ""Sid"",
        [""list""] = {
                [1] = ""Hey"",
                [2] = ""You"",
                },
        [""dict""] = {
                [""1""] = 1,
                [""2""] = 2,
                [""3""] = 3,
                [""4""] = 4,
                },
        [""dictNumKey""] = {
                [1] = 1,
                [2] = 2,
                [3] = 3,
                [4] = 4,
                },
        [""dictOBJ""] = {
                [""1""] = 1,
                [""1.5""] = 1.5,
                [""str""] = ""item"",
                [""lst""] = {
                        [1] = ""Hey"",
                        [2] = ""You"",
                        },
                },
        }");
        var jsonResult = JsonConvert.SerializeObject(result);
        jsonResult.Should().Be(@"{""Bob"":1,""James"":1.1,""Sid"":""Sid"",""list"":{""1"":""Hey"",""2"":""You""},""dict"":{""1"":1,""2"":2,""3"":3,""4"":4},""dictNumKey"":{""1"":1,""2"":2,""3"":3,""4"":4},""dictOBJ"":{""1"":1,""1.5"":1.5,""str"":""item"",""lst"":{""1"":""Hey"",""2"":""You""}}}");
    }
}