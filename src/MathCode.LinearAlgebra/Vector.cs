using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCode.LinearAlgebra
{
    /// <summary>
    /// Vector defenition
    /// </summary>
    /// <typeparam name="T">Vector element value type</typeparam>
    public struct Vector<T> where T : struct
    {
        private readonly T[] _vector;

        #region ctors
        public Vector()
        {
            _vector = new T[0];
        }

        /// <summary>
        /// Initialize vector from array
        /// </summary>
        /// <param name="vector">Array of values</param>
        public Vector(T[] vector)
        {
            _vector = vector;
        }

        /// <summary>
        /// initialize a vector with a given length and default value
        /// </summary>
        /// <param name="length">Lengtn of vector</param>
        /// <param name="value">Default value</param>
        public Vector(int length, T value)
        {
            _vector = Enumerable.Repeat<T>(value, length).ToArray();
        }

        /// <summary>
        /// Initialize vector from string
        /// </summary>
        /// <param name="line">Array of values as string</param>
        /// <param name="delimiter">Delimiter character between values</param>
        /// <exception cref="ArgumentException">If string is empty throw ArgumentException</exception>
        public Vector(string line, CultureInfo? culture = null, char delimiter = ' ')
        {
            if (string.IsNullOrWhiteSpace(line))
                throw new ArgumentException("Can't create vector. String is empty.");

            culture ??= CultureInfo.InvariantCulture;

            try
            {
                _vector = line
                    .Trim()
                    .Split(delimiter)
                    .Select(singleVal => (T)Convert.ChangeType(singleVal, typeof(T), culture))
                    .ToArray();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Get total number of elements in vector
        /// </summary>
        public int Length => _vector.Length;

        /// <summary>
        /// Convert vector to array
        /// </summary>
        public T[] ToArray() => _vector;

        /// <summary>
        /// Returns the element at the given index.
        /// </summary>
        public T this[int index]
        {
            get
            {
                if (index >= Length || index < 0)
                {
                    throw new IndexOutOfRangeException($"Index {index} out of range.");
                }
                return _vector[index];
            }
            set
            {
                if (index >= Length || index < 0)
                {
                    throw new IndexOutOfRangeException($"Index {index} out of range.");
                }

                if (value.GetType() != typeof(T))
                    throw new ArgumentException($"Value is not generic type {typeof(T)}.");

                _vector[index] = value;
            }
        }
        #endregion

        #region Operators
        /// <summary>
        /// Adds two vectors together.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The summed vector.</returns>
        public static Vector<T> operator +(Vector<T> left, Vector<T> right) 
        {
            if (left.Length != right.Length)
                throw new ArgumentException("Vectors must be same dimentions.");

            var result = new T[left.Length];
            for (int i = 0; i < left.Length; i++)
                result[i] = Operators<T>.Add (left[i], right[i]);

            return new Vector<T>(result);
        }

        /// <summary>
        /// Subtracts the second vector from the first.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The difference vector.</returns>
        public static Vector<T> operator -(Vector<T> left, Vector<T> right)
        {
            if (left.Length != right.Length)
                throw new ArgumentException("Vectors must be same dimentions.");

            var result = new T[left.Length];
            for (int i = 0; i < left.Length; i++)
                result[i] = Operators<T>.Sub(left[i], right[i]);

            return new Vector<T>(result);
        }

        /// <summary>
        /// Multiplies vector by number
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The source number.</param>
        /// <returns>The product vector.</returns>
        public static Vector<T> operator *(Vector<T> left, T right)
        {
            var result = new T[left.Length];
            for (int i = 0; i < left.Length; i++)
                result[i] = Operators<T>.Mul(left[i], right);

            return new Vector<T> (result);
        }

        /// <summary>
        /// Multiplies vector by number
        /// </summary>
        /// <param name="right">The first source vector.</param>
        /// <param name="left">The source number.</param>
        /// <returns>The product vector.</returns>
        public static Vector<T> operator *(T left, Vector<T> right) => right * left;

        /// <summary>
        /// Scalar multiplies two vectors together.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>The product vector.</returns>
        public static T operator *(Vector<T> left, Vector<T> right)
        {
            if (left.Length != right.Length)
                throw new ArgumentException("Vectors must be same dimentions.");

            T result = default;
            for (int i = 0; i < left.Length; i++)
                result = Operators<T>.Add(result, Operators<T>.Mul(left[i], right[i]));

            return result;
        }
        #endregion

        #region Comparison operation
        /// <summary>
        /// Override hash code for the current object
        /// </summary>
        /// <returns>Return hash code for the current object</returns>
        public override int GetHashCode() => _vector.GetHashCode();

        /// <summary>
        /// Equal operation
        /// </summary>
        /// <param name="obj">Сompared object</param>
        /// <returns>Result equal operation</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not Vector<T> equalVector)
            {
                return false;
            }

            if (equalVector.Length != Length)
                return false;

            for (int i = 0; i < Length; i++)
                if (!Operators<T>.Equal(equalVector[i], this[i]))
                        return false;
            return true;
        }

        /// <summary>
        /// Equal operator
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>Result equal operation</returns>
        public static bool operator ==(Vector<T> left, Vector<T> right)
        {
            // Equals handles case of null on right side.
            return left.Equals(right);
        }

        /// <summary>
        /// Not equal operator
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>Result equal operation</returns>
        public static bool operator !=(Vector<T> left, Vector<T> right) => !(left == right);
        #endregion

        /// <summary>
        /// Get vectors from columns of a matrix
        /// </summary>
        /// <param name="matrix">Matrix value</param>
        /// <returns>Expose enumerator by vector collection</returns>
        public static IEnumerable<Vector<T>> ColumnsToVectors(Matrix<T> matrix)
        {
            var rowCount = matrix.Value.GetLength(0);
            var columnCount = matrix.Value.GetLength(1);

            return Enumerable.Range(0, columnCount)
                .Select(y => Enumerable.Range(0, rowCount)
                    .Select(x => matrix[x, y])
                    .ToArray())
                .Select(v => new Vector<T>(v));
        }

        /// <summary>
        /// Get vectors from rows of a matrix
        /// </summary>
        /// <param name="matrix">Matrix value</param>
        /// <returns>Expose enumerator by vector collection</returns>
        public static IEnumerable<Vector<T>> RowsToVectors(Matrix<T> matrix)
        {
            var rowCount = matrix.Value.GetLength(0);
            var columnCount = matrix.Value.GetLength(1);

            return Enumerable.Range(0, rowCount)
                .Select(x => Enumerable.Range(0, columnCount)
                    .Select(y => matrix[x, y])
                    .ToArray())
                .Select(v => new Vector<T>(v))
                .ToArray();
        }
    }
}
