using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactorGeneric.Component
{
    public interface IHeatable : IComponent
    {
        int ItsMeltingPoint { get; }
        int ItsCurrentHeat { get; set; }
        void TakeHeat(int rebalance);
        void GiveHeat(int genHeat, Component from, int hops);
    }
}
