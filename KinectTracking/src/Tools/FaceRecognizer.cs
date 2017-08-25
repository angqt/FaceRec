using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.Face;
using KinectSample1.src.DataStructures;

namespace KinectSample1.src.Tools
{
    class FaceRecognizer
    {
        public Person recognize(IReadOnlyList<CameraSpacePoint> face)
        {
            int foundid = 2;

            DbConnection dbc = new DbConnection();

            List<FacePoints> selFc = dbc.SelectFace(foundid);

            Person p = dbc.SelectPerson(foundid);

            p.face = new List<List<FacePoints>>();


            p.face.Add(selFc);

            return p;
        }
    }
}
