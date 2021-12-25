using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;

namespace MathCode.LinearAlgebra
{
    /// <summary>
    /// Matrix defenition
    /// </summary>
    /// <typeparam name="T">Matrix element value type</typeparam>
    public struct Matrix<T> where T : struct
    {
        private readonly T[,] _matrix;

        #region ctors
        public Matrix(T[,] matrix)
        {
            var array = new T[matrix.GetLength(0), matrix.GetLength(1)];
            Array.Copy(matrix, array, matrix.Length);
            _matrix = array;
        }

        /// <summary>
        /// Initialize matrix from vectors array
        /// </summary>
        /// <typeparam name="T">The type of the variable in the matrix cell</typeparam>
        /// <param name="vectors">Array of vectors</param>
        public Matrix(Vector<T>[] vectors)
        {
            var minorLength = vectors.Min(v => v.Length);
            if (vectors.Any(v => v.Length != minorLength))
            {
                throw new ArgumentException("Vectors must be of the same length.");
            }

            var result = new T[vectors.Length, minorLength];
            for (var i = 0; i < vectors.Length; i++)
            {
                for (var j = 0; j < minorLength; j++)
                {
                    result[i, j] = vectors[i][j];
                }
            }
            _matrix = result;
        }

        /// <summary>
        /// Initialize matrix from strings array
        /// </summary>
        /// <param name="arrays">Array of strings</param>
        public Matrix(string[] lines)
        {
            if (lines == null || !lines.Any())
                throw new ArgumentException("Can't create matrix. Array of string is empty.");

            try
            {
                var vectorArray = lines.Where(l => !string.IsNullOrWhiteSpace(l))
                    .Select(l => new Vector<T>(l))
                    .ToArray();

                _matrix = vectorArray.ToMatrix();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Rows count in matrix
        /// </summary>
        public int Rows => _matrix.GetLength(0);

        /// <summary>
        /// Columns count in matrix
        /// </summary>
        public int Cols => _matrix.GetLength(1);

        /// <summary>
        /// Matrix value as a 2d array
        /// </summary>
        public T[,] Value => _matrix;

        /// <summary>
        /// Returns the element at the given index.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public T this[int row, int column]
        {
            get
            {
                if (row >= Rows || row < 0)
                {
                    throw new IndexOutOfRangeException($"Row index {row} out of range ");
                }

                if (column >= Cols || column < 0)
                {
                    throw new IndexOutOfRangeException($"Column index {column} out of range ");
                }

                return _matrix[row, column];
            }
            set
            {
                if (row >= Rows || row < 0)
                {
                    throw new IndexOutOfRangeException($"Row index {row} out of range ");
                }

                if (column >= Cols || column < 0)
                {
                    throw new IndexOutOfRangeException($"Column index {column} out of range ");
                }

                if (value.GetType() != typeof(T))
                    throw new ArgumentException($"Value is not generic type {typeof(T)}.");

                _matrix[row, column] = value;
            }
        }

        /// <summary>
        /// Symmetry criterion
        /// </summary>
        public bool IsSymmetric => this == this.Transpose();
        #endregion

        /// <summary>
        /// Compares matrix sizes
        /// </summary>
        /// <param name="compare">Matrix for compare</param>
        /// <returns>Result comparison of matrix sizes</returns>
        public bool DimensionEqual(Matrix<T> compare)
        {
            return Rows == compare.Rows && Cols == compare.Cols;
        }

        #region Operators
        /// <summary>
        /// Matrix addition operator
        /// </summary>
        /// <typeparam name="T">Type of the value in the matrix cell</typeparam>
        /// <param name="leftMatrix">Left operand of a matrix addition expression</param>
        /// <param name="rightMatrix">Right operand of a matrix addition expression</param>
        /// <returns>Matrix addition result</returns>
        public static Matrix<T> operator +(Matrix<T> leftMatrix, Matrix<T> rightMatrix) => leftMatrix.Add(rightMatrix);

        /// <summary>
        /// Matrix substract operator
        /// </summary>
        /// <typeparam name="T">Type of the value in the matrix cell</typeparam>
        /// <param name="leftMatrix">Left operand of a matrix addition expression</param>
        /// <param name="rightMatrix">Right operand of a matrix addition expression</param>
        /// <returns>Matrix substract result</returns>
        public static Matrix<T> operator -(Matrix<T> leftMatrix, Matrix<T> rightMatrix) => leftMatrix.Sub(rightMatrix);

        /// <summary>
        /// Matrix multiplication operator
        /// </summary>
        /// <typeparam name="T">Type of the value in the matrix cell</typeparam>
        /// <param name="leftMatrix">Left operand of a matrix addition expression</param>
        /// <param name="rightMatrix">Right operand of a matrix addition expression</param>
        /// <returns>Matrix multiplication result</returns>
        public static Matrix<T> operator *(Matrix<T> leftMatrix, Matrix<T> rightMatrix) => leftMatrix.Multiple(rightMatrix);

        /// <summary>
        /// Multiplies matrix by number
        /// </summary>
        /// <param name="leftMatrix">The source matrix.</param>
        /// <param name="right">The source number.</param>
        /// <returns>Result of multiplication matrices.</returns>
        public static Matrix<T> operator *(Matrix<T> leftMatrix, T right) => leftMatrix.Multiple(right);

        /// <summary>
        /// Multiplies matrix by number
        /// </summary>
        /// <param name="left">The source number.</param>
        /// <param name="rightMatrix">The source matrix.</param>
        /// <returns>Result of multiplication matrices.</returns>
        public static Matrix<T> operator *(T left, Matrix<T> rightMatrix) => rightMatrix * left;
        #endregion

        /// <summary>
        /// Convert matrix to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var matrix = _matrix;
            
            if (_matrix == null)
                return string.Empty;

            return string.Join(Environment.NewLine,
                matrix.OfType<T>()
                    .Select((value, index) => new { value, index })
                    .GroupBy(x => x.index / matrix.GetLength(1))
                    .Select(x => $"{string.Join(" ", x.Select(y => y.value))}"));
        }

        #region Comparison operation
        /// <summary>
        /// Override hash code for the current object
        /// </summary>
        /// <returns>Return hash code for the current object</returns>
        public override int GetHashCode() => this.ToString().GetHashCode();

        /// <summary>
        /// Equal operation
        /// </summary>
        /// <param name="obj">Сompared object</param>
        /// <returns>Result equal operation</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not Matrix<T> equalMatrix)
            {
                return false;
            }

            if (equalMatrix.Cols != Cols || equalMatrix.Rows != Rows)
                return false;

            for (var i = 0; i < Rows; i++)
                for (var j = 0; j < Cols; j++)
                    if (!Operators<T>.Equal(equalMatrix[i, j], this[i, j]))
                        return false;            
            return true;
        }

