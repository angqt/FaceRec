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
using KinectSample1.src.Tools.GuiManagement;
using System.Windows.Media.Imaging;
using System.Drawing;


namespace KinectTracking
{
    public partial class MainWindow : Window
    {
        //author: http://pterneas.com/2015/06/06/kinect-hd-face/

        
        // Acquires body frame data.
        private BodyFrameSource _bodySource = null;

        // Reads body frame data.
        private BodyFrameReader _bodyReader = null;

        // Acquires HD face data.
        private HighDefinitionFaceFrameSource _faceSource = null;

        // Reads HD face data.
        private HighDefinitionFaceFrameReader _faceReader = null;

        // Required to access the face vertices.
        private FaceAlignment _faceAlignment = null;

        // Required to access the face model points.
        private FaceModel _faceModel = null;

        // Used to display 1,000 points on screen.
        private List<Ellipse> _points = new List<Ellipse>();

        //end of author: http://pterneas.com/2015/06/06/kinect-hd-face/

        private KinectSensor kinectSensor = null;
        
        private System.Timers.Timer timer;
        private const int MinimumDBSize = 30;
        private IReadOnlyList<CameraSpacePoint> savedFace = null;
        private IReadOnlyList<CameraSpacePoint> recognizedFace = null;

        private List<IReadOnlyList<CameraSpacePoint>> listSf = null;

        private FacePrinter faceP;

        private int boundariesCount = 0; //1:18
        private int faceCount = 0; //1:9
        float[,] boundaries=new float[18,3];// three columns
        private FileReader fr = new FileReader();

        MultiSourceFrameReader _reader;
        private BitmapSource multiFrame_bitmapSource;

        public MainWindow()
        {

            //author: http://pterneas.com/2015/06/06/kinect-hd-face/
            kinectSensor = KinectSensor.GetDefault();
            
            if (kinectSensor != null)
            {
                // Listen for body data.
                _bodySource = kinectSensor.BodyFrameSource;
                _bodyReader = _bodySource.OpenReader();
                _bodyReader.FrameArrived += BodyReader_FrameArrived;

                // Listen for HD face data.
                _faceSource = new HighDefinitionFaceFrameSource(kinectSensor);
                _faceReader = _faceSource.OpenReader();
                _faceReader.FrameArrived += FaceReader_FrameArrived;

                faceP = FacePrinter.getInstance();

                _faceModel = new FaceModel();
                _faceAlignment = new FaceAlignment();
                timer = new System.Timers.Timer();

                _reader = kinectSensor.OpenMultiSourceFrameReader(FrameSourceTypes.Color);
                _reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;


                // Start tracking!        
                kinectSensor.Open();
            }
            //end of author: http://pterneas.com/2015/06/06/kinect-hd-face/

            //w boundaries are the same for all of them: 0.8<=w<=1

            //frontal picture
            boundaries[0,0] = -0.1f; boundaries[0,1] = -0.05f; boundaries[0,2] = -0.05f; 
            boundaries[1,0] = 0.1f; boundaries[1,1] = 0.05f; boundaries[1,2] = 0.05f; 
            
            //rightside picture
            boundaries[2,0] = -0.1f; boundaries[2,1] = -0.4f; boundaries[2,2] = 0.01f; 
            boundaries[3,0] = -0.01f; boundaries[3,1] = -0.3f; boundaries[3,2] = 0.05f; 
            
            //leftside picture  TODO:change boundaries to make them fair
            boundaries[4,0] = -0.06f; boundaries[4,1] = 0.2f; boundaries[4,2] = -0.1f; 
            boundaries[5,0] = 0f; boundaries[5,1] = 0.6f; boundaries[5,2] = 0.07f; 
            
            //top tilt picture
            boundaries[6,0] = 0.2f; boundaries[6,1] = -0.05f; boundaries[6,2] = 0.02f; 
            boundaries[7,0] = 0.35f; boundaries[7,1] = 0.05f; boundaries[7,2] = 0.1f; 
            
            //bottom tilt
            boundaries[8,0] = -0.3f; boundaries[8,1] = -0.2f; boundaries[8,2] = 0.01f; 
            boundaries[9,0] = 0; boundaries[9,1] = -0.05f; boundaries[9,2] = 0.1f; 
            
            //rightside top tilt picture
            boundaries[10,0] = 0.1f; boundaries[10,1] = -0.4f; boundaries[10,2] = 0.1f; 
            boundaries[11,0] = 0.2f; boundaries[11,1] = -0.25f; boundaries[11,2] = 0.2f; 
            
            //leftside top tilt picture
            boundaries[12,0] = 0.05f; boundaries[12,1] = 0.2f; boundaries[12,2] = -0.5f; 
            boundaries[13,0] = 0.2f; boundaries[13,1] = 0.5f; boundaries[13,2] = -0.1f;

            //rightside bottom tilt picture TODO:change boundaries to make them fair
            boundaries[14,0] = -0.2f; boundaries[14,1] = -0.3f; boundaries[14,2] = -0.1f; 
            boundaries[15,0] = -0.1f; boundaries[15,1] = -0.2f; boundaries[15,2] = 0; 
            
            //leftside bottom tilt picture
            boundaries[16,0] = -0.3f; boundaries[16,1] = 0.2f; boundaries[16,2] = 0;
            boundaries[17,0] = -0.1f; boundaries[17,1] = 0.3f; boundaries[17,2] = 0.1f;

            listSf = new List<IReadOnlyList<CameraSpacePoint>>();

            


            InitializeComponent();

            
        }



