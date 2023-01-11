using Stregsystem.Interfaces;

namespace Stregsystem.Models;

public class Logger<T> : ILogger<T>
{
    public string Filename { get; }
    public List<T> Backlog { get; } = new List<T>();
    public Logger(string filename)
    {
        Filename = filename;
    }

    public void Log(params string[] messages)
    {
        File.AppendAllLines(Filename, messages);
    }
}