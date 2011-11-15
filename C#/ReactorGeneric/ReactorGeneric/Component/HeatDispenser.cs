using System;

namespace ReactorGeneric.Component
{
    public class HeatDispenser : AbstractComponent, ICooler
    {
        public HeatDispenser(uint xpos, uint ypos, Component type)
            : base(xpos, ypos, type)
        {
            
        }

        public override void PulseHandler(object sender, EventArgs e)
        {
            base.PulseHandler(sender, e);

        }

        public override void GiveHeat(int genHeat)
        {
            ItsCurrentHeat += genHeat;
        }

        protected int ItsCurrentHeat { get; set; }

        public float ItsCoolingPerTick
        {
            get { throw new NotImplementedException(); }
        }
    }
}