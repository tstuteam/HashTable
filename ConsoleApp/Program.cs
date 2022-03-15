using Commands;
using HashTableClass;

namespace App;

static partial class Program
{
    /// <summary>
    ///     Условие главного цикла.
    /// </summary>
    private static bool run = true;

    /// <summary>
    ///     Интерфейс командной строки.
    /// </summary>
    private static readonly CommandLineInterface cli = new('>');

    /// <summary>
    ///     Таблица с данными.
    /// </summary>
    private static readonly HashTable<string, string> data = new();

    /// <summary>
    ///     Точка входа в программу.
    /// </summary>
    private static void Main()
    {
        RegisterCommands();

        cli.OnCommandNotFound = name => Console.WriteLine(
            "Команда \"{0}\" не существует. Введите \"help\" для отображения помощи.\n",
            name
        );

        while (run)
            cli.ProcessInput();
    }

    /// <summary>
    ///     Регистрирует команды.
    /// </summary>
    private static void RegisterCommands()
    {
        cli.AddCommand("help [name]", CommandHelp, "Отображает описание команд.");
        cli.AddCommand("set <key>=<value>", CommandSet, "Записать значение в таблицу.");
        cli.AddCommand("get <key>", CommandGet, "Получить значение из таблицы.");
        cli.AddCommand("del <key>", CommandDel, "Удалить значение из таблицы.");
        cli.AddCommand("print", CommandPrint, "Вывод всей таблицы.");
        cli.AddCommand("print-json", CommandPrintJson, "Вывод всей таблицы в виде JSON.");
        cli.AddCommand("load-json <path>", CommandLoadJson, "Загрузка таблицы из файла.");
        cli.AddCommand("exit", CommandExit, "Выход из программы.");
    }
}

