using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldGame.Model
{
    public interface ITimer : IDisposable
    {
        event EventHandler? OnTick;
        double ElapsedSeconds { get; }
        int TargetFramerate { get; }

        void Start();
        void Stop();
    }
}
