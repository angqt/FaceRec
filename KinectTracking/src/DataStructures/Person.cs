using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectSample1.src.DataStructures
{
    class Person
    {
        public int id;
        public string name;
        public string surname;
        public string passportId;
        public List<List<FacePoints>> face;

        public override string ToString()
        {
            return name + " " + surname;
        }
    }
}
