//CODE FROM "Game Dev Guide"
//https://www.youtube.com/watch?v=VzOEM-4A2OM

using System;

//Base of console commands
public class ConsoleCommandBase
{
    string _commandId;
    string _commandDescription;

    //Name of command
    public string commandId { get { return _commandId; } }

    //Description of command
    public string commandDescription { get { return _commandDescription; } }

    public ConsoleCommandBase(string id, string desc)
    {
        _commandId = id;
        _commandDescription = desc;
    }
}

//Class for commands
public class ConsoleCommand : ConsoleCommandBase
{
    Action command;

    public ConsoleCommand(string id, string desc, Action command) : base(id, desc)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}

//Same as above but with parameter passing
public class ConsoleCommand<T1> : ConsoleCommandBase
{
    Action<T1> command;

    public ConsoleCommand(string id, string desc, Action<T1> command) : base (id, desc)
    {
        this.command = command;
    }

    public void Invoke(T1 value)
    {
        command.Invoke(value);
    }
}

