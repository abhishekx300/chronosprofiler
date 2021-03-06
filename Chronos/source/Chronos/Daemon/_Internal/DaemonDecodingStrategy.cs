﻿using System;
using Chronos.Communication.Native;

namespace Chronos.Daemon
{
    internal class DaemonDecodingStrategy : RemoteBaseObject, IDaemonStrategy
    {
        private readonly Application _application;

        public DaemonDecodingStrategy(Application application)
        {
            _application = application;
        }

        //TODO: assign correct value
        public IProfilingTimer ProfilingTimer { get; private set; }

        public SessionState SessionState
        {
            get { return SessionState.Decoding; }
        }

        public IRequestClient AgentRequestClient
        {
            get { return null; }
        }

        public ProcessInformation ProcessInformation
        {
            get { return null; }
        }

        public void StartProfiling(int profiledProcessId, Guid agentApplicationUid, uint profilingBeginTime)
        {
            //throw new InvalidOperationException(string.Format("This operation is not avaliable in {0} state", SessionState));
        }

        public void ReloadData()
        {
            //throw new InvalidOperationException(string.Format("This operation is not avaliable in {0} state", SessionState));
        }
        
        public void StartDecoding()
        {
            throw new NotImplementedException();
        }

        public void StopProfiling()
        {
            throw new InvalidOperationException(string.Format("This operation is not avaliable in {0} state", SessionState));
        }

        public void SaveSession()
        {
            throw new InvalidOperationException(string.Format("This operation is not avaliable in {0} state", SessionState));
        }

        public void RemoveSession()
        {
            throw new NotImplementedException();
        }
    }
}
