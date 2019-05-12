using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetworkDescriptions;
using NetworkSimulator;

namespace Network_Simulation
{
    public class Start
    {

        static string file = "OneNetwork";

        public static void StartModel(Random rand, double FinishTime,
            double lambda, out double SimulationRT, out double ApproximationRT)
        {
            SimulationRT = 0;
            ApproximationRT = 0;

            string filename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + file + ".txt";
            //Описываем сеть
            var NetDesctiption = new Description(filename);
            Console.WriteLine(NetDesctiption);

            NetDesctiption.Lambda0 = lambda;

            Console.WriteLine("Lambda0 = {0:f4}", lambda);
            Console.WriteLine("Имитационное моделирование одноприборной сети обслуживания");

            //Создаем модель по её описанию 
            NetworkModel OriginalModel = OFJQN.CreateNetworkModel(NetDesctiption, rand);
            //Запускаем модель
            OriginalModel.Run(FinishTime);
            //Собираем статистику по модели
            OriginalModel.Analysis(out SimulationRT);
        }

        static void Main()
        {
            file = "OneNetwork";
            Random random = new Random();
            double FinishTime = 1000000;
            double Lambda = 0.5;
            double ApproximationRT = 0;
            double SimulationRT = 0;

            StartModel(random, FinishTime, Lambda, out SimulationRT, out ApproximationRT);
        }
    }
}
