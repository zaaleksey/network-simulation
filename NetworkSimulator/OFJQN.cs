using NetworkDescriptions;
using RandomVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkSimulator
{
    // Имитционная модель для открытой экспоненциальносй сети массового обслуживания 
    // с делением и слиянием требований и произвольным числом обслуживающих прибором с дисциплиной FCFS
    public static class OFJQN
    {
        public static NetworkModel CreateNetworkModel(Descriptoin descriptoin, Random random)
        {
            InfoNode Info = new InfoNode();
            Info.SetCurrentTime(0);

            var Nodes = new Node[descriptoin.Theta.Dimention];
            //Источник требований
            Nodes[0] = new SourceNode(0, random, new ExponentialVariable(random, descriptoin.Lambda0), Nodes, Info, descriptoin.Theta.);
        }
    }
}
