using Commands;

namespace App;

static partial class Program
{
    /// <summary>
    ///     Отображает описание команд.
    /// </summary>
    /// <param name="commandName">Название команды.</param>
    private static void CommandHelp(string commandName)
    {
        if (commandName != string.Empty)
        {
            Command? command = cli.GetCommand(commandName);

            if (command == null)
                Console.WriteLine("Команда \"{0}\" не существует.", commandName);
            else
                Console.WriteLine("{0} - {1}", command.Prototype, command.Description);
        }
        else
        {
            foreach (var (name, command) in cli)
                Console.WriteLine("{0} - {1}", command.Prototype, command.Description);
        }

        Console.WriteLine();
    }

    /// <summary>
    ///     Записать значение в таблицу.
    /// </summary>
    /// <param name="expression">Выражение в виде key=value.</param>
    private static void CommandSet(string expression)
    {
        int equalsSignAt = expression.IndexOf('=');

        if (equalsSignAt == -1)
        {
            Console.WriteLine("Выражение должно иметь вид <key>=<value>.\n");
            return;
        }

        string key = expression.Substring(0, equalsSignAt);

        if (key == "")
        {
            Console.WriteLine("Ключ не может быть пустым.\n");
            return;
        }

        string value = expression.Substring(equalsSignAt + 1);

        data[key] = value;
    }

    /// <summary>
    ///     Получить значение из таблицы.
    /// </summary>
    /// <param name="key">Ключ значения.</param>
    private static void CommandGet(string key)
    {
        if (data.Exists(key))
            Console.WriteLine("{0} = {1}\n", key, data[key]);
        else
            Console.WriteLine("Запись {0} не существует.\n", key);
    }

    /// <summary>
    ///     Удалить значение из таблицы.
    /// </summary>
    /// <param name="key">Ключ значения.</param>
    private static void CommandDel(string key)
    {
        if (data.Exists(key))
            data.Remove(key);
    }

    /// <summary>
    ///     Вывод всей таблицы.
    /// </summary>
    /// <param name="_">Не используется.</param>
    private static void CommandPrint(string _)
    {
        foreach (var (key, value) in data)
            Console.WriteLine("{0} = {1}", key, value);

        if (data.Size != 0)
            Console.WriteLine();
    }

    /// <summary>
    ///     Выход из программы.
    /// </summary>
    /// <param name="_">Не использутеся.</param>
    private static void CommandExit(string _)
    {
        run = false;
    }
}