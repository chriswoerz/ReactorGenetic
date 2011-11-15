using System;

namespace ReactorGeneric
{
    public class PulseEventArgs : EventArgs
    {
        public Reactor PulsingReactor
        {
            get; private set;
        }

        public PulseEventArgs(Reactor reactor)
        {
            PulsingReactor = reactor;
        }
    }
}