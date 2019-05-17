using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomVariables;


namespace NetworkSimulator
{
    public class SourceNode : Node
    {
        /// <summary>
        /// Среднее время отклика
        /// </summary>
        public List<double> ResponseTimes
        {
            get;
            private set;
        }

        /// <summary>
        /// Получение требования (возврат требования в исчтоник) 
        /// </summary>
        /// <param name="fragment"></param>
        public override void Receive(Fragment fragment)
        {
            //Console.WriteLine("Требование вернулось");
            ResponseTimes.Add(Info.GetCurrentTime() - fragment.TimeGeneration);
        }

        /// <summary>
        /// Отправяляет требование из источника по сети 
        /// </summary>
        /// <param name="fragment"></param>
        public override void Route(Fragment fragment)
        {
            double rand = random.NextDouble();
            double p = 0;
            for (int j = 0; j < RouteRow.GetLength(0); j++)
            {
                for (int k = 0; k < RouteRow.GetLength(1); k++)
                {
                    p += RouteRow[j, k];
                    if (rand < p)
                    {
                        //Посылаем фрагмент в указанный узел
                        Send(fragment, Nodes[j]);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Отправление требования от источника к другому узлу
        /// </summary>
        /// <param name="fragmetn">Требование</param>
        /// <param name="node">Узел-получатель</param>
        public override void Send(Fragment fragmetn, Node node)
        {
            node.Receive(fragmetn);
        }

        /// <summary>
        /// Счетчик всех созданных фрагментов
        /// </summary>
        private long FragmentCounter
        {
            get;
            set;
        }


        /// <summary>
        /// Передача управления источнику
        /// </summary>
        public override void Activate()
        {
            //Создаем требование
            Fragment f = new Fragment(Info.GetCurrentTime(), FragmentCounter, new SignatureForFragment(null, 1, 0));
            //Требование ссылается само на себя
            f.Sigma.ParentFragment = f;
            FragmentCounter++;

            //Время следующего события
            NextEventTime = Info.GetCurrentTime() + ArrivalInterval.NextValue();
            //Отправка требования по сети обслуживания
            Route(f);
        }

        /// <summary>
        /// Случайная величина между (интервалы между требованиями) 
        /// </summary>
        protected RandomVariable ArrivalInterval
        {
            get;
            set;
        }


        /// <summary>
        /// Маршрутная строка только для смежных узлов (первый индекс передает смежный узел, второй и третий передают фрагменты)
        /// </summary>
        private double[,] RouteRow
        {
            get;
            set;
        }

        /// <summary>
        /// Инициализация источника требований
        /// </summary>
        /// <param name="r">Интервалы между поступлениями требований</param>
        /// <param name="RouteRow">Строка для маршрутизации требований</param>
        /// <param name="ID">Идентификатор узла</param>
        public SourceNode(int ID, Random r, RandomVariable ArrivalInterval, Node[] Nodes, InfoNode Info, double[,] RouteRow)
        {
            //Передача параметров
            this.ID = ID;
            this.random = r;
            this.ArrivalInterval = ArrivalInterval;
            this.Nodes = Nodes;
            this.Info = Info;
            this.RouteRow = RouteRow;

            //Первое поступление происходит в нулевой момент времени
            this.NextEventTime = 0;
            FragmentCounter = 0;
            //Для сбора статистики 
            ResponseTimes = new List<double>();
        }
    }
}
