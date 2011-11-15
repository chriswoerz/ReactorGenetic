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
            _type = type;
        }

        public virtual void PulseHandler(object sender, EventArgs e)
        {
            ItsReactor = (Reactor)sender;

            ItsLeft = ItsLeft ?? ItsReactor.GetComponent(XPosition - 1, YPosition);
            ItsRight = ItsRight ?? ItsReactor.GetComponent(XPosition + 1, YPosition);
            ItsAbove = ItsAbove ?? ItsReactor.GetComponent(XPosition, YPosition + 1);
            ItsBelow = ItsBelow ?? ItsReactor.GetComponent(XPosition, YPosition - 1);
        }

        protected IComponent ItsBelow { get; set; }

        protected IComponent ItsAbove { get; set; }

        protected IComponent ItsRight { get; set; }

        protected IComponent ItsLeft { get; set; }

        protected Reactor ItsReactor { get; set; }

        public uint XPosition { get; private set; }
        public uint YPosition { get; private set; }

        public bool IsType(Component component)
        {
            return component == Type;
        }

        public abstract void GiveHeat(int genHeat);
    }
}
