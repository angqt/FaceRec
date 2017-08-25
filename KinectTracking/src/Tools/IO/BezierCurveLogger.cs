using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KinectTracking.src.DataStructures;

namespace KinectTracking.src.Tools.IO
{
    class BezierCurveLogger : FrameReader
    {
        public override void ShowResult() { }

        public override void Init() { }

        public override void WorkWithFiles()
        {
            personName = Microsoft.VisualBasic.Interaction.InputBox("Whose gestures are those?", "Gesture ID", "Siim");
        }

        public override void WorkWithFrames(Frames frames)
        {
            LogTools.LogBezierCurve(frames, personName);
        }

    }
}
