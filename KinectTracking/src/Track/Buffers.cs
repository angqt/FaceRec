using KinectTracking.src.Tools;
using KinectTracking.src.Tools.IO;
using KinectTracking.src.Track.GestureRecognition;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using KinectTracking.src.DataStructures;

namespace KinectTracking.src.DataStructures
{
    /// <summary>
    /// Holds buffers used for gesture recognition.
    /// </summary>
    class Buffers
    {
        private FixableSizeLinkedList<Frames> buffers;
        System.Timers.Timer aTimer;
        private Boolean timeToAddBuffer;
        List<BezierGesture> bezierGesturesDB;
        src.Math.Math math;
        public DistanceCalculator calculator;
        GestureRecogniser gestureRecogniser;

        public event RecognitionHandler Recognised;
        public EventArgs e = null;
        public delegate void RecognitionHandler(Buffers buffers, EventArgs e);

        public Buffers()
        {
            //read in Database:
            bezierGesturesDB = LogTools.ReadBezierDB();

            gestureRecogniser = new GestureRecogniser();

            this.buffers = new FixableSizeLinkedList<Frames>(GlobalVariables.NrOfBuffers);

            math = new src.Math.Math();
            calculator = new DistanceCalculator();
            calculator.Init();

            Thread TimerThread = new Thread(() => StartBufferTimer());
            TimerThread.Start();
        }

        public List<BezierGesture> getDB()
        {
            return bezierGesturesDB;
        }

        private void StartBufferTimer()
        {
            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = GlobalVariables.BufferCreationInterval;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            timeToAddBuffer = true;
        }

        /// <summary>
        /// add frame to all active buffers
        /// after some interval(tracked in this class), create new buffer
        /// search for gesture from every active buffer
        /// full buffers are deactivated
        /// </summary>
        /// <param name="frame"></param>
        public void addFrame(Frame frame)
        {
            if (timeToAddBuffer)
            {
                buffers.AddLast(new Frames(GlobalVariables.BufferFrameCount));
            }
            frame.CalculateBezierCurveAttributes(GlobalVariables.BodypartsForBezierCurve);
            IEnumerator enumerator = buffers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Frames buffer = (Frames)enumerator.Current;
                if (buffer.IsActive())
                {
                    buffer.Add(frame);
                    if (buffer.Count() < GlobalVariables.MinGestureFramesCount) { break; } 
                    try
                    {
                        bool isGesture = gestureRecogniser.IsThisGesture(buffer, bezierGesturesDB);
                        if (isGesture)
                        {
                            //MessageBox.Show("Leidsin!");
                            calculator.WorkWithFrames(buffer);
                            calculator.ShowResult();
                            LogTools.LogRandomData(buffer);
                            this.buffers = new FixableSizeLinkedList<Frames>();

                            //send event to mainwindow:
                            //Recognised(this, e);

                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                    if (buffer.IsFull())
                    {
                        buffer.Deactivate();
                    }
                }
            }
            timeToAddBuffer = false;
        }
    }
}
