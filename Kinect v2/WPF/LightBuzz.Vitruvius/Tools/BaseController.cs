//
// Copyright (c) LightBuzz Software.
// All rights reserved.
//
// http://lightbuzz.com
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
//
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
//
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
// FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE
// COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
// INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
// BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS
// OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED
// AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY
// WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.
//

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Generic class used to extract information from a series of frames.
    /// </summary>
    /// <typeparam name="T">The parameter we examine, e.g. <see cref="Microsoft.Kinect.Body"/>.</typeparam>
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
