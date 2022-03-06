namespace Commands;

public delegate void CommandHandler(string command);

public class Command
{
    private readonly CommandHandler handler;
    public readonly string Description;

    public Command(CommandHandler handler, string description)
    {
        if (handler == null)
            throw new ArgumentNullException(nameof(handler), "Обработчик равен `null`.");

        this.handler = handler;
        Description = description;
    }

    public void Execute(string arguments) => handler(arguments);
}