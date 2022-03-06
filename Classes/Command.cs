namespace Commands;

public delegate bool CommandHandler(string command);

public class Command
{
    private readonly CommandHandler handler;
    public readonly string Prototype;
    public readonly string Description;

    public Command(CommandHandler handler, string prototype, string description)
    {
        if (handler == null)
            throw new ArgumentNullException(nameof(handler), "Обработчик равен `null`.");

        this.handler = handler;
        Prototype = prototype;
        Description = description;
    }

    public bool Execute(string arguments) => handler(arguments);
}