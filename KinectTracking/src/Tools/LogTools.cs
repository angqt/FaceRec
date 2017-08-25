using KinectTracking.src.DataStructures;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Configuration;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;



namespace KinectTracking.src.Tools
{
    public static class LogTools
    {

        public static void saveFaceJPG(BitmapSource multiFrame_bitmapSource)
        {
            String filelocation = "facePictures\\" +
                DateTime.Now.ToString("yyyyMMdd;HHmmss") + ".jpg";

            CreateDirectory(filelocation);

            using (var fileStream = new FileStream(filelocation, FileMode.Create))
            {
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(multiFrame_bitmapSource));
                encoder.Save(fileStream);
            }
        }



        public static void LogFace(IReadOnlyList<CameraSpacePoint> vertices) 
        {
            String filelocation = GetNewFilePath();
            String finalLog = "";
            for (int index = 0; index < vertices.Count; index++)
            {
                String result = "";
                result += index.ToString();
                result += "," + vertices[index].X.ToString()
                    + "," + vertices[index].Y.ToString()
                    + "," + vertices[index].Z.ToString();

                finalLog += result + Environment.NewLine;
            }

            File.AppendAllText(filelocation, finalLog);
        }


        public static void LogBezierCurveInterval(List<BezierGesture> bezierGestures, String fileTag)
        {
            Math.Math math = new Math.Math();
            String filelocation = GetNewFilePath(fileTag);

            foreach (BezierGesture gesture in bezierGestures)
            {
                int interval = math.GetInterval(gesture);
                String result = "";
                result += ((BezierAttributes)gesture.frames.GetAttributes()[0 * interval]).varY.ToString();
                for (int i = 1; i < GlobalVariables.NrOfPointsOnBezierCurveCompared; i++)
                {
                    result += "," + ((BezierAttributes)gesture.frames.GetAttributes()[i * interval]).varY.ToString();
                }

                File.AppendAllText(filelocation, result + Environment.NewLine);
            }
        }

        public static void LogRandomData(ArrayList frames)
        {
            String filelocation = GetNewFilePath("DataForClustering");
            foreach (Frame frame in frames)
            {
                File.AppendAllText(filelocation, frame.varianceX + ",");
            }
            foreach (Frame frame in frames)
            {
                File.AppendAllText(filelocation, frame.varianceY + ",");
            }
            foreach (Frame frame in frames)
            {
                File.AppendAllText(filelocation, frame.mean + ",");
            }
            File.AppendAllText(filelocation, Environment.NewLine);
        }
        public static void LogRandomData(Frames gesture)
        {
            String filelocation = GetNewFilePath("frameIDs");
            foreach (Frame frame in gesture)
            {
                File.AppendAllText(filelocation, frame.id + Environment.NewLine);
            }
        }
        public static void LogRandomData(List<BezierGesture> gestures)
        {
            String filelocation = GetNewFilePath();
            foreach (BezierGesture gesture in gestures)
            {
                File.AppendAllText(filelocation,
                    gesture.distanceX
                    + "," + gesture.distanceY
                    + "," + gesture.distanceMean
                    + "," + gesture.gestureType
                    + "," + gesture.personName
                    + Environment.NewLine);
            }
        }
        public static void LogRandomData(List<BezierGesture> gestures, String fileTag)
        {
            String filelocation = GetNewFilePath(fileTag);
            foreach (BezierGesture gesture in gestures)
            {
                File.AppendAllText(filelocation, gesture.distanceY + Environment.NewLine);
            }
        }
        public static void LogDistance(List<BezierGesture> gestures, String fileTag)
        {
            gestures = gestures.OrderBy(x => x.distanceX).ToList();
            String filelocation = GetNewFilePath(fileTag);
            foreach (BezierGesture gesture in gestures)
            {
                File.AppendAllText(filelocation,
                    gesture.distanceX 
                    + "," + gesture.distanceY
                    + "," + gesture.distanceMean
                    + "," + gesture.gestureType.ToString() 
                    + "," + gesture.personName
                    + Environment.NewLine);
            }

            gestures = gestures.OrderBy(x => x.distanceY).ToList();
            String filelocation2 = GetNewFilePath(fileTag);
            foreach (BezierGesture gesture in gestures)
            {
                File.AppendAllText(filelocation2,
                    gesture.distanceX
                    + "," + gesture.distanceY
                    + "," + gesture.distanceMean
                    + "," + gesture.gestureType.ToString()
                    + "," + gesture.personName
                    + Environment.NewLine);
            }

            gestures = gestures.OrderBy(x => x.distanceMean).ToList();
            String filelocation3 = GetNewFilePath(fileTag);
            foreach (BezierGesture gesture in gestures)
            {
                File.AppendAllText(filelocation3,
                    gesture.distanceX
                    + "," + gesture.distanceY
                    + "," + gesture.distanceMean
                    + "," + gesture.gestureType.ToString()
                    + "," + gesture.personName
                    + Environment.NewLine);
            }
        }

