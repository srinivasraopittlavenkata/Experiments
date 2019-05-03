/*============================================================
**
** Class:  QuoteBracketsValidator
** 
** Purpose: An implementation of IValidator
*           which can be used to validate a string 
*           for balanced Quotes and Brackets
**
===========================================================*/

namespace Experiments.Validators
{

    using Experiments.Validators.Interfaces;
    using Experiments.Validators.Properties;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Exposes functions to validate a string for balacned quotes and brackets with
    /// additional helper methods are provided for extending
    /// </summary>
    public class QuoteBracketsValidator : IValidator<string>
    {
        /// <summary>
        /// internal member to store the bracket pairs created in initialization and used for validation
        /// </summary> 
        private Dictionary<char, char> _bracketPairs = new Dictionary<char, char>();

        /// <summary>
        /// internal member to store the quotes created in initialization and used for validation
        /// </summary> 
        private HashSet<char> _quotes = new HashSet<char>();

        /// <summary>
        /// Default constructor to be used for validation only checks single quote (') and parentheses '(' and ')'
        /// </summary>
        public QuoteBracketsValidator()
        {
            this.Init();
        }

        /// <summary>
        /// Parameterized constructor to assign custom bracket pairs, and quotes
        /// note you can only pass a supported list of brackets and quotes
        /// which are brackets- '(' ')' '{' '}' '<''>' '[' ']'  quotes- ' "
        /// </summary>
        /// <param name="bracketPairs">A dictionary map with brackets pair to be used for balance check- key as open bracket and value as closed bracket</param>
        /// <param name="quotes">An array of quotes to be used for balance check- can be single or double quote</param>
        public QuoteBracketsValidator(Dictionary<char, char> bracketPairs, char[] quotes)
        {
            if (bracketPairs == null)
            {
                throw new ArgumentNullException(string.Format(Resources.QuoteBracketsValidator_Parameters_Null, "bracketPairs"));
            }

            if (quotes == null)
            {
                throw new ArgumentNullException(string.Format(Resources.QuoteBracketsValidator_Parameters_Null, "quotes"));
            }

            foreach (var pair in bracketPairs)
            {
                this.AddOrUpdateBracketPair(pair.Key, pair.Value);
            }

            foreach (var quote in quotes)
            {
                this.AddQuote(quote);
            }
        }

        /// <summary>
        /// Adds a quote to the set of quotes used for balance check
        /// </summary>
        /// <param name="quote">single or double quote</param>
        public void AddQuote(char quote)
        {
            if (!DefaultValues.SupportedQuotes.Contains(quote))
            {
                throw new ArgumentException(string.Format(Resources.QuoteBracketsValidator_Quote_NotSupported, quote));
            }

            this._quotes.Add(quote);
        }

        /// <summary>
        /// Removes a quote from the set of quotes used for balance check
        /// </summary>
        /// <param name="quote">single or double quote</param>
        /// <returns>true if the quote exists and deletion is successful, false otherwise</returns>
        public bool RemoveQuote(char quote)
        {
            return this._quotes.Remove(quote);
        }

        /// <summary>
        /// Adds a new bracket pair for balance check in the validator, the pair should be a valid pari supported
        /// </summary>
        /// <param name="openBracket">open bracket should be one of the supported brackets</param>
        /// <param name="closeBracket">close bracket should be one of the supported brackets</param>
        public void AddOrUpdateBracketPair(char openBracket, char closeBracket)
        {
            if (!DefaultValues.SupportedBracketsMap.ContainsKey(openBracket) || DefaultValues.SupportedBracketsMap[openBracket] != closeBracket)
            {
                throw new ArgumentException(string.Format(Resources.QuoteBracketsValidator_BracketsCombination_NotSupported, openBracket, closeBracket));
            }

            if (!_bracketPairs.ContainsKey(openBracket))
            {
                this._bracketPairs.Add(openBracket, closeBracket);
            }
        }

        /// <summary>
        /// Removes a bracket pair by matching its key which is open bracket 
        /// </summary>
        /// <param name="openBracket">open bracket key which needs to be removed from balance check</param>
        /// <returns>true if the bracket exists and deletion is successful, false otherwise</returns>
        public bool RemoveBracket(char openBracket)
        {
            return this._bracketPairs.Remove(openBracket);
        }

        /// <summary>
        /// Helper method that intializes default bracket pari and quotes to the validation check
        /// </summary>
        private void Init()
        {
            this.AddOrUpdateBracketPair('(', ')');
            this.AddQuote('\'');
        }

        /// <summary>
        /// Validates the input string value if it is balanced
        /// Looks if the string is balanced as per the brackets and quotes supported by the validator
        /// </summary>
        /// <param name="value">string to be validated</param>
        /// <returns>true if string is valid and is balanced with quotes and brackets, false otherwise</returns>
        public bool Validate(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }
            
            var openIdentifierStack = new Stack<char>();

            foreach (char currChar in value)
            {
                if (this._bracketPairs.ContainsKey(currChar))
                {
                    // if open bracket add to stack
                    openIdentifierStack.Push(currChar);
                }                
                else if (this._bracketPairs.ContainsValue(currChar))
                {
                    // if closed bracket
                    if (openIdentifierStack.Count == 0 || // invalid if stack empty
                        !this._bracketPairs.ContainsKey(openIdentifierStack.Peek()) || // invalid top is not a bracket
                        !this._bracketPairs[openIdentifierStack.Pop()].Equals(currChar) // invalid top is not matching open bracket
                        )
                    {
                        return false;
                    }
                }                
                else if (this._quotes.Contains(currChar))
                {
                    // if quote add to stack if empty or pop if top is matching quote
                    if (openIdentifierStack.Count > 0 && 
                        openIdentifierStack.Peek().Equals(currChar))
                    {
                        openIdentifierStack.Pop();

                    }
                    else
                    {
                        openIdentifierStack.Push(currChar);
                    }
                }
            }

            return openIdentifierStack.Count == 0;
        }
    }
}
