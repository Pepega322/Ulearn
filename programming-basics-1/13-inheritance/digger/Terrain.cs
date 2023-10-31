namespace Digger {
    public class Terrain : ICreature {
        public CreatureCommand Act(int x, int y) => new CreatureCommand();

        public bool DeadInConflict(ICreature conflictedObject)
            => conflictedObject is Player;

        public int GetDrawingPriority() => (int)Priority.Terrain;

        public string GetImageFileName() => "Terrain.png";
    }
}