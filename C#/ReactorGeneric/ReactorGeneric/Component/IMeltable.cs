using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactorGeneric.Component
{
    public interface IMeltable
    {
        int ItsMeltingPoint { get; }
        float ItsCurrentHeat { get; set; }
    }
}
