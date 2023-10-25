using System;
using NUnit.Framework;

namespace Manipulation;

public class TriangleTask {
    public static double GetABAngle(double a, double b, double c)
        => (a < 0 || b < 0 || c < 0) ? double.NaN : Math.Acos((a * a + b * b - c * c) / (2 * a * b));
}

[TestFixture]
public class TriangleTask_Tests {
    [TestCase(3, 4, 5, Math.PI / 2)]
    [TestCase(1, 1, 1, Math.PI / 3)]
    [TestCase(0.198177329356865, 0.715455999465406, 0.793295098838068, 1.8500559680422)]
    [TestCase(3, 4, 5, Math.PI / 2)]
    [TestCase(1, 1, 1, Math.PI / 3)]
    [TestCase(1, 1, 0, 0)]
    [TestCase(2.001, 1, 1, double.NaN)]
    [TestCase(0, 1, 1, double.NaN)]
    [TestCase(1, -0.1, 1, double.NaN)]
    public void TestGetABAngle(double a, double b, double c, double expectedAngle) {
        var actualAngle = TriangleTask.GetABAngle(a, b, c);
        Assert.AreEqual(expectedAngle, actualAngle, 1e-8);
    }
}