using System;

namespace ReactorGeneric.Component
{
    public class UraninumCell : AbstractComponent
    {
        public UraninumCell(uint xpos, uint ypos, Component type)
            : base(xpos, ypos, type)
        {

        }

        public override void PulseHandler(object sender, EventArgs e)
        {
            base.PulseHandler(sender, e);

            int pulses = GetPulseCount();

            ItsReactor.EmitPulseToReactor(pulses);

            for (; pulses > 0; pulses--)
            {
                int coolerCount = GetCoolerCount();
                int genHeat;
                switch (coolerCount)
                {
                    case 2:
                        genHeat = 4;
                        break;
                    case 3:
                        genHeat = 2;
                        break;
                    case 4:
                        genHeat = 1;
                        break;
                    default:
                        genHeat = 10;
                        break;
                }

                if (coolerCount == 0)
                {
                    ItsReactor.ItsHeat += genHeat;
                }
                else
                {
                    GiveHeatAdjacent(genHeat);
                }
            }

            PostPulse();
        }

        public override void GiveHeat(int genHeat, Component from, int hops)
        {
            //Fat Interface
            //throw new NotImplementedException();
        }

        private void GiveHeatAdjacent(int genHeat)
        {
            var hops = 1;
            if (ItsLeft != null && (ItsLeft is IHeatable))
                (ItsLeft as IHeatable).GiveHeat(genHeat, this.Type, hops);
            if (ItsRight != null && (ItsRight is IHeatable))
                (ItsRight as IHeatable).GiveHeat(genHeat, this.Type, hops);
            if (ItsAbove != null && (ItsAbove is IHeatable))
                (ItsAbove as IHeatable).GiveHeat(genHeat, this.Type, hops);
            if (ItsBelow != null && (ItsBelow is IHeatable))
                (ItsBelow as IHeatable).GiveHeat(genHeat, this.Type, hops);
        }

        private int GetCoolerCount()
        {
            if (!ItsFirstPulse) return ItsCoolerCount;

            int count = 0;

            if (ItsLeft != null) count += ItsLeft is ICooler ? 1 : 0;
            if (ItsRight != null) count += ItsRight is ICooler ? 1 : 0;
            if (ItsAbove != null) count += ItsAbove is ICooler ? 1 : 0;
            if (ItsBelow != null) count += ItsBelow is ICooler ? 1 : 0;

            return ItsCoolerCount = count;
        }

        protected int ItsCoolerCount { get; set; }

        private int GetPulseCount()
        {
            if (!ItsFirstPulse) return ItsPulseCount;

            int count = 1;

            if (ItsLeft != null) count += ItsLeft.IsType(this.Type) ? 1 : 0;
            if (ItsRight != null) count += ItsRight.IsType(this.Type) ? 1 : 0;
            if (ItsAbove != null) count += ItsAbove.IsType(this.Type) ? 1 : 0;
            if (ItsBelow != null) count += ItsBelow.IsType(this.Type) ? 1 : 0;

            return ItsPulseCount = count;
        }

        protected int ItsPulseCount { get; set; }
    }
}