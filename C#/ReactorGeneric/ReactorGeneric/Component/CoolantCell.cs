﻿using System;

namespace ReactorGeneric.Component
{
    public class CoolantCell : AbstractComponent , ICooler, IMeltable
    {
        public CoolantCell(uint xpos, uint ypos, Component type)
            : base(xpos, ypos, type)
        {
            
        }

        public override void PulseHandler(object sender, EventArgs e)
        {
            ItsCurrentHeat -= ItsCoolingPerTick;
            base.PulseHandler(sender, e);
           
        }

        public override void GiveHeat(int genHeat)
        {
            ItsCurrentHeat += genHeat;
            if(ItsCurrentHeat >= ItsMeltingPoint) ItsReactor.OnMelt(new EventArgs());
        }

        private float _currentHeat;
        protected float ItsCurrentHeat
        {
            get { return _currentHeat; }
            set { _currentHeat = value < 0 ? 0 : value; }
        }

        public float ItsCoolingPerTick
        {
            get { return 1; }
        }

        public int ItsMeltingPoint
        {
            get { return 10000; }
        }
    }
}