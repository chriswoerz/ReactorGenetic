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

        public override void GiveHeat(int genHeat, Component from)
        {
            ItsCurrentHeat += genHeat;

        }

        private float _currentHeat;
        public float ItsCurrentHeat
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
            get { return 0.1f; }
        }

        public int ItsMeltingPoint
        {
            get { return 10000; }
        }
    }
}