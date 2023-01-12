using Stregsystem.Exceptions;
using Stregsystem.Interfaces;
using Stregsystem.Models;
using System.Linq.Expressions;
using System.Timers;

namespace Stregsystem;

public class StregsystemCommandParser
{
    private readonly IStregsystem _stregsystem;
    private readonly IStregsystemUI _stregsystemUI;
    private Dictionary<string, LambdaExpression> _adminCommands;
    private readonly ILogger _logger;
    public StregsystemCommandParser(IStregsystem stregsystem, IStregsystemUI stregsystemUI, ILogger logger)
	{
        _stregsystem = stregsystem;
        _stregsystemUI = stregsystemUI;
        _logger = logger;
        _adminCommands = new Dictionary<string, LambdaExpression>();
        RegisterAdminCommands(_adminCommands);
	}

    public void ParseCommand(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
            _stregsystemUI.Start();

        string[] splitCommand = command.Trim().Split(' ').Where(item => !string.IsNullOrWhiteSpace(item)).ToArray();

        if (splitCommand.Length == 0)
            _stregsystemUI.DisplayGeneralError($"Unrecognized command \"{command}\"");

        if (splitCommand.Length > 3)
            _stregsystemUI.DisplayTooManyArgumentsError(command);

        try
        {
            if (command.StartsWith(":")) {
                //is admin command
                LambdaExpression? exp;
                bool found = _adminCommands.TryGetValue(splitCommand[0], out exp);
                if (found && exp is not null)
                {
                    int paramsCount = exp.Compile().Method.GetParameters().Count();

                    if (splitCommand.Length == paramsCount)
                    {
                        exp.Compile().DynamicInvoke(splitCommand.Skip(1).Take(splitCommand.Length - 1).ToArray());

                    } else if (splitCommand.Length > paramsCount)
                    {
                        _stregsystemUI.DisplayTooManyArgumentsError(command);

                    } else
                    {
                        _stregsystemUI.DisplayTooFewArgumentsError(command);
                    }
                } else
                {
                    _stregsystemUI.DisplayAdminCommandNotFoundMessage(command);
                }
                
            } else
            {
                //is not admin command

                switch (splitCommand.Length)
                {
                    case 0:
                        _stregsystemUI.DisplayTooFewArgumentsError(command);
                        break;
                    case 1:
                        //user info
                        _stregsystemUI.DisplayUserInfo(
                            _stregsystem.GetUserByUsername(command.Trim())
                        );
                        break;
                    case 2:
                        //single quickbuy
                        BuyTransaction transaction = _stregsystem.BuyProduct(
                            _stregsystem.GetUserByUsername(splitCommand[0].Trim()), 
                            _stregsystem.GetProductByID(int.Parse(splitCommand[1].Trim()))
                        );
                        _stregsystemUI.DisplayUserBuysProduct(transaction);
                        break;
                    case 3:
                        //multi quickbuy
                        BuyTransaction? btransaction = _stregsystem.BuyProduct(
                                _stregsystem.GetUserByUsername(splitCommand[0].Trim()),
                                _stregsystem.GetProductByID(int.Parse(splitCommand[2].Trim())),
                                int.Parse(splitCommand[1].Trim())
                            );
                            _stregsystemUI.DisplayUserBuysProduct(int.Parse(splitCommand[1].Trim()), btransaction);
                        break;
                    default:
                        //invalid command syntax
                        _stregsystemUI.DisplayTooManyArgumentsError(command);
                        break;
                }
            }
        }
        catch (UserNotFoundException e)
        {
            _stregsystemUI.DisplayUserNotFound(e.Username);
        }
        catch (InsufficientCreditsException e)
        {
            _stregsystemUI.DisplayInsufficientCash(e.Transaction);
        }
        catch (ProductNotFoundException e)
        {
            _stregsystemUI.DisplayProductNotFound(e.ProductId.ToString());
        }
        catch (ProductUnavailableForPurchaseException e)
        {
            _stregsystemUI.DisplayProductNotFound(e.Product.Id.ToString());
        }
        catch (FormatException e)
        {
            _stregsystemUI.DisplayGeneralError(e.Message);
        }
        catch (System.Reflection.TargetInvocationException e)
        {
            _stregsystemUI.DisplayGeneralError(e.InnerException.Message);
        }
        catch (Exception e)
        {
            _logger.Log($"Error: {DateTime.Now}: {e.Message}");
        }


        Console.WriteLine();
        Console.Write("Done? Press any key > ");
        Console.ReadKey();

        _stregsystemUI.Start();

    }

    private void RegisterAdminCommands(Dictionary<string, LambdaExpression> dictionary)
    {
        dictionary[":quit"] = () => _stregsystemUI.Close();
        dictionary[":q"] = () => _stregsystemUI.Close();

        dictionary[":activate"] = (string id) => _stregsystem.SetProductStatus(int.Parse(id), true);
        dictionary[":deactivate"] = (string id) => _stregsystem.SetProductStatus(int.Parse(id), false);

        dictionary[":crediton"] = (string id) => _stregsystem.SetProductCreditStatus(int.Parse(id), true);
        dictionary[":creditoff"] = (string id) => _stregsystem.SetProductCreditStatus(int.Parse(id), false);

        dictionary[":addcredits"] = (string username, string amount) => _stregsystem.AddCreditsToAccount(_stregsystem.GetUserByUsername(username), int.Parse(amount));
    }

    private void Write(string command)
    {
        Console.WriteLine(command);
    }
}