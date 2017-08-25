using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Kinect;
using Microsoft.Kinect.Face;
using KinectSample1.src.Tools.GuiManagement;

namespace KinectSample1.src
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : Window
    {

        public Details(IReadOnlyList<CameraSpacePoint> saved, IReadOnlyList<CameraSpacePoint> recognized)
        {
            InitializeComponent();

            FacePrinter fp = FacePrinter.getInstance();
            fp.UpdateFacePoints(savedFace,saved,new List<Ellipse>());
            fp.UpdateFacePoints(recognizedFace, recognized, new List<Ellipse>());
        }
    }
}
