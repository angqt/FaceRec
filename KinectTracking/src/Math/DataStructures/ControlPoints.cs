using KinectTracking.src.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KinectTracking.src.Math
{
    public class ControlPoints
    {
        public Point[] points { get; set; }
        public ControlPoints(Point[] points)
        {
            this.points = points;
        }
    }
}
