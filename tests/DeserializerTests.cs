using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace LuaTableSerialiser.Tests;

public class DeserializerTests
{

    [Fact]
    public void Deserializer_List_ReturnString()
    {
        var result = LuaSerialiser.Deserialize(@"{[1] = ""Item1"",[2] = ""Item2"",}");
        var jsonResult = JsonConvert.SerializeObject(result);
        jsonResult.Should().Be(@"{""1"":""Item1"",""2"":""Item2""}");
    }

    [Fact]
    public void Deserializer_NestedList_ReturnString()
    {
        var result = LuaSerialiser.Deserialize(@"{[1] = {[1] = ""Item1"",[2] = ""Item2"",},[2] = {[1] = ""Item1"",[2] = ""Item2"",},}");
        var jsonResult = JsonConvert.SerializeObject(result);
        jsonResult.Should().Be(@"{""1"":{""1"":""Item1"",""2"":""Item2""},""2"":{""1"":""Item1"",""2"":""Item2""}}");
    }

    [Fact]
    public void Deserializer_Dict_ReturnString()
    {
        var result = LuaSerialiser.Deserialize(@"{[""Bob""] = ""Item1"",[""Jeff""] = ""Item2"",}");
        var jsonResult = JsonConvert.SerializeObject(result);
        jsonResult.Should().Be(@"{""Bob"":""Item1"",""Jeff"":""Item2""}");
    }

    [Fact]
    public void Deserializer_NestedDict_ReturnString()
    {
        var result = LuaSerialiser.Deserialize(@"{[""Bob""] = {[""Bob""] = ""Item1"",[""Jeff""] = ""Item2"",},[""Jeff""] = {[""Bob""] = ""Item1"",[""Jeff""] = ""Item2"",},}");
        var jsonResult = JsonConvert.SerializeObject(result);
        jsonResult.Should().Be(@"{""Bob"":{""Bob"":""Item1"",""Jeff"":""Item2""},""Jeff"":{""Bob"":""Item1"",""Jeff"":""Item2""}}");
    }
}