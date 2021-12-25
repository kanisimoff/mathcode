using System.Numerics;
using Elementary = MathCode.Elementary;

namespace MathCode.Probability
{
    /// <summary>
    /// Permutation a set is, loosely speaking, an arrangement of its members into a sequence or linear order, 
    /// or if the set is already ordered, a rearrangement of its elements. The word "permutation" also refers 
    /// to the act or process of changing the linear order of an ordered set.
    /// </summary>
    public static class Permutation
    {
        /// <summary>
        /// Partial permutation, or sequence without repetition, on a finite set S is a bijection between 
        /// two specified subsets of S. That is, it is defined by two subsets U and V of equal size, 
        /// and a one-to-one mapping from U to V. 
        /// </summary>
        /// <param name="k">Different ordered arrangements of k-element subset of an n-set</param>
        /// <param name="n">A given set of size n</param>
        /// <returns> The number of such k-permutations of n-set</returns>
        public static dynamic P(int k, int n)
        {
            return Enumerable.Range(n-k+1, k).Aggregate(1, (p, item) => p * item);
        } 
    }
}