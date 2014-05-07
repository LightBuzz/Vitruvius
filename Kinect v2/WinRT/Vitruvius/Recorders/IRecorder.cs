using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Interface for a Kinect stream recorder.
    /// </summary>
    public interface IRecorder<T>
    {
        StorageFile File { get; }

        void Start();

        void Stop();

        void Update(T frame);
    }
}
