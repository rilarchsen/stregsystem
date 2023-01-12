using Stregsystem.Interfaces;
using Stregsystem.Models;

namespace Stregsystem;

public class StregsystemController
{
    private readonly IStregsystem _stregsystem;
    private readonly IStregsystemUI _stregsystemUI;
    private readonly StregsystemCommandParser _commandParser;
    private readonly ILogger _logger;

    public StregsystemController(IStregsystem stregsystem, IStregsystemUI stregsystemUI, ILogger logger)
    {
        _stregsystem = stregsystem;
        _stregsystemUI = stregsystemUI;
        _logger = logger;
        _commandParser = new(_stregsystem, _stregsystemUI, _logger);
        _stregsystemUI.CommandEntered += _commandParser.ParseCommand;
        _stregsystem.UserBalanceWarning += _stregsystemUI.DisplayUserBalanceNotification;
    }
}