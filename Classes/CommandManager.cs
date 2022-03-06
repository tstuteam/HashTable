using System.Collections;
using System.Collections.Generic;
using HashTableClass;

namespace Commands;

public class CommandManager
{
    private readonly HashTable<string, Command> commands = new();

    public void AddCommand(string name, CommandHandler handler, string description)
    {
        name = name.ToLower();

        if (CommandExists(name))
            throw new ArgumentException(nameof(name), "Команда уже существует");

        commands[name] = new(handler, description);
    }

    public Command? GetCommand(string name)
    {
        try {
            return commands[name.ToLower()];
        }
        catch (IndexOutOfRangeException) {
            return null;
        }
    }

    public bool CommandExists(string name)
    {
        return commands.Exists(name.ToLower());
    }

    public IEnumerator<(string, Command?)> GetCommands()
    {
        // FIXME: Make it so the return value can be used in a foreach loop.

        foreach (var kv in commands)
        {
            yield return kv;
        }
    }
}