        private ImageSource ToBitmap(ColorFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            byte[] pixels = new byte[width * height * ((PixelFormats.Bgr32.BitsPerPixel + 7) / 8)];

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




        void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            // Get a reference to the multi-frame

            var reference = e.FrameReference.AcquireFrame();


            // Open color frame
            using (var frame = reference.ColorFrameReference.AcquireFrame())
            {
                if (frame != null)
                {

                    ImageSource ims = ToBitmap(frame);

                    multiFrame_bitmapSource = ims as BitmapSource;

                }
            }

        }




        






    //author: http://pterneas.com/2015/06/06/kinect-hd-face/
        //Only a face at time
        //In order to include other faces you can use trackid to identify bodies and apply that to face's trackingid
        //You can track at most 6 bodies => 6 faces 
    private void BodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            using (var frame = e.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    Body[] bodies = new Body[frame.BodyCount];
                    frame.GetAndRefreshBodyData(bodies);

                    Body body = bodies.Where(b => b.IsTracked).FirstOrDefault();

                    if (!_faceSource.IsTrackingIdValid)
                    {
                        if (body != null)
                        {
                            _faceSource.TrackingId = body.TrackingId;
                        }
                    }
                }
            }
        }

        //author: http://pterneas.com/2015/06/06/kinect-hd-face/
        private void FaceReader_FrameArrived(object sender, HighDefinitionFaceFrameArrivedEventArgs e)
        {
            using (var frame = e.FrameReference.AcquireFrame())
            {

                if (frame != null && frame.IsFaceTracked)
                {

                    frame.GetAndRefreshFaceAlignmentResult(_faceAlignment);
                    var vertices = _faceModel.CalculateVerticesForAlignment(_faceAlignment);
                    UpdateFacePoints(canvas,vertices);

                    //faceP.UpdateFacePoints(sample, fr.readFile(faceCount), new List<Ellipse>());
                    
                    if (CheckFaceAlignment())
                    {
                        topProjectionCanvas.Children.Clear();
                        savedFace = vertices;

                        listSf.Add(savedFace);

                        //PrintFace(savedFace,topProjectionCanvas);
                        UpdateFacePoints(topProjectionCanvas, savedFace, new List<Ellipse>());
                        LogTools.saveFaceJPG(multiFrame_bitmapSource);

                        boundariesCount+=2;
                        faceCount++;

                        sample.Children.Clear();

                        if (faceCount < 9)
                        {
                            UpdateFacePoints(sample, fr.readFile(faceCount), new List<Ellipse>());
                        }
                        else
                        {
                            boundariesCount = 0;
                            faceCount = 0;
                            listSf.Clear();
                        }

                    }
                }
            }
        }

        private bool CheckFaceAlignment()
        {

            bool ret = false;

            Vector4 orientation = _faceAlignment.FaceOrientation;
            Console.WriteLine("x: " + orientation.X +" y: " + orientation.Y + " z: " + orientation.Z +" w: " + orientation.W);

            if(boundariesCount+1<boundaries.GetLength(0) && faceCount<9)
            {
                
                bool x = orientation.X <= boundaries[boundariesCount+1,0] && orientation.X >= boundaries[boundariesCount,0];
                bool y = orientation.Y <= boundaries[boundariesCount+1,1] && orientation.Y >= boundaries[boundariesCount,1];
                bool z = orientation.Z <= boundaries[boundariesCount+1,2] && orientation.Z >= boundaries[boundariesCount,2];
                bool w = orientation.W <= 1 && orientation.W >= 0.8;

                ret = (x && y && z && w);


                Console.WriteLine("x: " + x + " y: " + y + " z: " + z +" w: " + w);
            }

            return ret;
            
        }



        //author: http://pterneas.com/2015/06/06/kinect-hd-face/
        private void UpdateFacePoints(Canvas c, IReadOnlyList<CameraSpacePoint> vertices)
        {

            UpdateFacePoints(c,vertices,_points);


        }

        private void UpdateFacePoints(Canvas c, IReadOnlyList<CameraSpacePoint> vertices, List<Ellipse> lst)
        {
            faceP.UpdateFacePoints(c,vertices,lst);
        }

        private void InsertPerson(object sender, RoutedEventArgs e)
        {
            
            //IReadOnlyList<CameraSpacePoint> csp = _faceModel.CalculateVerticesForAlignment(_faceAlignment);

            if(savedFace==null || listSf.Count==0 || listSf==null){
                MessageBox.Show("Input Face not Found", "Error");
            }
            else
            {
                //List<IReadOnlyList<CameraSpacePoint>> spts = new List<IReadOnlyList<CameraSpacePoint>>();
                //spts.Add(savedFace);
                //InsertForm inf = new InsertForm(savedFace);
                InsertForm inf = new InsertForm(listSf);
                inf.Show();
                listSf.Clear();
            }

            
        }

        
        private void ClearCanvas()
        {
            canvas.Children.Clear();
            topProjectionCanvas.Children.Clear();
        }

        private void ShowDb(object sender, RoutedEventArgs e)
        {
            showDb sdb=new showDb(listSf);
            sdb.Show();
            if(sdb.buttonHit){
                listSf.Clear();
            }
        }

        private void recognize_Click(object sender, RoutedEventArgs e)
        {


            if (savedFace == null || listSf==null || listSf.Count==0 )
            {
                MessageBox.Show("Input Face not Found","Error");
            }
            else
            {

                FaceRecognizer fr = new FaceRecognizer();
                Person per = fr.recognize(listSf[0]);

                FacePrinter fp = FacePrinter.getInstance();

                recognizedFace = fp.FacePointsToCameraPoints(per.face[0]);//(IReadOnlyList<CameraSpacePoint>)pts;


                foundFace.Children.Clear();
                UpdateFacePoints(foundFace, recognizedFace, new List<Ellipse>());

                foundName.Text = per.name;
                foundSurname.Text = per.surname;
                foundPass.Text = per.passportId;
            }

        }

        private void ShowDetails(object sender, MouseButtonEventArgs e)
        {
            if (recognizedFace == null)
            {
                MessageBox.Show("Recognized Face not Found","Error");
            }
            else
            {
                Details d = new Details(listSf[0], recognizedFace);
                d.Show();
            }
            
        }

        



   

        

       

    }
}
