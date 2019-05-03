/*============================================================
**
** Class:  QuoteBracketsValidatorTests
** 
** Purpose: Unit tests to validate methods of QuoteBracketsValidator
*           Tests cover following,
*           -valid cases
*           -invalid cases
*           -expection cases
**
===========================================================*/

namespace Experiments.Validators.Tests
{
    using Experiments.Validators.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    [TestClass]
    public class QuoteBracketsValidatorTests
    {
        #region Instantiation Tests

        [TestMethod]
        public void Instantiate_DefaultBracketsQuotes()
        {
            var validator = new QuoteBracketsValidator();
            Assert.IsNotNull(validator);
        }

        [TestMethod]
        public void Instantiate_AdditionalBracketsQuotes()
        {
            var validator = new QuoteBracketsValidator(new System.Collections.Generic.Dictionary<char, char>(), new char[1] { '\'' });
            Assert.IsNotNull(validator);
        }

        [TestMethod]
        public void Instantiate_AsIValidator()
        {
            IValidator<string> validator = new QuoteBracketsValidator(new System.Collections.Generic.Dictionary<char, char>(), new char[1] { '\'' });
            Assert.IsNotNull(validator);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Instantiate_NullBrackets_ThrowArgumentNullException()
        {
            new QuoteBracketsValidator(null, new char[1] { '\'' });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Instantiate_NullQuotes_ThrowArgumentNullException()
        {
            new QuoteBracketsValidator(new System.Collections.Generic.Dictionary<char, char>(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Instantiate_NullQuotes_NullBrackets_ThrowArgumentNullException()
        {
            new QuoteBracketsValidator(null, null);
        }

        #endregion      

        #region AddQuote Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddQuote_NotSupported_ThrowArgumentException()
        {
            var validator = new QuoteBracketsValidator();
            validator.AddQuote('?');
        }

        [TestMethod]
        public void AddQuote_Supported()
        {
            var validator = new QuoteBracketsValidator();
            validator.AddQuote('\'');
            validator.AddQuote('"');
        }

        #endregion

        #region AddBracketPair Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddBracketPair_NotSupported_ThrowArgumentException()
        {
            var validator = new QuoteBracketsValidator();
            validator.AddOrUpdateBracketPair('A', 'B');
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddBracketPair_NotSupportedClosingBracket_ThrowArgumentException()
        {
            var validator = new QuoteBracketsValidator();
            validator.AddOrUpdateBracketPair('{', 'B');
        }

        [TestMethod]
        public void AddBracketPair_Supported()
        {
            var validator = new QuoteBracketsValidator();
            validator.AddOrUpdateBracketPair('{', '}');
        }
        #endregion

        #region Valid Cases Tests
        [TestMethod]
        public void DefaultValidator_EmptyString_Valid()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsTrue(validator.Validate(""));
        }

        [TestMethod]
        public void DefaultValidator_Null_Valid()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsTrue(validator.Validate(null));
        }

        [TestMethod]
        public void DefaultValidator_NoBracketsOrQuotes_Valid()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsTrue(validator.Validate("a b c"));
        }

        [TestMethod]
        public void DefaultValidator_OpenCloseBracketPair_Valid()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsTrue(validator.Validate("(a b c)"));
        }

        [TestMethod]
        public void DefaultValidator_QuotePair_Valid()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsTrue(validator.Validate("'a b c'"));
        }

        [TestMethod]
        public void DefaultValidator_QuotePairNestedInBrackets_Valid()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsTrue(validator.Validate("('a b c')"));
        }

        [TestMethod]
        public void DefaultValidator_BackertsPairNestedInQuotes_Valid()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsTrue(validator.Validate("'(a b c)'"));
        }

