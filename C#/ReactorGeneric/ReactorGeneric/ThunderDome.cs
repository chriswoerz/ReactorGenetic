using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactorGeneric
{
    public static class ThunderDome
    {
        public static Population ItsPopulation { get; set; }

        public static event ReportEventHandler Report;

        public static void OnReport(ReportEventArgs e)
        {
            ReportEventHandler handler = Report;
            if (handler != null) handler(null, e);
        }


        public static void Start(int population, int chambers, int generations, int ticksPerGeneration)
        {
            ItsPopulation = new Population(population, chambers);

            for (int i = 0; i < ticksPerGeneration; i++)
            {
                if(ItsStopFlag){
                    ItsStopFlag = false;
                    break;
                }

                ItsPopulation.OnPulse(new EventArgs());
            }

            Fitness.Determine(ItsPopulation);

            var reporttxt = ItsPopulation.GenerateReport();

            OnReport(new ReportEventArgs{ReportText = reporttxt});

        }

        private static bool ItsStopFlag { get; set; }

        public static void Pause()
        {
            throw new NotImplementedException();
        }

        public static void Stop()
        {
            ItsStopFlag = true;
        }
    }

    public static class Fitness
    {
        public static void Determine(Population itsPopulation)
        {
            itsPopulation.ItsReactors.ToList().Sort();
        }
    }
}
