namespace Commands;

/// <summary>
///     Обработчик команды.
/// </summary>
/// <param name="arguments">Аргументы команды.</param>
/// <returns>Нужно ли вставить перенос строки после выполнения команды.</returns>
public delegate bool CommandHandler(string arguments);

/// <summary>
///     Класс, содержащий данные о команде.
/// </summary>
public class Command
{
    /// <summary>
    ///     Обработчик команды.
    /// </summary>
    private readonly CommandHandler handler;

    /// <summary>
    ///     Прототип команды.
    /// </summary>
    public readonly string Prototype;

    /// <summary>
    ///     Описание команды.
    /// </summary>
    public readonly string Description;

    /// <summary>
    ///     Инициализирует команду.
    /// </summary>
    /// <param name="handler">Обработчик команды.</param>
    /// <param name="prototype">Прототип команды.</param>
    /// <param name="description">Описание команды.</param>
    /// <exception cref="ArgumentNullException">Вызывается, когда обработчик равен `null`.</exception>
    public Command(CommandHandler handler, string prototype, string description)
    {
        if (handler == null)
            throw new ArgumentNullException(nameof(handler), "Обработчик равен `null`.");

        this.handler = handler;
        Prototype = prototype;
        Description = description;
    }

    /// <summary>
    ///     Вызов команды.
    /// </summary>
    /// <param name="arguments">Аргументы команды.</param>
    /// <returns>Нужно ли вставить перенос строки после выполнения команды.</returns>
    public bool Execute(string arguments) => handler(arguments);
}