namespace Digger {
    public class Sack : ICreature {
        private int framesFalling;
        public bool CanKill => framesFalling >= 1;

        private bool CanMove(CreatureCommand c, int x0, int y0) {
            if (!c.InBorders(x0, y0)) return false;
            var onWay = c.GetCreatureOnWay(x0, y0);
            return onWay is null || CanKill
                && (onWay is Player || onWay is Monster);
        }

        public CreatureCommand Act(int x, int y) {
            var move = new CreatureCommand { DeltaY = 1 };
            if (CanMove(move, x, y)) {
                framesFalling += 1;
                return move;
            }
            if (framesFalling > 1) return new CreatureCommand { TransformTo = new Gold() };
            framesFalling = 0;
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject) => false;

        public int GetDrawingPriority() => (int)Priority.Sack;

        public string GetImageFileName() => "Sack.png";
    }
}