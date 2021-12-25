using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCode.LinearAlgebra
{
    /// <summary>
    /// Matrix operations
    /// </summary>
    public static class MatrixOperations
    {
        /// <summary>
        /// Adds two matrix together.
        /// </summary>
        /// <param name="leftMatrix">The first source matrix.</param>
        /// <param name="rightMatrix">The second source matrix.</param>
        /// <returns>The summed matrix.</returns>
        public static Matrix<T> Add<T>(this Matrix<T> leftMatrix, Matrix<T> rightMatrix) where T : struct
        {
            if (!leftMatrix.DimensionEqual(rightMatrix))
                throw new ArgumentException("The dimensions of the matrices must be the same");


            var result = Enumerable.Range(0, leftMatrix.Rows)
                .Select(x => new Vector<T>(
                    Enumerable.Range(0, leftMatrix.Cols)
                        .Select(y => Operators<T>.Add(leftMatrix.Value[x, y], rightMatrix.Value[x, y]))
                    .ToArray()
                    ))
                .ToArray();

            return new Matrix<T>(result);
        }

        /// <summary>
        /// Subtracts the second matrix from the first.
        /// </summary>
        /// <param name="leftMatrix">The first source matrix.</param>
        /// <param name="rightMatrix">The second source matrix.</param>
        /// <returns>The difference matrix.</returns>
        public static Matrix<T> Sub<T>(this Matrix<T> leftMatrix, Matrix<T> rightMatrix) where T : struct
        {
            if (!leftMatrix.DimensionEqual(rightMatrix))
                throw new ArgumentException("The dimensions of the matrices must be the same");

            var result = Enumerable.Range(0, leftMatrix.Rows)
                .Select(x => new Vector<T>(Enumerable.Range(0, leftMatrix.Cols)
                    .Select(y => Operators<T>.Sub(leftMatrix.Value[x, y], rightMatrix.Value[x, y]))
                    .ToArray())
                )
                .ToArray();

            return new Matrix<T>(result);
        }

        /// <summary>
        /// Multiplies two matrix together.
        /// </summary>
        /// <param name="leftMatrix">The first source matrix.</param>
        /// <param name="rightMatrix">The second source matrix.</param>
        /// <returns>Result of multiplication matrices.</returns>
        public static Matrix<T> Multiple<T>(this Matrix<T> leftMatrix, Matrix<T> rightMatrix) where T : struct
        {
            if (leftMatrix.Cols != rightMatrix.Rows)
                throw new ArgumentException("Result of multiplication is defined if and only if the number of columns in left matrix equals the number of rows in right matrix.");

            var left = Vector<T>.RowsToVectors(leftMatrix);
            var right = Vector<T>.ColumnsToVectors(rightMatrix);

            var result = left.Select((row, i) => 
                    new Vector<T>(
                        right.Select((col, j) => row * col).ToArray()
                    )
                )
                .ToArray();

            return new Matrix<T>(result);
        }

        /// <summary>
        /// Multiplies matrix by number
        /// </summary>
        /// <param name="leftMatrix">The source matrix.</param>
        /// <param name="rightMatrix">The source number.</param>
        /// <returns>Result of multiplication matrices.</returns>
        public static Matrix<T> Multiple<T>(this Matrix<T> leftMatrix, T rightMatrix) where T : struct
        {
            var left = Vector<T>.RowsToVectors(leftMatrix);

            var result = left.Select((row, i) => new Vector<T> ((rightMatrix * row).ToArray())).ToArray();

            return new Matrix<T>(result);
        }

        /// <summary>
        /// Transpose a matrix
        /// </summary>
        /// <typeparam name="T">The type of the variable in the matrix cell</typeparam>
        /// <param name="matrix">Matrix for transpose</param>
        /// <returns>Result of transposing matrix</returns>
        public static Matrix<T> Transpose<T>(this Matrix<T> matrix) where T : struct
        {
            var result = new T[matrix.Cols, matrix.Rows];
            for (var i = 0; i < matrix.Rows; i++)
                for (var j = 0; j < matrix.Cols; j++)
                    result[j, i] = matrix.Value[i, j];

            return new Matrix<T>(result);
        }

        /// <summary>
        /// Round matrix values to a specified number of fraction digits using specified rounding convertion
        /// </summary>
        /// <typeparam name="TIn">Source matrix type</typeparam>
        /// <typeparam name="TOut">Output matrix type</typeparam>
        /// <param name="matrix">Decimal matrix values to be rounding</param>
        /// <param name="decimals">The number of decimal places in the return value</param>
        /// <param name="rounding">One of enumeration values that specifies which rounding strategy to use</param>
        /// <returns>Matrix with values nearest origin values. If the fraction component of value halfway
        /// between two integers, one of which is even and the other odd, the even number is returned.
        /// Note that this method return a <see cref="Matrix{TOut}"/> instead of an integer type.</returns>
        public static Matrix<TOut> Round<TIn, TOut>(Matrix<TIn> matrix, int decimals, MidpointRounding rounding = MidpointRounding.ToEven) 
            where TIn : struct 
            where TOut : struct
        {
            var result = Enumerable.Range(0, matrix.Rows)
                .Select(x => new Vector<TOut>(Enumerable.Range(0, matrix.Cols)
                    .Select(y =>
                    {
                        dynamic value = matrix.Value[x, y];
                        return (TOut)Convert.ChangeType(Math.Round(value, decimals, rounding), typeof(TOut));
                    }).ToArray())
                ).ToArray();

            return new Matrix<TOut>(result);
        }
    }
}
