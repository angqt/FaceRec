using Microsoft.Kinect;
using System;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using KinectTracking.src.DataStructures;
using KinectTracking.src.Tools;

namespace KinectTracking.src.Track
{
    class Tracker
    {
        private string coordLogPath;
        private string orientationLogPath;
        private string bezierLogPath;
        private int frameID = 0;
        private Frames frameBuffer;
        string coordinateFrameString = "";
        string orientationFrameString = "";
        private Buffers buffers;
        private Frame frame;
        public bool recognisingGestures = true;

        //Markers
        public int MarkerCount = 4;
        public int CurrentMarker = 0;

        public Tracker()
        {
            CreateLogFiles();

            frameBuffer = new Frames(GlobalVariables.BufferFrameCount);

            buffers = new Buffers();
        }

        public Buffers GetRecognitionBuffers()
        {
            return buffers;
        }

        public Frames getBuffer()
        {
            return frameBuffer;
        }

        public Frame GetCurrentFrame()
        {
            return frame;
        }

        /// <summary>
        /// Saves (into buffer and logfile) one frame: 75 coordinate points and 100 orientation points, 
        /// 2 bezier curve variances for X and Y and 1 mean.
        /// Fills coordBuffer and orientationBuffer LinkedLists.
        /// </summary>
        /// <param name="body"></param>
        public void saveFrame(Body body)
        {
            DateTime timestamp = DateTime.UtcNow;

            //logging framedata:
            frame = GetFrame(OrderJoints(body), body, timestamp);

            //marking frames:
            MarkFrames();

            //LogBezierCurve(frame, timestamp);

            //live tracking:
            if (recognisingGestures)
            {
                buffers.addFrame(frame);
            }
        }

        private void MarkFrames()
        {
            string marker = "," + CurrentMarker;
            File.AppendAllText(coordLogPath, marker + Environment.NewLine);
            //File.AppendAllText(orientationLogPath, marker + Environment.NewLine);
        }

        private void LogBezierCurve(Frame frame, DateTime timestamp)
        {
            LogTools.LogBezierCurve(frame, bezierLogPath, timestamp);
        }

        /// <summary>
        /// Gets frame, coordinate and orientation points ordered as specified with orderedJoints. 
        /// +Logging to logfiles(database)
        /// </summary>
        /// <param name="orderedJoints"></param>
        private Frame GetFrame(Queue<Joint> orderedJoints, Body body, DateTime timestamp)
        {
            int i = 0;

            Frame frame = new Frame(frameID, timestamp);

            //logging frame header
            InitFrameHeaders(timestamp);

            //loop through joints to get frame data

            while (orderedJoints.Count > 0)
            {
                Joint joint = orderedJoints.Dequeue();
                //JointOrientation orientation = body.JointOrientations[joint.JointType];

                frame.joints[i].position.X = joint.Position.X;
                frame.joints[i].position.Y = joint.Position.Y;
                frame.joints[i].position.Z = joint.Position.Z;
                /*frame.joints[i].orientation.W = orientation.Orientation.W;
                frame.joints[i].orientation.X = orientation.Orientation.X;
                frame.joints[i].orientation.Y = orientation.Orientation.Y;
                frame.joints[i].orientation.Z = orientation.Orientation.Z;*/

                frame.joints[i].trackingState = joint.TrackingState;

                //logging:
                if (joint.TrackingState.Equals(TrackingState.Tracked))
                {
                    coordinateFrameString += GetCoordinateString(joint);
                    //orientationFrameString += GetOrientationString(orientation);
                } else
                {
                    coordinateFrameString += GetUntrackedCoordinateString(joint);
                    //orientationFrameString += GetUntrackedOrientationString(orientation);
                }
                i++;
            }

            //vajalik vist ainult replay nupu funktsionaalsuseks:
            frameBuffer.Add(frame);

            //logging:
            File.AppendAllText(coordLogPath, coordinateFrameString);
            //File.AppendAllText(orientationLogPath, orientationFrameString);

            return frame;
        }

