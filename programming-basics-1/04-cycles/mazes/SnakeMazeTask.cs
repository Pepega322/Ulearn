using System.Drawing;

namespace Mazes {
    public static class SnakeMazeTask {
        public static void MoveOut(Robot robot, int width, int heigth) {
            while (true) {
                MoveHorizontal(robot, width);
                if (robot.Finished) break;
                robot.MoveTo(Direction.Down, 2);
                if (robot.Finished) break;
            }
        }

        static void MoveHorizontal(Robot robot, int width) {
            if (robot.X < (width - 2) / 2) robot.MoveTo(Direction.Right, width - 3);
            else robot.MoveTo(Direction.Left, width - 3);
        }
    }

    public static class RobotExtensions {
        public static void MoveTo(this Robot robot, Direction direction, int steps) {
            for (int i = 0; i < steps; i++)
                robot.MoveTo(direction);
        }
    }
}