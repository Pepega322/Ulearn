using System;
using System.Diagnostics.CodeAnalysis;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] SobelFilter(double[,] original, double[,] sx)
        {
            var width = original.GetLength(0);
            var height = original.GetLength(1);
            var result = new double[width, height];

            var sy = GetTransposeMatrix(sx);
            var matrixCenterIndex = sx.GetLength(0) / 2;
            for (int x = matrixCenterIndex; x < width - matrixCenterIndex; x++)
                for (int y = matrixCenterIndex; y < height - matrixCenterIndex; y++)
                {
                    var gx = GetConvolution(original, sx, x, y);
                    var gy = GetConvolution(original, sy, x, y);
                    result[x, y] = Math.Sqrt(gx * gx + gy * gy);
                }

            return result;
        }

        private static double GetConvolution(double[,] original, double[,] convolutionMatrix, int x, int y)
        {
            double convolution = 0;
            var matrixCenterIndex = convolutionMatrix.GetLength(0) / 2;
            var xi = 0;
            var yi = 0;
            for (var i = x - matrixCenterIndex; i <= x + matrixCenterIndex; i++)
            {
                for (var j = y - matrixCenterIndex; j <= y + matrixCenterIndex; j++)
                {
                    var orig = original[i, j];
                    var kren = convolutionMatrix[xi, yi];
                    convolution += orig * kren;
                    yi++;
                }
                xi++;
                yi = 0;
            }

            return convolution;
        }

        private static double[,] GetTransposeMatrix(double[,] matrix)
        {
            var xSize = matrix.GetLength(0);
            var ySize = matrix.GetLength(1);
            var transposeMatrix = new double[ySize, xSize];

            for (var i = 0; i < xSize; i++)
                for (var j = 0; j < ySize; j++)
                    transposeMatrix[j, i] = matrix[i, j];

            return transposeMatrix;
        }
    }
}