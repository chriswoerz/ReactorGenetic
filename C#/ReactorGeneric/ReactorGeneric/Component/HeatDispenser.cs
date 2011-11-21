using System;

namespace ReactorGeneric.Component
{
    public class HeatDispenser : AbstractComponent, IHeatable
    {
        public HeatDispenser(uint xpos, uint ypos, Component type)
            : base(xpos, ypos, type)
        {
            
        }

        public override void PulseHandler(object sender, EventArgs e)
        {
            base.PulseHandler(sender, e);

            PostPulse();
        }

        public override void GiveHeat(int genHeat, Component from, int hops)
        {
            ItsCurrentHeat += genHeat;
            
            if(ItsReactor != null)
                RebalanceHeat(hops);
        }

        private void RebalanceHeat(int hops)
        {
            RebalanceHeat(ItsLeft, hops);
            RebalanceHeat(ItsRight, hops);
            RebalanceHeat(ItsBelow, hops);
            RebalanceHeat(ItsAbove, hops);
            RebalanceHeat(ItsReactor);  
        }

        private void RebalanceHeat(Reactor reactor)
        {
            int difference = reactor.ItsHeat - this.ItsCurrentHeat;
            if (Math.Abs(difference - 0.0f) < 0) return;
            
            int rebalance = (int)Math.Abs(difference) /2;

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

        private void RebalanceHeat(IComponent component, int hops )
        {
            if (component == null || (!(component is ReactorPlating) && !(component is CoolantCell))) return;

            var heatable = component as IHeatable;
            if (heatable == null) return;


            int difference = heatable.ItsCurrentHeat - this.ItsCurrentHeat;
            if (Math.Abs(difference - 0.0f) < 0) return;

            int rebalance = (int)Math.Abs(difference) / 2;

            if (rebalance > 6) rebalance = 6;

            //component has more heat
            if (difference > 0)
            {
                this.ItsCurrentHeat += rebalance;
                heatable.TakeHeat(rebalance);
            }
            //this has more heat
            else
            {
                this.ItsCurrentHeat -= rebalance;
                if (component is CoolantCell)
                {
                    heatable.GiveHeat(rebalance, this.Type, hops);
                }
                else{

                }
            }
            
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

        public int ItsCoolingPerTick
        {
            get { throw new NotImplementedException(); }
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