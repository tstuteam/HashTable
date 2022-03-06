using HashTableClass;

namespace Commands;

/// <summary>
///     Класс, содержащий команды.
/// </summary>
public class CommandManager
{
    /// <summary>
    ///     Таблица команд.
    /// </summary>
    private readonly HashTable<string, Command> commands = new();

    /// <summary>
    ///     Добавляет команду.
    /// </summary>
    /// <param name="prototype">Прототип команды.</param>
    /// <param name="handler">Обработчик команды.</param>
    /// <param name="description">Описание команды.</param>
    /// <exception cref="ArgumentException">Вызывается, когда команда уже существует.</exception>
    /// <exception cref="ArgumentNullException">Вызывается, когда обрабочтик равен `null`.</exception>
    public void AddCommand(string prototype, CommandHandler handler, string description)
    {
        prototype = prototype.Trim();
        string name = GetNameFromPrototype(prototype);

        if (CommandExists(name))
            throw new ArgumentException(nameof(name), "Команда уже существует.");

        if (handler == null)
            throw new ArgumentNullException(nameof(handler), "Обработчик равен `null`.");

        commands[name] = new(handler, prototype, description);
    }

    /// <summary>
    ///     Получает имя команды из её прототипа.
    /// </summary>
    /// <param name="prototype">Прототип команды.</param>
    /// <returns>Имя команды.</returns>
    private static string GetNameFromPrototype(string prototype)
    {
        int nameEnd = prototype.IndexOf(' ');

        if (nameEnd == -1)
            return prototype;
        else
            return prototype.Substring(0, nameEnd);
    }

    /// <summary>
    ///     Получает команду по имени.
    /// </summary>
    /// <param name="name">Имя команды.</param>
    /// <returns>Команда.</returns>
    public Command? GetCommand(string name)
    {
        try {
            return commands[name.ToLower()];
        }
        catch (ArgumentOutOfRangeException) {
            return null;
        }
    }

    /// <summary>
    ///     Проверяет команду на существование.
    /// </summary>
    /// <param name="name">Имя команды.</param>
    /// <returns>`true` если команда существует.</returns>
    public bool CommandExists(string name)
    {
        return commands.Exists(name.ToLower());
    }

    /// <summary>
    ///     Возвращает перечислитель существующих команд.
    /// </summary>
    /// <returns>Перечислитель.</returns>
    public IEnumerator<(string Name, Command Command)> GetEnumerator()
    {
        return commands.GetEnumerator()!;
    }
}
