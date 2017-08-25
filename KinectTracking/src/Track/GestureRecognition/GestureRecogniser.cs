using KinectTracking.src.DataStructures;
using KinectTracking.src.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectTracking.src.Track.GestureRecognition
{
    class GestureRecogniser
    {
        protected src.Math.Math math;

        public GestureRecogniser()
        {
            math = new Math.Math();
        }

        /// <summary>
        /// If varX, varY and Mean agree that "frames" is a  specific gesture in "bezierGestures"
        /// then "frames" is gesture.
        /// </summary>
        /// <returns></returns>
        public bool IsThisGesture(Frames frames, List<BezierGesture> bezierGestures)
        {
            if (!math.MovementDetected(frames))
            {
                return false;
            }

            frames.CalculateBezierCurveAttributes(GlobalVariables.BodypartsForBezierCurve);
            List<BezierGesture> answer = math.CalcDistanceFromDBGestures(frames, bezierGestures);

            /*
            if 3 parameters (x, y, mean) agree on gesture (myAnwerType's are equal) 
            and distance is below statically set GestureMaxDistance 
            then it (frames) is gesture
            */

            answer = answer.OrderBy(x => x.distanceX).ToList();
            String myAnwerType = (from item in answer.Take(GlobalVariables.nrOfClosestGesturesComparedKNN)
                                  group item by (item.gestureType + item.personName) into g
                                  orderby g.Count() descending
                                  select new { Item = g.Key }).First().ToString();
            if (!IsGestureX(answer))
            {
                return false;
            }

            answer = answer.OrderBy(x => x.distanceY).ToList();
            if (!myAnwerType.Equals((from item in answer.Take(GlobalVariables.nrOfClosestGesturesComparedKNN)
                                     group item by (item.gestureType + item.personName) into g
                                     orderby g.Count() descending
                                     select new { Item = g.Key }).First().ToString()))
            {
                return false;
            }
            if (!IsGestureY(answer))
            {
                return false;
            }

            answer = answer.OrderBy(x => x.distanceMean).ToList();
            if (!myAnwerType.Equals((from item in answer.Take(GlobalVariables.nrOfClosestGesturesComparedKNN)
                                     group item by (item.gestureType + item.personName) into g
                                     orderby g.Count() descending
                                     select new { Item = g.Key }).First().ToString()))
            {
                return false;
            }
            if (!IsGestureMean(answer))
            {
                return false;
            }

            return true;
        }

        public bool IsGestureMean(List<BezierGesture> answer)
        {
            for (int i = 0; i < GlobalVariables.nrOfClosestGesturesComparedKNN; i++)
            {
                if (answer[i].distanceMean > GlobalVariables.GestureMaxDistance)
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsGestureX(List<BezierGesture> answer)
        {
            for (int i = 0; i < GlobalVariables.nrOfClosestGesturesComparedKNN; i++)
            {
                if (answer[i].distanceX > GlobalVariables.GestureMaxDistance)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsGestureY(List<BezierGesture> answer)
        {
            for (int i = 0; i < GlobalVariables.nrOfClosestGesturesComparedKNN; i++)
            {
                if (answer[i].distanceY > GlobalVariables.GestureMaxDistance)
                {
                    return false;
                }
            }
            return true;
            return (answer[0].distanceY < GlobalVariables.GestureMaxDistance
                            && answer[1].distanceY < GlobalVariables.GestureMaxDistance
                            && answer[2].distanceY < GlobalVariables.GestureMaxDistance
                            && answer[3].distanceY < GlobalVariables.GestureMaxDistance
                            && answer[4].distanceY < GlobalVariables.GestureMaxDistance
                            && answer[5].distanceY < GlobalVariables.GestureMaxDistance
                            && answer[6].distanceY < GlobalVariables.GestureMaxDistance
                            && answer[7].distanceY < GlobalVariables.GestureMaxDistance
                            && answer[8].distanceY < GlobalVariables.GestureMaxDistance
                            && answer[9].distanceY < GlobalVariables.GestureMaxDistance
                            && answer[10].distanceY < GlobalVariables.GestureMaxDistance
                            && answer[11].distanceY < GlobalVariables.GestureMaxDistance
                            && answer[12].distanceY < GlobalVariables.GestureMaxDistance
                            && answer[13].distanceY < GlobalVariables.GestureMaxDistance
                            && answer[14].distanceY < GlobalVariables.GestureMaxDistance);
        }
    }
}
