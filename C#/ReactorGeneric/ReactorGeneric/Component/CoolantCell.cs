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

        public override void GiveHeat(int genHeat, Component from)
        {
            ItsCurrentHeat += genHeat;
        }

        private float _currentHeat;
        public float ItsCurrentHeat
        {
            get { return _currentHeat; }
            set { 
                _currentHeat = value < 0 ? 0 : value;
                if (_currentHeat >= ItsMeltingPoint) ItsReactor.OnMelt(new EventArgs());
            }
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