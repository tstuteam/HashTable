using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // TODO: Make proper unit tests.

        HashTable<string, int> table = new();

        Console.WriteLine(table.Exists("test")); // false
        table["test"] = 123;
        Console.WriteLine(table.Exists("test")); // true

        Console.WriteLine(table["test"]);

        table.Remove("test");
        Console.WriteLine(table.Exists("test")); // false

        table["zero"] = 0;
        table["one"] = 1;
        table["two"] = 2;
        table["three"] = 3;
        table["four"] = 4;
        table["five"] = 5;
        table["six"] = 6;
        table["seven"] = 7;
        table["eight"] = 8;
        table["nine"] = 9;

        Console.WriteLine("Size = {0}", table.Size);

        foreach (var kv in table)
            Console.WriteLine("{0}: {1}", kv.Key, kv.Value);
    }
}
