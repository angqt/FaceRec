using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectTracking.src.DataStructures
{
    public class BezierAttributes
    {
        public int index;
        public string personName;
        public double varX;
        public double varY;
        public double mean;

        public BezierAttributes(int index,string personName, double varX, double varY, double mean)
        {
            this.index = index;
            this.personName = personName;
            this.varX = varX;
            this.varY = varY;
            this.mean = mean;
        }
    }
}
