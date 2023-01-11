namespace Stregsystem.Interfaces;

public interface ILogger
{
    public void Log(params string[] messages);
}

public interface ILogger<T> : ILogger
{
    public List<T> Backlog { get; }
}