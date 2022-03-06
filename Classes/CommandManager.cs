using System.Collections;
using System.Collections.Generic;
using HashTableClass;

namespace Commands;

public class CommandManager
{
    private readonly HashTable<string, Command> commands = new();

    public void AddCommand(string prototype, CommandHandler handler, string description)
    {
        prototype = prototype.Trim();

        string name;
        int nameEnd = prototype.IndexOf(' ');

        if (nameEnd == -1)
            name = prototype;
        else
            name = prototype.Substring(0, nameEnd);

        if (CommandExists(name))
            throw new ArgumentException(nameof(name), "Команда уже существует.");

        if (handler == null)
            throw new ArgumentNullException(nameof(handler), "Обработчик равен `null`.");

        commands[name] = new(handler, prototype, description);
    }

    public Command? GetCommand(string name)
    {
        try {
            return commands[name.ToLower()];
        }
        catch (ArgumentOutOfRangeException) {
            return null;
        }
    }

    public bool CommandExists(string name)
    {
        return commands.Exists(name.ToLower());
    }

    public IEnumerator<(string Name, Command Command)> GetEnumerator()
    {
        return commands.GetEnumerator()!;
    }
}
