using System;
using System.Linq;
using System.Text;

namespace GaussAlgorithm;

public class Solver {
    public double[] Solve(double[][] matrix, double[] freeMembers) {
        var system = new EquationsSystem(matrix, freeMembers);
        return system.GetSolutions();
    }
}

public class EquationsSystem {
    private readonly double[][] Matrix;
    private readonly bool[] IsUsedRow;
    private readonly int VariableCount;
    private readonly double Delta;

    public EquationsSystem(double[][] coefficients, double[] freeMembers, double delta) {
        VariableCount = coefficients.Max(c => c.Length);
        Matrix = BuildMatrix(coefficients, freeMembers);
        IsUsedRow = Enumerable.Repeat(false, Matrix.Length).ToArray();
        Delta = delta;
    }

    public EquationsSystem(double[][] coefficients, double[] freeMembers)
        : this(coefficients, freeMembers, 1e-3) {
    }

    private double[][] BuildMatrix(double[][] coefficients, double[] freeMembers) {
        var matrix = new double[coefficients.Length][];
        for (var i = 0; i < matrix.Length; i++) {
            matrix[i] = new double[VariableCount + 1];
            coefficients[i].CopyTo(matrix[i], 0);
            matrix[i][VariableCount] = freeMembers[i];
        }
        return matrix;
    }

    private int FindUnusedRowWithNoZeroAtColumn(int column) {
        if (column > VariableCount) throw new Exception();
        for (var row = 0; row < Matrix.Length; row++) {
            var isFit = !IsUsedRow[row] && Math.Abs(Matrix[row][column]) > Delta;
            if (isFit) return row;
        }
        return -1;
    }

    private void SubstractRowToZeroAtOtherColumns(int row, int column) {
        if (column > VariableCount) throw new Exception();
        IsUsedRow[row] = true;
        for (var currentRow = 0; currentRow < Matrix.Length; currentRow++) {
            if (currentRow == row) continue;
            var multiplier = Matrix[currentRow][column] / Matrix[row][column];
            SubstractRowAt(currentRow, row, multiplier);
        }
    }

    private void SubstractRowAt(int row, int rowToSubstract, double rowMultiplier) {
        for (var i = 0; i < Matrix[row].Length; i++) {
            Matrix[row][i] -= Matrix[rowToSubstract][i] * rowMultiplier;
            if (Math.Abs(Matrix[row][i]) < Delta) Matrix[row][i] = 0;
        }
    }

    private void ToJordanForm() {
        for (var column = 0; column < VariableCount; column++) {
            var row = FindUnusedRowWithNoZeroAtColumn(column);
            if (row == -1) continue;
            SubstractRowToZeroAtOtherColumns(row, column);
        }
    }

    private void CheckCorrectness() {
        foreach (var row in Matrix) {
            var IsOnlyZeros = row.Take(VariableCount).All(e => Math.Abs(e) < Delta);
            var IsZeroAnswer = Math.Abs(row[VariableCount]) <= 0;
            if (IsOnlyZeros && !IsZeroAnswer)
                throw new NoSolutionException("no solutions");
        }
    }

    public double[] GetSolutions() {
        ToJordanForm();
        if (Matrix.Length > 1) CheckCorrectness();
        var solutions = new double[VariableCount];
        for (var row = 0; row < Matrix.Length; row++) {
            for (var column = 0; column < VariableCount; column++) {
                if (Math.Abs(Matrix[row][column]) > Delta) {
                    solutions[column] = Matrix[row][VariableCount] / Matrix[row][column];
                    break;
                }
            }
        }
        return solutions;
    }
}