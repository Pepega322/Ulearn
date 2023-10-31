using System;
using System.Linq;
using Avalonia;
using NUnit.Framework;

namespace Manipulation;

public static class ManipulatorTask {
    public static double[] MoveManipulatorTo(double x, double y, double alpha) {
        var forearm = new Joint1(new Point(x, y), Manipulator.Palm, Math.PI - alpha);
        var v = new Vector(forearm.End.X - 0, forearm.End.Y - 0);
        var shoulderAngle = TriangleTask.GetABAngle(Manipulator.UpperArm, v.Length, Manipulator.Forearm) +
            Math.Atan2(forearm.End.Y, forearm.End.X);
        var elbowAngle = TriangleTask.GetABAngle(Manipulator.UpperArm, Manipulator.Forearm, v.Length);
        var wristAngle = -alpha - shoulderAngle - elbowAngle;
        return new[] { shoulderAngle, elbowAngle, wristAngle };
    }
}
// опипастим дл€ удобства, чтобы можно было легко скопировать и вставить на сайте
public class Joint1 {
    public readonly Point Start;
    public readonly Point End;
    public readonly double Length;

    public Joint1(Point start, double length, double angle) {
        Start = start;
        Length = length;
        var endX = start.X + length * Math.Cos(angle);
        var endY = start.Y + length * Math.Sin(angle);
        End = new Point(endX, endY);
    }
}

[TestFixture]
public class ManipulatorTask_Tests {
    static Random random = new Random();

    public bool IsCorrect(double x, double y) {
        var min = Manipulator.Palm / 2;
        var max = Manipulator.UpperArm + Manipulator.Forearm;
        var v = new Vector(x - Manipulator.Palm, y);
        return min <= v.Length && v.Length <= max;
    }

    [Test]
    public void TestMoveManipulatorTo() {
        for (var i = 0; i < 1000; i++) {
            var x = -399 + random.NextDouble() * 799;
            var y = -299 + random.NextDouble() * 599;
            var result = ManipulatorTask.MoveManipulatorTo(x, y, 0);
            if (IsCorrect(x, y)) Assert.That(result.All(p => !double.IsNaN(p)));
            else Assert.That(result.All(p => double.IsNaN(p)));
        }
    }
}