        public static void LogBezierCurve(Frames frames, String personName)
        {
            DateTime time = DateTime.Now;
            String bezierLog = ConfigurationManager.AppSettings["bezierTestLog"] +
                time.ToString("yyyyMMdd;HHmmssfff") + "_generated_from_buffer.csv";
            foreach (Frame frame in frames)
            {
                time = frame.timestamp;
                LogBezierCurve(frame, bezierLog, time, personName);
            }
        }

        public static void LogBezierCurve(Frame frame, string logLocation, DateTime timestamp, String name)
        {
            String bezierLog = frame.id.ToString()
                 + "," + timestamp.ToString("yyyy-MM-dd-HH:mm:ss.fff")
                 + "," + name;
            LogBezier(frame, logLocation, timestamp, bezierLog);
        }

        public static void LogBezierCurve(Frame frame, string logLocation, DateTime timestamp)
        {
            String bezierLog = frame.id.ToString()
                 + "," + timestamp.ToString("yyyy-MM-dd-HH:mm:ss.fff");
            LogBezier(frame, logLocation, timestamp, bezierLog);
        }

        /// <summary>
        /// Log only if all joints used for Bezier curve are tracked
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="logLocation"></param>
        /// <param name="timestamp"></param>
        private static void LogBezier(Frame frame, string logLocation, DateTime timestamp, String bezierLog)
        {
            if (!AllBezierJointsTracked(frame))
            {
                return;
            }
            
            frame.CalculateBezierCurveAttributes(GlobalVariables.BodypartsForBezierCurve);

            bezierLog += 
                "," + frame.varianceX
                + "," + frame.varianceY
                + "," + frame.mean;

            File.AppendAllText(logLocation, bezierLog + Environment.NewLine);
        }

        private static bool AllBezierJointsTracked(Frame frame)
        {
            foreach (MyBody.BodyParts joint in GlobalVariables.BodypartsForBezierCurve)
            {
                if (!frame.joints[(int)joint].trackingState.Equals(TrackingState.Tracked))
                {
                    return false;
                }

            }
            return true;
        }

