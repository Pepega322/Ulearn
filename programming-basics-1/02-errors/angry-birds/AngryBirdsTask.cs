using System;

namespace AngryBirds {
    public static class AngryBirdsTask {
        const double Gravity = 9.8;

        public static double FindSightAngle(double startSpeed, double distanceToTarget) {
            var sinus = (Gravity * distanceToTarget) / (startSpeed * startSpeed);
            var angle = Math.Asin(sinus) / 2;
            return Math.Abs(angle - Math.PI / 2) > 1e-3 ? angle : double.NaN;
        }
    }
}