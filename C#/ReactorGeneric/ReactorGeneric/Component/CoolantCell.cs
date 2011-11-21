using System;

namespace ReactorGeneric.Component
{
    public class CoolantCell : AbstractComponent , IHeatable, ICooler
    {
        public CoolantCell(uint xpos, uint ypos, Component type)
            : base(xpos, ypos, type)
        {
            
        }

        public override void PulseHandler(object sender, EventArgs e)
        {
            base.PulseHandler(sender, e);

            ItsCurrentHeat -= ItsCoolingPerTick;

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
            set { 
                _currentHeat = value < 0 ? 0 : value;
                if (_currentHeat >= ItsMeltingPoint) ItsReactor.OnMelt(new EventArgs());
            }
        }

        public int ItsCoolingPerTick
        {
            get { return 1; }
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