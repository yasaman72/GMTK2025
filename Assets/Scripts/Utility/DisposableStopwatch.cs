using System;
using System.Diagnostics;

namespace Deviloop
{
    public class DisposableStopwatch : IDisposable
    {
        private Stopwatch _stopWatch;
        private string _name;

        public DisposableStopwatch(string name)
        {
            _stopWatch = Stopwatch.StartNew();
            _stopWatch.Start();
            _name = name;
        }

        /// <summary>
        /// usage example:
        /// using (new DisposableStopwatch())
        /// {
        ///     SlowMethod();
        /// }
        /// 
        /// the dispose will be called at the end of the scope, so you can use it like this:
        /// using var stopwatch = new DisposableStopwatch();
        /// </summary>
        public void Dispose()
        {
            _stopWatch.Stop();
            var elapsedMS = _stopWatch.ElapsedMilliseconds;
            Logger.Log($"Elapsed time for {_name}: {elapsedMS} ms");
        }
    }
}
