﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkSimulator
{
    //Дивайдер
    public class ForkNode : Node
    {
        //Идентификатор дивайдера
        public int ForkNodeID { get; protected set; }

        //Маршрутная строка для дивайдера
        protected double[] RouteRow { get; set; }

        //Число фрагментов на которое делится поступивший фрагмент
        private int ForkDegree { get; }

        public ForkNode(int id, int forkNodeID, Random random, Node[] nodes, InfoNode info, double[] routeRow)
        {
            ID = id;
            ForkNodeID = forkNodeID;
            this.random = random;
            Nodes = nodes;
            Info = info;
            RouteRow = routeRow;

            NumberOfArrivedDemands = 0;
            ForkDegree = (int)RouteRow.Sum();
            NextEventTime = double.PositiveInfinity;
        }

        //Отправляет фрагмент указанному узлу
        protected override void Send(Fragment fragment, Node node)
        {
            node.Receive(fragment);
        }

        //Распределяет фрагмент по узлам
        protected override void Route(Fragment fragment)
        {
            //Номер фрагмента начиная с единицы
            int partIndex = 1;
            //Проход по каждому смежному узлу
            for (int i = 0; i < Nodes.Length; i++)
            {
                //Создаем фрагмент необходимое количество раз
                for (int j = 0; j < RouteRow[i]; j++)
                {
                    Fragment part = new Fragment(fragment.TimeGeneration, fragment.ID, new SignatureForFragment(fragment, partIndex, ForkNodeID));
                    part.NumberOfParts = ForkDegree;
                    //Отправляем фрагмент в смежный узел
                    Send(part, Nodes[i]);
                    //Увеличиваем индекс фрагмента
                    partIndex++;
                }
            }
        }

        //Получение фрагмента из некого узла
        public override void Receive(Fragment fragment)
        {
            NumberOfArrivedDemands++;
            Route(fragment);
        }

        //Активация дивайдера
        public override void Activate()
        {/**/}
    }
}