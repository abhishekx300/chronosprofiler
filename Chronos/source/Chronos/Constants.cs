﻿using System;
using System.Globalization;

namespace Chronos
{
    public static class Constants
    {
        public const string HostApplicationServiceName = "Chronos Host Service";
        public const string DaemonApplicationExecutableName = "Chronos.Daemon.Application.exe";
        public const string SessionUidEnvironmentVariableName = "CHRONOS_PROFILER_SESSION_UID";
        public const string ExtensionsDirectoryEnvironmentVariableName = "CHRONOS_PROFILER_EXTENSIONS";
        public const string XChronexExtension = ".xchronex";
        public const string JChronexExtension = ".jchronex";

        public static readonly CultureInfo DefaultCulture;

        static Constants()
        {
            //1033 is en-US
            DefaultCulture = CultureInfo.GetCultureInfo(1033);
        }

        public class EnvironmentVariablesNames
        {
            public const string ConfigurationToken = "CHRONOS_PROFILER_CONFIGURATION_TOKEN";
            public const string Path = "PATH";
        }

        public static class ApplicationCodeName
        {
            public const string Core = "core";
            public const string Agent = "agent";
        }

        public static class Remoting
        {
            //public static TimeSpan InitialLeaseTime = TimeSpan.FromSeconds(300);
            //public static TimeSpan RenewOnCallTime = TimeSpan.FromSeconds(120);
            //public static TimeSpan SponsorshipTimeout = TimeSpan.FromSeconds(20);
            //public static TimeSpan InitialLeaseTime = TimeSpan.FromSeconds(20);
            //public static TimeSpan RenewOnCallTime = TimeSpan.FromSeconds(30);
            public static TimeSpan SponsorshipTimeout = TimeSpan.FromSeconds(10);
        }

        public static class CoreProcessName
        {
            public const string Host = "Chronos.Host.Application.exe";
            public const string Daemon = "Chronos.Daemon.Application.exe";
        }
    }
}
