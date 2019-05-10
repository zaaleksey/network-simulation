using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomVariables;

namespace NetworkSimulator
{
    public class ServiceNode : Node
    {
        //Список базовых классов
        public List<BasicNode> basicNodes { get; set; }

        public ServiceNode(int ID)
        {
            this.ID = ID;
            basicNodes = new List<BasicNode>();
        }
        //Создание базовой системе с типом ID
        public override void AddBasicNode(int ID, Random r, RandomVariable ServiceTime, Buffer InBuffer, int kappa, Node[] Nodes, InfoNode Info, double[,,] RouteMatrix)
        {
            basicNodes.Add(new BasicNode(ID, r, ServiceTime, InBuffer, kappa, Nodes, Info, RouteMatrix));
        }

        //Осуществляется выбор в какую очередь отправить требование 
        public void ReceiveQueue(Fragment fragment)
        {
            int min = int.MaxValue;
            int i_min = 0;
            //Проходит по списку базовых систем и находит систему 
            //с минимальной оцередью
            for (int i = 0; i < basicNodes.Count; i++)
            {
                if (basicNodes[i].NumberOfFragments() < min)
                {
                    min = basicNodes[i].NumberOfFragments();
                    i_min = i;
                }
            }
            //Отправляет фрагмент в систему с самой маленькой очередью
            basicNodes[i_min].Receive(fragment);
        }

        public override void Receive(Fragment fragment)
        {
            throw new NotImplementedException();
        }

        protected override void Route(Fragment fragment)
        {
            throw new NotImplementedException();
        }

        protected override void Send(Fragment fragment, Node node)
        {
            throw new NotImplementedException();
        }

        public override void Activate()
        {
            throw new NotImplementedException();
        }
    }
}
