namespace Digger {
    public static class CreatureExtensions {
        public static bool InBorders(this CreatureCommand c, int x0, int y0) {
            var x1 = x0 + c.DeltaX;
            var y1 = y0 + c.DeltaY;
            return x1 >= 0 && x1 < Game.MapWidth && y1 >= 0 && y1 < Game.MapHeight;
        }

        public static ICreature GetCreatureOnWay(this CreatureCommand c, int x0, int y0)
            => Game.Map[x0 + c.DeltaX, y0 + c.DeltaY];
    }
}