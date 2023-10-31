using System;
using System.Linq;
using System.Windows;

namespace Digger {
    public class Monster : ICreature {
        private static readonly CreatureCommand[] possibleMoves = new[] {
            new CreatureCommand { DeltaX = 1 },
            new CreatureCommand { DeltaX = -1 },
            new CreatureCommand { DeltaY = 1 },
            new CreatureCommand { DeltaY = -1 }
        };

        private Point? GetPlayerPosition() {
            for (var i = 0; i < Game.MapWidth; i++)
                for (var j = 0; j < Game.MapHeight; j++)
                    if (Game.Map[i, j] is Player) return new Point { X = i, Y = j };
            return null;
        }

        private double? GetDistance(int x0, int y0, Point p) {
            var dx = x0 - p.X;
            var dy = y0 - p.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private bool CanMoove(int x, int y, CreatureCommand move) {
            if (!move.InBorders(x, y)) return false;
            var onWay = move.GetCreatureOnWay(x, y);
            return (onWay is null)
                || (onWay is Player)
                || (onWay is Gold);
        }

        public CreatureCommand Act(int x0, int y0) {
            var player = GetPlayerPosition();
            if (!player.HasValue) return new CreatureCommand();
            var currentDistance = GetDistance(x0, y0, player.Value);
            var bestMove = possibleMoves
                .Where(m => CanMoove(x0, y0, m))
                .Where(m => GetDistance(x0 + m.DeltaX, y0 + m.DeltaY, player.Value) < currentDistance)
                .FirstOrDefault();
            return (bestMove != default) ? bestMove : new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
            => conflictedObject is Monster
            || conflictedObject is Sack sack && sack.CanKill;

        public int GetDrawingPriority() => (int)Priority.Monster;

        public string GetImageFileName() => "Monster.png";
    }
}