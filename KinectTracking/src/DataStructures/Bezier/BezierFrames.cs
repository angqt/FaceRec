using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectTracking.src.DataStructures
{
    public class BezierFrames
    {
        //attribute=frame
        List<BezierAttributes> attributes;

        public BezierFrames ()
        {
            attributes = new List<BezierAttributes>();
        }

        public void Add (BezierAttributes attribute)
        {
            attributes.Add(attribute);
        }

        public List<BezierAttributes> GetAttributes()
        {
            return attributes;
        }
    }
}
