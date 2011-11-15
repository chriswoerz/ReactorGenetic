using System;
using System.Collections.Generic;
using System.Linq;
using ReactorGeneric.Component;

namespace ReactorGeneric
{
    public delegate void PulseEventHandler(object sender, PulseEventArgs args);

    public class Reactor : IDisposable
    {
        public IList<Chamber> Chambers
        {
            get; set; 
        }

        public int PowerOutput { get; private set; }
        
        public Reactor(Population population, int chambers, string contents)
        {
            ItsPopulation = population;
            Chambers = new List<Chamber>();
            Components = new List<IComponent>();
            for (int i = 0; i < chambers; i++)
            {
                var chamber = new Chamber(this);
                this.Pulse += chamber.PulseHandler;
                Chambers.Add(chamber);
            }



            HookupEvents();

        }

        public event EventHandler<EventArgs> Pulse;

        private void HookupEvents()
        {
            ItsPopulation.Pulse += PulseHandler;
        }

        private void PulseHandler(object sender, EventArgs e)
        {
            CurrentHeat -= 1;
            Pulse(this, new EventArgs());
        }

        protected Population ItsPopulation { get; set; }


        public int HeatCapacity
        {
            get { return 10000; }
        }
        public int CurrentHeat { get; set; }

        public int SystemHeatCapacity
        {
            get { return this.HeatCapacity + Chamber.HeatCapacity*Chambers.Count; }
        }

        public IList<IComponent> Components { get; set; }

        public void Dispose()
        {
            UnhookEvents();
        }

        private void UnhookEvents()
        {
            ItsPopulation.Pulse -= PulseHandler;
            foreach (var chamber in Chambers)
            {
                Pulse -= chamber.PulseHandler;
            }

            foreach (var component in Components)
            {
                Pulse -= component.PulseHandler;
            }
        }

        public string GetReport()
        {
            return string.Format("H: {0} E: {1} M: {2}", CurrentHeat, PowerOutput, GetManifest());
        }

        private string GetManifest()
        {
            string toReturn = string.Empty;

            for (uint x = 0; x < GetWidth(); x++)
            {
                for (uint y = 0; y < GetHeight(); y++)
                {
                    var component = GetComponent(x, y);

                    toReturn += (int)component.Type;
                }
                toReturn += ":";
            }
            return toReturn;
        }

        public int GetWidth()
        {
            return 3 + Chambers.Count;
        }

        public int GetHeight()
        {
            return 6;
        }

        public IComponent GetComponent(uint xpos, uint ypos)
        {
            if (xpos < 0 || ypos < 0) return null;

            return Components.ToList().Find(c => c.XPosition == xpos && c.YPosition == ypos);
        }

        public void EmitEU(int pulses)
        {
            PowerOutput += 5*pulses;
        }
    }
}