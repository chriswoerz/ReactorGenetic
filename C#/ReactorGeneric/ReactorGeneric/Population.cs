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
                            newComponent = new UraninumCell(x, y);
                            break;
                        case Component.Component.CoolCell:
                            newComponent = new CoolantCell(x, y);
                            break;
                        case Component.Component.HeatDispenser:
                            newComponent = new HeatDispenser(x, y);
                            break;
                        case Component.Component.ReactorPlating:
                            newComponent = new ReactorPlating(x, y);
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

    public class ReactorPlating : AbstractComponent
    {
        public ReactorPlating(uint xpos, uint ypos)
            : base(xpos, ypos)
        {
            
        }

        public override void PulseHandler(object sender, EventArgs e)
        {
            var reactor = (Reactor)sender;
            reactor.GetComponent((int)XPosition - 1, (int)YPosition);
        }
    }

    public class HeatDispenser : AbstractComponent
    {
        public HeatDispenser(uint xpos, uint ypos)
            : base(xpos, ypos)
        {
            
        }

        public override void PulseHandler(object sender, EventArgs e)
        {

        }
    }

    public class CoolantCell : AbstractComponent
    {
        public CoolantCell(uint xpos, uint ypos) : base(xpos, ypos)
        {
            
        }

        public override void PulseHandler(object sender, EventArgs e)
        {
           
        }
    }

    public class UraninumCell : AbstractComponent
    {
        public UraninumCell(uint xpos, uint ypos)
            : base(xpos, ypos)
        {
            
        }

        public override void PulseHandler(object sender, EventArgs e)
        {
           
        }
    }
}