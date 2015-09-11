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

using System;
using System.Collections.Generic;
using Microsoft.Kinect;

namespace LightBuzz.Vitruvius
{
    /// <summary>
    /// Notifies when one or more players enter or leave the scene.
    /// </summary>
    public class PlayersController : BaseController<IEnumerable<Body>>
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
