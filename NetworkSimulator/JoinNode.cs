using System;
using System.Collections.Generic;
using RandomVariables;

namespace NetworkSimulator
{
    public class JoinNode : Node
    {
        //Матрица для маршрутизации фрагментов
        protected double[,,] RouteMatrixForNode { get; set; }

        //Буффер для хранения фрагментов в дивайдере
        protected List<Fragment> InBuffet { get; set; }

        //Прием фрагмента в интегратор
        public override void Receive(Fragment fragment)
        {
            for (int i = 0; i < InBuffet.Count; i++)
            {
                if (InBuffet[i].Sigma.ParentFragment == fragment.Sigma.ParentFragment)
                {
                    InBuffet[i].NumberOfParts--;
                    if (InBuffet[i].NumberOfParts == 1)
                    {
                        var parent = InBuffet[i].Sigma.ParentFragment;
                        InBuffet.RemoveAt(i);
                        Route(parent);
                    }
                    return;
                }
            }
            //Если таких фрагментов нет, то добавляем фрагмент в очередь на ожидание
            InBuffet.Add(fragment);
            //Время активации - бесконечность
            NextEventTime = double.PositiveInfinity;
        }

        //Отправляет фрагмент по сети
        protected override void Route(Fragment fragment)
        {
            double rand = random.NextDouble();
            double p = 0;
            int k = fragment.Sigma.ForkNodeID;

            for (int i = 0; i < RouteMatrixForNode.GetLength(1); i++)
            {
                for (int j = 0; j < RouteMatrixForNode.GetLength(2); j++)
                {
                    p += RouteMatrixForNode[k, i, j];
                    if (rand < p)
                    {
                        //Посылаем фрагмент в указанный узел
                        Send(fragment, Nodes[i]);
                        break;
                    }
                }
            }
        }

        //Отправка фрагмента в заданный узел сети
        protected override void Send(Fragment fragment, Node node)
        {
            node.Receive(fragment);
        }

        //Выполняемое действие дивайдера
        public override void Activate()
        {
            //Следующий момент активации
            NextEventTime = double.PositiveInfinity;
        }

        public override void AddBasicNode(int ID, Random r, RandomVariable ServiceTime, Buffer InBuffer, int kappa, Node[] Nodes, InfoNode Info, double[,,] RouteMatrix)
        {
            throw new NotImplementedException();
        }

        //Создание и инициализация интегратора
        public JoinNode(int ID, Random random, Node[] nodes, InfoNode info, double[,,] routeMatrixForNode)
        {
            this.ID = ID;
            this.random = random;
            Nodes = nodes;
            Info = info;
            RouteMatrixForNode = routeMatrixForNode;

            NextEventTime = double.PositiveInfinity;
            InBuffet = new List<Fragment>();
        }
    }
}
