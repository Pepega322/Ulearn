using System;

namespace Mazes
{
    public static class DiagonalMazeTask
    {
        enum Axis : int
        {
            X, Y
        }

        public static void MoveOut(Robot robot, int width, int height)
        {
            int stepsByX = GetStepsPerMove(width, height, Axis.X);
            int stepsByY = GetStepsPerMove(width, height, Axis.Y);
            if (height > width) MoveInDirection(robot, Direction.Down, stepsByY);
            while (true)
            {
                MoveInDirection(robot, Direction.Right, stepsByX);
                if (robot.Finished) break;
                MoveInDirection(robot, Direction.Down, stepsByY);
                if (robot.Finished) break;
            }
        }

        static int GetStepsPerMove(int width, int height, Axis axis)
        {
            int maxSide = Math.Max(width - 3, height - 3);
            int minSide = Math.Min(width - 3, height - 3);
            int stepsPerMoveForMaxSide = maxSide % minSide;
            int stepsPerMoveForMinSide = minSide / (maxSide / stepsPerMoveForMaxSide - 1);
            return ((width > height && axis == Axis.X) || (height > width && axis == Axis.Y)) ?
                stepsPerMoveForMaxSide : stepsPerMoveForMinSide;
        }

        static void MoveInDirection(Robot robot, Direction direction, int steps)
        {
            for (int i = 0; i < steps; i++)
                robot.MoveTo(direction);
        }
    }
}