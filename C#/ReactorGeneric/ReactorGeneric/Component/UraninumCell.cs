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

            int pulses = GetPulses();

            ItsReactor.EmitEU(pulses);

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
                    ItsReactor.CurrentHeat += genHeat;
                }
                else
                {
                    GiveHeatAdjacent(genHeat);
                }
            }
        }

        public override void GiveHeat(int genHeat)
        {
            //Fat Interface
        }

        private void GiveHeatAdjacent(int genHeat)
        {
            if (ItsLeft != null) ItsLeft.GiveHeat(genHeat);
            if (ItsRight != null) ItsRight.GiveHeat(genHeat);
            if (ItsAbove != null) ItsAbove.GiveHeat(genHeat);
            if (ItsBelow != null) ItsBelow.GiveHeat(genHeat);
        }

        private int GetCoolerCount()
        {
            int count = 0;

            if (ItsLeft != null) count = ItsLeft is ICooler ? 1 : 0;
            if (ItsRight != null) count += ItsRight is ICooler ? 1 : 0;
            if (ItsAbove != null) count += ItsAbove is ICooler ? 1 : 0;
            if (ItsBelow != null) count += ItsBelow is ICooler ? 1 : 0;

            return count;
        }

        private int GetPulses()
        {
            int count = 0;

            if (ItsLeft != null) count = ItsLeft.IsType(this.Type) ? 1 : 0;
            if (ItsRight != null) count += ItsRight.IsType(this.Type) ? 1 : 0;
            if (ItsAbove != null) count += ItsAbove.IsType(this.Type) ? 1 : 0;
            if (ItsBelow != null) count += ItsBelow.IsType(this.Type) ? 1 : 0;

            return count;
        }
    }
}