using System;
using System.Collections.Generic;
using System.Linq;
using ReactorGeneric.Component;

namespace ReactorGeneric
{
    public class Reactor : IHeatContainer, IDisposable
    {
        public IList<Chamber> Chambers
        {
            get; set; 
        }
        
        public Reactor(Population population, int chambers, string contents)
        {
            ItsPopulation = population;
            Chambers = new List<Chamber>();
            for (int i = 0; i < chambers; i++)
            {
                var chamber = new Chamber(this);
                ItsPopulation.Pulse += chamber.PulseHandler;
                Chambers.Add(chamber);
            }



            HookupEvents();

        }

        private void HookupEvents()
        {
            ItsPopulation.Pulse += PulseHandler;
        }

        private void PulseHandler(object sender, EventArgs e)
        {
            CurrentHeat -= 1;
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

        public void Dispose()
        {
            UnhookEvents();
        }

        private void UnhookEvents()
        {
            ItsPopulation.Pulse -= PulseHandler;
            foreach (var chamber in Chambers)
            {
                ItsPopulation.Pulse -= chamber.PulseHandler;
            }
        }

        public string GetReport()
        {
            return string.Format("H: {0}", CurrentHeat);
        }
    }
}