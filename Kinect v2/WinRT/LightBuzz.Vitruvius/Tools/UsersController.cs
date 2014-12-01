using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPreview.Kinect;

namespace LightBuzz.Vitruvius
{
    public class UsersController : BaseController<IEnumerable<Body>>
    {
        #region Members

        /// <summary>
        /// The number of the consecutive frames.
        /// </summary>
        int _consecutiveFrames;

        HashSet<ulong> _IDs = new HashSet<ulong>();

        UsersControllerEventArgs _args = new UsersControllerEventArgs();

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a body leaves the field of view.
        /// </summary>
        public event EventHandler<UsersControllerEventArgs> BodyLeft;

        /// <summary>
        /// Occurs when a body enters the field of view.
        /// </summary>
        public event EventHandler<UsersControllerEventArgs> BodyEntered;

        #endregion

        #region Public methods

        /// <summary>
        /// Starts the controller.
        /// </summary>
        public override void Start()
        {
            base.Start();

            _consecutiveFrames = 0;
        }

        /// <summary>
        /// Stops the controller.
        /// </summary>
        public override void Stop()
        {
            base.Stop();

            _consecutiveFrames = 0;
        }

        /// <summary>
        /// Updates the controller.
        /// </summary>
        /// <param name="bodies">The bodies to gather data from.</param>
        public override void Update(IEnumerable<Body> bodies)
        {
            base.Update(bodies);

            HashSet<ulong> IDs = new HashSet<ulong>();

            foreach (var body in bodies)
            {
                if (body.IsTracked)
                {
                    IDs.Add(body.TrackingId);
                }
            }

            int currentCount = IDs.Count;
            int previousCount = _IDs.Count;

            if (currentCount != previousCount)
            {
                _consecutiveFrames++;

                if (_consecutiveFrames >= _windowSize)
                {
                    // The users that entered or left.
                    HashSet<ulong> users = new HashSet<ulong>();

                    if (currentCount > previousCount)
                    {
                        // ONE OR MORE USERS ENTERED
                        foreach (ulong id in IDs)
                        {
                            if (!_IDs.Contains(id))
                            {
                                users.Add(id);
                            }
                        }

                        _args.Users = users;

                        if (BodyEntered != null)
                        {
                            BodyEntered(this, _args);
                        }
                    }
                    else
                    {
                        // ONE OR MORE USERS LEFT
                        foreach (ulong id in _IDs)
                        {
                            if (!IDs.Contains(id))
                            {
                                users.Add(id);
                            }
                        }

                        _args.Users = users;

                        if (BodyLeft != null)
                        {
                            BodyLeft(this, _args);
                        }
                    }

                    _IDs = IDs;
                    _consecutiveFrames = 0;
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// The information the reported gives back to the calling function.
    /// </summary>
    public class UsersControllerEventArgs : EventArgs
    {
        /// <summary>
        /// The tracking ID of the person who entered or exited.
        /// </summary>
        public HashSet<ulong> Users { get; set; }
    }
}
