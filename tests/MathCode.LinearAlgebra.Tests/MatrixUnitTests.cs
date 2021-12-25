using System;
using Xunit;

namespace MathCode.LinearAlgebra.Tests
{
    public class MatrixUnitTests
    {
        [Fact]
        public void ThrowException_If_LinesNull()
        {
            // arrange
            string[]? lines = null;

            // act
            Action act = () => new Matrix<int>(lines!);

            // assert
            Exception ex = Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ThrowException_If_LinesContainLetter()
        {
            // arrange
            var lines = new[] { "1 2", "3 4", "5 a" };

            // act
            Action act = () => new Matrix<int>(lines);

            // assert
            Exception ex = Assert.Throws<FormatException>(act);
        }

        [Fact]
        public void ThrowException_If_Count_Collumns_Different()
        {
            // arrange
            var lines = new[] { "1 2", "3 4", "5 6 7" };

            // act
            Action act = () => new Matrix<int>(lines);

            // assert
            Exception ex = Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void ThrowException_If_Arrays_Length_Different()
        {
            // arrange
            var lines = new Vector<int>[]
            { 
                new(new[] { 1, 2 }),
                new(new[] { 3, 4 }),
                new(new[] { 5, 6, 7 })
            };

            // act
            Action act = () => new Matrix<int>(lines);

            // assert
            Exception ex = Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void Init_Matrix_by_Strings()
        {
            // arrange
            var lines = new[] { "1 2", "3 4", "5 6" };

            // act
            var matrix = new Matrix<int>(lines);

            // assert
            Assert.Equal(3, matrix.Rows);
            Assert.Equal(2, matrix.Cols);
            
            Assert.Equal(4, matrix[1,1]);
        }

        [Fact]
        public void Init_Matrix_by_Typed_VectorsArray()
        {
            // arrange
            var lines = new Vector<int>[]
            {
                new(new[] { 1, 2 }),
                new(new[] { 3, 4 }),
                new(new[] { 5, 6 })
            };

            // act
            var matrix = new Matrix<int>(lines);

            // assert
            Assert.Equal(3, matrix.Rows);
            Assert.Equal(2, matrix.Cols);

            Assert.Equal(4, matrix[1, 1]);
        }

        [Fact]
        public void GetDimension()
        {
            // arrange
            var lines = new[] { "1 2", "3 4", "5 6" };

            // act
            var matrix = new Matrix<int>(lines);

            // assert
            Assert.Equal(3, matrix.Rows);
            Assert.Equal(2, matrix.Cols);
        }

        [Fact]
        public void Get_Value_By_Index()
        {
            // arrange
            var lines = new[] { "1 2", "3 4", "5 6" };
            var matrix = new Matrix<int>(lines);

            // act
            var indexValue = matrix[2,1];

            // assert
            Assert.Equal(6, indexValue);
        }

        [Fact]
        public void ThrowException_If_IndexOutOfRange()
        {
            // arrange
            var lines = new[] { "1 2", "3 4", "5 6" };
            var matrix = new Matrix<int>(lines);

            // act
            var ex1 = Assert.Throws<IndexOutOfRangeException>(() => matrix[10, 0]);
            var ex2 = Assert.Throws<IndexOutOfRangeException>(() => matrix[0, 10]);

            // assert            
            Assert.IsType<IndexOutOfRangeException>(ex1);
            Assert.IsType<IndexOutOfRangeException>(ex2);
        }

        [Fact]
        public void CompareRank()
        {
            // arrange
            var matrix1 = new Matrix<int>(new[] { "1 2", "3 4", "5 6" });
            var matrix2 = new Matrix<int>(new[] { "1 2 3", "3 4 3", "5 6 3" });
            var matrix3 = new Matrix<int>(new[] { "2 1", "4 3" });

            // assert
            Assert.False(matrix1.DimensionEqual(matrix2));
            Assert.False(matrix1.DimensionEqual(matrix3));
            Assert.True(matrix1.DimensionEqual(matrix1));
        }

        [Fact]
        public void CanTranspose()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 2", "3 4", "5 6" });

            // act
            var transpose = matrix.Transpose();

            // assert
            Assert.Equal(1, transpose.Value[0, 0]);
            Assert.Equal(3, transpose.Value[0, 1]);
            Assert.Equal(5, transpose.Value[0, 2]);

            Assert.Equal(2, transpose.Rows);
            Assert.Equal(3, transpose.Cols);
        }

        [Fact]
        public void CanRectangleTranspose()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 2 3", "-4 5 6", "7 8 9" });

            // act
            var transpose = matrix.Transpose();

            // assert
            Assert.Equal(1, transpose.Value[0, 0]);
            Assert.Equal(-4, transpose.Value[0, 1]);
            Assert.Equal(7, transpose.Value[0, 2]);
        }

        #region Operations

        [Fact]
        public void TestNonMatchedAdd()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 1 1", "1 1 1", "1 1 1" });
            var matrix2 = new Matrix<int>(new[] { "1 2 3 4", "5 6 7 8" });

            // act
            Action act = () => matrix.Add(matrix2);

            // assert
            Exception ex = Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void TestAdd()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 2 3", "3 4 6" });
            var matrix2 = new Matrix<int>(new[] { "2 2 2", "1 2 3" });

            // act
            var result = matrix.Add(matrix2);

            // assert
            Assert.Equal(result.Value[0, 0], 1 + 2);
            Assert.Equal(result.Value[0, 1], 2 + 2);
            Assert.Equal(result.Value[0, 2], 3 + 2);

            Assert.Equal(result.Value[1, 0], 3 + 1);
            Assert.Equal(result.Value[1, 1], 4 + 2);
            Assert.Equal(result.Value[1, 2], 6 + 3);
        }

