using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.Face;
using Microsoft.VisualBasic.FileIO;

namespace KinectSample1.src.Tools
{
    class FileReader
    {
        string path = "C:\\Users\\Angelo\\Desktop\\AngeloProject\\kinect-angelo-training\\KinectTracking\\bin\\x64\\Release\\KinectData\\RandomData\\samples\\";
        string[] filenames = { "front" , "right" , "left", "top", "bottom", "topRight" ,"topLeft","bottomRight","bottomLeft"};
        string format = ".csv";

        public IReadOnlyList<CameraSpacePoint> readFile(int placeholder){


            List<CameraSpacePoint> list = new List<CameraSpacePoint>();

            Console.WriteLine(path + filenames[placeholder] + format);


            using (TextFieldParser parser = new TextFieldParser(@path+filenames[placeholder]+format))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();

                    CameraSpacePoint cmp = new CameraSpacePoint();
                    cmp.X=float.Parse(fields[1]);
                    cmp.Y = float.Parse(fields[2]);
                    cmp.Z = float.Parse(fields[3]);

                    //foreach (string field in fields)
                    //{
                    //    //TODO: Process field
                    //}

                    list.Add(cmp);
                }
            }


            return list;
        }

    }
}
