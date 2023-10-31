using System;
using System.Collections.Generic;
using System.Linq;

namespace func_rocket;

public enum LevelType {
    Zero,
    Heavy,
    Up,
    WhiteHole,
    BlackHole,
    BlackAndWhite
}

public enum HoleType {
    Black = 300,
    White = 140,
}

public class LevelsTask {
    static readonly Func<LevelInfo, Level> CreateLevel =
        info => new Level(
            info.Type.ToString(),
            info.Rocket,
            info.Target,
            info.Gravity,
            info.Physics);

    public static IEnumerable<Level> CreateLevels() {
        var levels = AddLevels();
        foreach (var level in levels)
            yield return CreateLevel(level);
    }

    private static List<LevelInfo> AddLevels() {
        var levels = new List<LevelInfo>();
        var blackHolePosition = (LevelInfo.standartStart + LevelInfo.standartTarget) / 2;
        levels.Add(new LevelInfo(LevelType.Zero));
        levels.Add(new LevelInfo(LevelType.Heavy));
        levels.Add(new LevelInfo(LevelType.Up,
            LevelInfo.standartStart,
            new Vector(700, 500)));
        levels.Add(new LevelInfo(LevelType.WhiteHole,
            new Hole(HoleType.White, LevelInfo.standartTarget)));
        levels.Add(new LevelInfo(LevelType.BlackHole,
            new Hole(HoleType.Black, blackHolePosition)));
        levels.Add(new LevelInfo(LevelType.BlackAndWhite,
            new Hole(HoleType.White, LevelInfo.standartTarget),
            new Hole(HoleType.White, new Vector(100, 300)),
            new Hole(HoleType.Black, blackHolePosition)));
        return levels;
    }

    public class LevelInfo {
        public static readonly Vector standartStart = new(200, 500);
        public static readonly Vector standartTarget = new(600, 200);
        public static readonly Physics standartPhysics = new();

        public readonly LevelType Type;
        public readonly Rocket Rocket;
        public readonly Vector Target;
        public readonly Gravity Gravity;
        public readonly Physics Physics;
        public readonly Hole[] Holes;

        public LevelInfo(LevelType name,
            Vector startPosition,
            Vector targetPosition,
            Physics physics,
            params Hole[] holes) {
            Type = name;
            Rocket = new Rocket(startPosition, Vector.Zero, -0.5 * Math.PI);
            Target = targetPosition;
            Physics = physics;
            Holes = holes;
            Gravity = GetGravity();
        }

        public LevelInfo(LevelType name, params Hole[] holes)
            : this(name, standartStart, standartTarget, standartPhysics, holes) {
        }

        public LevelInfo(LevelType name, Vector startPosition, params Hole[] holes)
            : this(name, startPosition, standartTarget, standartPhysics, holes) {
        }

        public LevelInfo(LevelType name, Vector startPosition, Vector targetPosition, params Hole[] holes)
            : this(name, startPosition, targetPosition, standartPhysics, holes) {
        }

        public Gravity GetGravity() {
            return Type switch {
                LevelType.Zero => (size, v) => Vector.Zero,
                LevelType.Heavy => (size, v) => new Vector(0, 0.9),
                LevelType.Up => (size, v) => new Vector(0, -300 / (Math.Abs(v.Y - size.Y) + 300.0)),
                _ => (size, v) => Holes
                    .Select(h => h.GetGravity(v))
                    .Aggregate((x, y) => x + y) / Holes.Length,
            };
        }
    }

    public class Hole {
        public readonly HoleType Type;
        public readonly Vector Position;

        public Hole(HoleType type, Vector position) {
            Type = type;
            Position = position;
        }

        public Vector GetGravity(Vector playerPosition) {
            var force = GetForce(playerPosition);
            var angle = GetAngle(playerPosition);
            return new Vector(force * Math.Cos(angle), force * Math.Sin(angle));
        }

        private double GetDistance(Vector playerPosition)
            => (playerPosition - Position).Length;

        private double GetForce(Vector playerPosition) {
            var distance = GetDistance(playerPosition);
            return (int)Type * distance / (distance * distance + 1);
        }

        private double GetAngle(Vector playerPosition) {
            var angle = (Position - playerPosition).Angle;
            if (Type == HoleType.White)
                angle += Math.PI;
            return angle;
        }
    }
}