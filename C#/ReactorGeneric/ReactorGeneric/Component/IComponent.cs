using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactorGeneric.Component
{
    public interface IComponent
    {
        void PulseHandler(object sender, EventArgs e);
        uint XPosition { get; }
        uint YPosition { get; }
        bool IsType(Component component);
        void GiveHeat(int genHeat, Component from);
        Component Type { get; }
    }
}
