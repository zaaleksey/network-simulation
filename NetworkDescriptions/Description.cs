using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NetworkDescriptions
{
    //Описание открытой экспоненциальной СеМО с делением и слиянием требований
    //Пауссоновский входящий поток поток
    //Дисциплина FCFS
    //Одноприборные базовые системы
    public class Description
    {
        //Номер источника
        public int N { get; set; }
        //Массив балансировщиков 
        public int[] B { get; set; }
        //Массив номеров интеграторов
        public int[] J { get; set; }
        //Массив номеров дивайдеров
        public int[] F { get; set; }
        //Массив базовых систем
        public int[] S { get; set; }
        //Матрица перехода
        public RoutingMatrix Theta { get; set; }
        //Интенсивность входящего потока
        public double Lambda0 { get; set; }
        //Массив интенсивностей обслуживания
        public double[] mu { get; set; }
        //Число обслуживающих устройст в базовых системах
        public int[] kappa { get; set; }

        public Description(int[] B, int[] F, int[] J, double[] mu, double Lambda0, int[] S, int[] kappa, RoutingMatrix Theta)
        {
            N = 0;//Номер источника (всегда 0)
            this.B = B;
            this.F = F;
            this.J = J;
            this.mu = mu;
            this.Lambda0 = Lambda0;
            this.S = S;
            this.kappa = kappa;
            this.Theta = Theta;
            //место для матрицы перехода
        }

        //Создает открытую сеть с деление и слиянием требований, считывая данные из файла
        public Description(string FileName)
        {
            using (StreamReader file = new StreamReader(FileName))
            {
                //Балансировщики
                var temp = file.ReadLine().Split(';');
                B = new int[temp.Length];
                for (int i = 0; i < temp.Length; i++)
                {
                    B[i] = int.Parse(temp[i]);
                }

                //Дивайдеры ForkNode
                temp = file.ReadLine().Split(';');
                if (temp[0].Length != 0)
                {
                    F = new int[temp.Length];
                    for (int i = 0; i < temp.Length; i++)
                    {
                        F[i] = int.Parse(temp[i]);
                    }
                }
                else
                {
                    F = new int[0];
                }

                //Интеграторы JoinNode
                temp = file.ReadLine().Split(';');
                if (temp[0].Length != 0)
                {
                    J = new int[temp.Length];
                    for (int i = 0; i < temp.Length; i++)
                    {
                        J[i] = int.Parse(temp[i]);
                    }
                }
                else
                {
                    J = new int[0];
                }

                //Параметры СМО. Интенсивность обслуживания
                temp = file.ReadLine().Split(';');
                mu = new double[B.Length];
                for (int i = 0; i < mu.Length; i++)
                {
                    mu[i] = double.Parse(temp[i]);
                }

                //Базовые системы
                temp = file.ReadLine().Split(';');
                S = new int[B.Length];
                for (int i = 0; i < S.Length; i++)
                {
                    S[i] = int.Parse(temp[i]);
                }

                //Число обслуживающих устройств в каждом типе (множестве базовых систем)
                temp = file.ReadLine().Split(';');
                kappa = new int[S.Length];
                for (int i = 0; i < kappa.Length; i++)
                {
                    kappa[i] = int.Parse(temp[i]);
                }

                //Интенсивность входящего потока
                Lambda0 = double.Parse(file.ReadLine());

                //Матрица перехода
                string str;
                Theta = new RoutingMatrix(B.Length + F.Length + J.Length + 1, F.Length + J.Length + 2);
                while ((str = file.ReadLine()) != null)
                {
                    Theta.FillingTheta(str);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder(string.Empty);
            str.AppendLine("Параметра сети");

            str.AppendLine("B = ");
            for (int i = 0; i < B.Length; i++)
            {
                str.AppendFormat("{0}  ", B[i]);
            }
            str.AppendLine();

            str.AppendLine("F = ");
            for (int i = 0; i < F.Length; i++)
            {
                str.AppendFormat("{0}  ", F[i]);

            }
            str.AppendLine();

            str.AppendLine("J = ");
            for (int i = 0; i < J.Length; i++)
            {
                str.AppendFormat("{0}  ", J[i]);
            }
            str.AppendLine();

            //Интесивности обслуживания для одного прибора в системе
            str.AppendLine("mu = ");
            foreach (var item in mu)
            {
                str.AppendFormat("{0:f2}  ", item);
            }
            str.AppendLine();

            //Число обслуживающих приборов в системах
            str.AppendLine("S = ");
            foreach (var item in S)
            {
                str.AppendFormat("{0}  ", item);
            }
            str.AppendLine();

            //Интенсивность входящего потока
            str.AppendLine("lambda0 = ");
            str.AppendFormat("{0:f2}", Lambda0);


            str.AppendLine();
            str.AppendLine(Theta.ToString());

            return str.ToString();
        }
    }
}
