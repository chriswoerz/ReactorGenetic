using System;

namespace ReactorGeneric.Component
{
    public class HeatDispenser : AbstractComponent, ICooler, IMeltable
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

        private float _currentHeat;
        protected float ItsCurrentHeat
        {
            get { return _currentHeat; }
            set
            {
                _currentHeat = value < 0 ? 0 : value;
                if (_currentHeat >= ItsMeltingPoint) ItsReactor.OnMelt(new EventArgs());
            }
        }

        public float ItsCoolingPerTick
        {
            get { throw new NotImplementedException(); }
        }

        public int ItsMeltingPoint
        {
            get { return 10000; }
        }
    }
}