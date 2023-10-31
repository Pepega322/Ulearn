using System.Windows.Forms;

namespace Digger {
    public class Player : ICreature {
        private bool CanMove(CreatureCommand c, int x0, int y0)
            => c.InBorders(x0, y0) && !(c.GetCreatureOnWay(x0, y0) is Sack);

        public CreatureCommand Act(int x, int y) {
            var move = new CreatureCommand();
            switch (Game.KeyPressed) {
                case Keys.Right: move.DeltaX = 1; break;
                case Keys.Left: move.DeltaX = -1; break;
                case Keys.Up: move.DeltaY = -1; break;
                case Keys.Down: move.DeltaY = 1; break;
                default: break;
            }
            return (CanMove(move, x, y)) ? move : new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
            => conflictedObject is Monster
              || (conflictedObject is Sack sack && sack.CanKill);

        public int GetDrawingPriority() => (int)Priority.Player;

        public string GetImageFileName() => "Digger.png";
    }
}