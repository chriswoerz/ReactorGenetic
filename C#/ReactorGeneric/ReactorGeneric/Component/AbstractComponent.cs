using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactorGeneric.Component
{
    public abstract class AbstractComponent :IComponent
    {
        protected AbstractComponent(uint xpos, uint ypos)
        {
            XPosition = xpos;
            YPosition = ypos;
        }

        public virtual void PulseHandler(object sender, EventArgs e)
        {
            ItsReactor = (Reactor)sender;

            ItsLeft = ItsLeft ?? ItsReactor.GetComponent((int)XPosition - 1, (int)YPosition);
            ItsRight = ItsRight ?? ItsReactor.GetComponent((int)XPosition + 1, (int)YPosition);
            ItsAbove = ItsAbove ?? ItsReactor.GetComponent((int)XPosition, (int)YPosition + 1);
            ItsBelow = ItsBelow ?? ItsReactor.GetComponent((int)XPosition, (int)YPosition - 1);
        }

        protected IComponent ItsBelow { get; set; }

        protected IComponent ItsAbove { get; set; }

        protected IComponent ItsRight { get; set; }

        protected IComponent ItsLeft { get; set; }

        protected Reactor ItsReactor { get; set; }

        public uint XPosition { get; private set; }
        public uint YPosition { get; private set; }
    }
}
