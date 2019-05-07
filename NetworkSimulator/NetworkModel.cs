using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkSimulator
{
    //Имитационная модель сети 
    public class NetworkModel
    {
        //Узлы в сети
        public Node[] Nodes { get; private set; }

        //Информационный узел
        public InfoNode Info { get; private set; }

        //Генератор случайных чисел
        private Random Random { get; set; }

        public NetworkModel(Node[] nodes, InfoNode info, Random random)
        {
            Nodes = nodes;
            Info = info;
            Random = random;
        }

        //Запускаем имитационную модель
        public void Run(double FinishTime)
        {
            double CurrentTime = 0;

            Info.SetCurrentTime(CurrentTime);
            Node NextActionNode;
            double NextTime;

            while (CurrentTime <= FinishTime)
            {
                //Выбор узла для передачи управления
                NextActionNode = Nodes[0];
                NextTime = Nodes[0].NextEventTime;
                for (int i = 0; i < Nodes.Length; i++)
                {
                    if (Nodes[i].NextEventTime < NextTime)
                    {
                        NextActionNode = Nodes[i];
                        NextTime = NextActionNode.NextEventTime;
                    }
                }

                CurrentTime = NextTime;
                Info.SetCurrentTime(CurrentTime);
                //Передача управления узлу
                NextActionNode.Activate();
            }
        }

        //Анализ имитационной сети
        public void Analysis(out double AverageRT)
        {
            Console.WriteLine("E(tau) = {0:f4}", (Nodes[0] as SourceNode).ResponseTimes.Average());
            double tau = (Nodes[0] as SourceNode).ResponseTimes.Average();
            AverageRT = tau;
            var rt = (Nodes[0] as SourceNode).ResponseTimes;
            List<double> vars = new List<double>();

            foreach (var item in rt)
            {
                vars.Add((item - tau) * (item - tau));
            }

            Console.WriteLine("Var(tau) = {0:f4}", vars.Average());
        }
    }
}
