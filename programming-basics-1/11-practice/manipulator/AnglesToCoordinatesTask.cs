using System;
using Avalonia;
using NUnit.Framework;

namespace Manipulation;

public static class AnglesToCoordinatesTask {
    public static Point[] GetJointPositions(double shoulderAngle, double elbowAngle, double wristAngle) {
        var angle1 = shoulderAngle;
        var angle2 = shoulderAngle - Math.PI + elbowAngle;
        var angle3 = shoulderAngle + elbowAngle + wristAngle;
        var elbow = new Joint(new Point(0, 0), Manipulator.UpperArm, angle1);
        var wrist = new Joint(elbow.End, Manipulator.Forearm, angle2);
        var palm = new Joint(wrist.End, Manipulator.Palm, angle3);
        return new[] { elbow.End, wrist.End, palm.End };
    }
}

public class Joint {
    public readonly Point Start;
    public readonly Point End;
    public readonly double Length;
   
    public Joint(Point start, double length, double angle) {
        Start = start;
        Length = length;
        var endX = start.X + length * Math.Cos(angle);
        var endY = start.Y + length * Math.Sin(angle);
        End = new Point(endX, endY);
    }
}

[TestFixture]
public class AnglesToCoordinatesTask_Tests {
    [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
    [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
    [TestCase(Math.PI / 2, Math.PI, Math.PI, 0, Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm)]
    [TestCase(0, Math.PI, Math.PI, Manipulator.UpperArm + Manipulator.Forearm + Manipulator.Palm, 0)]
    [TestCase(Math.PI / 2, 3 * Math.PI / 4, 2 * Math.PI / 3, 142.8083633f, 219.323671f)]
    public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY) {
        var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
        Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
        Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
    }
}