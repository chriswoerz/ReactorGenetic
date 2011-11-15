﻿using System;

namespace ReactorGeneric.Component
{
    public class ReactorPlating : AbstractComponent, ICooler
    {
        public ReactorPlating(uint xpos, uint ypos, Component type)
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

        protected int ItsCurrentHeat { get; set; }

        public float CoolingPerTick
        {
            get { throw new NotImplementedException(); }
        }
    }
}