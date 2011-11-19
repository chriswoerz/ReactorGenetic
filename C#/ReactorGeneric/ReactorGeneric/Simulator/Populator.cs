using System;
using System.Collections.Generic;
using ReactorGeneric.Component;

namespace ReactorGeneric.Simulator
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

                    var newComponent = BuildComponent(x, y, component);

                    reactor.Pulse += newComponent.PulseHandler;

                    reactor.ItsComponents.Add(newComponent);
                }
            }
        }

        public static IList<IComponent> BuildComponentsFromRNA(Reactor reactor, string rna )
        {
            if (rna.Length - reactor.GetWidth() != reactor.GetWidth()*reactor.GetHeight()) throw new ArgumentOutOfRangeException("rna", @"rna didnt match reactor parameters");

            var gridRNA = rna.Split(':');

            var toReturn = new List<IComponent>();

            for (uint x = 0; x < gridRNA.Length; x++)
            {
                string column = gridRNA[x];

                for (uint y = 0; y < column.Length; y++)
                {
                    Component.Component toBuild;

                    switch (column[(int)y])
                    {
                        case 'U':
                            toBuild = Component.Component.UranCell;
                            break;
                        case 'C':
                            toBuild = Component.Component.CoolCell;
                            break;
                        case 'E':
                            toBuild = Component.Component.Empty;
                            break;
                        case 'R':
                            toBuild = Component.Component.ReactorPlating;
                            break;
                        case 'H':
                            toBuild = Component.Component.HeatDispenser;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    var newComponent = BuildComponent(x, y, toBuild);
                    reactor.Pulse += newComponent.PulseHandler;
                    toReturn.Add(newComponent);
                }
            }

            return toReturn;
        }

        public static IComponent BuildComponent(uint x, uint y, Component.Component component)
        {
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
            return newComponent;
        }

        public static Component.Component GetNextComponent()
        {
            return Components[Random.Next(0, Components.Length)];
        }

        public static IList<Reactor> Repopulate(Generation generation, IList<ReactorResult> startingPopulation, int targetPopulation, int chambers, int externalCoolingPerTick)
        {
            IList<Reactor> toReturn = new List<Reactor>();

            for (int i = 0; i < targetPopulation; i++)
            {
                var mother = startingPopulation[Random.Next(0, startingPopulation.Count)];
                var father = startingPopulation[Random.Next(0, startingPopulation.Count)];

                string childRNA = SingleCrossover(mother.ItsManifest, father.ItsManifest);

                string mutatedChildRNA = SingleMutation(childRNA);

                toReturn.Add(new Reactor(generation, chambers, childRNA, externalCoolingPerTick));
            }

            return toReturn;
        }

        private static string SingleMutation(string childRna)
        {
            int mutationSite = Random.Next(0, childRna.Length - 1 );

            while (childRna[mutationSite] == ':')
            {
                mutationSite = Random.Next(0, childRna.Length - 1 );
            }
            var newComponent = GetNextComponent();
            string newGene;
            switch (newComponent)
            {
                case Component.Component.UranCell:
                    newGene = "U";
                    break;
                case Component.Component.CoolCell:
                    newGene = "C";
                    break;
                case Component.Component.HeatDispenser:
                    newGene = "H";
                    break;
                case Component.Component.ReactorPlating:
                    newGene = "R";
                    break;
                case Component.Component.Empty:
                    newGene = "E";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return childRna.Remove(mutationSite, 1).Insert(mutationSite, newGene);
        }

        public static string SingleCrossover(string motherRNA, string fatherRNA)
        {
            if(motherRNA.Length != fatherRNA.Length) throw new ArgumentOutOfRangeException("motherRNA", @"parent's RNA are different lengths");

            var crossoverIndex = Random.Next(0, motherRNA.Length - 1 );
            var chileRNA = motherRNA.Remove(crossoverIndex) + fatherRNA.Substring(crossoverIndex);

            return chileRNA;
        }
    }
}