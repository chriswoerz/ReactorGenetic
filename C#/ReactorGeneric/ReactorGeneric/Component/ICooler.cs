using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactorGeneric.Component
{
    interface ICooler : IComponent
    {
        float CoolingPerTick { get; }
    }
}
