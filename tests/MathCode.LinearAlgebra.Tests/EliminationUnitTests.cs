using System;
using Xunit;

namespace MathCode.LinearAlgebra.Tests
{
    public class EliminationUnitTests
    {
        [Fact]
        public void Inverse_Matrix_By_Gauss_Jordan()
        {
            // arrange
            var matrixA = new Matrix<double>(new[] { "1 0 5", "2 1 6", "3 4 0" });

            // act 
            // A*A^-1 = |I|
            var matrix = Elimination.GaussJordan(matrixA);
            var result = matrixA * MatrixOperations.Round<double, double>(matrix, 0, MidpointRounding.AwayFromZero);

            // assert
            Assert.Equal(Matrix<double>.I(matrixA.Rows), result);
        }

        [Fact]
        public void Calculation_Of_System_Equations_By_Gauss_Jordan()
        {
            // Example https://en.wikipedia.org/wiki/Gaussian_elimination 
            
            // arrange
            var matrixA = new Matrix<double>(new[] { "1 1 1", "4 2 1", "9 3 1" });
            var matrixB = new Matrix<double>(new[] { "0", "1", "3" });

            // act 
            var linearSolution = Elimination.GaussJordan(matrixA, matrixB).LinearSource;
            var result = MatrixOperations.Round<double, double>(linearSolution, 1, MidpointRounding.AwayFromZero);

            // assert
            Assert.Equal(new Matrix<double>(new[] { "0.5", "-0.5", "0" }), result);
        }
    }
}