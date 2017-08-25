using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KinectTracking.src.DataStructures;

namespace KinectTracking.src.Math
{
    public class BezierCurve
    {
        public Point[] points { get; set; }
        public Point GetPoint(int index)
        {
            return new Point(points[index].X, points[index].Y);
        }
        public BezierCurve(Point[] points)
        {
            this.points = points;
        }
    }
}
