using System;
using ReactorGeneric.Component;

namespace ReactorGeneric
{
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

                    reactor.ItsComponents.Add(newComponent);
                }
            }
        }

        public static Component.Component GetNextComponent()
        {
            return Components[Random.Next(0, Components.Length)];
        }
    }
}