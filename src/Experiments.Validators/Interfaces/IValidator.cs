/*============================================================
**
** Interface:  IValidator
** 
** Purpose: Defines the specification for a class to be a validator.
===========================================================*/
namespace Experiments.Validators.Interfaces
{
    public interface IValidator<T>
    {
        /// <summary>
        /// Validates the value as per the rules of an validator class
        /// </summary>
        /// <param name="value">input data</param>
        /// <returns>true if value meets the rules of the validator class, false otherwise.</returns>
        bool Validate(T value);
    }
}
