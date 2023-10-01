using System;
using System.Drawing;
using NUnit.Framework;
using static System.Windows.Forms.AxHost;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        private static PointF GetJointPosition(float startX, float startY, float length, double angle)
        {
            var endX = startX + length * (float)Math.Cos(angle);
            var endY = startY + length * (float)Math.Sin(angle);
            return new PointF(endX, endY);
        }

        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var angle1 = shoulder;
            var angle2 = shoulder - Math.PI + elbow;
            var angle3 = shoulder + elbow + wrist;
            var elbowPos = GetJointPosition(0, 0,
                Manipulator.UpperArm, angle1);
            var wristPos = GetJointPosition(elbowPos.X, elbowPos.Y,
                Manipulator.Forearm, angle2);
            var palmEndPos = GetJointPosition(wristPos.X, wristPos.Y,
                Manipulator.Palm, angle3);
            return new PointF[] { elbowPos, wristPos, palmEndPos };
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, Math.PI, Math.PI, 0, Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm)]
        [TestCase(0, Math.PI, Math.PI, Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm, 0)]
        [TestCase(Math.PI / 2, 3 * Math.PI / 4, 2 * Math.PI / 3, 142.8083633f, 219.323671f)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
        }
    }
}