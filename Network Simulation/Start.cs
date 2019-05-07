using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkDescriptions;
using NetworkSimulator;

namespace Network_Simulation
{
    public class Start
    {

        static string file = "OneNetwork";

        //Анализ одноприборной сети обслуживания и проверка на имитационную модель
        public static bool FiniteAnalysis(Random random, double FinishTime, double lambda,
            out double SimulationRT, out double ApproximationRT, out double[] rho)
        {
            //Предустановленные значения
            SimulationRT = 0;
            ApproximationRT = 0;

            Console.Clear();

            string filename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + file + ".txt";

            //Получаем описание сети
            var Description = new Descriptoin(filename);

            Console.WriteLine("================================================================================");
            Console.WriteLine("Lambda0 = {0:f4}", lambda);
            Console.WriteLine("Имитационное моделирование одноприборной сети обслуживания");

            //Создаем модель по описанию из файла
            NetworkModel Model = 

        }

        static void Main()
        {

        }
    }
}
