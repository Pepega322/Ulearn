using System;
using System.Linq;
using Avalonia;
using Avalonia.Input;
using Avalonia.Media;

namespace Manipulation;

public static class VisualizerTask {
    public static double X = 220;
    public static double Y = -100;
    public static double Alpha = 0.05;
    public static double Wrist = 2 * Math.PI / 3;
    public static double Elbow = 3 * Math.PI / 4;
    public static double Shoulder = Math.PI / 2;

    public static Brush UnreachableAreaBrush = new SolidColorBrush(Color.FromArgb(255, 255, 230, 230));
    public static Brush ReachableAreaBrush = new SolidColorBrush(Color.FromArgb(255, 230, 255, 230));
    public static Pen ManipulatorPen = new Pen(Brushes.Black, 3);
    public static Brush JointBrush = new SolidColorBrush(Colors.Gray);

    public static void KeyDown(IDisplay display, KeyEventArgs key) {
        switch (key.Key) {
            case Key.Q:
                Shoulder += Alpha;
                break;
            case Key.A:
                Shoulder -= Alpha;
                break;
            case Key.W:
                Elbow += Alpha;
                break;
            case Key.S:
                Elbow -= Alpha;
                break;
            default:
                break;
        }
        Wrist = -Alpha - Shoulder - Elbow;
        display.InvalidateVisual();
    }

    public static void MouseMove(IDisplay display, PointerEventArgs e) {
        var shoulderPos = GetShoulderPos(display);
        var mathLocation = ConvertWindowToMath(e.GetPosition(display), shoulderPos);
        X = mathLocation.X;
        Y = mathLocation.Y;
        UpdateManipulator();
        display.InvalidateVisual();
    }

    public static void MouseWheel(IDisplay display, PointerWheelEventArgs e) {
        if (e.Delta.Y > 0) Alpha += 0.05;
        else Alpha -= 0.05;
        UpdateManipulator();
        display.InvalidateVisual();
    }

    public static void UpdateManipulator() {
        var position = ManipulatorTask.MoveManipulatorTo(X, Y, Alpha);
        if (position.All(p => !double.IsNaN(p))) {
            Shoulder = position[0];
            Elbow = position[1];
            Wrist = position[2];
        }
    }

    public static void DrawManipulator(DrawingContext context, Point shoulderPos) {
        var joints = AnglesToCoordinatesTask.GetJointPositions(Shoulder, Elbow, Wrist);
        DrawReachableZone(context, ReachableAreaBrush, UnreachableAreaBrush, shoulderPos, joints);
        var formattedText = new FormattedText($"X={X:0}, Y={Y:0}, Alpha={Alpha:0.00}",
                                              Typeface.Default,
                                              18,
                                              TextAlignment.Center,
                                              TextWrapping.Wrap,
                                              Size.Empty);

        context.DrawText(Brushes.DarkRed, new Point(10, 10), formattedText);

        var previousJoint = ConvertMathToWindow(new Point(0, 0), shoulderPos);
        for (var i = 0; i < joints.Length; i++) {
            var currentJoint = ConvertMathToWindow(joints[i], shoulderPos);
            context.DrawLine(ManipulatorPen, previousJoint, currentJoint);
            context.DrawEllipse(JointBrush, null, currentJoint, 10, 10);
            previousJoint = currentJoint;
        }
    }

    private static void DrawReachableZone(DrawingContext context,
                                          Brush reachableBrush, Brush unreachableBrush,
                                          Point shoulderPos, Point[] joints) {
        var rmin = Math.Abs(Manipulator.UpperArm - Manipulator.Forearm);
        var rmax = Manipulator.UpperArm + Manipulator.Forearm;
        var mathCenter = new Point(joints[2].X - joints[1].X, joints[2].Y - joints[1].Y);
        var center = ConvertMathToWindow(mathCenter, shoulderPos);
        context.DrawEllipse(reachableBrush, null, new Point(center.X, center.Y), rmax, rmax);
        context.DrawEllipse(unreachableBrush, null, new Point(center.X, center.Y), rmin, rmin);
    }

    public static Point GetShoulderPos(IDisplay display)
        => new Point(display.Bounds.Width / 2, display.Bounds.Height / 2);

    public static Point ConvertMathToWindow(Point mathPoint, Point shoulderPos)
        => new Point(mathPoint.X + shoulderPos.X, shoulderPos.Y - mathPoint.Y);

    public static Point ConvertWindowToMath(Point windowPoint, Point shoulderPos)
        => new Point(windowPoint.X - shoulderPos.X, shoulderPos.Y - windowPoint.Y);
}