        public static void MessageBoxShowBezierGestures(List<BezierGesture> gestures)
        {
            MessageBox.Show(
            gestures[0].gestureType.ToString() + ": " + gestures[0].personName.ToString() + " - " + gestures[0].distanceX.ToString() + " - " + gestures[0].distanceY.ToString() + " - " + gestures[0].distanceMean.ToString() + "\n"
            + gestures[1].gestureType.ToString() + ": " + gestures[1].personName.ToString() + " - " + gestures[1].distanceX.ToString() + " - " + gestures[1].distanceY.ToString() + " - " + gestures[1].distanceMean.ToString() + "\n"
            + gestures[2].gestureType.ToString() + ": " + gestures[2].personName.ToString() + " - " + gestures[2].distanceX.ToString() + " - " + gestures[2].distanceY.ToString() + " - " + gestures[2].distanceMean.ToString() + "\n"
            + gestures[3].gestureType.ToString() + ": " + gestures[3].personName.ToString() + " - " + gestures[3].distanceX.ToString() + " - " + gestures[3].distanceY.ToString() + " - " + gestures[3].distanceMean.ToString() + "\n"
            + gestures[4].gestureType.ToString() + ": " + gestures[4].personName.ToString() + " - " + gestures[4].distanceX.ToString() + " - " + gestures[4].distanceY.ToString() + " - " + gestures[4].distanceMean.ToString() + "\n"
            + gestures[5].gestureType.ToString() + ": " + gestures[5].personName.ToString() + " - " + gestures[5].distanceX.ToString() + " - " + gestures[5].distanceY.ToString() + " - " + gestures[5].distanceMean.ToString() + "\n"
            + gestures[6].gestureType.ToString() + ": " + gestures[6].personName.ToString() + " - " + gestures[6].distanceX.ToString() + " - " + gestures[6].distanceY.ToString() + " - " + gestures[6].distanceMean.ToString() + "\n"
            + gestures[7].gestureType.ToString() + ": " + gestures[7].personName.ToString() + " - " + gestures[7].distanceX.ToString() + " - " + gestures[7].distanceY.ToString() + " - " + gestures[7].distanceMean.ToString() + "\n"
            + gestures[8].gestureType.ToString() + ": " + gestures[8].personName.ToString() + " - " + gestures[8].distanceX.ToString() + " - " + gestures[8].distanceY.ToString() + " - " + gestures[8].distanceMean.ToString() + "\n"
            + gestures[9].gestureType.ToString() + ": " + gestures[9].personName.ToString() + " - " + gestures[9].distanceX.ToString() + " - " + gestures[9].distanceY.ToString() + " - " + gestures[9].distanceMean.ToString() + "\n"
            );
        }

