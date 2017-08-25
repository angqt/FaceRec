using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KinectTracking.src.DataStructures;
using System.Windows;

namespace KinectTracking.src.Tools.IO
{
    class DistanceCalculator : FrameReader
    {
        public override void WorkWithFiles() { }
        public override void ShowResult()
        {
            string result = "";
            foreach (string s in answerType)
            {
                result += s + "\n";
            }
            answerType = new List<string>();
            MessageBox.Show(result);
        }
        public string Result()
        {
            string result = "";
            foreach (string s in answerType)
            {
                result += s + "\n";
            }
            answerType = new List<string>();
            return result;
        }

        public override void Init()
        {
            gestureAnswer = "";
            answerType = new List<string>();
            bezierGestures = LogTools.ReadBezierDB();
            math = new src.Math.Math();
        }

        public override void WorkWithFrames(Frames frames)
        {
            answerType = FindClosestGesture(frames);
        }

        private List<string> FindClosestGesture(Frames frames)
        {
            frames.CalculateBezierCurveAttributes(GlobalVariables.BodypartsForBezierCurve);

            List<BezierGesture> answer = math.CalcDistanceFromDBGestures(frames, bezierGestures);
            List<String> gestureAnswerType = new List<String>();
            List<String> answerType = new List<String>();

            answer = answer.OrderBy(x => x.distanceX).ToList();
            try
            {
                gestureAnswerType.Add((from item in answer.Take(GlobalVariables.nrOfClosestGesturesComparedKNN)
                                       group item by (item.gestureType + item.personName) into g
                                       orderby g.Count() descending
                                       select new { Item = g.Key }).First().ToString());
                answer = answer.OrderBy(x => x.distanceY).ToList();
                gestureAnswerType.Add((from item in answer.Take(GlobalVariables.nrOfClosestGesturesComparedKNN)
                                       group item by (item.gestureType + item.personName) into g
                                       orderby g.Count() descending
                                       select new { Item = g.Key }).First().ToString());
                answer = answer.OrderBy(x => x.distanceMean).ToList();
                gestureAnswerType.Add((from item in answer.Take(GlobalVariables.nrOfClosestGesturesComparedKNN)
                                       group item by (item.gestureType + item.personName) into g
                                       orderby g.Count() descending
                                       select new { Item = g.Key }).First().ToString());

                answerType.Add((from myAnswer in gestureAnswerType
                                group myAnswer by myAnswer into g
                                orderby g.Count() descending
                                select new { Item = g.Key, Count = g.Count() }).First().ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show("Do you even have a database?", "Error");
            }

            //LogTools.LogDistance(answer, "distance_from_recorded_gestures");
            return answerType;
        }
    }
}
