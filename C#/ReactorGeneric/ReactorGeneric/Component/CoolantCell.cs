using System;

namespace ReactorGeneric.Component
{
    public class CoolantCell : AbstractComponent
    {
        public CoolantCell(uint xpos, uint ypos) : base(xpos, ypos)
        {
            
        }

        public override void PulseHandler(object sender, EventArgs e)
        {
            base.PulseHandler(sender, e);
           
        }
    }
}