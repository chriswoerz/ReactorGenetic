using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactorGeneric.Component
{
    public abstract class AbstractComponent :IComponent
    {
        private readonly Component _type;

        public Component Type { get { return _type; } }

        protected AbstractComponent(uint xpos, uint ypos, Component type)
        {
            XPosition = xpos;
            YPosition = ypos;
            ItsFirstPulse = true;
            _type = type;
        }

        public virtual void PulseHandler(object sender, EventArgs e)
        {
            ItsReactor = (Reactor)sender;

            if (!ItsFirstPulse) return;

            if (ItsLeft == null && XPosition != 0)
                ItsLeft = ItsReactor.GetComponent(XPosition - 1, YPosition);
    
            if (ItsRight == null && XPosition != ItsReactor.GetWidth())
                ItsRight = ItsReactor.GetComponent(XPosition + 1, YPosition);
    
            if (ItsAbove == null && XPosition != ItsReactor.GetHeight())
                ItsAbove = ItsReactor.GetComponent(XPosition, YPosition + 1);
    
            if (ItsBelow == null && YPosition != 0)
                ItsBelow = ItsReactor.GetComponent(XPosition, YPosition - 1);
        }

        protected IComponent ItsBelow { get; set; }

        protected IComponent ItsAbove { get; set; }

        protected IComponent ItsRight { get; set; }

        protected IComponent ItsLeft { get; set; }

        protected bool ItsFirstPulse { get; set; }

        protected Reactor ItsReactor { get; set; }

        public uint XPosition { get; private set; }
        public uint YPosition { get; private set; }

        public bool IsType(Component component)
        {
            return component == Type;
        }

        protected void PostPulse(){            
            ItsFirstPulse = false;
        }
        public abstract void GiveHeat(int genHeat, Component from, int hops);
    }
}
