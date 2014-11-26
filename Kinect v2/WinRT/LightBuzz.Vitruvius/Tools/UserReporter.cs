using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsPreview.Kinect;

namespace LightBuzz.Vitruvius
{
    public class UsersReporter
    {
        #region Constants

        /// <summary>
        /// The default window size.
        /// </summary>
        protected readonly int DEFAULT_WINDOW_SIZE = 12;

        #endregion

        #region Members

        /// <summary>
        /// The window size of the reporter.
        /// </summary>
        protected int _windowSize;

        /// <summary>
        /// Indicates whether the reporter is running.
        /// </summary>
        protected bool _isRunning;

        /// <summary>
        /// The number of the consecutive frames.
        /// </summary>
        int _consecutiveFrames;

        HashSet<ulong> _IDs = new HashSet<ulong>();

        ActiveUserReporterEventArgs _args = new ActiveUserReporterEventArgs();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the reporter.
        /// </summary>
        public UsersReporter()
        {
            _windowSize = DEFAULT_WINDOW_SIZE;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the window size for the reporter.
        /// </summary>
        public int WindowSize
        {
            get { return _windowSize; }
            set { _windowSize = value; }
        }

        /// <summary>
        /// Determines whether the reporter is running.
        /// </summary>
        public bool IsRunning
        {
            get { return _isRunning; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a body leaves the field of view.
        /// </summary>
        public event EventHandler<ActiveUserReporterEventArgs> BodyLeft;

        /// <summary>
        /// Occurs when a body enters the field of view.
        /// </summary>
        public event EventHandler<ActiveUserReporterEventArgs> BodyEntered;

        #endregion

        #region Public methods

        /// <summary>
        /// Starts reporting.
        /// </summary>
        public void Start()
        {
            _isRunning = true;
            _consecutiveFrames = 0;
        }

        /// <summary>
        /// Stops reporting.
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            _consecutiveFrames = 0;
        }

        /// <summary>
        /// Updates the reporting mechanism.
        /// </summary>
        /// <param name="bodies">The bodies to gather data from.</param>
        public void Update(IEnumerable<Body> bodies)
        {
            if (!_isRunning || bodies == null) return;

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
    public class ActiveUserReporterEventArgs : EventArgs
    {
        /// <summary>
        /// The tracking ID of the person who entered or exited.
        /// </summary>
        public HashSet<ulong> Users { get; set; }
    }
}