        /// <summary>
        /// Equal operator
        /// </summary>
        /// <param name="leftMatrix">Left operand of a matrix</param>
        /// <param name="rightMatrix">Right operand of a matrix</param>
        /// <returns>Result equal operation</returns>
        public static bool operator ==(Matrix<T> leftMatrix, Matrix<T> rightMatrix)
        {
            // Equals handles case of null on right side.
            return leftMatrix.Equals(rightMatrix);
        }

        /// <summary>
        /// Not equal operator
        /// </summary>
        /// <param name="leftMatrix">Left operand of a matrix</param>
        /// <param name="rightMatrix">Right operand of a matrix</param>
        /// <returns>Result equal operation</returns>
        public static bool operator !=(Matrix<T> leftMatrix, Matrix<T> rightMatrix) => !(leftMatrix == rightMatrix);
        #endregion

        #region Identity matrix
        /// <summary>
        /// Identity matrix of size n is the n × n square matrix with ones on the main diagonal and zeros elsewhere.
        /// It is denoted by In, or simply by I if the size is immaterial or can be trivially determined by the context
        /// </summary>
        /// <param name="n">Size n is the n × n square matrix</param>
        /// <returns> Identity matrix of size n is the n × n square matrix</returns>
        public static Matrix<T> I(int n)
        {
            var result = new T[n, n];
            for (var i = 0; i < n; i++)
            {
                result[i, i] = (T)Convert.ChangeType(1, typeof(T));
            }
            return new Matrix<T>(result);
        }
        #endregion

        #region Utility methods
        /// <summary>
        /// Replace matrix column by vector value
        /// </summary>
        /// <remarks>The number rows of the matrix must correspond to the number of vector elements</remarks>
        /// <param name="colNum">Ordinal column number in the matrix</param>
        /// <param name="vector">Vector for replace column values</param>
        public void ReplaceColumn(int colNum, Vector<T> vector)
        {
            if (vector.Length != Rows)
            {
                throw new ArgumentException("Vector length must be same as matrix rows count.");
            }

            for (var i = 0; i < vector.Length; i++)
            {
                _matrix[i, colNum] = vector[i];
            }
        }

        /// <summary>
        /// Replace matrix row by vector value
        /// </summary>
        /// <remarks>The number columns of the matrix must correspond to the number of vector elements</remarks>
        /// <param name="rowNum">Ordinal row number in the matrix</param>
        /// <param name="vector">Vector for replace column values</param>
        public void ReplaceRow(int rowNum, Vector<T> vector)
        {
            if (vector.Length != Cols)
            {
                throw new ArgumentException("Vector length must be same as matrix columns count.");
            }

            for (var i = 0; i < vector.Length; i++)
            {
                _matrix[rowNum, i] = vector[i];
            }
        }
        #endregion
    }
}