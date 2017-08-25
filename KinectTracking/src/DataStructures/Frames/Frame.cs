using KinectTracking.src.Math;
using KinectTracking.src.Tools;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KinectTracking.src.DataStructures;

namespace KinectTracking.src.DataStructures
{
    public class Frame
    {
        private static readonly int nrOfJoints = GlobalVariables.NrOfKinectBodyParts;
        public int id { get; set; }
        public DateTime timestamp { get; set; }
        public FrameJoint[] joints { get; set; }
        private BezierCurve bezierCurvePoints;

        public double varianceX { get; set; }
        public double varianceY { get; set; }
        public double mean { get; set; }

        public bool marker = false;

        public Frame(int id, DateTime timestamp) : this()
        {
            this.id = id;
            this.timestamp = timestamp;
        }

        public Frame()
        {
            joints = new FrameJoint[nrOfJoints];
            for (int i = 0; i < nrOfJoints; i++)
            {
                joints[i] = new FrameJoint();
            }
            InitJointTypes();
        }

        private void InitJointTypes()
        {
            joints[0].jointType = JointType.Head;
            joints[1].jointType = JointType.Neck;
            joints[2].jointType = JointType.SpineShoulder;
            joints[3].jointType = JointType.SpineMid;
            joints[4].jointType = JointType.SpineBase;
            joints[5].jointType = JointType.ShoulderLeft;
            joints[6].jointType = JointType.ElbowLeft;
            joints[7].jointType = JointType.WristLeft;
            joints[8].jointType = JointType.HandLeft;
            joints[9].jointType = JointType.ThumbLeft;
            joints[10].jointType = JointType.HandTipLeft;
            joints[11].jointType = JointType.ShoulderRight;
            joints[12].jointType = JointType.ElbowRight;
            joints[13].jointType = JointType.WristRight;
            joints[14].jointType = JointType.HandRight;
            joints[15].jointType = JointType.ThumbRight;
            joints[16].jointType = JointType.HandTipRight;
            joints[17].jointType = JointType.HipLeft;
            joints[18].jointType = JointType.KneeLeft;
            joints[19].jointType = JointType.AnkleLeft;
            joints[20].jointType = JointType.FootLeft;
            joints[21].jointType = JointType.HipRight;
            joints[22].jointType = JointType.KneeRight;
            joints[23].jointType = JointType.AnkleRight;
            joints[24].jointType = JointType.FootRight;
        }

        public void CalculateBezierCurveAttributes(MyBody.BodyParts[] parts)
        {
            //calc bezier curve:
            src.Math.Math math = new src.Math.Math();
            ControlPoints controlPoints = GetControlPointsFromFrames(parts);
            bezierCurvePoints = math.GetBezierApproximation(controlPoints,GlobalVariables.BezierCurveOutputSegmentCount);

            //calc variances and mean based on bezier curve:
            double[] X = new double[GlobalVariables.BezierCurveOutputSegmentCount + 1];
            double[] Y = new double[GlobalVariables.BezierCurveOutputSegmentCount + 1];
            for(int i = 0; i <= GlobalVariables.BezierCurveOutputSegmentCount; i++)
            {
                X[i] = bezierCurvePoints.points[i].X;
                Y[i] = bezierCurvePoints.points[i].Y;
            }
            varianceX = math.Variance(X);
            varianceY = math.Variance(Y);
            mean = math.Mean(new double[] { varianceX, varianceY });
        }

        private ControlPoints GetControlPointsFromFrames(MyBody.BodyParts[] parts)
        {
            ControlPoints controlPoints = new ControlPoints(new Point[parts.Length]);
            int i = 0;
            foreach (MyBody.BodyParts part in parts)
            {
                controlPoints.points[i].X = (double)joints[(int)part].position.X;
                controlPoints.points[i++].Y = (double)joints[(int)part].position.Y;
            }
            return controlPoints;
        }

        public BezierCurve GetBezierCurvePoints()
        {
            return bezierCurvePoints;
        }
    }
}
