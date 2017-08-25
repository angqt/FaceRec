using Microsoft.Kinect;

namespace KinectTracking.src.DataStructures
{
    public class FrameJoint
    {
        public Coordinate position;
        public Orinetation orientation;
        public TrackingState trackingState;
        public JointType jointType;

        public FrameJoint()
        {
            position = new Coordinate();
            orientation = new Orinetation();
        }
    }
}