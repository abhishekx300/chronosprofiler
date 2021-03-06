﻿using System;
using System.Diagnostics;
namespace Chronos.Client.Win
{
    internal sealed class HostApplicationExtensionAdapter : IApplicationExtensionAdapter
    {
        private Host.IApplication _application;
        private Process _clientProcess;

        public void BeginInitialize(IChronosApplication application)
        {
            _application = (Host.IApplication)application;
            _clientProcess = ApplicationManager.Main.RunApplication();
            _clientProcess.EnableRaisingEvents = true;
            _clientProcess.Exited += OnClientProcessExited;
        }

        public void EndInitialize()
        {
        }

        public void BeginShutdown()
        {
            _clientProcess.Close();
        }

        public void EndShutdown()
        {
        }

        private void OnClientProcessExited(object sender, EventArgs e)
        {
            _application.Close();
        }
    }
}
