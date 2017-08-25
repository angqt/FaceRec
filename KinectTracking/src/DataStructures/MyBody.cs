using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectTracking.src.DataStructures
{
    public static class MyBody
    {
        public enum Gestures
        {
            HandWave = 0,
            Circle = 1,
            Pickup = 2,
            BringItem = 3,
            PullOut = 4,
            PutBack = 5
        }
        public enum BodyParts
        {
            Head = 0,
            Neck = 1,
            SpineShoulder = 2,
            SpineMid = 3,
            SpineBase = 4,
            ShoulderLeft = 5,
            ElbowLeft = 6,
            WristLeft = 7,
            HandLeft = 8,
            ThumbLeft = 9,
            HandTipLeft = 10,
            ShoulderRight = 11,
            ElbowRight = 12,
            WristRight = 13,
            HandRight = 14,
            ThumbRight = 15,
            HandTipRight = 16,
            HipLeft = 17,
            KneeLeft = 18,
            AnkleLeft = 19,
            FootLeft = 20,
            HipRight = 21,
            KneeRight = 22,
            AnkleRight = 23,
            FootRight = 24
        }
    }
}
