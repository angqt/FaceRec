using KinectTracking.src.DataStructures;
using KinectTracking.src.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using KinectTracking.src.DataStructures;

namespace KinectTracking.src.Math
{
    public class Math
    {
        public void NormalizeBezierGestureDistances(List<BezierGesture> bezierGestures)
        {
            //calc min/max distances:
            double minX = Double.MaxValue;
            double maxX = 0;
            double minY = Double.MaxValue;
            double maxY = 0;
            double minMean = Double.MaxValue;
            double maxMean = 0;
            foreach (BezierGesture bezierGesture in bezierGestures)
            {
                if (bezierGesture.distanceX < minX) { minX = bezierGesture.distanceX; }
                else if (bezierGesture.distanceX > maxX) { maxX = bezierGesture.distanceX; }

                if (bezierGesture.distanceY < minY) { minY = bezierGesture.distanceY; }
                else if (bezierGesture.distanceY > maxY) { maxY = bezierGesture.distanceY; }

                if (bezierGesture.distanceMean < minMean) { minMean = bezierGesture.distanceMean; }
                else if (bezierGesture.distanceMean > maxMean) { maxMean = bezierGesture.distanceMean; }
            }
            double minMaxX = maxX - minX;
            double minMaxY = maxY - minY;
            double minMaxMean = maxMean - minMean;

            //calc normalized data:
            foreach (BezierGesture bezierGesture in bezierGestures)
            {
                bezierGesture.normDistanceX = (bezierGesture.distanceX - minX) / minMaxX;
                bezierGesture.normDistanceY = (bezierGesture.distanceY - minY) / minMaxY;
                bezierGesture.normDistanceMean = (bezierGesture.distanceMean - minMean) / minMaxMean;
            }
        }

        internal bool MovementDetected(Frames frames)
        {
            ArrayList comparedFrames = GetComparedFrames(frames);
            double distanceTraveledX = 0;
            double distanceTraveledY = 0;
            for (int i = 0; i < comparedFrames.Count - 1; i++)
            {
                foreach (MyBody.BodyParts part in GlobalVariables.BodypartsForBezierCurve)
                {
                    distanceTraveledX += System.Math.Abs((double)(((Frame)comparedFrames[i + 1]).joints[(int)part].position.X - ((Frame)comparedFrames[i]).joints[(int)part].position.X));
                    distanceTraveledY += System.Math.Abs((double)(((Frame)comparedFrames[i + 1]).joints[(int)part].position.Y - ((Frame)comparedFrames[i]).joints[(int)part].position.Y));
                }
            }
            if ((distanceTraveledX + distanceTraveledY) < GlobalVariables.minDistanceTraveled)
            {
                return false;
            } else
            {
                return true;
            }
        }

