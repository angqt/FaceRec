using KinectTracking.src.DataStructures;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KinectTracking.src.Tools.IO
{
    abstract class FrameReader
    {
        public abstract void WorkWithFrames(Frames frames);
        public abstract void ShowResult();
        public abstract void Init();
        public abstract void WorkWithFiles();
        protected string gestureAnswer;
        protected List<BezierGesture> bezierGestures;
        protected src.Math.Math math;
        public List<String> answerType;
        protected string personName;

        public void ReadFramesFromFiles()
        {
            Init();
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = true;
            bool? userClickedOK = openFileDialog1.ShowDialog();
            if (userClickedOK == true)
            {
                WorkWithFiles();
                System.IO.Stream fileStream = null;
                foreach (Stream stream in openFileDialog1.OpenFiles())
                {
                    try
                    {
                        fileStream = stream;
                        Frames frames = LogTools.readCoordinateLog(fileStream);

                        WorkWithFrames(frames);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString(), "File read error.");
                    }
                    finally
                    {
                        if (stream != null)
                        {
                            stream.Close();
                        }
                    }
                }
            }
            ShowResult();
        }
    }
}
