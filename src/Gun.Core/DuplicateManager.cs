using Gun.Core.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Gun.Core
{
    public class DuplicateManager : IDisposable, IDuplicateManager
    {
        private readonly DuplicateManagerOptions _options;
        private readonly Timer _timer;
        private Dictionary<string, long> _trackedMessages = new Dictionary<string, long>();
        private bool _disposed;

        public DuplicateManager(IOptions<DuplicateManagerOptions> options)
        {
            _options = options.Value;
            _timer = new Timer(DoCleanUpTracks, null, Timeout.Infinite, 0);
        }

        private void DoCleanUpTracks(object state)
        {
            _trackedMessages
                .Where(t => _options.Age.Milliseconds <= (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - t.Value)).ToList()
                .ForEach(t => _trackedMessages.Remove(t.Key));
            //Stop the timer
            _timer.Change(Timeout.Infinite, 0);
        }


        public bool Check(string id) => _trackedMessages.ContainsKey(id) ? !String.IsNullOrEmpty(Track(id)) : false;


        public string Track(string id)
        {
            _trackedMessages[id] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            //Start the timer
            _timer.Change(TimeSpan.Zero, _options.Age);
            return id;
        }

        public static string Random() => new Random().NextLong(0, long.MaxValue).EncodeToBase36().Substring(0, 3);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _timer.Dispose();
            }

            _disposed = true;
        }
    }

    public class DuplicateManagerOptions
    {
        public int Max { get; set; } = 1000;
        public TimeSpan Age { get; set; } = TimeSpan.FromMilliseconds(1000 * 9);
    }
}
