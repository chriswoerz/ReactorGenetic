using System;

namespace ReactorGeneric.Component
{
    public class ReactorPlating : AbstractComponent, ICooler, IHeatable
    {
        public static int SystemCapacityAdd = 100;
        private int PulsesSinceCooling = 0;

        public ReactorPlating(uint xpos, uint ypos, Component type)
            : base(xpos, ypos, type)
        {
            
        }

        public override void PulseHandler(object sender, EventArgs e)
        {
            base.PulseHandler(sender, e);

            PulsesSinceCooling++;
            if (PulsesSinceCooling * ItsCoolingPerTick == 1)
            {
                ItsCurrentHeat -= 1;
                PulsesSinceCooling = 0;
            }

            PostPulse();
        }

        public override void GiveHeat(int genHeat, Component from, int hops)
        {
            ItsCurrentHeat += genHeat;
        }

        private int _currentHeat;
        public int ItsCurrentHeat
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

        #region IHeatable Members


        public void TakeHeat(int rebalance)
        {
            ItsCurrentHeat -= rebalance;
        }

        #endregion
    }
}