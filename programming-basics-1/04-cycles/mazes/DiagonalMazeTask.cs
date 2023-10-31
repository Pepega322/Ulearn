using System;

namespace Mazes {
    public static class DiagonalMazeTask {
        public static void MoveOut(Robot robot, int width, int height) {
            var steps = GetStepsPerMove(width, height);
            if (height > width) robot.MoveTo(Direction.Down, steps.Item2);
            while (true) {
                robot.MoveTo(Direction.Right, steps.Item1); 
                if (robot.Finished) break;
                robot.MoveTo(Direction.Down, steps.Item2);
                if (robot.Finished) break;
            }
        }

        private static (int, int) GetStepsPerMove(int width, int height) {
            var maxSide = Math.Max(width - 3, height - 3);
            var minSide = Math.Min(width - 3, height - 3);
            var stepsForMaxSide = maxSide % minSide;
            var stepsForMinSide = minSide / (maxSide / stepsForMaxSide - 1);
            var stepsForX = (width > height) ? stepsForMaxSide : stepsForMinSide;
            var stepsForY = (width > height) ? stepsForMinSide : stepsForMaxSide;
            return (stepsForX, stepsForY);
        }
    }

    //Для того чтобы проект работал в VS нужно удалить этот класс в SnakeMazeTask или тут,
    //но для Ulearn нужно вставлять именно в таком виде
    public static class RobotExtensions {
        public static void MoveTo(this Robot robot, Direction direction, int steps) {
            for (int i = 0; i < steps; i++)
                robot.MoveTo(direction);
        }
    }
}