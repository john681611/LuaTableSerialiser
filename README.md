# C# Data to Lua Table
Silly little bit of code to convert C# data to a string representing a Lua Table

## Example

Code:
```c#
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
    {"dictOBJ", new Dictionary<string, object>{
        {"1", 1},
        {"1.5", 1.5},
        {"str", "item"},
        {"lst", lst},
        }
    }
};
Console.WriteLine($"mission = {CSharpToLua.ToLuaTableString(dict)}");
```

Output:

```lua
mission = {
        ["Bob"] = 1,
        ["James"] = 1.1,
        ["Sid"] = "Sid",
        ["list"] = {
                [1] = "Hey",
                [2] = "You",
                },
        ["dict"] = {
                ["1"] = 1,
                ["2"] = 2,
                ["3"] = 3,
                ["4"] = 4,
                },
        ["dictOBJ"] = {
                ["1"] = 1,
                ["1.5"] = 1.5,
                ["str"] = "item",
                ["lst"] = {
                        [1] = "Hey",
                        [2] = "You",
                        },
                },
        }
```