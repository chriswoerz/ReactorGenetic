using System;

namespace ReactorGeneric.Component
{
    public class Empty :AbstractComponent
    {
        public Empty(uint xpos, uint ypos, Component type)
            : base(xpos, ypos, type)
        {
            
        }

        public new void PulseHandler(object sender, System.EventArgs e)
        {
            //Do Nothing
        }

        public override void GiveHeat(int genHeat, Component from)
        {
            //Do Nothing
        }
    }
}