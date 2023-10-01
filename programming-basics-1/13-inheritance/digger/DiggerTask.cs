using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    public static class CreatureExtensions
    {
        public static bool IsInBorders(int x, int y, CreatureCommand movement)
        {
            var xPosition = x + movement.DeltaX;
            var yPosition = y + movement.DeltaY;
            return xPosition >= 0 && xPosition < Game.MapWidth &&
                yPosition >= 0 && yPosition < Game.MapHeight;
        }

        public static ICreature GetCreatureOnWay(int x, int y, CreatureCommand movement)
        {
            return Game.Map[x + movement.DeltaX, y + movement.DeltaY];
        }
    }

    public class Player : ICreature
    {
        private CreatureCommand MoveDigger()
        {
            switch (Game.KeyPressed)
            {
                case System.Windows.Forms.Keys.Right:
                    return new CreatureCommand { DeltaX = 1 };
                case System.Windows.Forms.Keys.Left:
                    return new CreatureCommand { DeltaX = -1 };
                case System.Windows.Forms.Keys.Up:
                    return new CreatureCommand { DeltaY = -1 };
                case System.Windows.Forms.Keys.Down:
                    return new CreatureCommand { DeltaY = 1 };
                default:
                    return new CreatureCommand();
            }
        }

        private bool IsCorrectMove(int x, int y, CreatureCommand movement)
        {
            if (!CreatureExtensions.IsInBorders(x, y, movement)) return false;
            var creatureOnWay = CreatureExtensions.GetCreatureOnWay(x, y, movement);
            return !(creatureOnWay is Sack); ;
        }

        public CreatureCommand Act(int x, int y)
        {
            var move = MoveDigger();
            if (IsCorrectMove(x, y, move)) return move;
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return (conflictedObject is Sack sack && sack.FallDistance >= 1)
                || conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 4;
        }

        public string GetImageFileName()
        {
            return "Digger.png";
        }
    }

    public class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Player;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName()
        {
            return "Terrain.png";
        }
    }

    public class Sack : ICreature
    {
        public int FallDistance;

        private bool IsCorrectMove(int x, int y, CreatureCommand movement)
        {
            if (!CreatureExtensions.IsInBorders(x, y, movement)) return false;
            var creatureOnWay = CreatureExtensions.GetCreatureOnWay(x, y, movement);
            return creatureOnWay == null || (FallDistance >= 1
                && (creatureOnWay is Player || creatureOnWay is Monster));
        }

        public CreatureCommand Act(int x, int y)
        {
            var movement = new CreatureCommand { DeltaY = 1 };
            if (IsCorrectMove(x, y, movement))
            {
                FallDistance += 1;
                return movement;
            }
            if (FallDistance > 1)
                return new CreatureCommand { TransformTo = new Gold() };
            FallDistance = 0;
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public string GetImageFileName()
        {
            return "Sack.png";
        }
    }

    public class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Player)
            {
                Game.Scores += 10;
                return true;
            }
            return conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 3;
        }

        public string GetImageFileName()
        {
            return "Gold.png";
        }
    }

    public class Monster : ICreature
    {
        private Point GetPlayerPosition()
        {
            for (var i = 0; i < Game.MapWidth; i++)
                for (var j = 0; j < Game.MapHeight; j++)
                    if (Game.Map[i, j] is Player) return new Point { X = i, Y = j };
            return new Point { X = -1, Y = -1 };
        }

        private double GetDistanceToPlayer(int x, int y, Point playerPosition)
        {
            var dx = Math.Max(x, playerPosition.X) - Math.Min(x, playerPosition.X);
            var dy = Math.Max(y, playerPosition.Y) - Math.Min(y, playerPosition.Y);
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private CreatureCommand MoveMonster(int x, int y, Point playerPosition)
        {
            var distance = GetDistanceToPlayer(x, y, playerPosition);
            var moves = new CreatureCommand[]
            {
                new CreatureCommand { DeltaX = 1 },
                new CreatureCommand { DeltaX = -1 },
                new CreatureCommand { DeltaY = 1 },
                new CreatureCommand { DeltaY = -1 },
            };
            foreach (var move in moves)
            {
                if (IsCorrectMove(x, y, move) &&
                    GetDistanceToPlayer(x + move.DeltaX, y + move.DeltaY, playerPosition) < distance)
                    return move;
            }
            return new CreatureCommand();
        }

        private bool IsCorrectMove(int x, int y, CreatureCommand move)
        {
            if (!CreatureExtensions.IsInBorders(x, y, move)) return false;
            var creatureOnWay = CreatureExtensions.GetCreatureOnWay(x, y, move);
            return !(creatureOnWay is Terrain)
                && !(creatureOnWay is Sack)
                && !(creatureOnWay is Monster);
        }

        public CreatureCommand Act(int x, int y)
        {
            var playerPosition = GetPlayerPosition();
            if (playerPosition.X == -1) return new CreatureCommand();
            var move = MoveMonster(x, y, playerPosition);
            if (IsCorrectMove(x, y, move)) return move;
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return (conflictedObject is Sack sack && sack.FallDistance >= 1)
                || conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 5;
        }

        public string GetImageFileName()
        {
            return "Monster.png";
        }
    }
}
