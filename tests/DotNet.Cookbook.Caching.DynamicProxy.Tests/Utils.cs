using System;
using System.Diagnostics;

namespace DotNet.Cookbook.Caching.DynamicProxy.Tests
{
    internal class Utils
    {
        public static TimeSpan DurationOf(Action toTime)
        {
            var timer = Stopwatch.StartNew();
            toTime();
            timer.Stop();

            return timer.Elapsed;
        }
    }
}
