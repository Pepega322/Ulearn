using System;

namespace Recognizer {
    internal static class SobelFilterTask {
        public static double[,] SobelFilter(double[,] original, double[,] filter) {
            var image = new Matrix(original);
            var fileteredImage = image.MakeConvolution(new Matrix(filter));
            return fileteredImage.Data;
        }
    }

    public class Matrix {
        public readonly double[,] Data;
        public readonly int Width;
        public readonly int Heigth;

        public Matrix(double[,] data) {
            Data = data;
            Width = data.GetLength(0);
            Heigth = data.GetLength(1);
        }

        public Matrix Transpose() {
            var t = new double[Heigth, Width];
            for (var i = 0; i < Heigth; i++)
                for (var j = 0; j < Width; j++)
                    t[i, j] = Data[j, i];
            return new Matrix(t);
        }

        public Matrix MakeConvolution(Matrix conv) {
            var center = conv.Width / 2;
            var sobelT = conv.Transpose();
            var filtered = new double[Width, Heigth];
            for (int x = center; x < Width - center; x++)
                for (int y = center; y < Heigth - center; y++) {
                    var gx = MakeСonvolutionAt(conv, x, y);
                    var gy = MakeСonvolutionAt(sobelT, x, y);
                    filtered[x, y] = Math.Sqrt(gx * gx + gy * gy);
                }
            return new Matrix(filtered);
        }

        private double MakeСonvolutionAt(Matrix convolution, int x, int y) {
            double result = 0;
            var center = convolution.Width / 2;
            var xi = 0;
            var yi = 0;
            for (var i = x - center; i <= x + center; i++) {
                for (var j = y - center; j <= y + center; j++) {
                    result += Data[i, j] * convolution.Data[xi, yi];
                    yi++;
                }
                xi++;
                yi = 0;
            }
            return result;
        }
    }
}