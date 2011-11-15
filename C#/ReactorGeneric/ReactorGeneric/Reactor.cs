using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ReactorGeneric.Component;

namespace ReactorGeneric
{
    public delegate void PulseEventHandler(object sender, ReactorEventArgs args);

    public class Reactor : IDisposable, IComparable<Reactor>
    {
        private int? _totalHeatCap;
        private readonly ManualResetEvent _itsFinishedHandle;

        public IList<Chamber> Chambers
        {
            get; set; 
        }

        public int ItsPowerOutput { get; private set; }
        
        public Reactor(Population population, int chambers, string contents, ManualResetEvent finishedHandle)
        {
            _itsFinishedHandle = finishedHandle;
            ItsPopulation = population;
            Chambers = new List<Chamber>();
            ItsComponents = new List<IComponent>();
            for (int i = 0; i < chambers; i++)
            {
                var chamber = new Chamber(this);
                this.Pulse += chamber.PulseHandler;
                Chambers.Add(chamber);
            }



            HookupEvents();

        }


        public event EventHandler<EventArgs> Pulse;
        public event EventHandler<EventArgs> Melt;

        public void OnMelt(EventArgs e)
        {
            EventHandler<EventArgs> handler = Melt;
            if (handler != null) handler(this, e);
        }

        public void OnTick()
        {
            CurrentHeat -= 1;
            if (Pulse != null)
            {
                Pulse(this, new EventArgs());
            }
        }

        public void MarkFinal()
        {
            _itsFinishedHandle.Set();
        }

        private void HookupEvents()
        {
            Melt += MeltHandler;
        }

        private void MeltHandler(object sender, EventArgs e)
        {
            ItsMeltedFlag = true;
            UnhookEvents();
        }

        private bool ItsMeltedFlag { get; set; }

        protected Population ItsPopulation { get; set; }


        public int HeatCapacity
        {
            get { return 10000; }
        }
        public int CurrentHeat { get; set; }

        public int SystemHeatCapacity
        {
            get
            {
                _totalHeatCap = _totalHeatCap ??
                    this.HeatCapacity + 
                    Chamber.SystemCapacityAdd*Chambers.Count + 
                    ReactorPlating.SystemCapacityAdd*ItsComponents.ToList().FindAll(c => c.IsType(Component.Component.ReactorPlating)).Count ;

                return _totalHeatCap.Value;
            }
        }

        public IList<IComponent> ItsComponents { get; set; }

        public void Dispose()
        {
            UnhookEvents();
        }

        private void UnhookEvents()
        {
            Melt -= MeltHandler;

            foreach (var chamber in Chambers)
            {
                Pulse -= chamber.PulseHandler;
            }

            foreach (var component in ItsComponents)
            {
                Pulse -= component.PulseHandler;
            }
        }

        public string GetReport(int ticks)
        {
            if(ItsMeltedFlag)
            {
                return "Melted";
            }
            return string.Format("H: {0, -10} E: {1, -10} EF: {4, -10} R: {2, -10} M: {3}", CurrentHeat, ItsPowerOutput, GetPowerHeatRatio(), GetPrettyManifest(), GetEfficency()/ticks);
        }

        private string GetPrettyManifest()
        {
            string toReturn = string.Empty;

            for (uint x = 0; x < GetWidth(); x++)
            {
                for (uint y = 0; y < GetHeight(); y++)
                {
                    string cString = string.Empty;
                    cString += GetComponent(x, y).Type;
                    
                    toReturn += cString.First();
                }
                toReturn += ":";
            }
            return toReturn;
        }

        private int GetEfficency()
        {
            var spentFuel = ItsComponents.ToList().FindAll(c => c.IsType(Component.Component.UranCell));
            if (spentFuel.Count > 0)
                return ItsPowerOutput/spentFuel.Count;
            return 0;
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

            return ItsComponents.ToList().Find(c => c.XPosition == xpos && c.YPosition == ypos);
        }

        public void EmitEU(int pulses)
        {
            ItsPowerOutput += 5*pulses;
        }

        private float GetPowerHeatRatio()
        {
            if (ItsPowerOutput == 0 || CurrentHeat == 0) return 0;

            return (float)Math.Round(((decimal)ItsPowerOutput/CurrentHeat), 3);
        }

        public int CompareTo(Reactor other)
        {
            return GetPowerHeatRatio().CompareTo(other.GetPowerHeatRatio());
        }
    }
}