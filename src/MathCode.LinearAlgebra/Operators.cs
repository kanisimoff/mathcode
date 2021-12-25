using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCode.LinearAlgebra
{
    /// <summary>
    /// Operators on dynamic types
    /// </summary>
    /// <typeparam name="T">Operand type</typeparam>
    internal static class Operators<T> where T : struct
    {
        /// <summary>
        /// Addition operation
        /// </summary>
        /// <param name="leftVal">Left value in expression</param>
        /// <param name="rightVal">Right value in expression</param>
        /// <returns>Addition operation result</returns>
        public static T Add(T leftVal, T rightVal) 
        {
            dynamic a = leftVal;
            dynamic b = rightVal;
            return a + b;
        }

        /// <summary>
        /// Subtraction operation
        /// </summary>
        /// <param name="leftVal">Left value in expression</param>
        /// <param name="rightVal">Right value in expression</param>
        /// <returns>Subtraction operation result</returns>
        public static T Sub(T number1, T number2)
        {
            dynamic a = number1;
            dynamic b = number2;
            return a - b;
        }

        /// <summary>
        /// Multiplication operation
        /// </summary>
        /// <param name="leftVal">Left value in expression</param>
        /// <param name="rightVal">Right value in expression</param>
        /// <returns>Multiplication operation result</returns>
        public static T Mul(T number1, T number2)
        {
            dynamic a = number1;
            dynamic b = number2;
            return a * b;
        }

        /// <summary>
        /// Equal operation
        /// </summary>
        /// <param name="leftVal">Left value in expression</param>
        /// <param name="rightVal">Right value in expression</param>
        /// <returns>Equal operation result</returns>
        public static bool Equal(T number1, T number2)
        {
            dynamic a = number1;
            dynamic b = number2;
            return a == b;
        }
    }
}
