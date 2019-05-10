using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkDescriptions
{
    //Маршрутная матрица для сети с делениеи слиянием требований
    public class RoutingMatrix
    {
        private int[,,,] Theta;

        //Маршрутная матрица 
        public RoutingMatrix(int ij, int kl)
        {
            Theta = new int[ij, ij, kl, kl];
        }

        //Размерность матрицы (количество узлов в сети)
        public int Dimention1 { get { return Theta.GetLength(0); } }
        public int Dimention2 { get { return Theta.GetLength(2); } }

        public int this[int i, int j, int k, int l]
        {
            get { return Theta[i, j, k, l]; }
            set { Theta[i, j, k, l] = value; }
        }

        //Строка матрицы передачи
        public double[,] RoutingRow(int i, int j)
        {
            double[,] row = new double[Dimention2, Dimention2];
            for (int k = 0; k < Dimention2; k++)
            {
                for (int l = 0; l < Dimention2; l++)
                {
                    row[k, l] = Theta[i, j, k, l];
                }
            }

            return row;
        }

        //Маршрутная матрица для фиксированного узла
        public double[,,] RoutingMatrixForNode(int i)
        {
            double[,,] row = new double[Dimention1, Dimention2, Dimention2];
            for (int j = 0; j < Dimention1; j++)
            {
                for (int k = 0; k < Dimention2; k++)
                {
                    for (int l = 0; l < Dimention2; l++)
                    {
                        row[j, k, l] = Theta[i, j, k, l];
                    }
                }
            }

            return row;
        }

        //Заполение матрицы строкой из файла
        public void FillingTheta(string line)
        {
            string[] data = line.Split(';');
            Theta[int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3])] = 1;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder(string.Empty);
            str.AppendLine("Theta: ");

            for (int i = 0; i < Theta.GetLength(0); i++)
            {
                for (int j = 0; j < Theta.GetLength(0); j++)
                {
                    for (int k = 0; k < Theta.GetLength(2); k++)
                    {
                        for (int l = 0; l < Theta.GetLength(2); l++)
                        {
                            if (Theta[i, j, k, l] == 1)
                            {
                                str.AppendLine($"Theta[{i}, {j}, {k}, {l}] = 1");
                            }
                        }
                    }
                }
            }
            str.AppendLine();
            return str.ToString();
        }
    }
}
