using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Interface for a Kinect stream recorder.
    /// </summary>
    public interface IRecorder<T>
    {
        void Start();

        void Stop();

        void Update(T frame);
    }
}
