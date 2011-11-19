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

        public override void GiveHeat(int genHeat, Component from)
        {
            ItsCurrentHeat += genHeat;
            RebalanceHeat();
        }

        private void RebalanceHeat()
        {
            RebalanceHeat(ItsLeft);
            RebalanceHeat(ItsRight);
            RebalanceHeat(ItsBelow);
            RebalanceHeat(ItsAbove);
            RebalanceHeat(ItsReactor);  
        }

        private void RebalanceHeat(Reactor reactor)
        {
            float difference = reactor.ItsHeat - this.ItsCurrentHeat;
            if (Math.Abs(difference - 0.0f) < 0) return;
            
            float rebalance = Math.Abs(difference) /2;

            if (rebalance > 25) rebalance = 25;

            //reactor has more heat
            if (difference > 0)
            {
                this.ItsCurrentHeat -= rebalance;
                reactor.ItsHeat += rebalance;
            }
            //this has more heat
            else
            {
                this.ItsCurrentHeat += rebalance;
                reactor.ItsHeat -= rebalance;
            }
        }

        private void RebalanceHeat(IComponent component )
        {
            if (component == null || component is ReactorPlating || component is CoolantCell) return;

            var meltable = component as IMeltable;
            if (meltable == null) return;


            float difference = meltable.ItsCurrentHeat - this.ItsCurrentHeat;
            if (Math.Abs(difference - 0.0f) < 0) return;

            float rebalance = Math.Abs(difference) / 2;

            if (rebalance > 6) rebalance = 6;

            //component has more heat
            if (difference > 0)
            {
                this.ItsCurrentHeat -= rebalance;
                meltable.ItsCurrentHeat += rebalance;
            }
            //this has more heat
            else
            {
                this.ItsCurrentHeat += rebalance;
                meltable.ItsCurrentHeat -= rebalance;
            }
            
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
            get { throw new NotImplementedException(); }
        }

        public int ItsMeltingPoint
        {
            get { return 10000; }
        }
    }
}