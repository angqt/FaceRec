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
using KinectSample1.src.DataStructures;
using KinectSample1.src.Tools;
using Microsoft.Kinect;
using KinectSample1.src.Tools.GuiManagement;


namespace KinectSample1.src
{
    /// <summary>
    /// Interaction logic for showDb.xaml
    /// </summary>
    public partial class showDb : Window
    {
        List<Person> ppl;
        List<IReadOnlyList<CameraSpacePoint>> cameraSp;
        public bool buttonHit = false;

        public showDb(List<IReadOnlyList<CameraSpacePoint>> cmsp)
        {

            InitializeComponent();
            if(cmsp==null){
                cameraSp = new List<IReadOnlyList<CameraSpacePoint>>();
            }else{
                cameraSp = cmsp;
            }
            
            
            DbConnection db = new DbConnection();
            ppl = db.SelectPeople();

            foreach (Person p in ppl)
            {
                people.Items.Add(p);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cnvsCh.Children.Clear();
            Person p=(Person)people.SelectedItem;
            tbName.Text = p.name;
            tbSurname.Text = p.surname;
            tbPass.Text = p.passportId;

            DbConnection db=new DbConnection();

            List<FacePoints> lst = db.SelectFace(p.id);

            List<CameraSpacePoint> pts = new List<CameraSpacePoint>();

            foreach (FacePoints f in lst)
            {
                CameraSpacePoint csp = new CameraSpacePoint();
                csp.X = f.x;
                csp.Y = f.y;
                csp.Z = f.z;

                pts.Insert(f.id, csp);
            }

            var vertices = (IReadOnlyList<CameraSpacePoint>)pts;

            PrintFace(pts);


        }

        private void PrintFace(IReadOnlyList<CameraSpacePoint> vertices)
        {

            FacePrinter p = FacePrinter.getInstance();
            p.UpdateFacePoints(cnvsCh, vertices, new List<Ellipse>());

        }

        private void ShowDetails(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Details", "Details");
        }

        private void AddFaces(object sender, RoutedEventArgs e)
        {
            Person p = (Person)people.SelectedItem;
            DbConnection dbc = new DbConnection();

            List<List<FacePoints>> faces = new List<List<FacePoints>>();

            for(int i=0 ; i<cameraSp.Count;i++)
            {
                List<FacePoints> lstfp = new List<FacePoints>();

                for (int j = 0; j < cameraSp[i].Count; j++)
                {
                    FacePoints fp = new FacePoints();
                    fp.id = j;
                    fp.x = cameraSp[i][j].X;
                    fp.y = cameraSp[i][j].Y;
                    fp.z = cameraSp[i][j].Z;

                    lstfp.Add(fp);
                }

                faces.Add(lstfp);
                buttonHit = true;
            }

            dbc.InsertFaces(p,faces);
        }

    }
}
