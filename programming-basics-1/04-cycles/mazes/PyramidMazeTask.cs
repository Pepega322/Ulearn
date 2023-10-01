namespace Mazes
{
    public static class PyramidMazeTask
    {
        public static void MoveOut(Robot robot, int width, int height)
        {
            int i = 0;
            while (true)
            {
                MoveHorizontal(robot, width, width - 3 - i);
                i += 2;
                if (robot.Finished) break;
                MoveInDirection(robot, Direction.Up, 2);
            }
        }
        static void MoveHorizontal(Robot robot, int width, int steps)
        {
            if (robot.X < (width - 2) / 2)
                MoveInDirection(robot, Direction.Right, steps);
            else
                MoveInDirection(robot, Direction.Left, steps);
        }

        static void MoveInDirection(Robot robot, Direction direction, int steps)
        {
            for (int i = 0; i < steps; i++)
                robot.MoveTo(direction);
        }
    }
}