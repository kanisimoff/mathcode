using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCode.LinearAlgebra
{
    public static class Elimination
    {
        /// <summary>
        /// Gauss–Jordan elimination
        /// </summary>
        /// <typeparam name="T">Matrix element value type</typeparam>
        /// <param name="matrix">Equation matrix</param>
        /// <param name="variableSet">Set of variables of linear equation</param>
        /// <returns>Tuple witn Inverse matrix and a Solution to a linear system is an assignment of values to the variables
        /// such that all the equations are simultaneously satisfied</returns>
        /// <exception cref="Exception"></exception>
        public static (Matrix<T> Inverse, Matrix<T> LinearSource) GaussJordan<T>(Matrix<T> matrix, Matrix<T> variableSet) where T : struct
        {
            var aMatrix = new Matrix<T>(matrix.Value);
            var bMatrix = new Matrix<T>(variableSet.Value);
            int icol = 0, irow = 0;
            Vector<int> indxc = new Vector<int>(aMatrix.Rows, 0);
            Vector<int> indxr = new Vector<int>(aMatrix.Rows, 0);
            Vector<int> ipiv = new Vector<int>(aMatrix.Rows, 0);

            for (var i = 0; i < aMatrix.Rows; i++)
            {
                var big = 0.0;
                for (var j = 0; j < aMatrix.Rows; j++)
                    if (ipiv[j] != 1)
                        for (var k = 0; k < aMatrix.Rows; k++)
                        {
                            if (ipiv[k] == 0)
                            {
                                dynamic cell = aMatrix[j, k];
                                if (Math.Abs(cell) >= big)
                                {
                                    big = Math.Abs(cell);
                                    irow = j;
                                    icol = k;
                                }
                            }
                        }
                ++(ipiv[icol]);
                if (irow != icol)
                {
                    for (var l = 0; l < aMatrix.Rows; l++) (aMatrix[irow, l], aMatrix[icol, l]) = (aMatrix[icol, l], aMatrix[irow, l]); //SWAP(a[irow,l], a[icol,l]);
                    for (var l = 0; l < bMatrix.Cols; l++) (bMatrix[irow,l], bMatrix[icol,l]) = (bMatrix[icol, l], bMatrix[irow, l]);
                }
                indxr[i] = irow;
                indxc[i] = icol;
                dynamic diagCell = aMatrix[icol, icol];
                if (diagCell == 0) throw new Exception("gaussj: Singular Matrix");
                dynamic pivinv = 1.0 / diagCell;
                aMatrix[icol,icol] = (T)Convert.ChangeType(1, typeof(T));
                for (var l = 0; l < aMatrix.Rows; l++) aMatrix[icol,l] *= pivinv;
                for (var l = 0; l < bMatrix.Cols; l++) bMatrix[icol,l] *= pivinv;

                for (var ll = 0; ll < aMatrix.Rows; ll++)
                    if (ll != icol)
                    {
                        dynamic dum = aMatrix[ll,icol];
                        aMatrix[ll,icol] = (T)Convert.ChangeType(0, typeof(T));
                        for (var l = 0; l < aMatrix.Rows; l++) aMatrix[ll,l] -= aMatrix[icol,l] * dum;
                        for (var l = 0; l < bMatrix.Cols; l++) bMatrix[ll,l] -= bMatrix[icol,l] * dum;
                    }
            }
            for (var l = aMatrix.Rows - 1; l >= 0; l--)
            {
                if (indxr[l] != indxc[l])
                    for (var k = 0; k < aMatrix.Rows; k++)
                        (aMatrix[k,indxr[l]], aMatrix[k,indxc[l]]) = (aMatrix[k, indxc[l]], aMatrix[k, indxr[l]]);
            }

            return (aMatrix, bMatrix);
        }

        /// <summary>
        /// Inverse matrix by Gauss–Jordan elimination
        /// </summary>
        /// <typeparam name="T">Matrix element value type</typeparam>
        /// <param name="matrix">Equation matrix</param>
        /// <returns>Inverse matrix</returns>
        public static Matrix<T> GaussJordan<T>(Matrix<T> matrix) where T : struct
        {
            var matrixB = Matrix<T>.I(matrix.Rows);
            return GaussJordan(matrix, matrixB).Inverse;
        }
    }
}
