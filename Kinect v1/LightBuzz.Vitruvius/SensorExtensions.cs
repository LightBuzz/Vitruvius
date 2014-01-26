using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Provides some common functionality applied to a Kinect sensor.
    /// </summary>
    public static class SensorExtensions
    {
        #region Public methods

        /// <summary>
        /// Quickly enables the Color, Depth and Skeleton streams with their default parameters.
        /// </summary>
        /// <param name="sensor">The specified Kinect sensor.</param>
        public static void EnableAllStreams(this KinectSensor sensor)
        {
            sensor.ColorStream.Enable();
            sensor.DepthStream.Enable();
            sensor.SkeletonStream.Enable();
        }

        /// <summary>
        /// Gets the default Kinect sensor.
        /// </summary>
        /// <returns>The default connected Kinect sensor, or null if no properly connected devices are found.</returns>
        public static KinectSensor Default()
        {
            return KinectSensor.KinectSensors.Where(s => s.Status == KinectStatus.Connected).FirstOrDefault();
        }

        #endregion
    }
}
