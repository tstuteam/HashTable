using System;
using System.Collections.Generic;
using Commands;
using HashTableClass;

namespace App;

static class Program
{
    private static bool run = true;
    private static readonly CommandManager commands = new();

    private static readonly HashTable<string, string> data = new();

    private static void Main()
    {
        RegisterCommands();

        while (run)
        {
            Console.Write(">");
            string? input = Console.ReadLine();

            if (input == null)
                continue;

            (string name, string arguments) = ParseInput(input);

            Command? command = commands.GetCommand(name);

            if (command == null)
            {
                Console.WriteLine("Команда \"{0}\" не существует. Введите \"help\" для отображения помощи.\n", name);
                continue;
            }
            else
            {
                bool shouldInsertNewline = command.Execute(arguments);

                if (shouldInsertNewline)
                    Console.WriteLine();
            }
        }
    }

    private static void RegisterCommands()
    {
        commands.AddCommand("help [name]", CommandHelp, "Отображает описание команд.");
        commands.AddCommand("exit", CommandExit, "Выход из программы.");
        commands.AddCommand("set <key>=<value>", CommandSet, "Записать значение в таблицу.");
        commands.AddCommand("get <key>", CommandGet, "Получить значение из таблицы");
        commands.AddCommand("del <key>", CommandDel, "Удалить значение из таблицы");
        commands.AddCommand("print", CommandPrint, "Вывод всей таблицы.");
    }

    private static (string name, string arguments) ParseInput(string input)
    {
        input = input.Trim();

        string name, arguments;

        int nameEnd = input.IndexOf(' ');

        if (nameEnd == -1)
        {
            name = input;
            arguments = string.Empty;
        }
        else
        {
            name = input.Substring(0, nameEnd);
            arguments = input.Substring(nameEnd + 1).TrimStart();
        }

        return (name, arguments);
    }

    private static bool CommandHelp(string commandName)
    {
        if (commandName != string.Empty)
        {
            Command? command = commands.GetCommand(commandName);

            if (command == null)
                Console.WriteLine("Команда \"{0}\" не существует.", commandName);
            else
                Console.WriteLine("{0} - {1}", command.Prototype, command.Description);
        }
        else
        {
            foreach (var (name, command) in commands)
                Console.WriteLine("{0} - {1}", command.Prototype, command.Description);
        }

        return true;
    }

    private static bool CommandSet(string expression)
    {
        int equalsSignAt = expression.IndexOf('=');

        if (equalsSignAt == -1)
        {
            Console.WriteLine("Выражение должно иметь вид <key>=<value>.");
            return true;
        }

        string key = expression.Substring(0, equalsSignAt);

        if (key == "")
        {
            Console.WriteLine("Ключ не может быть пустым.");
            return true;
        }

        string value = expression.Substring(equalsSignAt + 1);

        data[key] = value;

        return false;
    }

    private static bool CommandGet(string key)
    {
        if (data.Exists(key))
            Console.WriteLine("{0} = {1}", key, data[key]);
        else
            Console.WriteLine("Запись {0} не существует.", key);

        return true;
    }

    private static bool CommandDel(string key)
    {
        if (data.Exists(key))
            data.Remove(key);

        return false;
    }

    private static bool CommandPrint(string _)
    {
        foreach (var (key, value) in data)
            Console.WriteLine("{0} = {1}", key, value);

        return data.Size != 0;
    }

    private static bool CommandExit(string _)
    {
        run = false;
        return false;
    }
}

