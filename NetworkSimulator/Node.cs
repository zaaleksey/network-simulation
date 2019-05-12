using RandomVariables;
using System;

namespace NetworkSimulator
{
    //Узел СеМО
    public abstract class Node
    {
        //Число поступивших фрагментов
        public long NumberOfArrivedDemands { get; protected set; }
        //Идентификатор узла
        public int ID { get; protected set; }
        //Генератор случайных чисел
        public Random random;

        //Узлы с которыми происходит обмен фрагментами
        public Node[] Nodes { get; protected set; }

        //Информационный узел
        public InfoNode Info { get; protected set; }

        //Время активации узла
        public double NextEventTime { get; protected set; }
        
        //Получение фрагмента узлом
        public abstract void Receive(Fragment fragment);
        
        //Отправляет фрагмент по сети обслуживания согласно маршрутизации
        public abstract void Route(Fragment fragment);

        //Отправляет фрагмент в узел 
        public abstract void Send(Fragment fragment, Node node);

        //Активация узла
        public abstract void Activate();
    }
}
