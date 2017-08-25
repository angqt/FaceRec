using KinectTracking.src.DataStructures;
using KinectTracking.src.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectTracking.src.DataStructures
{
    public class Frames
    {
        FixableSizeLinkedList<Frame> frames;
        
        //used in Buffers for "multithreaded" buffertracking
        private Boolean active = true;

        public Frames() { frames = new FixableSizeLinkedList<Frame>(); }
        public Frames(int maxSize) { frames = new FixableSizeLinkedList<Frame>(maxSize); }

        public void CalculateBezierCurveAttributes(MyBody.BodyParts[] bodyParts)
        {
            foreach (Frame frame in frames)
            {
                frame.CalculateBezierCurveAttributes(bodyParts);
            }
        }

        public void Add(Frame frame)
        {
            frames.AddLast(frame);
        }

        public int Count()
        {
            return frames.Count();
        }

        public IEnumerator GetEnumerator()
        {
            return frames.GetEnumerator();
        }

        public Boolean IsActive()
        {
            return active;
        }

        public void Deactivate()
        {
            active = false;
        }

        public Boolean IsFull()
        {
            return frames.isFull();
        }
    }
}
