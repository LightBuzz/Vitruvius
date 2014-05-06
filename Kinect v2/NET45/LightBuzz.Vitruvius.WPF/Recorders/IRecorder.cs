using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBuzz.Vitruvius.WPF
{
    /// <summary>
    /// NOT YET AVAILABLE!
    /// </summary>
    public interface IRecorder<T>
    {
        void Start();

        void Stop();

        void Update(T frame);
    }
}
