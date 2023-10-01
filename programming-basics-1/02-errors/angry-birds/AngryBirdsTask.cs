using System;

namespace AngryBirds
{
	public static class AngryBirdsTask
	{
		const double GravityConst = 9.8;
		const double PI = Math.PI;
		// Ниже — это XML документация, её использует ваша среда разработки, 
		// чтобы показывать подсказки по использованию методов. 
		// Но писать её естественно не обязательно.
		/// <param name="v">Начальная скорость</param>
		/// <param name="distance">Расстояние до цели</param>
		/// <returns>Угол прицеливания в радианах от 0 до Pi/2</returns>
		public static double FindSightAngle(double v, double distance)
		{
			double arcSinArgument = (GravityConst * distance) / (v * v);
            double sightAngle = Math.Asin(arcSinArgument)/2;
			if (sightAngle == PI / 2) 
			{
				return double.NaN;
			}
			else
			{
				return sightAngle;
			}
		}
	}
}