using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Kinect;
using Microsoft.Kinect.Face;
using KinectTracking.src.Track;
using System.Diagnostics;
using System.Configuration;
using System.IO;
using System.Threading;
using KinectTracking.src.DataStructures;
using Microsoft.Win32;
using KinectTracking.src.Tools;
using System.Timers;
using KinectTracking.src.Tools.IO;
using System.Windows.Input;
using WpfApplicationHotKey.WinApi;
using System.Windows.Shapes;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using KinectSample1.src.Tools;
using KinectSample1.src.DataStructures;
using KinectSample1.src;


namespace KinectSample1.src.Tools.GuiManagement
{
    class FacePrinter
    {
        private static FacePrinter instance = null;
        private static KinectSensor kinectSensor=null;

        int[] faceIndexes = { 4, 8, 10, 14, 18, 19, 24, 28, 91, 140, 151,
                    156, 210, 222, 241, 346, 412, 458, 469, 674, 687, 731, 758, 772, 783,
                    803, 843, 849, 933, 1072, 1090, 1104, 1117, 1307, 1327 };

        public static FacePrinter getInstance(){

            if (instance == null)
            {
                instance = new FacePrinter();
                kinectSensor = KinectSensor.GetDefault();
            }
            return instance;
        }

        public void UpdateFacePoints(Canvas c, IReadOnlyList<CameraSpacePoint> vertices, List<Ellipse> lst)
        {

            //List<Ellipse> lst = new List<Ellipse>();
            List<DepthSpacePoint> listpoint = new List<DepthSpacePoint>();

            if (vertices.Count > 0)
            {
                if (lst.Count == 0)
                {
                    for (int index = 0; index < vertices.Count; index++)
                    {
                        Ellipse ellipse = new Ellipse
                        {
                            Width = 2.0,
                            Height = 2.0,
                            Fill = new SolidColorBrush(Colors.Blue)
                        };

                        lst.Add(ellipse);
                    }

                    foreach (Ellipse ellipse in lst)
                    {
                        c.Children.Add(ellipse);
                    }
                }


                for (int i = 0; i < vertices.Count; i++)
                {
                    CameraSpacePoint vertice = vertices[i];
                    DepthSpacePoint point = kinectSensor.CoordinateMapper.MapCameraPointToDepthSpace(vertice);
                    listpoint.Add(point);
                }

                //color the named face spots from HighDetailFacePoints red:
                foreach (int index in faceIndexes)
                {
                    DepthSpacePoint point = listpoint[index];

                    if (float.IsInfinity(point.X) || float.IsInfinity(point.Y)) return;

                    Ellipse ellipse = lst[index];

                    ellipse.Fill = new SolidColorBrush(Colors.Red);

                    Canvas.SetLeft(ellipse, point.X);
                    Canvas.SetTop(ellipse, point.Y);
                }

                for (int index = 0; index < vertices.Count; index++)
                {

                    DepthSpacePoint point = listpoint[index];

                    if (float.IsInfinity(point.X) || float.IsInfinity(point.Y)) return;

                    Ellipse ellipse = lst[index];

                    float hgt = (float)c.ActualHeight;
                    float wth = (float)c.ActualHeight;


                    if (c.ActualHeight == 0)
                    {
                        hgt = (float)c.Height;
                    }

                    if (c.ActualWidth == 0)
                    {
                        wth = (float)c.Width;
                    }

                    Point pnt = PanToPosition(point.X, point.Y, new Point(listpoint[18].X, listpoint[18].Y), new Point(wth / 2, hgt / 2));

                    point.X = (float)pnt.X;
                    point.Y = (float)pnt.Y;

                    Canvas.SetLeft(ellipse, point.X);
                    Canvas.SetTop(ellipse, point.Y);
                }
            }

        }


        public Point PanToPosition(double x, double y, Point center, Point origin)
        {

            Point start = new Point(x, y);
            Point p = new Point(center.X, center.Y);
            Vector v = p - start;
            return new Point(origin.X - v.X, origin.Y - v.Y);
        }


        public List<CameraSpacePoint> FacePointsToCameraPoints(List<FacePoints> flp)
        {

            List<CameraSpacePoint> printable = new List<CameraSpacePoint>();


            foreach (FacePoints f in flp)
            {
                CameraSpacePoint csp = new CameraSpacePoint();
                csp.X = f.x;
                csp.Y = f.y;
                csp.Z = f.z;

                printable.Insert(f.id, csp);
            }

            return printable;
        }



    }
}
