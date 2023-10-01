using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GeometryTasks;

namespace GeometryPainting
{
    public static class SegmentExtensions
    {
        public static Dictionary<Segment, Color> Colors = new Dictionary<Segment, Color>();

        public static void SetColor(this Segment segment, Color color)
        {
            if (!Colors.ContainsKey(segment)) Colors.Add(segment, color);
            else Colors[segment] = color;
        }

        public static Color GetColor(this Segment segment)
        {
            if (Colors.ContainsKey(segment)) return Colors[segment];
            else return Color.Black;
        }
    }
}