        private void InitFrameHeaders(DateTime timestamp)
        {
            coordinateFrameString = 
            orientationFrameString = 
                frameID++.ToString() +
                "," + timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        private Queue<Joint> OrderJoints(Body body)
        {
            Queue<Joint> myOrderedJoints = new Queue<Joint>();
            myOrderedJoints.Enqueue(body.Joints[JointType.Head]);
            myOrderedJoints.Enqueue(body.Joints[JointType.Neck]);
            myOrderedJoints.Enqueue(body.Joints[JointType.SpineShoulder]);
            myOrderedJoints.Enqueue(body.Joints[JointType.SpineMid]);
            myOrderedJoints.Enqueue(body.Joints[JointType.SpineBase]);
            myOrderedJoints.Enqueue(body.Joints[JointType.ShoulderLeft]);
            myOrderedJoints.Enqueue(body.Joints[JointType.ElbowLeft]);
            myOrderedJoints.Enqueue(body.Joints[JointType.WristLeft]);
            myOrderedJoints.Enqueue(body.Joints[JointType.HandLeft]);
            myOrderedJoints.Enqueue(body.Joints[JointType.ThumbLeft]);
            myOrderedJoints.Enqueue(body.Joints[JointType.HandTipLeft]);
            myOrderedJoints.Enqueue(body.Joints[JointType.ShoulderRight]);
            myOrderedJoints.Enqueue(body.Joints[JointType.ElbowRight]);
            myOrderedJoints.Enqueue(body.Joints[JointType.WristRight]);
            myOrderedJoints.Enqueue(body.Joints[JointType.HandRight]);
            myOrderedJoints.Enqueue(body.Joints[JointType.ThumbRight]);
            myOrderedJoints.Enqueue(body.Joints[JointType.HandTipRight]);
            myOrderedJoints.Enqueue(body.Joints[JointType.HipLeft]);
            myOrderedJoints.Enqueue(body.Joints[JointType.KneeLeft]);
            myOrderedJoints.Enqueue(body.Joints[JointType.AnkleLeft]);
            myOrderedJoints.Enqueue(body.Joints[JointType.FootLeft]);
            myOrderedJoints.Enqueue(body.Joints[JointType.HipRight]);
            myOrderedJoints.Enqueue(body.Joints[JointType.KneeRight]);
            myOrderedJoints.Enqueue(body.Joints[JointType.AnkleRight]);
            myOrderedJoints.Enqueue(body.Joints[JointType.FootRight]);
            return myOrderedJoints;
        }

        public void openCoordLogFile()
        {
            OpenLogFile(coordLogPath);
        }

        public void openOrientationLogFile()
        {
            OpenLogFile(orientationLogPath);
        }

        private void OpenLogFile(string fileLocation)
        {
            if (File.Exists(fileLocation)) { Process.Start(fileLocation); }
            else {  MessageBox.Show("Log file does not exist! (..until kinect starts tracking your body)"); }
        }

        private string GetCoordinateString(Joint joint)
        {
            return "," + joint.Position.X + "," + joint.Position.Y + "," + joint.Position.Z;
        }

        private string GetOrientationString(JointOrientation jointOrientation)
        {
            return "," + jointOrientation.Orientation.W +
                "," + jointOrientation.Orientation.X +
                "," + jointOrientation.Orientation.Y +
                "," + jointOrientation.Orientation.Z;
        }

        private string GetUntrackedCoordinateString(Joint joint)
        {
            return "," + GlobalVariables.untrackedMarker + joint.Position.X 
                + "," + GlobalVariables.untrackedMarker + joint.Position.Y
                + "," + GlobalVariables.untrackedMarker + joint.Position.Z;
        }

        private string GetUntrackedOrientationString(JointOrientation jointOrientation)
        {
            return "," + GlobalVariables.untrackedMarker + jointOrientation.Orientation.W +
                "," + GlobalVariables.untrackedMarker + jointOrientation.Orientation.X +
                "," + GlobalVariables.untrackedMarker + jointOrientation.Orientation.Y +
                "," + GlobalVariables.untrackedMarker + jointOrientation.Orientation.Z;
        }

        private void CreateLogFiles()
        {
            DateTime dateTime = DateTime.Now;
            coordLogPath = ConfigurationManager.AppSettings["coordLogFile"] +
                dateTime.ToString("yyyyMMdd;HHmmss") + ".csv";
            orientationLogPath = ConfigurationManager.AppSettings["orientationLog"] +
                dateTime.ToString("yyyyMMdd;HHmmss") + ".csv";
            bezierLogPath = ConfigurationManager.AppSettings["bezierLog"] +
                dateTime.ToString("yyyyMMdd;HHmmss") + ".csv";

            //CreateLogDirectories();
        }

        private void CreateLogDirectories()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(coordLogPath));
            Directory.CreateDirectory(Path.GetDirectoryName(orientationLogPath));
            Directory.CreateDirectory(Path.GetDirectoryName(bezierLogPath));
            Directory.CreateDirectory(Path.GetDirectoryName(ConfigurationManager.AppSettings["bezierTestLog"]));
        }

        public string GetCoordLogPath()
        {
            return coordLogPath;
        }
    }
}
