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

            var Nodes = new Node[descriptoin.Theta.Dimention1];
            //Источник требований
            Nodes[0] = new SourceNode(0, random, new ExponentialVariable(random, descriptoin.Lambda0), Nodes, Info, descriptoin.Theta.RoutingRow(0, 0));
            //Создается множество (тип) систем
            for (int i = 0; i < descriptoin.S.Length; i++)
            {
                Nodes[descriptoin.S[i]] = new ServiceNode(i);
                //Множество (тип) заполняется базовыми системами с одним устройством обслуживания 
                for (int j = 0; j < descriptoin.s.Length; j++)
                {
                    Nodes[descriptoin.S[i]].AddBasicNode(j, random, new ExponentialVariable(random, descriptoin.mu[j]),
                        new QueueBuffer(), descriptoin.kappa[j], Nodes, Info, descriptoin.Theta.RoutingMatrixForNode(descriptoin.s[i]));
                }
                //Дивайдеры
                for (int k = 0; k < descriptoin.F.Length; k++)
                {
                    Nodes[descriptoin.F[k]] = new ForkNode(descriptoin.F[k], k + 1, random, Nodes, Info, descriptoin.Theta.RoutingRow(descriptoin.F[k], k + 1));
                }
                //Интеграторы
                for (int k = 0; k < descriptoin.J.Length; k++)
                {
                    Nodes[descriptoin.J[k]] = new JoinNode(descriptoin.J[k], random, Nodes, Info, descriptoin.Theta.RoutingMatrixForNode(descriptoin.J[k]));
                }
            }
            return new NetworkModel(Nodes, Info, random);
        }
    }
}
