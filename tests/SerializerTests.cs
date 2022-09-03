using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace LuaTableSerializer.Tests
{
    public class SerializerTests
    {

        [Theory]
        [InlineData("Hello", @"""Hello""")]
        [InlineData((int)1, "1")]
        [InlineData((float)1.4, "1.4")]
        [InlineData((double)1.2, "1.2")]
        public void Serialise_VariertyOfTypes_ReturnString(object data, string expected)
        {
            var result = LuaSerializer.Serialize(data);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("\\", @"""\\""")]
        [InlineData("\t", @"""\t""")]
        [InlineData("\n", @"""\n""")]
        [InlineData("\r", @"""\r""")]
        [InlineData("\"", @"""\""""")]
        [InlineData("\'", @"""\'""")]
        [InlineData("[", @"""\[""")]
        [InlineData("]", @"""\]""")]
        public void Serialise_EscapeStrings_ReturnString(object data, string expected)
        {
            var result = LuaSerializer.Serialize(data);
            result.Should().Be(expected);
        }


        [Fact]
        public void Serialise_List_ReturnString()
        {
            var result = LuaSerializer.Serialize(new List<string> { "Item1", "Item2" });
            var formatResult = result.Replace("\t", "").Replace("\n", "");
            formatResult.Should().Be(@"{[1] = ""Item1"",[2] = ""Item2"",}");
        }

        [Fact]
        public void Serialise_NestedList_ReturnString()
        {
            var list = new List<string> { "Item1", "Item2" };
            var result = LuaSerializer.Serialize(new List<List<string>> { list, list });
            var formatResult = result.Replace("\t", "").Replace("\n", "");
            formatResult.Should().Be(@"{[1] = {[1] = ""Item1"",[2] = ""Item2"",},[2] = {[1] = ""Item1"",[2] = ""Item2"",},}");
        }

        [Fact]
        public void Serialise_Dictionary_ReturnString()
        {
            var result = LuaSerializer.Serialize(new Dictionary<string, string>{
            {"bob", "1"},
            {"jim", "2"},
        });
            var formatResult = result.Replace("\t", "").Replace("\n", "");
            formatResult.Should().Be(@"{[""bob""] = ""1"",[""jim""] = ""2"",}");
        }

        [Fact]
        public void Serialise_MixedDictionary_ReturnString()
        {
            var result = LuaSerializer.Serialize(new Dictionary<string, object>{
            {"bob", "1"},
            {"jim", 2},
        });
            var formatResult = result.Replace("\t", "").Replace("\n", "");
            formatResult.Should().Be(@"{[""bob""] = ""1"",[""jim""] = 2,}");
        }

        [Fact]
        public void Serialise_NestedDictionary_ReturnString()
        {
            var dict = new Dictionary<string, string>{
            {"bob", "1"},
            {"jim", "2"},
        };
            var result = LuaSerializer.Serialize(new Dictionary<string, Dictionary<string, string>>{
            {"Dave", dict},
            {"Ian", dict},
        });
            var formatResult = result.Replace("\t", "").Replace("\n", "");
            formatResult.Should().Be(@"{[""Dave""] = {[""bob""] = ""1"",[""jim""] = ""2"",},[""Ian""] = {[""bob""] = ""1"",[""jim""] = ""2"",},}");
        }

        [Fact]
        public void Serialise_Class_with_ToLuaString()
        {
            var result = LuaSerializer.Serialize(new testClass("dark side"));
            var formatResult = result.Replace("\t", "").Replace("\n", "");
            formatResult.Should().Be(@"{[""value""] = ""dark side"",}");
        }

        [Fact]
        public void Serialise_Class_with_ToLuaString_with_index()
        {
            var result = LuaSerializer.Serialize(new testUseIndexClass(666));
            var formatResult = result.Replace("\t", "").Replace("\n", "");
            formatResult.Should().Be(@"{[1] = {[""value""] = 666,[""index""] = 0,},}");
        }

        [Fact]
        public void Serialise_Class_with_no_ToLuaString()
        {
            Action act = () => LuaSerializer.Serialize(new testNoConvertClass("dark side"));
            act.Should().Throw<MissingMethodException>()
                .WithMessage("LuaTableSerializer.Tests.SerializerTests+testNoConvertClass has no accessible ToLuaString method");
        }

        private class testClass
        {
            private string value { get; set; }

            internal testClass(string _value)
            {
                value = _value;
            }

            public string ToLuaString() => LuaSerializer.Serialize(new Dictionary<string, string> { { "value", value } });
        }

        private class testUseIndexClass
        {
            private int value { get; set; }

            internal testUseIndexClass(int _value)
            {
                value = _value;
            }

            public string ToLuaString(int index) => LuaSerializer.Serialize(new List<Dictionary<string, int>> { { new Dictionary<string, int> { { "value", value }, { "index", index } } } });
        }

        private class testNoConvertClass
        {
            private string value { get; set; }

            internal testNoConvertClass(string _value)
            {
                value = _value;
            }

        }

    }
}