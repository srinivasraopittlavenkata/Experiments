/*============================================================
**
** Interface:  DefaultValues
** 
** Purpose: Static class shared by all the validators
*           which contains getters for default values 
*           used by validators
**
===========================================================*/
namespace Experiments.Validators
{
    using System.Collections.Generic;

    /// <summary>
    /// A static class that has default values that are supported by validators, like brackets and quotes
    /// </summary>
    public static class DefaultValues
    {
        /// <summary>
        /// internal member that stores all the default supported bracket combinations by string QuoteBracketsValidator
        /// </summary>        
        private static Dictionary<char, char> _supportedBrackets = new Dictionary<char, char>
        {
            { '(', ')' },
            { '{', '}' },
            { '<', '>' },
            { '[', ']' }
        };

        /// <summary>
        /// internal member that stores all the default supported quotes by string QuoteBracketsValidator
        /// </summary>   
        private static HashSet<char> _supportedQuotes = new HashSet<char>
        {
            '\'',
            '"'
        };

        /// <summary>
        /// Returns a dictionary map that contains supported open bracket & closed bracket combination as key & value respectively
        /// </summary>
        public static Dictionary<char, char> SupportedBracketsMap
        {
            get
            {
                return _supportedBrackets;
            }
        }

        /// <summary>
        /// Returns a Hashset that contains all the supported quotes
        /// </summary>
        public static HashSet<char> SupportedQuotes
        {
            get
            {
                return _supportedQuotes;
            }
        }

    }
}
