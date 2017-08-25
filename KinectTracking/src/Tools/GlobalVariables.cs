using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KinectTracking.src.DataStructures;

namespace KinectTracking.src.Tools
{
    public static class GlobalVariables
    {
        public const int NrOfKinectBodyParts = 25;
        public const int CSVBodyPositionStartingIndex = 2;
        public const string untrackedMarker = "untracked";
        public const string GestureMark = "**GestureMark**";

        //Buffer:
        public const int NrOfBuffers = 16;
        public const int BufferCreationInterval = 250;
        public const int BufferFrameCount = 60;

        internal static int MinGestureFramesCount = 11;

        //Gesture recognition precision:
        public const int nrOfClosestGesturesComparedKNN = 15;
        public const int nrOfClosestGesturesRequiredKNN = 12;
        public const int nrOfPointsComparedMovement = 5;
        public const float minDistanceTraveled = 0.3f;//0.25f;

        public const double GestureMaxDistance = 0.028;//0.028;
        public const int NrOfPointsOnBezierCurveCompared = 5;//5;//increases distance between gestures

        //variance and mean calculation precision:
        public const int BezierCurveOutputSegmentCount = 14;//0 to 14

        //walking:
        /*public static readonly MyBody.BodyParts[] BodypartsForBezierCurve = new[] {
                MyBody.BodyParts.AnkleRight,
                MyBody.BodyParts.FootRight,
                MyBody.BodyParts.KneeRight,
                MyBody.BodyParts.HandRight,
                MyBody.BodyParts.ShoulderRight
            };*/

        //right hand gesture:
        public static readonly MyBody.BodyParts[] BodypartsForBezierCurve = new[] {
                MyBody.BodyParts.ShoulderLeft,
                MyBody.BodyParts.ShoulderRight,
                MyBody.BodyParts.ElbowRight,
                MyBody.BodyParts.HandRight
            };
    }
}