        public static Frames readCoordinateLog(System.IO.Stream fileStream)
        {
            Frames frames = new Frames();
            Frame frame;
            int i;
            try {
                var reader = new StreamReader(fileStream);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    i = GlobalVariables.CSVBodyPositionStartingIndex;
                    frame = new Frame();
                    frame.id = Convert.ToInt32(values[0]);
                    frame.timestamp = Convert.ToDateTime(values[1]);
                    for (int j = 0; j < GlobalVariables.NrOfKinectBodyParts; j++)
                    {
                        if (values[i].Contains(GlobalVariables.untrackedMarker))
                        {
                            frame.joints[j].position.X = Convert.ToSingle(
                                values[i].Substring(
                                    values[i].IndexOf(GlobalVariables.untrackedMarker) 
                                    + GlobalVariables.untrackedMarker.Length));
                            i++;
                            frame.joints[j].position.Y = Convert.ToSingle(
                                values[i].Substring(
                                    values[i].IndexOf(GlobalVariables.untrackedMarker)
                                    + GlobalVariables.untrackedMarker.Length));
                            i++;
                            frame.joints[j].position.Z = Convert.ToSingle(
                                values[i].Substring(
                                    values[i].IndexOf(GlobalVariables.untrackedMarker)
                                    + GlobalVariables.untrackedMarker.Length));
                            i++;
                            frame.joints[j].trackingState = TrackingState.NotTracked;
                        } else
                        {
                            frame.joints[j].position.X = Convert.ToSingle(values[i++]);
                            frame.joints[j].position.Y = Convert.ToSingle(values[i++]);
                            frame.joints[j].position.Z = Convert.ToSingle(values[i++]);
                            frame.joints[j].trackingState = TrackingState.Tracked;
                        }
                    }
                    if (values[values.Count()-1].Equals(GlobalVariables.GestureMark))
                    {
                        frame.marker = true;
                    }
                    frames.Add(frame);
                }
            } catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "File read error.");
            }
            return frames;
        }

        internal static void LogMarkedGestures(IEnumerable<string> gesture)
        {
            String filepath = ConfigurationManager.AppSettings["extractedGestures"] +
                DateTime.Now.ToString("yyyyMMdd;HHmmssfff") + ".csv";
            CreateDirectory(filepath);

            bool isGesture = false;
            foreach (string line in gesture)
            {
                if (line.Contains(GlobalVariables.GestureMark))
                {
                    if (isGesture)
                    {
                        filepath = ConfigurationManager.AppSettings["extractedGestures"] +
                            DateTime.Now.ToString("yyyyMMdd;HHmmssfff") + ".csv";
                    } else
                    {
                        isGesture = true;
                    }
                }
                if (isGesture)
                {
                    File.AppendAllText(filepath, line + Environment.NewLine);
                }

            }
        }

        public static List<BezierGesture> ReadBezierDB()
        {
            List<BezierGesture> gestures = new List<BezierGesture>();
            String baseFolderPath = ConfigurationManager.AppSettings["bezierGestures"];
            CreateDirectory(baseFolderPath);
            CreateDirectory(baseFolderPath + "Circle\\");
            CreateDirectory(baseFolderPath + "Handwave\\");
            ReadBezierGesturesFromDirecory(gestures, baseFolderPath + "Circle", MyBody.Gestures.Circle);
            ReadBezierGesturesFromDirecory(gestures, baseFolderPath + "Handwave", MyBody.Gestures.HandWave);
            /*CreateDirectory(baseFolderPath + "Pickup");*/
            /*CreateDirectory(baseFolderPath + "BringItem");
            CreateDirectory(baseFolderPath + "PullOut");
            CreateDirectory(baseFolderPath + "PutBack");*/
            /*ReadBezierGesturesFromDirecory(gestures, baseFolderPath + "Pickup", Gestures.Pickup);*/
            /*ReadBezierGesturesFromDirecory(gestures, baseFolderPath + "BringItem", Gestures.BringItem);
            ReadBezierGesturesFromDirecory(gestures, baseFolderPath + "PullOut", Gestures.PullOut);
            ReadBezierGesturesFromDirecory(gestures, baseFolderPath + "PutBack", Gestures.PutBack);*/
            return gestures;
        }

        private static List<BezierGesture> ReadBezierGesturesFromDirecory(List<BezierGesture> gestures, string dirPath, MyBody.Gestures gestureType)
        {
            string[] tempVar = new string[6];
            foreach (string file in Directory.EnumerateFiles(dirPath, "*.csv"))
            {
                try
                {
                    var reader = new StreamReader(File.OpenRead(file));
                    BezierGesture gesture = new BezierGesture();
                    gesture.gestureType = gestureType;
                    gestures.Add(gesture);
                    BezierFrames bezierFrames = new BezierFrames();
                    gesture.frames = bezierFrames;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        tempVar = values;
                        int id = Convert.ToInt32(values[0]);
                        string name = values[2];
                        double varX = Convert.ToDouble(values[3], System.Globalization.CultureInfo.InvariantCulture);
                        double varY = Convert.ToDouble(values[4], System.Globalization.CultureInfo.InvariantCulture);
                        double mean = Convert.ToDouble(values[5], System.Globalization.CultureInfo.InvariantCulture);
                        BezierAttributes attribute = new BezierAttributes(
                            id
                            , name//its not really necessary to have this value in BezierAttributes
                            , varX
                            , varY
                            , mean
                        );
                        bezierFrames.Add(attribute);
                    }
                    gesture.personName = bezierFrames.GetAttributes()[0].personName;
                }
                catch (Exception e)
                {
                    String tempS = ",int - " + tempVar[0]
                        + "\n" + ",string - " + tempVar[2]
                        + "\n" + ",double - " + tempVar[3]
                        + "\n" + ",double - " + tempVar[4]
                        + "\n" + ",double - " + tempVar[5];
                    MessageBox.Show(
                        tempS + e
                        , "DB read error."
                        );
                    throw e;
                }
            }
            return gestures;
        }

        public static void CreateDirectory(String filelocation)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filelocation));
        }
        private static string RandomFilePath()
        {
            return ConfigurationManager.AppSettings["randomData"] +
                DateTime.Now.ToString("yyyyMMdd;HHmmssfff") + ".csv"; ;
        }
        private static string RandomFilePath(String fileTag)
        {
            return ConfigurationManager.AppSettings["randomData"] +
                DateTime.Now.ToString("yyyyMMdd;HHmmssfff") + "-" + fileTag + ".csv"; ;
        }
        private static string GetNewFilePath()
        {
            String filelocation = RandomFilePath();
            CreateDirectory(filelocation);
            return filelocation;
        }
        private static string GetNewFilePath(String fileTag)
        {
            String filelocation = RandomFilePath(fileTag);
            CreateDirectory(filelocation);
            return filelocation;
        }
    }
}