        /// <summary>
        /// Gesture = Curve of one parameter of one gesture
        /// </summary>
        /// <param name="frames"></param>
        /// <param name="bezierGestures"></param>
        /// <returns></returns>
        public List<BezierGesture> CalcDistanceFromDBGestures(Frames frames, List<BezierGesture> bezierGestures)
        {
            ArrayList comparedFrames = new ArrayList();
            try
            {
                comparedFrames = GetComparedFrames(frames);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            //LogTools.LogRandomData(comparedFrames);

            foreach (BezierGesture bezierGesture in bezierGestures)
            {
                bezierGesture.distanceY = DistanceY(comparedFrames, bezierGesture);
                bezierGesture.distanceX = DistanceX(comparedFrames, bezierGesture);
                bezierGesture.distanceMean = DistanceMean(comparedFrames, bezierGesture);
            }
            return bezierGestures;
        }

        /// <summary>
        /// used in alternative distance calculation
        /// author: http://stackoverflow.com/a/8914725
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        private double DistanceN(double[] first, double[] second)
        {
            double sum = first.Select((x, i) => (x - second[i]) * (x - second[i])).Sum();
            return System.Math.Sqrt(sum);
        }

        /// <summary>
        /// Used to doublecheck main distance calculating results.
        /// </summary>
        /// <param name="frames"></param>
        /// <param name="bezierGesture"></param>
        /// <returns></returns>
        private double DistanceYAlternative(ArrayList frames, BezierGesture bezierGesture)
        {
            int interval = (bezierGesture.frames.GetAttributes().Count
                / (GlobalVariables.NrOfPointsOnBezierCurveCompared));
            double result = 0;
            double[] frame = new double[5];
            double[] DBGesture = new double[5];
            //IEnumerable<BezierAttributes> gestures = (IEnumerable<BezierAttributes>)bezierGesture.frames.GetAttributes();

            //split gesture into n parts:
            IEnumerable<BezierAttributes> thisBezierGesture = bezierGesture.frames.GetAttributes();
            for (int j = 0; j < 5; j++)
            {
                frame[j] = ((Frame)frames[j]).varianceY;
                DBGesture[j] = thisBezierGesture.First().varY;
                thisBezierGesture = (IEnumerable<BezierAttributes>)thisBezierGesture.Skip(interval);
            }
            result = DistanceN(frame, DBGesture);
            return result;
        }

        private double DistanceMean(ArrayList frames, BezierGesture bezierGesture)
        {
            int interval = GetInterval(bezierGesture);
            double result = 0;
            for (int i = 0; i < frames.Count; i++)
            {
                result += System.Math.Pow(((Frame)frames[i]).mean
                    - ((BezierAttributes)bezierGesture.frames.GetAttributes()[i * interval]).mean, 2);
            }
            result = System.Math.Sqrt(result);
            return result;
        }

        private double DistanceX(ArrayList frames, BezierGesture bezierGesture)
        {
            int interval = GetInterval(bezierGesture);
            double result = 0;
            for (int i = 0; i < frames.Count; i++)
            {
                result += System.Math.Pow(((Frame)frames[i]).varianceX
                    - ((BezierAttributes)bezierGesture.frames.GetAttributes()[i * interval]).varX, 2);
            }
            result = System.Math.Sqrt(result);
            return result;
        }

        private double DistanceY(ArrayList frames, BezierGesture bezierGesture)
        {
            int interval = GetInterval(bezierGesture);
            double result = 0;
            for (int i = 0; i < frames.Count; i++)
            {
                result += System.Math.Pow(((Frame)frames[i]).varianceY 
                    - ((BezierAttributes)bezierGesture.frames.GetAttributes()[i* interval]).varY, 2);
            }
            result = System.Math.Sqrt(result);
            return result;
        }

        public int GetInterval(BezierGesture bezierGesture)
        {
            return (bezierGesture.frames.GetAttributes().Count
                / (GlobalVariables.NrOfPointsOnBezierCurveCompared));
        }

        private ArrayList GetComparedFrames(Frames frames)
        {
            IEnumerator enumerator = frames.GetEnumerator();
            ArrayList comparedFrames = new ArrayList();
            int interval = (frames.Count() / (GlobalVariables.NrOfPointsOnBezierCurveCompared));
            if (interval == 0)
            {
                throw new System.Exception("Everything went wrong while measuring distance: while choosing frames to compare: interval = 0");
            }
            enumerator.MoveNext();
            comparedFrames.Add(enumerator.Current);
            for (int i = 0; i < GlobalVariables.NrOfPointsOnBezierCurveCompared-1; i++)
            {
                for (int j = 0; j < interval; j++)
                {
                    enumerator.MoveNext();
                }
                comparedFrames.Add(enumerator.Current);
            }
            if (comparedFrames.Count < 5)
            {
                throw new System.Exception("Everything went wrong while  measuring distance: while choosing frames to compare: comparedFrames < 5");
            }
            return comparedFrames;
        }

        /// <summary>
        /// author: https://sinairv.wordpress.com/2011/05/15/code-snippet-mean-variance-and-standard-deviation-methods/
        /// Calculates the variance of an array of values
        /// </summary>
        /// <param name="v">the array of values to calculate their variance</param>
        /// <returns>The variance of the array of values</returns>
        public double Variance(double[] v)
        {
            double mean = Mean(v);
            double sum = 0.0;

            for (int i = 0; i < v.Length; i++)
            {
                sum += (v[i] - mean) * (v[i] - mean);
            }

            int denom = v.Length - 1;
            if (v.Length <= 1)
                denom = v.Length;

            return sum / denom;
        }

        /// <summary>
        /// author: https://sinairv.wordpress.com/2011/05/15/code-snippet-mean-variance-and-standard-deviation-methods/
        /// Calculates the mean of an array of values
        /// </summary>
        /// <param name="v">the array of values to calculate their mean</param>
        /// <returns>The mean of the array of values</returns>
        public double Mean(double[] v)
        {
            double sum = 0.0;

            for (int i = 0; i < v.Length; i++)
            {
                sum += v[i];
            }

            return sum / v.Length;
        }

        /// <summary>
        /// One frame, one curve
        /// author: http://stackoverflow.com/a/13948059
        /// </summary>
        public BezierCurve GetBezierApproximation(ControlPoints controlPoints, int outputSegmentCount)
        {
            Point[] points = new Point[outputSegmentCount + 1];
            for (int i = 0; i <= outputSegmentCount; i++)
            {
                double t = (double)i / outputSegmentCount;
                points[i] = GetBezierPoint(t, controlPoints.points, 0, controlPoints.points.Length);
            }
            return new BezierCurve(points);
        }

        /// <summary>
        /// author: http://stackoverflow.com/a/13948059
        /// </summary>
        private Point GetBezierPoint(double t, Point[] controlPoints, int index, int count)
        {
            if (count == 1)
                return controlPoints[index];
            var P0 = GetBezierPoint(t, controlPoints, index, count - 1);
            var P1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
            return new Point((1 - t) * P0.X + t * P1.X, (1 - t) * P0.Y + t * P1.Y);
        }
    }
}
