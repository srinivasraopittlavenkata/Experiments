/*============================================================
**
** Class:  Program
** 
** Purpose: Console runner app 
*           to test the outcome of quotes and brackets validator
**
===========================================================*/
namespace Experiments.ConsoleApp
{
    using Experiments.Validators;
    using Experiments.Validators.ConsoleApp.Properties;
    using Experiments.Validators.Interfaces;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Resources.WelcomeMessage);

            // initialize the validator
            IValidator<string> validator = new QuoteBracketsValidator();
            do
            {
                
                // read the input
                string input = Console.ReadLine();

                try
                {
                    // validate
                    Console.WriteLine(Resources.ResultMessage, validator.Validate(input));
                }
                catch (Exception unKnownException)
                {
                    Console.WriteLine(Resources.UnhandledExceptionMessage, Guid.NewGuid(), unKnownException.ToString());
                }

                Console.WriteLine(Resources.QuitOrEnterMessage);

            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
    }
}
