using Xunit;

namespace MathCode.Probability.Tests
{
    public class PermutationTest
    {
        [Fact]
        public void Permutation_Value()
        {
            //arrange
            //For example, how many different permutations can you get if you take 5 cards from a deck containing 52 cards

            //act
            double permutationRes = Permutation.P(5, 52);
            
            //asset
            Assert.Equal(311875200, permutationRes);
        }
    }
}