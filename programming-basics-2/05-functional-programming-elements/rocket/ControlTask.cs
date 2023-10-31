using System;

namespace func_rocket;

public class ControlTask {
    public static Turn ControlRocket(Rocket rocket, Vector target) {
        var perfectAngle = (target - rocket.Location).Angle;
        var currentAngle = rocket.Direction / 3 + rocket.Velocity.Angle * 2 / 3;
        if (Math.Abs(perfectAngle - currentAngle) < 1e-3)
            return Turn.None;
        if (currentAngle > perfectAngle)
            return Turn.Left;
        else
            return Turn.Right;

        return Turn.None;
    }
}