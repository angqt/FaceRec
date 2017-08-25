using KinectTracking.src.DataStructures;
using Microsoft.Kinect;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System;
using KinectTracking.src.Tools;

///<summary>
///author: http://pterneas.com/2014/03/13/kinect-for-windows-version-2-body-tracking/
///</summary>
namespace KinectTracking
{
    public static class Extensions
    {
        private const int ZDimensionMultiplier = 50;
        #region Camera

        public static ImageSource ToBitmap(this ColorFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            byte[] pixels = new byte[width * height * ((format.BitsPerPixel + 7) / 8)];

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
        }

        public static ImageSource ToBitmap(this DepthFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            ushort minDepth = frame.DepthMinReliableDistance;
            ushort maxDepth = frame.DepthMaxReliableDistance;

            ushort[] pixelData = new ushort[width * height];
            byte[] pixels = new byte[width * height * (format.BitsPerPixel + 7) / 8];

            frame.CopyFrameDataToArray(pixelData);

            int colorIndex = 0;
            for (int depthIndex = 0; depthIndex < pixelData.Length; ++depthIndex)
            {
                ushort depth = pixelData[depthIndex];

                byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                pixels[colorIndex++] = intensity; // Blue
                pixels[colorIndex++] = intensity; // Green
                pixels[colorIndex++] = intensity; // Red

                ++colorIndex;
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
        }

        public static ImageSource ToBitmap(this InfraredFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            ushort[] frameData = new ushort[width * height];
            byte[] pixels = new byte[width * height * (format.BitsPerPixel + 7) / 8];

            frame.CopyFrameDataToArray(frameData);

            int colorIndex = 0;
            for (int infraredIndex = 0; infraredIndex < frameData.Length; infraredIndex++)
            {
                ushort ir = frameData[infraredIndex];

                byte intensity = (byte)(ir >> 7);

                pixels[colorIndex++] = (byte)(intensity / 1); // Blue
                pixels[colorIndex++] = (byte)(intensity / 1); // Green   
                pixels[colorIndex++] = (byte)(intensity / 0.4); // Red

                colorIndex++;
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
        }

        #endregion

        #region Body

        public static Joint ScaleTo(this Joint joint, double width, double height, float skeletonMaxX, float skeletonMaxY)
        {
            joint.Position = new CameraSpacePoint
            {
                X = Scale(width, skeletonMaxX, joint.Position.X),
                Y = Scale(height, skeletonMaxY, -joint.Position.Y),
                Z = joint.Position.Z
            };

            return joint;
        }

        public static Joint ScaleTo(this Joint joint, double width, double height)
        {
            return ScaleTo(joint, width, height, 1.0f, 1.0f);
        }

        private static float Scale(double maxPixel, double maxSkeleton, float position)
        {
            float value = (float)((((maxPixel / maxSkeleton) / 2) * position) + (maxPixel / 2));

            if (value > maxPixel)
            {
                return (float)maxPixel;
            }

            if (value < 0)
            {
                return 0;
            }

            return value;
        }

        #endregion

        #region Drawing

        public static void DrawSkeleton(this Canvas canvas, Body body, Canvas topProjCanvas, Canvas sideProjCanvas)
        {
            if (body == null) return;

            foreach (Joint joint in body.Joints.Values)
            {
                canvas.DrawPoint(joint, topProjCanvas, sideProjCanvas);
            }

            canvas.DrawLine(body.Joints[JointType.Head], body.Joints[JointType.Neck], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.Neck], body.Joints[JointType.SpineShoulder], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderLeft], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderRight], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.SpineMid], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.WristRight], body.Joints[JointType.HandRight], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.HandLeft], body.Joints[JointType.HandTipLeft], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.HandRight], body.Joints[JointType.HandTipRight], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.HandLeft], body.Joints[JointType.ThumbLeft], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.HandRight], body.Joints[JointType.ThumbRight], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.SpineMid], body.Joints[JointType.SpineBase], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipLeft], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipRight], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.KneeLeft], body.Joints[JointType.AnkleLeft], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.KneeRight], body.Joints[JointType.AnkleRight], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.AnkleLeft], body.Joints[JointType.FootLeft], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(body.Joints[JointType.AnkleRight], body.Joints[JointType.FootRight], topProjCanvas, sideProjCanvas);
        }

        public static void DrawPoint(this Canvas canvas, Joint joint, Canvas topProjCanvas, Canvas sideProjCanvas)
        {
            if (joint.TrackingState == TrackingState.NotTracked) return;

            Joint mainJoint = joint.ScaleTo(canvas.ActualWidth, canvas.ActualHeight);

            SolidColorBrush color = new SolidColorBrush(Colors.LightBlue);

            Ellipse ellipse = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = color
            };

            Canvas.SetLeft(ellipse, mainJoint.Position.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, mainJoint.Position.Y - ellipse.Height / 2);

            canvas.Children.Add(ellipse);

            Joint topJoint = joint.ScaleTo(topProjCanvas.ActualWidth, topProjCanvas.ActualHeight);

            Ellipse topEllipse = new Ellipse
            {
                Width = 7,
                Height = 7,
                Fill = color
            };
            Canvas.SetLeft(topEllipse, topJoint.Position.X - topEllipse.Width / 2);
            Canvas.SetTop(topEllipse, (topJoint.Position.Z * ZDimensionMultiplier) - topEllipse.Height / 2);

            topProjCanvas.Children.Add(topEllipse);

            Joint sideJoint = joint.ScaleTo(topProjCanvas.ActualWidth, topProjCanvas.ActualHeight);

            Ellipse sideEllipse = new Ellipse
            {
                Width = 7,
                Height = 7,
                Fill = color
            };
            Canvas.SetLeft(sideEllipse, (sideJoint.Position.Z * ZDimensionMultiplier) - sideEllipse.Width / 2);
            Canvas.SetTop(sideEllipse, sideJoint.Position.Y - sideEllipse.Height / 2);

            sideProjCanvas.Children.Add(sideEllipse);
        }

        public static void DrawLine(this Canvas canvas, Joint first, Joint second, Canvas topProjCanvas, Canvas sideProjCanvas)
        {
            if (first.TrackingState == TrackingState.NotTracked || second.TrackingState == TrackingState.NotTracked) return;

            Joint mainFirst = first.ScaleTo(canvas.ActualWidth, canvas.ActualHeight);
            Joint mainSecond = second.ScaleTo(canvas.ActualWidth, canvas.ActualHeight);

            Line line = new Line
            {
                X1 = mainFirst.Position.X,
                Y1 = mainFirst.Position.Y,
                X2 = mainSecond.Position.X,
                Y2 = mainSecond.Position.Y,
                StrokeThickness = 8,
                Stroke = new SolidColorBrush(Colors.LightBlue)
            };

            canvas.Children.Add(line);

            Joint topfirst = first.ScaleTo(topProjCanvas.ActualWidth, topProjCanvas.ActualHeight);
            Joint topsecond = second.ScaleTo(topProjCanvas.ActualWidth, topProjCanvas.ActualHeight);

            Line topline = new Line
            {
                X1 = topfirst.Position.X,
                Y1 = topfirst.Position.Z * ZDimensionMultiplier,
                X2 = topsecond.Position.X,
                Y2 = topsecond.Position.Z * ZDimensionMultiplier,
                StrokeThickness = 3,
                Stroke = new SolidColorBrush(Colors.LightBlue)
            };

            topProjCanvas.Children.Add(topline);

            Joint sidefirst = first.ScaleTo(sideProjCanvas.ActualWidth, sideProjCanvas.ActualHeight);
            Joint sidesecond = second.ScaleTo(sideProjCanvas.ActualWidth, sideProjCanvas.ActualHeight);

            Line sideline = new Line
            {
                X1 = sidefirst.Position.Z * ZDimensionMultiplier,
                Y1 = sidefirst.Position.Y,
                X2 = sidesecond.Position.Z * ZDimensionMultiplier,
                Y2 = sidesecond.Position.Y,
                StrokeThickness = 3,
                Stroke = new SolidColorBrush(Colors.LightBlue)
            };

            sideProjCanvas.Children.Add(sideline);
        }

        /// <summary>
        /// author: Siim Kirme
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="coordBufferFrame"></param>
        public static void DrawBuffer(this Canvas canvas, src.DataStructures.Frame coordBufferFrame
            , Canvas topProjCanvas, Canvas sideProjCanvas)//string[][] coordBufferFrame
        {
            if (coordBufferFrame == null) return;

            Joint[] joints = new Joint[GlobalVariables.NrOfKinectBodyParts];

            Joint joint = new Joint();
            joint.TrackingState = TrackingState.Tracked;

            //canvas.ActualWidth, canvas.ActualHeight, 1.0f
            for (int i = 0; i < coordBufferFrame.joints.Length; i++)
            {
                //float[] jointPos = getJointXYZ(coordBufferFrame.[i]);
                joint.Position = new CameraSpacePoint
                {
                    X = Convert.ToSingle(coordBufferFrame.joints[i].position.X),
                    Y = Convert.ToSingle(coordBufferFrame.joints[i].position.Y),
                    Z = Convert.ToSingle(coordBufferFrame.joints[i].position.Z)
                };
                joint.JointType = coordBufferFrame.joints[i].jointType;
                joints[i] = joint;
                canvas.DrawPoint(joint, topProjCanvas, sideProjCanvas);
            }

            /*IEnumerator e = coordBufferFrame.getEnumerator();
            while (e.MoveNext())
            {
                canvas.DrawBufferPoint(e);
            }*/

            canvas.DrawLine(joints[0], joints[1], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[1], joints[2], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[2], joints[5], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[2], joints[11], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[2], joints[3], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[5], joints[6], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[11], joints[12], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[6], joints[7], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[12], joints[13], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[7], joints[8], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[13], joints[14], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[8], joints[10], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[14], joints[16], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[8], joints[9], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[14], joints[15], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[3], joints[4], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[4], joints[17], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[4], joints[21], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[17], joints[18], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[21], joints[22], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[18], joints[19], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[22], joints[23], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[19], joints[20], topProjCanvas, sideProjCanvas);
            canvas.DrawLine(joints[23], joints[24], topProjCanvas, sideProjCanvas);
        }

        private static float[] getJointXYZ(string[] coordBufferFrame)
        {
            return new float[] { Convert.ToSingle(coordBufferFrame[0])
                , Convert.ToSingle(coordBufferFrame[1])
                , Convert.ToSingle(coordBufferFrame[2]) };
        }

        /*public static void DrawBufferPoint(this Canvas canvas, IEnumerator e)
        {
            //if (joint.TrackingState == TrackingState.NotTracked) return;

            joint = joint.ScaleTo(canvas.ActualWidth, canvas.ActualHeight);

            Ellipse ellipse = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = new SolidColorBrush(Colors.LightBlue)
            };

            Canvas.SetLeft(ellipse, joint.Position.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, joint.Position.Y - ellipse.Height / 2);

            canvas.Children.Add(ellipse);
        }*/

        #endregion
    }
}
