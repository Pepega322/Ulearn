namespace Digger {
    public class Gold : ICreature {
        public CreatureCommand Act(int x, int y) => new CreatureCommand();

        public bool DeadInConflict(ICreature conflictedObject) {
            if (conflictedObject is Player) {
                Game.Scores += 10;
                return true;
            }
            return conflictedObject is Monster;
        }

        public int GetDrawingPriority() => (int)Priority.Gold;

        public string GetImageFileName() => "Gold.png";
    }
}