﻿using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using System.Security.Cryptography.X509Certificates;

namespace BudgetBuddyAPI.Extensions
{
    public class LogSetting
    {
        public static void SetUpSerilog(IConfiguration config)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(
                    path: @"./Logs/log-.txt",
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information
                )
                .Enrich.WithExceptionDetails()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .CreateLogger();
        }
    }
}
