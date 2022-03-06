using System;
using System.Collections.Generic;
using Commands;

namespace App;

static class Program
{
    private static readonly CommandManager commands = new();

    private static void Main()
    {
        RegisterCommands();
    }

    private static void RegisterCommands()
    {
        commands.AddCommand("help", CommandHelp, "Отображает все команды.");
    }

    private static void CommandHelp(string arguments)
    {
        /*
        foreach (var kv in commands.GetCommands())
        {

        }
        */
    }
}

