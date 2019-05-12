using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RandomVariables;

namespace NetworkSimulator
{
    public class BalancerNode : Node
    {
        public int ID { get; set; }

        public List<ServiceNode> ServiceNodes { get; set; }

        public BalancerNode(int id)
        {
            ID = id;
            ServiceNodes = new List<ServiceNode>();
        }

        //Добавляет узел к балансировщику
        public void AddServiceNode(ServiceNode serviceNode)
        {
            ServiceNodes.Add(serviceNode);
        }

        //Возвращает узел с наименьшей длиной очереди
        private ServiceNode PassNode()
        {          
            int min = int.MaxValue;
            int i_min = 0;

            for (int i = 0; i < ServiceNodes.Count; i++)
            {
                if (min > ServiceNodes[i].NumberOfFragments())
                {
                    min = ServiceNodes[i].NumberOfFragments();
                    i_min = i;
                }
            }
            return ServiceNodes[i_min];
        }

        public override void Activate()
        {
            PassNode().Activate();
        }

        public override void Receive(Fragment fragment)
        {
            PassNode().Receive(fragment);
        }

        public override void Route(Fragment fragment)
        {
            PassNode().Route(fragment);
        }

        public override void Send(Fragment fragment, Node node)
        {
            PassNode().Send(fragment, node);
        }
    }
}
