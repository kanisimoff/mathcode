using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCode.LinearAlgebra
{
    /// <summary>
    /// Helper class for preparing a matrix array
    /// </summary>
    internal static class MatrixConverter
    {
        /// <summary>
        /// Initialize 2d array from vectors
        /// </summary>
        /// <typeparam name="T">The type of the variable in the matrix cell</typeparam>
        /// <param name="vectorArrays">Array of vectors</param>
        /// <returns>Resul 2d array</returns>
        public static T[,] ToMatrix<T>(this Vector<T>[] vectorArrays) where T : struct
        {
            var minorLength = vectorArrays.Min(v => v.Length) ;
            var result = new T[vectorArrays.Length, minorLength];

            for (var i = 0; i < vectorArrays.Length; i++)
            {
                var array = vectorArrays[i];
                if (array.Length != minorLength)
                {
                    throw new ArgumentException("Vectors must be of the same length.");
                }
                for (int j = 0; j < minorLength; j++)
                {
                    result[i, j] = array.ToArray()[j];
                }
            }
            return result;
        }
    }
}
