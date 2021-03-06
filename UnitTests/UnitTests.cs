using Xunit;
using System.Text.Json;
using HashTableClass;

namespace UnitTests;

public class UnitTests
{
    private static HashTable<string, int> sampleTable {
        get => new()
        {
            ["zero"]  = 0,
            ["one"]   = 1,
            ["two"]   = 2,
            ["three"] = 3,
            ["four"]  = 4,
            ["five"]  = 5,
            ["six"]   = 6,
            ["seven"] = 7,
            ["eight"] = 8,
            ["nine"]  = 9
        };
    }

    [Theory]
    [InlineData("zero", 0)]
    [InlineData("one", 1)]
    [InlineData("two", 2)]
    [InlineData("three", 3)]
    [InlineData("four", 4)]
    [InlineData("five", 5)]
    [InlineData("six", 6)]
    [InlineData("seven", 7)]
    [InlineData("eight", 8)]
    [InlineData("nine", 9)]
    public void GetterTest(string key, int expectedValue)
    {
        HashTable<string, int> table = sampleTable;
        int value = table[key];

        Assert.Equal(expectedValue, value);
    }

    [Fact]
    public void SetterTest()
    {
        HashTable<string, int> table = new();

        Assert.False(table.ContainsKey("abc"));
        table["abc"] = 123;
        Assert.True(table.ContainsKey("abc"));

        table["abc"] = 321;

        Assert.Equal(321, table["abc"]);
    }

    [Fact]
    public void LargeTableTest()
    {
        HashTable<string, int> table = new();

        for (int i = 0; i < 1_000_000; ++i) {
            string key = $"pre_{i}_post";
            table[key] = key.GetHashCode();
        }

        Assert.Equal(1_000_000, table.Count);

        for (int i = 0; i < 1_000_000; ++i)
        {
            string key = $"pre_{i}_post";
            table.Remove(key);
        }

        Assert.Empty(table);
    }

    [Fact]
    public void JsonSerializationTest()
    {
        HashTable<string, int> numbers1 = sampleTable;

        string json = JsonSerializer.Serialize(numbers1);
        HashTable<string, int>? numbers2 = JsonSerializer.Deserialize<HashTable<string, int>>(json);

        Assert.NotNull(numbers2);
        Assert.Equal(numbers1.Count, numbers2!.Count);

        foreach (var (key, value) in numbers1)
            Assert.Equal(value, numbers2[key]);
    }
}