using System.Drawing;
using System.Globalization;

namespace Mazes
{
    public static class EmptyMazeTask
    {
        public static void MoveOut(Robot robot, int width, int height)
        {
            while (!robot.Finished)
            {
                MoveDirectly(robot, width, height);
            }
        }

        private static void MoveDirectly(Robot robot, int width, int height)
        {
            if (robot.X != width - 2) robot.MoveTo(Direction.Right);
            if (robot.Y != height - 2) robot.MoveTo(Direction.Down);
        }
    }
}