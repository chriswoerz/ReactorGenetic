using System;

namespace ReactorGeneric.Component
{
    public class ReactorPlating : AbstractComponent, ICooler, IMeltable
    {
        public static int SystemCapacityAdd = 100;

        public ReactorPlating(uint xpos, uint ypos, Component type)
            : base(xpos, ypos, type)
        {
            
        }

        public override void PulseHandler(object sender, EventArgs e)
        {
            ItsCurrentHeat -= ItsCoolingPerTick;
            base.PulseHandler(sender, e);
        }

        public override void GiveHeat(int genHeat)
        {
            ItsCurrentHeat += genHeat;
        }

        protected float ItsCurrentHeat { get; set; }

        public float ItsCoolingPerTick
        {
            get { return 0.1f; }
        }

        public int ItsMeltingPoint
        {
            get { return 10000; }
        }
    }
}