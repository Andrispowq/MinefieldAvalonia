using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MinefieldGame.Model;

namespace MinefieldGame.Tests
{
    internal class MockTimer : Model.ITimer
    {
        public double ElapsedSeconds { get; private set; } = 0.0;

        public int TargetFramerate => 0;

        public event EventHandler? OnTick;

        public void Dispose()
        {
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public void RaiseTickEvent()
        {
            ElapsedSeconds += 1.0f;
            OnTick?.Invoke(this, EventArgs.Empty);
        }
    }
}
