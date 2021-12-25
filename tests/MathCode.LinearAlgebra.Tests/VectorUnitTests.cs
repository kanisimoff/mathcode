using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MathCode.LinearAlgebra.Tests
{
    public class VectorUnitTests
    {
        [Fact]
        public void ThrowException_If_String_Empty()
        {
            // arrange
            string line = string.Empty;

            // act
            Action act = () => new Vector<int>(line);

            // assert
            Exception ex = Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void Init_Vector_From_Valid_String()
        {
            // arrange
            string line = "1 2 3";

            // act
            var vector = new Vector<int>(line);

            // assert
            Assert.Equal(3, vector.Length);
            Assert.Equal(1, vector.ToArray()[0]);
            Assert.Equal(2, vector.ToArray()[1]);
            Assert.Equal(3, vector.ToArray()[2]);
        }

        [Fact]
        public void DoubleVector_From_Valid_String()
        {
            // arrange
            string line = "1,1 2 3";

            // act
            var vector = new Vector<double>(line, CultureInfo.CurrentCulture);

            // assert
            Assert.Equal(3, vector.Length);
            Assert.Equal(1.1, vector.ToArray()[0]);
            Assert.Equal(2, vector.ToArray()[1]);
            Assert.Equal(3, vector.ToArray()[2]);
        }

        [Fact]
        public void DoubleVector_From_Valid_String_With_IvariantCulture()
        {
            // arrange
            string line = "1.1 2 3";

            // act
            var vector = new Vector<double>(line);

            // assert
            Assert.Equal(3, vector.Length);
            Assert.Equal(1.1, vector.ToArray()[0]);
            Assert.Equal(2, vector.ToArray()[1]);
            Assert.Equal(3, vector.ToArray()[2]);
        }

        [Fact]
        public void ThrowException_If_Line_Contain_NonDigital_Value()
        {
            // arrange
            var line = "1 3 45 a";

            // act
            Action act = () => new Vector<int>(line);

            // assert
            Exception ex = Assert.Throws<FormatException>(act);
        }


        [Fact]
        public void Init_Vector_By_Size_And_Value()
        {
            // arrange
            var length = 10;
            var value = 3;

            // act
            var vector = new Vector<int>(length, value);

            // assert
            Assert.Equal(length, vector.Length);
            for (int i = 0; i < length; i++)
            {
                Assert.Equal(value, vector.ToArray()[i]);
            }
        }

        [Fact]
        public void Get_Value_By_Index()
        {
            // arrange
            var line = "1 2 3 4 5";
            var vector = new Vector<int>(line);

            // act
            var indexValue = vector[3];

            // assert
            Assert.Equal(4, indexValue);
        }

        [Fact]
        public void ThrowException_If_IndexOutOfRange()
        {
            // arrange
            var line = "1 2 3 4 5";
            var vector = new Vector<int>(line);

            // act
            var ex = Assert.Throws<IndexOutOfRangeException>(() => vector[10]);

            // assert            
            Assert.IsType<IndexOutOfRangeException>(ex);
        }

        [Fact]
        public void TestNonMatchedAdd()
        {
            // arrange
            var vector1 = new Vector<int>("1 2 3");
            var vector2 = new Vector<int>("10 20 30 40");

            // act
            var ex = Assert.Throws<ArgumentException>(() => vector1 + vector2);

            // assert
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void TestAdd()
        {
            // arrange
            var vector1 = new Vector<int>(new[] { 1, 2, 3 });
            var vector2 = new Vector<int>(new[] { 10, 20, 30 });

            // act
            var result = vector1 + vector2;

            // assert
            for (int i = 0; i < result.Length; i++)
                Assert.Equal(result[i], vector1[i] + vector2[i]);
        }

        [Fact]
        public void TestNonMatchedSub()
        {
            // arrange
            var vector1 = new Vector<int>("1 2 3");
            var vector2 = new Vector<int>("10 20 30 40");

            // act
            var ex = Assert.Throws<ArgumentException>(() => vector1 - vector2);

            // assert
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void TestSub()
        {
            // arrange
            var vector1 = new Vector<int>(new[] { 10, 20, 30 });
            var vector2 = new Vector<int>(new[] { 1, 2, 3 });

            // act
            var result = vector1 - vector2;

            // assert
            for (int i = 0; i < result.Length; i++)
                Assert.Equal(result[i], vector1[i] - vector2[i]);
        }

        [Fact]
        public void TestNonMatchedScalarMul()
        {
            // arrange
            var vector1 = new Vector<int>("1 2 3");
            var vector2 = new Vector<int>("10 20 30 40");

            // act
            var ex = Assert.Throws<ArgumentException>(() => vector1 * vector2);

            // assert
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public void TestScalarMul()
        {
            // arrange
            var vector1 = new Vector<int>(new[] { 1, 3, -5 });
            var vector2 = new Vector<int>(new[] { 4, -2, -1 });

            // act
            var result = vector1 * vector2;

            // assert
            Assert.Equal(3, result);
        }

        [Fact]
        public void TestMul_Vector_By_Number()
        {
            // arrange
            var vector = new Vector<int>(new[] { 1, 3, -5 });
            var num = 5;

            // act
            var result1 = vector * num;
            var result2 = num * vector;

            // assert
            Assert.Equal(5, result1[0]);
            Assert.Equal(15, result1[1]);
            Assert.Equal(-25, result1[2]);

            Assert.Equal(5, result2[0]);
            Assert.Equal(15, result2[1]);
            Assert.Equal(-25, result2[2]);
        }

        [Fact]
        public void Test_ColumnsToVectors()
        {
            // arrange
            var matrix = new Matrix<int>(new Vector<int>[] 
            { 
                new(new [] { 1, 3, -5 }),
                new(new [] { 4, 6, 8 }) 
            });

            // act
            var vectors = Vector<int>.ColumnsToVectors(matrix).ToList();

            // assert
            Assert.Equal(3, vectors.Count);

            Assert.Equal(1, vectors[0][0]);
            Assert.Equal(4, vectors[0][1]);

            Assert.Equal(3, vectors[1][0]);
            Assert.Equal(6, vectors[1][1]);

            Assert.Equal(-5, vectors[2][0]);
            Assert.Equal(8, vectors[2][1]);
        }

        [Fact]
        public void Test_RowsToVectors()
        {
            // arrange
            var matrix = new Matrix<int>(new Vector<int>[]
            {
                new(new [] { 1, 3, -5 }),
                new(new [] { 4, 6, 8 })
            });

            // act
            var vectors = Vector<int>.RowsToVectors(matrix).ToList();

            // assert
            Assert.Equal(2, vectors.Count);

            Assert.Equal(1, vectors[0][0]);
            Assert.Equal(3, vectors[0][1]);
            Assert.Equal(-5, vectors[0][2]);

            Assert.Equal(4, vectors[1][0]);
            Assert.Equal(6, vectors[1][1]);
            Assert.Equal(8, vectors[1][2]);
        }

        #region Testing comparison operations
        [Fact]
        public void Test_Equal()
        {
            // arrange
            var vector1 = new Vector<int>("1 2 3");
            var vector2 = new Vector<int>("1 2 3");

            // act
            var result1 = vector1 == vector2;
            var result2 = vector1.Equals(vector2);

            // assert
            Assert.True(result1);
            Assert.True(result2);
        }

        [Fact]
        public void Test_NotEqual_Different_Dimensions()
        {
            // arrange
            var vector1 = new Vector<int>("1 2 3 4 5 6");
            var vector2 = new Vector<int>("1 2 3 4");

            // act
            var result1 = vector1 == vector2;
            var result2 = vector1.Equals(vector2);

            // assert
            Assert.False(result1);
            Assert.False(result2);
        }

        [Fact]
        public void Test_NotEqual_Different_Values()
        {
            // arrange
            var vector1 = new Vector<int>("1 2 3 4");
            var vector2 = new Vector<int>("3 4 5 6");

            // act
            var result1 = vector1 == vector2;
            var result2 = vector1.Equals(vector2);
            var result3 = vector1 != vector2;

            // assert
            Assert.False(result1);
            Assert.False(result2);
            Assert.True(result3);
        }

        [Fact]
        public void Test_NotEqual_Different_CellTypes()
        {
            // arrange
            var vector1 = new Vector<int>("1 2 3 4");
            var vector2 = new Vector<double>("1 2 3 4");

            // act
            var result = vector1.Equals(vector2);

            // assert
            Assert.False(result);
        }
        #endregion

        [Fact]
        public void Can_Change_Value_InVector()
        {
            // arrange
            var vector = new Vector<int>(new[] { 1, 2, 3 });

            // act
            vector[1] = 5;
            
            // assert
            Assert.Equal(new Vector<int>("1 5 3"), vector);
        }

        [Fact]
        public void Can_Change_Value_NotSameType()
        {
            // arrange
            var vector = new Vector<double>(new[] { 1.0, 2, 3 });

            // act
            vector[1] = 5;

            // assert
            Assert.Equal(new Vector<double>("1 5 3"), vector);
        }
    }
}
