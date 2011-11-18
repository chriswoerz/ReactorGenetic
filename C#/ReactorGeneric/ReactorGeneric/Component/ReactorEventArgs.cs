using System;

namespace ReactorGeneric.Component
{
    public class ReactorEventArgs : EventArgs
    {
        public Reactor PulsingReactor
        {
            get; private set;
        }

        public ReactorEventArgs(Reactor reactor)
        {
            PulsingReactor = reactor;
        }
    }
}