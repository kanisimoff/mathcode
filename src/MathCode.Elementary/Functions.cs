using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MathCode.Elementary
{
    /// <summary>
    /// Math fuctions
    /// </summary>
    public static class Math
    {
        /// <summary>
        /// Factorial is non-negative integer n, denoted by n!, is the product of all positive integers less than or equal to n
        /// </summary>
        /// <param name="n">Non-negative integer</param>
        /// <returns>The result of the factorial function</returns>
        /// <exception cref="ArgumentException">An exception is returned if the argument is negative</exception>
        public static BigInteger Factorial (int n)
        {
            return n switch
            {
                <0 => throw new ArgumentException("Argument value can't be negative"),
                0 => 1,
                _ => (ulong) Enumerable.Range(1, n).Aggregate(1, (p, item) => p * item)
            };
        }
    }
}
