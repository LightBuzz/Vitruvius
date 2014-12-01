using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightBuzz.Vitruvius
{
    public class BaseController<T>
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

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the controller.
        /// </summary>
        public BaseController()
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

        #region Methods

        /// <summary>
        /// Starts the controller.
        /// </summary>
        public virtual void Start()
        {
            _isRunning = true;
        }

        /// <summary>
        /// Stops the controller.
        /// </summary>
        public virtual void Stop()
        {
            _isRunning = false;
        }

        /// <summary>
        /// Updates the controller.
        /// </summary>
        /// <param name="value">The parameters used to update the controller.</param>
        public virtual void Update(T value)
        {
            if (!_isRunning || value == null) return;
        }

        #endregion
    }
}
