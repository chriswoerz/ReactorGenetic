using System;
using System.Collections.Generic;
using System.Linq;
using ReactorGeneric.Simulator;

namespace ReactorGeneric.Component
{
    public delegate void PulseEventHandler(object sender, ReactorEventArgs args);

    public class Reactor : IDisposable, IComparable<Reactor>
    {
        private int? _totalHeatCap;

        public IList<Chamber> Chambers
        {
            get; set; 
        }

        public int ItsPowerOutput { get; private set; }
        
        public Reactor(Generation generation, int chambers)
        {
            Initialize(generation, chambers);
            Populator.RandomReactor(this);
        }

        public Reactor(Generation generation, int chambers, string rna)
        {
            Initialize(generation, chambers);
            ItsComponents = Populator.BuildComponentsFromRNA(this, rna);
        }

        private void Initialize(Generation generation, int chambers)
        {
            ItsGeneration = generation;
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
            if (Pulse != null && !ItsMeltedFlag)
            {
                ItsPulsesRecieved++;
                ItsHeat -= 1;
                Pulse(this, new EventArgs());
            }
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

        public bool ItsMeltedFlag { get; private set; }

        protected Generation ItsGeneration { get; set; }


        public int HeatCapacity
        {
            get { return 10000; }
        }

        private int _heat;
        public int ItsHeat
        {
            get { return _heat; }
            set 
            { 
                _heat = value < 0 ? 0 : value;
                if (_heat > 100000)
                {
                    var bang = "";
                }
                if (_heat >= SystemHeatCapacity) OnMelt(new EventArgs());
            }
        }

        public int SystemHeatCapacity
        {
            get
            {
                _totalHeatCap = _totalHeatCap ??
                    this.HeatCapacity + 
                    Chamber.SystemCapacityAdd*Chambers.Count + 
                    ReactorPlating.SystemCapacityAdd*ItsComponents.ToList().FindAll(c => c.IsType(Component.ReactorPlating)).Count ;

                return _totalHeatCap.Value;
            }
        }

        public IList<IComponent> ItsComponents { get; set; }

        private int ItsPulsesRecieved { get; set; }

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
            return string.Format("H: {0, -10} E: {1, -10} EF: {4, -10} R: {2, -10} M: {3}", ItsHeat, ItsPowerOutput, GetPowerHeatRatio(), GetPrettyManifest(), GetEfficency()/ticks);
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
            var spentFuel = ItsComponents.ToList().FindAll(c => c.IsType(Component.UranCell));
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

        public void EmitPulseToReactor(int pulses)
        {
            ItsPowerOutput += 5*pulses;
        }

        private float GetPowerHeatRatio()
        {
            if (ItsPowerOutput == 0 || ItsHeat == 0) return 0;

            return (float)Math.Round(((decimal)ItsPowerOutput/ItsHeat), 3);
        }

        public int CompareTo(Reactor other)
        {
            return GetPowerHeatRatio().CompareTo(other.GetPowerHeatRatio());
        }

        public ReactorResult GetResult()
        {
            var result = new ReactorResult();
            result.ItsGenerationId = ItsGeneration.ItsId;
            result.ItsManifest = GetPrettyManifest();
            result.ItsEUOutput = ItsPowerOutput;
            result.ItsHeat = ItsHeat;
            result.ItsEfficency = GetEfficency();

            return result;
        }
    }
}