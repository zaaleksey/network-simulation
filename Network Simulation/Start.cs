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

        //Анализ одноприборной сети обслуживания и проверка на имитационную модель
        public static bool FiniteAnalysis(Random random, double FinishTime, double lambda,
            out double SimulationRT, out double ApproximationRT)
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
            NetworkModel Model = OFJQN.CreateNetworkModel(Description, random);
            //Запуск модели
            Model.Run(FinishTime);
            //Сбор статистики
            Model.Analysis(out SimulationRT);

            Console.WriteLine();


            //Console.WriteLine("Выполнение приближённого метода анализа");
            ////Вычисление интенсивностей входящего потока в каждую из систем обслуживания
            //var Lambda = InfinityServerOpenForkJoinAnalizator.TotalInputRates(
            //                 InfinityServerOpenForkJoinAnalizator.InputRates(Description.S, Description.F, Description.J,
            //                     Description.Lambda0, Description.Theta));

            //Выполняется преобразование интенсивностей и увеличение числа приборов
            //Console.WriteLine("Выполняем переход к бесконечноприборной сети с интенсивностями обслуживания:");
            ////Меняем описание для сети

            //rho = new double[Description.S.Length];
            //for (int i = 0; i < Description.S.Length; i++)
            //{
            //    //Увеличиваем число обслуживающих приборов до "бесконечности"
            //    Description.kappa[i] = 1000000;

            //    //Считаем ro так как если бы это была одноприборная сеть обслуживания
            //    rho[i] = Lambda[i] / Description.mu[i];

            //    //Модифицируем интенсинвность обслуживания фрагментов одним прибором
            //    Description.mu[i] = Description.mu[i] - Lambda[i];

            //    Console.WriteLine("mu[{0}] = {1:f4}, ro[{0}] = {2:f4}", i + 1, Description.mu[i], rho[i]);
            //    if (Description.mu[i] <= 0.01)
            //    {
            //        Console.WriteLine("Отсутствие стационарного режима");
            //        return false;
            //    }
            //}



            //Console.WriteLine("Аналитическое моделирование");
            //var ph = InfinityServerOpenForkJoinAnalizator.ResponseTimeDistribution(Description);
            //Console.WriteLine("Число фаз {0}", ph.NumberOfPhases);
            //ApproximationRT = ph.ExpectedValue();
            //Console.WriteLine("E(tau) = {0:f4}", ApproximationRT);
            ////Console.WriteLine("Var(tau) = {0:f4}", ph.Variance());

            //Console.WriteLine("Имитационное моделирование бесконечноприборной - проверка");
            //NetworkModel TransformedModel = OFJQN.CreateNetworkModel(Description, rand);
            //TransformedModel.Run(FinishTime);
            //double temp;
            //TransformedModel.Analysis(out temp);



            //Console.WriteLine("Press any key ...");
            //Console.ReadKey();



            return true;
        }

        /// <summary>
        /// Моделирует сеть, зависимую от нагрузки выдает графики 
        /// </summary>
        public static void Example1()
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ru-RU");
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture("ru-RU");


            Random rand = new Random();
            double FinishTime = 1000000;

            //Console.Clear(); 
            //CreateFile(5); 

            double LambdaMin = 0.5;
            double LambdaMax = 4.4;
            double Lambda = 0;
            double h = 0.2;


            //Списки с результатами моделирования
            List<double> ListLambda = new List<double>();
            List<double> ListApproximationRT = new List<double>();
            List<double> ListSimulationRT = new List<double>();
            List<double[]> ListRho = new List<double[]>();


            bool stationary = true;
            //Пока существует стационарный режим и не перешли границу по LambdaMax

            Lambda = LambdaMin;
            do
            {
                double ApproximationRT = 0;
                double SimulationRT = 0;
                double[] rho;


                if (!FiniteAnalysis(rand, FinishTime, Lambda, out SimulationRT, out ApproximationRT))
                {
                    stationary = false;
                }
                else
                {
                    ListLambda.Add(Lambda);
                    ListApproximationRT.Add(ApproximationRT);
                    ListSimulationRT.Add(SimulationRT);
                }


                Lambda += h;



            }
            while ((stationary) && (Lambda < LambdaMax));




            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Results/" + file + ".csv"))
            {
                sw.Write("Lambda App Sim");
                for (int j = 0; j < ListRho[0].Length; j++)
                {
                    sw.Write(" rho{0}", j + 1);
                }
                sw.WriteLine();

                for (int i = 0; i < ListLambda.Count(); i++)
                {
                    sw.Write("{0:f4} {1:f4} {2:f4}", ListLambda[i], ListApproximationRT[i], ListSimulationRT[i]);
                    for (int j = 0; j < ListRho[i].Length; j++)
                    {
                        sw.Write(" {0:f4}", ListRho[i][j]);
                    }
                    sw.WriteLine();
                }

            }




            Console.WriteLine("Программа завершила свою работу");
            Console.ReadKey();


        }

        static void Main()
        {
            file = "OneNetwork";
            Example1();
        }
    }
}
