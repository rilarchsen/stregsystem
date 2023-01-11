using Stregsystem;
using Stregsystem.Interfaces;
using Stregsystem.Models;


ILogger logger = new Logger(Path.Combine(Environment.CurrentDirectory, "log.txt"));
User u1 = new User(1, "f", "l", "fl", "f@l.com", 100);
Product p1 = new(1, "product 1", 24, true, false);
BuyTransaction bt1 = new(1, u1, DateTime.Now, p1, logger);
//bt1.Execute();


Console.ReadKey();