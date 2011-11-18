using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactorGeneric.Component
{
    public class Chamber
    {
        public static int SystemCapacityAdd = 10000;

        private Reactor ItsReactor { get; set; }

        public Chamber(Reactor reactor)
        {
            ItsReactor = reactor;
        }

        public void PulseHandler(object sender, EventArgs e)
        {
            ItsReactor.ItsHeat -= 2;
        }
    }
}
