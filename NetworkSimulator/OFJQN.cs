using NetworkDescriptions;
using RandomVariables;
using System;

namespace NetworkSimulator
{
    // Имитционная модель для открытой экспоненциальносй сети массового обслуживания 
    // с делением и слиянием требований и произвольным числом обслуживающих прибором с дисциплиной FCFS
    public static class OFJQN
    {
        public static NetworkModel CreateNetworkModel(Description description, Random random)
        {
            InfoNode Info = new InfoNode();
            Info.SetCurrentTime(0);

            var Nodes = new Node[description.Theta.Dimention1];
            //Источник требований
            Nodes[0] = new SourceNode(0, random, new ExponentialVariable(random, description.Lambda0), Nodes, Info, description.Theta.RoutingRow(0, 0));
            //Создание балансировщиков и присвоение ему базовых систем
            for (int i = 0; i < description.S.Length; i++)
            {
                Nodes[description.B[i]] = new BalancerNode(i);
                //Множество (тип) заполняется базовыми системами с одним устройством обслуживания 
                for (int j = 0; j < description.S.Length; j++)
                {
                    Nodes[description.S[j]] = new ServiceNode(j, random, new ExponentialVariable(random, description.mu[j]),
                        new QueueBuffer(), description.kappa[j], Nodes, Info, description.Theta.RoutingMatrixForNode(description.S[i]));
                    (Nodes[description.B[i]] as BalancerNode).AddServiceNode((ServiceNode)Nodes[description.S[j]]);
                }
                //Дивайдеры
                for (int k = 0; k < description.F.Length; k++)
                {
                    Nodes[description.F[k]] = new ForkNode(description.F[k], k + 1, random, Nodes, Info, description.Theta.RoutingRow(description.F[k], k + 1));
                }
                //Интеграторы
                for (int k = 0; k < description.J.Length; k++)
                {
                    Nodes[description.J[k]] = new JoinNode(description.J[k], random, Nodes, Info, description.Theta.RoutingMatrixForNode(description.J[k]));
                }
            }
            return new NetworkModel(Nodes, Info, random);
        }
    }
}
