using System;

namespace KinectTracking.src.DataStructures
{
    public class BezierGesture: IComparable<BezierGesture>
    {
        public MyBody.Gestures gestureType;
        public string personName = "";
        public BezierFrames frames;
        public double distanceX;
        public double distanceY;
        public double distanceMean;
        public double normDistanceX;
        public double normDistanceY;
        public double normDistanceMean;

        public BezierGesture()
        {
            frames = new BezierFrames();
        }

        public int CompareTo(BezierGesture other)
        {
            return distanceX.CompareTo(other.distanceX);
        }
    }
}
