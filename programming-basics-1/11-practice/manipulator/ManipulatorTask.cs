using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        static double GetLength(double x1, double y1, double x2, double y2)
        {
            var deltaX = x2 - x1;
            var deltaY = y2 - y1;
            return Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public static bool ChechCondition(double x, double y)
        {
            var r0 = Manipulator.Palm / 2;
            var r1 = Manipulator.UpperArm + Manipulator.Forearm;
            var vectorX = x - Manipulator.Palm;
            var vectorY = y;
            var vectorLength = Math.Sqrt(vectorX * vectorX + vectorY * vectorY);
            return r0 <= vectorLength && vectorLength <= r1;
        }

        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            var forearmEndX = x - Manipulator.Palm * Math.Cos(alpha);
            var forearmEndY = y + Manipulator.Palm * Math.Sin(alpha);
            var ac = GetLength(0, 0, forearmEndX, forearmEndY);
            var shoulder = TriangleTask.GetABAngle(Manipulator.UpperArm, ac, Manipulator.Forearm) +
                Math.Atan2(forearmEndY, forearmEndX);
            var elbow = TriangleTask.GetABAngle(Manipulator.UpperArm, Manipulator.Forearm, ac);
            var wrist = -alpha - shoulder - elbow;
            return new[] { shoulder, elbow, wrist };
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        static Random random = new Random();

        [Test]
        public void TestMoveManipulatorTo()
        {
            for (var i = 0; i < 1000; i++)
            {
                var x = -399 + random.NextDouble() * 799;
                var y = -299 + random.NextDouble() * 599;
                var result = ManipulatorTask.MoveManipulatorTo(x, y, 0);
                if (ManipulatorTask.ChechCondition(x, y))
                    Assert.That(!double.IsNaN(result[0]));
                else
                    Assert.That(double.IsNaN(result[0]));
            }
        }
    }
}