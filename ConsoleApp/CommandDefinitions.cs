using Commands;
using HashTableClass;
using System.Text.Json;

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

        if (key == string.Empty)
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
        if (data.TryGetValue(key, out string value))
            Console.WriteLine("{0} = {1}\n", key, value);
        else
            Console.WriteLine("Запись {0} не существует.\n", key);
    }

    /// <summary>
    ///     Удалить значение из таблицы.
    /// </summary>
    /// <param name="key">Ключ значения.</param>
    private static void CommandDel(string key)
    {
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

        if (data.Count != 0)
            Console.WriteLine();
    }

    /// <summary>
    ///     Вывод всей таблицы в виде JSON.
    /// </summary>
    /// <param name="_">Не используется.</param>
    private static void CommandPrintJson(string _)
    {
        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions() {
            WriteIndented = true
        });

        Console.WriteLine("{0}\n", json);
    }

    /// <summary>
    ///     Загрузка таблицы из файла.
    /// </summary>
    /// <param name="path">Путь к файлу.</param>
    private static void CommandLoadJson(string path)
    {
        path = path.Trim();

        if (path == "")
        {
            Console.WriteLine("Требуется путь к файлу: load-json <path>\n");
            return;
        }

        string json;

        try
        {
            json = File.ReadAllText(path);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex switch
            {
                FileNotFoundException or DirectoryNotFoundException => "Файл не найден.\n",
                PathTooLongException => "Путь слишком длинный.\n",
                _ => "Не удалось прочитать файл.\n"
            });
            return;
        }

        HashTable<string, string>? table = JsonSerializer.Deserialize<HashTable<string, string>>(json);

        if (table == null)
        {
            Console.WriteLine("Не удалось прочитать таблицу.\n");
            return;
        }

        foreach (var (key, value) in table)
            data[key] = value;
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