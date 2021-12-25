using System;
using System.Numerics;
using Xunit;

namespace MathCode.Elementary.Tests
{
    public class FactorialFunc
    {
        [Fact]
        public void Factorial_Test()
        {
            //arrange
            //act
            var factorialResult = Math.Factorial(5);
            
            //assert
            Assert.Equal(factorialResult, (ulong) 1*2*3*4*5);
        }

        [Fact]
        public void Factorial_NegativeArg()
        {
            //arrange
            //act
            Action act = () => Math.Factorial(-5);

            //assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void Factorial_ZeroValue()
        {
            //arrange
            //act
            var factorialResult = Math.Factorial(0);

            //assert
            Assert.Equal((ulong)1, factorialResult);
        }
    }
}