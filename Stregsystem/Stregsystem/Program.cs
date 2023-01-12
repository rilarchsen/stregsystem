using Stregsystem;
using Stregsystem.Interfaces;
using Stregsystem.Models;


ILogger<Transaction> logger = new Logger<Transaction>(Path.Combine(Environment.CurrentDirectory, "log.txt"));
IStregsystem stregsystem = new Stregsystem.Stregsystem(logger);
IStregsystemUI ui = new StregsystemCLI(stregsystem);
StregsystemController sc = new StregsystemController(stregsystem, ui, logger);

ui.Start();