        [Fact]
        public void TestAddOperator()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 2 3", "3 4 6" });
            var matrix2 = new Matrix<int>(new[] { "2 2 2", "1 2 3" });

            // act
            var result = matrix + matrix2;

            // assert
            Assert.Equal(result.Value[0, 0], 1 + 2);
            Assert.Equal(result.Value[0, 1], 2 + 2);
            Assert.Equal(result.Value[0, 2], 3 + 2);

            Assert.Equal(result.Value[1, 0], 3 + 1);
            Assert.Equal(result.Value[1, 1], 4 + 2);
            Assert.Equal(result.Value[1, 2], 6 + 3);
        }

        [Fact]
        public void TestNonMatchedSub()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 1 1", "1 1 1", "1 1 1" });
            var matrix2 = new Matrix<int>(new[] { "1 2 3 4", "5 6 7 8" });

            // act
            Action act = () => matrix.Sub(matrix2);

            // assert
            Exception ex = Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void TestSub()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "9 8 7", "6 5 4" });
            var matrix2 = new Matrix<int>(new[] { "3 2 1", "10 20 30" });

            // act

            var result = matrix.Sub(matrix2);

            // assert
            Assert.Equal(result.Value[0, 0], 9 - 3);
            Assert.Equal(result.Value[0, 1], 8 - 2);
            Assert.Equal(result.Value[0, 2], 7 - 1);

            Assert.Equal(result.Value[1, 0], 6 - 10);
            Assert.Equal(result.Value[1, 1], 5 - 20);
            Assert.Equal(result.Value[1, 2], 4 - 30);
        }

        [Fact]
        public void TestSubOperator()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "9 8 7", "6 5 4" });
            var matrix2 = new Matrix<int>(new[] { "3 2 1", "10 20 30" });

            // act

            var result = matrix - matrix2;

            // assert
            Assert.Equal(result.Value[0, 0], 9 - 3);
            Assert.Equal(result.Value[0, 1], 8 - 2);
            Assert.Equal(result.Value[0, 2], 7 - 1);

            Assert.Equal(result.Value[1, 0], 6 - 10);
            Assert.Equal(result.Value[1, 1], 5 - 20);
            Assert.Equal(result.Value[1, 2], 4 - 30);
        }

        [Fact]
        public void TestNonMatchedMultiply()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 1 1", "1 1 1", "1 1 1" });
            var matrix2 = new Matrix<int>(new[] { "1 2 3 4", "5 6 7 8" });

            // act
            Action act = () => matrix.Multiple(matrix2);

            // assert
            Exception ex = Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void TestMultiply()
        {
            // arrange
            var matrix = new Matrix<int>(new[] 
            {   "10 20", 
                "30 40", 
                "50 60" 
            });
            var matrix2 = new Matrix<int>(new[] 
            { 
                "1 2 3 4", 
                "5 6 7 8" 
            });

            // act
            var result = matrix.Multiple(matrix2);

            // assert
            Assert.Equal(result.Value[0, 0], 10 * 1 + 20 * 5);
            Assert.Equal(result.Value[0, 1], 10 * 2 + 20 * 6);
            Assert.Equal(result.Value[0, 2], 10 * 3 + 20 * 7);
            Assert.Equal(result.Value[0, 3], 10 * 4 + 20 * 8);

            Assert.Equal(result.Value[1, 0], 30 * 1 + 40 * 5);
            Assert.Equal(result.Value[1, 1], 30 * 2 + 40 * 6);
            Assert.Equal(result.Value[1, 2], 30 * 3 + 40 * 7);
            Assert.Equal(result.Value[1, 3], 30 * 4 + 40 * 8);

            Assert.Equal(result.Value[2, 0], 50 * 1 + 60 * 5);
            Assert.Equal(result.Value[2, 1], 50 * 2 + 60 * 6);
            Assert.Equal(result.Value[2, 2], 50 * 3 + 60 * 7);
            Assert.Equal(result.Value[2, 3], 50 * 4 + 60 * 8);
        }

        [Fact]
        public void TestMultiplyOperator()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 2", "3 4", "5 6" });
            var matrix2 = new Matrix<int>(new[] { "1 2 3 4", "5 6 7 8" });

            // act
            var result = matrix * matrix2;

            // assert
            Assert.Equal(result.Value[0, 0], 1 * 1 + 2 * 5);
            Assert.Equal(result.Value[0, 1], 1 * 2 + 2 * 6);
            Assert.Equal(result.Value[0, 2], 1 * 3 + 2 * 7);
            Assert.Equal(result.Value[0, 3], 1 * 4 + 2 * 8);

            Assert.Equal(result.Value[1, 0], 3 * 1 + 4 * 5);
            Assert.Equal(result.Value[1, 1], 3 * 2 + 4 * 6);
            Assert.Equal(result.Value[1, 2], 3 * 3 + 4 * 7);
            Assert.Equal(result.Value[1, 3], 3 * 4 + 4 * 8);

            Assert.Equal(result.Value[2, 0], 5 * 1 + 6 * 5);
            Assert.Equal(result.Value[2, 1], 5 * 2 + 6 * 6);
            Assert.Equal(result.Value[2, 2], 5 * 3 + 6 * 7);
            Assert.Equal(result.Value[2, 3], 5 * 4 + 6 * 8);
        }

        [Fact]
        public void TestMultiplyOperator_By_Number()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 2 3", "4 5 6", "7 8 9" });

            // act
            var result1 = matrix * 5;
            var result2 = 3 * matrix;

            // assert
            // result1 -> matrix * 5
            Assert.Equal(result1.Value[0, 0], 1 * 5);
            Assert.Equal(result1.Value[0, 1], 2 * 5);
            Assert.Equal(result1.Value[0, 2], 3 * 5);

            Assert.Equal(result1.Value[1, 0], 4 * 5);
            Assert.Equal(result1.Value[1, 1], 5 * 5);
            Assert.Equal(result1.Value[1, 2], 6 * 5);

            Assert.Equal(result1.Value[2, 0], 7 * 5);
            Assert.Equal(result1.Value[2, 1], 8 * 5);
            Assert.Equal(result1.Value[2, 2], 9 * 5);

            // result2 -> 3 * matrix
            Assert.Equal(result2.Value[0, 0], 1 * 3);
            Assert.Equal(result2.Value[0, 1], 2 * 3);
            Assert.Equal(result2.Value[0, 2], 3 * 3);

            Assert.Equal(result2.Value[1, 0], 4 * 3);
            Assert.Equal(result2.Value[1, 1], 5 * 3);
            Assert.Equal(result2.Value[1, 2], 6 * 3);

            Assert.Equal(result2.Value[2, 0], 7 * 3);
            Assert.Equal(result2.Value[2, 1], 8 * 3);
            Assert.Equal(result2.Value[2, 2], 9 * 3);

        }
        #endregion

        #region Testing comparison operations
        [Fact]
        public void Test_EqualMatrix()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 2", "3 4", "5 6" });
            var matrix2 = new Matrix<int>(new[] { "1 2", "3 4", "5 6" });

            // act
            var result1 = matrix == matrix2;
            var result2 = matrix.Equals(matrix2);

            // assert
            Assert.True(result1);
            Assert.True(result2);
        }

        [Fact]
        public void Test_NotEqual_Different_Dimensions_Matrix()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 2", "3 4", "5 6" });
            var matrix2 = new Matrix<int>(new[] { "1 2 3 4", "5 6 7 8" });

            // act
            var result1 = matrix == matrix2;
            var result2 = matrix.Equals(matrix2);

            // assert
            Assert.False(result1);
            Assert.False(result2);
        }

        [Fact]
        public void Test_NotEqual_Different_Values_Matrix()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 2", "3 4", "5 6" });
            var matrix2 = new Matrix<int>(new[] { "3 4", "5 6", "7 8" });

            // act
            var result1 = matrix == matrix2;
            var result2 = matrix.Equals(matrix2);
            var result3 = matrix != matrix2;

            // assert
            Assert.False(result1);
            Assert.False(result2);
            Assert.True(result3);
        }

        [Fact]
        public void Test_NotEqual_Different_CellTypes_Matrix()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 2", "3 4", "5 6" });
            var matrix2 = new Matrix<double>(new[] { "1 2", "3 4", "5 6" });

            // act
            var result = matrix.Equals(matrix2);

            // assert
            Assert.False(result);
        }
        #endregion
        
        #region Is symmetric property
        [Fact]
        public void Is_Matrix_Symmetric()
        {
            // arrange
            var matrix = new Matrix<int>(new[] 
            { 
                "1 2 3", 
                "2 1 2", 
                "3 2 1" 
            });

            // act
            // assert
            Assert.True(matrix.IsSymmetric);
        }

        [Fact]
        public void Is_Matrix_Not_Symmetric()
        {
            // arrange
            var matrix1 = new Matrix<int>(new[]
            {
                "1 2 3",
                "1 2 3",
                "1 2 3"
            });

            var matrix2 = new Matrix<int>(new[]
            {
                "1 2 3",
                "1 2 3"
            });

            var matrix3 = new Matrix<int>(new[]
            {
                "1 2",
                "1 2",
                "1 2"
            });

            // act
            // assert
            Assert.False(matrix1.IsSymmetric);
            Assert.False(matrix2.IsSymmetric);
            Assert.False(matrix3.IsSymmetric);
        }
        #endregion

        #region Identity matrix
        [Fact]
        public void Test_CreateIdentityMatrix()
        {
            // arrange
            var etalonInt = new Matrix<int>(new[] 
            { 
                "1 0 0", 
                "0 1 0", 
                "0 0 1" });

            var etalonDouble = new Matrix<double>(new[]
            {
                "1 0 0",
                "0 1 0",
                "0 0 1" });

            // act
            var matrixInt = Matrix<int>.I(3);
            var matrixDouble = Matrix<double>.I(3);

            // assert
            Assert.Equal(etalonInt, matrixInt);
            Assert.Equal(3, matrixInt.Rows);
            Assert.Equal(3, matrixInt.Cols);

            Assert.Equal(etalonDouble, matrixDouble);
            Assert.Equal(3, matrixDouble.Rows);
            Assert.Equal(3, matrixDouble.Cols);
        }

        [Fact]
        // When A is m×n, it is a property of matrix multiplication that In*A = A
        public void Test_Multiplication_By_IdentityMatrix()
        {
            // arrange
            var etalon = new Matrix<int>(new[]
            {
                "1 2 3",
                "4 5 6",
                "7 8 9" });

            // act
            var matrix = Matrix<int>.I(3);
            var mulResult = etalon * matrix;

            // assert
            Assert.Equal(etalon, mulResult);
        }
        #endregion

        #region Index test
        [Fact]
        public void Can_Change_Value_InCell()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 2 3", "4 5 6", "7 8 9" })
            {
                // act
                [1, 1] = 10
            };

            // assert
            Assert.Equal(new Matrix<int>(new[] { "1 2 3", "4 10 6", "7 8 9" }), matrix);
        }

        [Fact]
        public void Can_Change_Value_InCell_NotSameType()
        {
            // arrange
            var matrix = new Matrix<double>(new[] { "1 2 3", "4 5 6", "7 8 9" })
            {
                // act
                [1, 1] = 10
            };

            // assert
            Assert.Equal(new Matrix<double>(new[] { "1 2 3", "4 10 6", "7 8 9" }), matrix);
        }
        #endregion

        #region ColumnReplaceTests
        [Fact]
        public void ThrowError_If_VectorLength_NotEqual_MatrixRow()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 1 1", "1 1 1", "1 1 1" });
            var vector = new Vector<int>("1 2 3 4");

            // act
            Action act = () => matrix.ReplaceColumn(1, vector);

            // assert
            Exception ex = Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void Replace_MatrixColumn_ByVector()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 1 1", "1 1 1", "1 1 1" });
            var vector = new Vector<int>("2 2 2");

            // act
            matrix.ReplaceColumn(1, vector);

            // assert
            var etalon = new Matrix<int>(new[] { "1 2 1", "1 2 1", "1 2 1" });
            Assert.Equal(etalon, matrix);
        }
        #endregion

        #region RowReplaceTests
        [Fact]
        public void ThrowError_If_VectorLength_NotEqual_MatrixColumn()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 1 1", "1 1 1", "1 1 1" });
            var vector = new Vector<int>("1 2 3 4");

            // act
            Action act = () => matrix.ReplaceRow(1, vector);

            // assert
            Exception ex = Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void Replace_MatrixRow_ByVector()
        {
            // arrange
            var matrix = new Matrix<int>(new[] { "1 1 1", "1 1 1", "1 1 1" });
            var vector = new Vector<int>("2 2 2");

            // act
            matrix.ReplaceRow(1, vector);

            // assert
            var etalon = new Matrix<int>(new[] { "1 1 1", "2 2 2", "1 1 1" });
            Assert.Equal(etalon, matrix);
        }
        #endregion

        [Fact]
        public void RounDouble_ToInt_Matrix()
        {
            // arrange
            var matrix = new Matrix<double>(new[] { "1.4 1.45 1.5", "10.01 10.99 10.50", "20.001 20.445 20.444" });

            // act
            var result = MatrixOperations.Round<double, int>(matrix, 0);

            // assert
            var etalon = new Matrix<int>(new[] { "1 1 2", "10 11 10", "20 20 20" });
            Assert.Equal(etalon, result);
        }

        [Fact]
        public void RounDouble_ToDouble_Matrix()
        {
            // arrange
            var matrix = new Matrix<double>(new[] { "1.44 1.45 1.51", "10.01 10.99 10.50", "20.001 20.445 20.444" });

            // act
            var result = MatrixOperations.Round<double, double>(matrix, 1, MidpointRounding.AwayFromZero);

            // assert
            var etalon = new Matrix<double>(new[] { "1.4 1.5 1.5", "10.0 11.0 10.5", "20.0 20.4 20.4" });
            Assert.Equal(etalon, result);
        }

    }
}