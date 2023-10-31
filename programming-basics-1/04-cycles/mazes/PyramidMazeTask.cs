namespace Mazes {
    public static class PyramidMazeTask {
        public static void MoveOut(Robot robot, int width, int height) {
            var steps = width - 3;

            while (true) {
                if (robot.X < (width - 2) / 2) robot.MoveTo(Direction.Right, steps);
                else robot.MoveTo(Direction.Left, steps);
                steps -= 2;
                if (robot.Finished) break;
                robot.MoveTo(Direction.Up, 2);
            }
        }
    }
}