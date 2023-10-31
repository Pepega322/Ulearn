using System;

namespace Billiards {
    public static class BilliardsTask {
        public static double BounceWall(double directionAngle, double wallAngle)
            => 2 * wallAngle - directionAngle;
    }
}