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
using KinectSample1.src.DataStructures;
using KinectSample1.src.Tools;
using KinectSample1.src.Tools.GuiManagement;

namespace KinectSample1.src
{
    /// <summary>
    /// Interaction logic for InsertForm.xaml
    /// </summary>
    public partial class InsertForm : Window
    {

        List<IReadOnlyList<CameraSpacePoint>> cmsp;
        

        public InsertForm(List<IReadOnlyList<CameraSpacePoint>> c)
        {
            cmsp = c;
            
            InitializeComponent();

            PrintFace(cmsp[0]);

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Person person = new Person();
            person.name = tbName.Text;
            person.surname = tbSurname.Text;
            person.passportId = tbPass.Text;

            List<List<FacePoints>> lstfpl=new List<List<FacePoints>>();

            for (int i = 0; i < cmsp.Count;i++)
            {
                List<FacePoints> fpl = new List<FacePoints>();
                for( int j=0; j<cmsp[i].Count;j++)
                {
                    FacePoints p = new FacePoints();

                    p.id = i;
                    p.x = cmsp[i][j].X;
                    p.y = cmsp[i][j].Y;
                    p.z = cmsp[i][j].Z;

                    fpl.Add(p);
                }
                lstfpl.Add(fpl);
            }

            DbConnection db = new DbConnection();
            db.Insert(person,lstfpl);

            this.Close();
        }

        private void PrintFace(IReadOnlyList<CameraSpacePoint> vertices)
        {
            FacePrinter p = FacePrinter.getInstance();
            p.UpdateFacePoints(cnv,vertices,new List<Ellipse>());
        }


    }
}
