using System;
using System.Collections.Generic;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace LuaTableSerializer.Tests
{
    public class DeserializerTests
    {

        [Fact]
        public void Deserializer_List_ReturnString()
        {
            var result = LuaSerializer.Deserialize(@"{[1] = ""Item1"",[2] = ""Item2"",}");
            var jsonResult = JsonConvert.SerializeObject(result);
            jsonResult.Should().Be(@"{""1"":""Item1"",""2"":""Item2""}");
        }

        [Fact]
        public void Deserializer_NestedList_ReturnString()
        {
            var result = LuaSerializer.Deserialize(@"{[1] = {[1] = ""Item1"",[2] = ""Item2"",},[2] = {[1] = ""Item1"",[2] = ""Item2"",},}");
            var jsonResult = JsonConvert.SerializeObject(result);
            jsonResult.Should().Be(@"{""1"":{""1"":""Item1"",""2"":""Item2""},""2"":{""1"":""Item1"",""2"":""Item2""}}");
        }

        [Fact]
        public void Deserializer_Dict_ReturnString()
        {
            var result = LuaSerializer.Deserialize(@"{[""Bob""] = ""Item1"",[""Jeff""] = ""Item2"",}");
            var jsonResult = JsonConvert.SerializeObject(result);
            jsonResult.Should().Be(@"{""Bob"":""Item1"",""Jeff"":""Item2""}");
        }

        [Fact]
        public void Deserializer_Dict_EscapedQuotation_ReturnString()
        {
            var result = LuaSerializer.Deserialize(@"{[""Bob""] = ""Item1"",[""Jeff""] = ""Item2 (\""Jezza])"",}");
            var jsonResult = JsonConvert.SerializeObject(result);
            jsonResult.Should().Be(@"{""Bob"":""Item1"",""Jeff"":""Item2 (\""Jezza])""}");
        }

        [Fact]
        public void Deserializer_NestedDict_ReturnString()
        {
            var result = LuaSerializer.Deserialize(@"{[""Bob""] = {[""Bob""] = ""Item1"",[""Jeff""] = ""Item2"",},[""Jeff""] = {[""Bob""] = ""Item1"",[""Jeff""] = ""Item2"",},}");
            var jsonResult = JsonConvert.SerializeObject(result);
            jsonResult.Should().Be(@"{""Bob"":{""Bob"":""Item1"",""Jeff"":""Item2""},""Jeff"":{""Bob"":""Item1"",""Jeff"":""Item2""}}");
        }

        [Fact]
        public void Deserializer_RealWorldExample_ReturnString()
        {
            var result = LuaSerializer.Deserialize(@"{
        [""name""] = ""AH-64D_BLK_II"",
        [""payloads""] = {
                [1] = {
                        [""displayName""] = ""2 * M261: A/B - M151 (6PD), E - M257 (6IL), 2 * Hellfire station: 4*AGM-114K"",
                        [""name""] = ""2 * M261: A/B - M151 (6PD), E - M257 (6IL), 2 * Hellfire station: 4*AGM-114K"",
                        [""pylons""] = {
                                [1] = {
                                        [""CLSID""] = ""{M261_OUTBOARD_AB_M151_E_M257}"",
                                        [""num""] = 4,
                                },
                                [2] = {
                                        [""CLSID""] = ""{88D18A5E-99C8-4B04-B40B-1C02F2018B6E}"",
                                        [""num""] = 3,
                                },
                                [3] = {
                                        [""CLSID""] = ""{88D18A5E-99C8-4B04-B40B-1C02F2018B6E}"",
                                        [""num""] = 2,
                                },
                                [4] = {
                                        [""CLSID""] = ""{M261_OUTBOARD_AB_M151_E_M257}"",
                                        [""num""] = 1,
                                },
                                [5] = {
                                        [""CLSID""] = ""{AN_APG_78}"",
                                        [""num""] = 6,
                                },
                        },
                        [""tasks""] = {
                                [1] = 16,
                                [2] = 30,
                                [3] = 31,
                                [4] = 18,
                                [5] = 32,
                        },
                },
        },
        [""unitType""] = ""AH-64D_BLK_II"",

}");
            var jsonResult = JsonConvert.SerializeObject(result);
            jsonResult.Should().Be(@"{""name"":""AH-64D_BLK_II"",""payloads"":{""1"":{""displayName"":""2 * M261: A/B - M151 (6PD), E - M257 (6IL), 2 * Hellfire station: 4*AGM-114K"",""name"":""2 * M261: A/B - M151 (6PD), E - M257 (6IL), 2 * Hellfire station: 4*AGM-114K"",""pylons"":{""1"":{""CLSID"":""{M261_OUTBOARD_AB_M151_E_M257}"",""num"":4},""2"":{""CLSID"":""{88D18A5E-99C8-4B04-B40B-1C02F2018B6E}"",""num"":3},""3"":{""CLSID"":""{88D18A5E-99C8-4B04-B40B-1C02F2018B6E}"",""num"":2},""4"":{""CLSID"":""{M261_OUTBOARD_AB_M151_E_M257}"",""num"":1},""5"":{""CLSID"":""{AN_APG_78}"",""num"":6}},""tasks"":{""1"":16,""2"":30,""3"":31,""4"":18,""5"":32}}},""unitType"":""AH-64D_BLK_II""}");
        }

    }
}