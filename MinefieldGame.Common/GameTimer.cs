using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Avalonia
{
    internal class GameTimer : Model.ITimer, IDisposable
    {
        public double ElapsedSeconds { get; private set; }
        public int TargetFramerate => 60;

        public event EventHandler? OnTick;

        private readonly DispatcherTimer _timer;

        public GameTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000.0 / TargetFramerate)
            };
            _timer.Tick += OnTickEvent;
        }

        private void OnTickEvent(object? sender, EventArgs e)
        {
            ElapsedSeconds += 1.0 / TargetFramerate;
            OnTick?.Invoke(this, e);
        }

        public void Dispose()
        {
            Stop();
            _timer.Tick -= OnTickEvent;
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
