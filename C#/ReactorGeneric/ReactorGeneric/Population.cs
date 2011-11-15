using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactorGeneric.Component;

namespace ReactorGeneric
{
    public class Population
    {
        public IList<Reactor> ItsReactors { get; set; }

        public event EventHandler Pulse;

        public void OnPulse(EventArgs e)
        {
            EventHandler handler = Pulse;
            if (handler != null) handler(this, e);
        }

        public Population(int populationSize, int chambersPer)
        {
            var contents = "blarg";

            ItsReactors = new List<Reactor>();
            for (int i = 0; i < populationSize; i++)
            {
                var reactor = new Reactor(this, chambersPer, contents);
                Populator.RandomReactor(reactor);
                ItsReactors.Add(reactor);
            }
        }

        public string GenerateReport()
        {
            var reportSF = new StringBuilder();
            reportSF.AppendLine("Final Report");

            foreach (var reactor in ItsReactors)
            {
                reportSF.AppendLine(reactor.GetReport());
            }

            return reportSF.ToString();
        }
    }

    public static class Populator
    {
        private static readonly Random Random = new Random();
        private static readonly Component.Component[] Components = (Component.Component[])Enum.GetValues(typeof(Component.Component));

        public static void RandomReactor(Reactor reactor)
        {
            for (uint x = 0; x < reactor.GetWidth(); x++)
            {
                for (uint y = 0; y < reactor.GetHeight(); y++)
                {
                    Component.Component component = GetNextComponent();

                    IComponent newComponent;

                    switch (component)
                    {
                        case Component.Component.UranCell:
                            newComponent = new UraninumCell(x, y, component);
                            break;
                        case Component.Component.CoolCell:
                            newComponent = new CoolantCell(x, y, component);
                            break;
                        case Component.Component.HeatDispenser:
                            newComponent = new HeatDispenser(x, y, component);
                            break;
                        case Component.Component.ReactorPlating:
                            newComponent = new ReactorPlating(x, y, component);
                            break;
                        case Component.Component.Empty:
                            newComponent = new Empty(x, y, component);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    reactor.Pulse += newComponent.PulseHandler;

                    reactor.Components.Add(newComponent);
                }
            }
        }

        public static Component.Component GetNextComponent()
        {
            return Components[Random.Next(0, Components.Length)];
        }
    }
}