        [TestMethod]
        public void DefaultValidator_BackertsPairQuotesPairSeperate_Valid()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsTrue(validator.Validate("'a b c' (a b c)"));
        }

        [TestMethod]
        public void DefaultValidator_MultipleValidCases()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsTrue(validator.Validate("This is a test "));
            Assert.IsTrue(validator.Validate("This (is a) test"));
            Assert.IsTrue(validator.Validate("(This (is a) test)"));
            Assert.IsTrue(validator.Validate("This 'is a' test"));
            Assert.IsTrue(validator.Validate("'This 'is a' test'"));
            Assert.IsTrue(validator.Validate("(This 'is a' test)"));
            Assert.IsTrue(validator.Validate("'This (is a) test'"));
            Assert.IsTrue(validator.Validate("'This 'is a( test)"));
        }

        [TestMethod]
        public void AdvancedValidator_MultipleValidCases()
        {
            var validator = new QuoteBracketsValidator();
            validator.AddQuote('"');
            validator.AddOrUpdateBracketPair('[', ']');
            Assert.IsTrue(validator.Validate("()[]"));
            Assert.IsTrue(validator.Validate("([])"));
            Assert.IsTrue(validator.Validate("'\"\"\"\"'"));
            Assert.IsTrue(validator.Validate("'\"[\"[\"[\"[]\"]\"]\"]\"'"));
            Assert.IsTrue(validator.Validate("\"\"''([])"));
            Assert.IsTrue(validator.Validate("([[\"\"]('')]([]))"));
            Assert.IsTrue(validator.Validate("'\"\"[(\"\")]'"));
            Assert.IsTrue(validator.Validate("'\"\"[(\"\")]\"\"'"));
        }
        #endregion

        #region InValid Cases Tests

        [TestMethod]
        public void DefaultValidator_OpenBracketWithoutClose_InValid()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsFalse(validator.Validate("a ( b c"));
        }

        [TestMethod]
        public void DefaultValidator_CloseBracketWithoutOpen_InValid()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsFalse(validator.Validate("a b c )"));
        }

        [TestMethod]
        public void DefaultValidator_OnlyOneQuote_InValid()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsFalse(validator.Validate("a 'b c"));
        }

        [TestMethod]
        public void DefaultValidator_OneQuotesNotPaired_InValid()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsFalse(validator.Validate("'a' 'b' 'c "));
        }

        [TestMethod]
        public void DefaultValidator_QuotesPaired_WithOrphanOpenBracketNestedIn_InValid()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsFalse(validator.Validate("'a ( b ' c"));
        }

        [TestMethod]
        public void DefaultValidator_BracketsPaired_WithOrphanQuoteNestedIn_InValid()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsFalse(validator.Validate("(a ' b ) c"));
        }

        [TestMethod]
        public void DefaultValidator_BracketsPaired_QuotesParied_Overlapping_EndsNestedIn_InValid()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsFalse(validator.Validate("(a ' b ) c ' d "));
            Assert.IsFalse(validator.Validate("'a ( b ' c ) d "));
        }

        [TestMethod]
        public void DefaultValidator_MultipleInValidCases()
        {
            IValidator<string> validator = new QuoteBracketsValidator();
            Assert.IsFalse(validator.Validate("((a)"));
            Assert.IsFalse(validator.Validate("(a)(a))"));
            Assert.IsFalse(validator.Validate("This (is a test"));
            Assert.IsFalse(validator.Validate("This is a) test"));
            Assert.IsFalse(validator.Validate("This 'is a test"));
            Assert.IsFalse(validator.Validate("T'This (is a' test)"));
        }

        [TestMethod]
        public void AdvancedValidator_MultipleInValidCases()
        {
            var validator = new QuoteBracketsValidator();
            validator.AddQuote('"');
            validator.AddOrUpdateBracketPair('[', ']');
            Assert.IsFalse(validator.Validate("(()"));
            Assert.IsFalse(validator.Validate("()())"));
            Assert.IsFalse(validator.Validate("(()"));
            Assert.IsFalse(validator.Validate("()())"));
            Assert.IsFalse(validator.Validate("\"'\"'"));
            Assert.IsFalse(validator.Validate("(')[']"));
            Assert.IsFalse(validator.Validate("\"[\"]\"\"'"));
            Assert.IsFalse(validator.Validate("('\"\"\"\"'"));
            Assert.IsFalse(validator.Validate("'\"[\"[\"[\"[]\"]\"]\"]\"')"));
            Assert.IsFalse(validator.Validate("(\"\"''([])"));
            Assert.IsFalse(validator.Validate("([[\"\"]('')]([]))["));
            Assert.IsFalse(validator.Validate("'\"\"[(\"\")]['"));
            Assert.IsFalse(validator.Validate("'\"\"[(\"\")]\"\"']"));
        }

        #endregion
    }
}
