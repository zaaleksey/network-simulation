using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExactNetworkAnalysis
{/// <summary>
 /// Анализ открытой экспоненциальной сети массового обслуживания произвольной топологии с делением и слиянием 
 /// требований, в которой все базовые системы бесконечноприборные
 /// </summary>
    public static class InfinityServerOpenForkJoinAnalizator
    {
        /// <summary>
        /// Получает интенсивности потоков для каждой из систем в сети 
        /// </summary>
        /// <param name="S">Массив номеров базовых систем </param>
        /// <param name="F">Массив номеров дивайдеров</param>
        /// <param name="J">Массив номеров интеграторов</param>
        /// <param name="Lambda0">ИНтенсивность входящего потока</param>
        /// <param name="Theta">Матрица передачи</param>
        /// <returns>Словарь, в котором ключ - вектор перемещений, а значение - вектор интенсвиностей для входящего потока 
        /// в базовые системы и дивайдеры (S_1, S_2, ...S_LS, F_1, F_2, ..., F_LF) </returns>
        public static Dictionary<string, double[]> InputRates(int[] S, int[] F, int[] J, double Lambda0, RoutingMatrix Theta)
        {
            //Словарь с элементами (вектор перемещений; вектор интенсивостей)
            Dictionary<string, double[]> rates = new Dictionary<string, double[]>();


            //Первый этап - Решение уравнения потока для требований
            //Размерность СЛАУ
            int n = F.Length + S.Length;
            int k = 0; //0-фрагменты 
            //Перенумерация интенсивностей происходит по порядку 
            //сначала для БС, а затем для дивайдеров:
            //lambda(S1) = lambda(1), ..., lambda(SL_S) =  lambda(L_S)
            //lambda (F1) = lambda(L_S), ... lambdA(FL_F) = lambda(L_F+L_S)
            Matrix A = new Matrix(n, n);
            double[] b = new double[n];

            //Нумерация базовых систем и интеграторов
            //базовых систем и дивайдеров
            int[] SF = new int[S.Length + F.Length];
            int[] SJ = new int[S.Length + F.Length];
            for (int i = 0; i < S.Length; i++)
            {
                SF[i] = S[i];
                SJ[i] = S[i];
            }
            for (int i = 0; i < F.Length; i++)
            {
                SF[i + S.Length] = F[i];
                SJ[i + S.Length] = J[i];
            }


            for (int j = 0; j < A.CountRow; j++)
            {
                for (int i = 0; i < A.CountColumn; i++)
                {
                    A[j, i] = Theta[k, SJ[i], SF[j]];
                    if (i == j)
                    {
                        A[i, j]--;
                    }

                }
                //Поток требований из источника
                b[j] = -Lambda0 * Theta[k, 0, SF[j]];
            }

            rates.Add("0", Computation.Gauss(A, b));
            HashSet<string> UnknownRates = new HashSet<string>();
            UnknownRates.Add("0");

            //Пока множество не пусто 
            while (UnknownRates.Count != 0)
            {
                string v = UnknownRates.ElementAt(0);
                UnknownRates.Remove(v);
                //Проверяю поток в дивайдер
                for (k = 0; k < F.Length; k++)
                {
                    //Если есть поток в k-ый дивайдер, то определяю итенсивности для 
                    //вектора перемещений (v,k)
                    int indexFk = Array.IndexOf(SF, F[k]);

                    if (rates[v][indexFk] > 0)
                    {
                        A.Initialize();
                        b.Initialize();

                        for (int j = 0; j < A.CountRow; j++)
                        {
                            for (int i = 0; i < SF.Length; i++)
                            {
                                A[j, i] = Theta[k + 1, SJ[i], SF[j]];
                                if (i == j)
                                {
                                    A[i, j]--;
                                }

                            }
                            //Поток требований в БС и дивайдер (в интегратор поток требований идти не может)
                            b[j] = -rates[v][indexFk] * Theta[k + 1, F[k], SF[j]];
                        }

                        string new_v = v + "," + (k + 1).ToString();
                        rates.Add(new_v, Computation.Gauss(A, b));
                        UnknownRates.Add(new_v);
                    }

                }
            }
            return rates;
        }

        /// <summary>
        /// Возвращает суммарную интенсивность для каждой из систем 
        /// </summary>
        /// <param name="rates">Словарь с интенсивностями</param>
        /// <returns></returns>
        public static double[] TotalInputRates(Dictionary<string, double[]> rates)
        {
            double[] Lambda = new double[rates.ElementAt(0).Value.Length];
            foreach (var item in rates)
            {
                for (int i = 0; i < Lambda.Length; i++)
                {
                    Lambda[i] += item.Value[i];
                }
            }
            return Lambda;

        }

        /// <summary>
        /// Проверяет на элементарность матрицу в матрице передачи 
        /// </summary>
        /// <param name="Theta">Матрица передачи</param>
        /// <param name="k">Номер матрицы, которую необходимо проверить</param>
        /// <param name="F">Массив номеров дивайдеров</param>
        /// <returns></returns>
        public static bool IsTrivialMatrix(RoutingMatrix Theta, int k, int[] F)
        {
            for (int i = 0; i < Theta.Dimention; i++)
            {
                for (int j = 0; j < F.Length; j++)
                {
                    if (Theta[k][i, F[j]] > 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }





        /// <summary>
        /// 
        /// </summary>
        /// <param name="S"></param>
        /// <param name="F"></param>
        /// <param name="J"></param>
        /// <param name="Theta"></param>
        /// <param name="mu"></param>
        /// <param name="TrivialIndex">Индекс элеметарной подсети (k>0) </param>
        /// <returns></returns>
        public static PhaseTypeVarible ResponseTimeDistributionForTrivialSubNetwork(int[] S, int[] F, int[] J, RoutingMatrix Theta, double[] mu, int TrivialIndex)
        {

#if (DEBUG)
            Console.WriteLine("Получение длительности реациии для элементарной подсети соглано теореме");
#endif
            //Счет всех собственных номеров идет с нуля!
            //Множество собственных номеров базовых систем, в которые переходят фрагменты непосредственно после деления  
            // в дивайдере F_k
            List<int> R = new List<int>();
            int k = TrivialIndex;
            for (int j = 0; j < S.Length; j++)
            {
                if (Theta[k][F[k - 1], S[j]] > 0)
                {
                    R.Add(j);
                }
            }
            int cR = R.Count;

            List<int>[] B = new List<int>[cR];
            for (int i = 0; i < B.Length; i++)
            {
                //B[i]  - последовательность, состоящая из собственных номеров базовых систем в которые 
                //возможно поступление k-фрагмента из базовой системы S_R[i], i = 0, ..., cR-1
                B[i] = new List<int>();
                //Начинаем обход в ширину из вершины S_R[i] 
                Queue<int> queue = new Queue<int>();
                queue.Enqueue(R[i]); //Первый элемент - сосбственный номер базовой системы S_R[i]
                while (queue.Count != 0)
                {
                    //Достать из очереди собственный номер базовой системы
                    int j = queue.Dequeue();
                    //Положить в список просмотренных вершин 
                    B[i].Add(j);
                    //Для всех смежных вершин выполнить 
                    for (int l = 0; l < S.Length; l++)
                    {
                        //Если возможен переход
                        //вершина не посещена
                        //и не нарпавлена в очередь для посещения 
                        if ((Theta[k][S[j], S[l]] > 0) &&
                            (queue.Contains(l) == false) &&
                            (B[i].Contains(l) == false))
                        {
                            //Запланировал посетить эту вершину
                            queue.Enqueue(l);
                        }
                    }

                }
                //Упорядочиваем, но на первом месте должен стоять элемент R[i]
                B[i].Sort();
                B[i][B[i].IndexOf(R[i])] = B[i][0];
                B[i][0] = R[i];
            }

            List<PhaseTypeVarible> ph = new List<PhaseTypeVarible>();
            //Вычисления по формуле 
            for (int i = 0; i < R.Count; i++)
            {
                int j = R[i];
                //Число систем достижимых из Sj (включая Sj)
                var b = B[i];

                int cb = b.Count;
                //Начальное распределение 
                double[] alpha = new double[cb];
                alpha[0] = 1;

                //Генератор 
                Matrix A = new Matrix(cb, cb);
                for (int n = 0; n < cb; n++)
                {
                    for (int m = 0; m < cb; m++)
                    {
                        //n* m* номера базовых систем во множестве всех систем
                        int n_ = b[n] + 1;
                        int m_ = b[m] + 1;

                        A[n, m] = mu[b[n]] * Theta[k][n_, m_];
                        if (n == m)
                        {
                            A[n, m] = -mu[b[n]];
                        }
                    }
                }

                //Получили генератор и начальное распределение для одной из начальных систем
                for (int l = 1; l <= Theta[k][F[k - 1], S[j]]; l++)
                {
                    ph.Add(new PhaseTypeVarible(A, alpha));
#if (DEBUG)
                    Console.WriteLine("Вывод параметров фазового распределения");
                    Console.WriteLine(new PhaseTypeVarible(A, alpha));
#endif
                }
            }

#if (DEBUG)
            Console.WriteLine("Распределение для всей подсети");
            Console.WriteLine(PHOperations.Max(ph));
            Console.ReadKey();
#endif
            return PHOperations.Max(ph);
        }

        /// <summary>
        /// Находит элементраную подсеть и возвращает номер соотвествующей элементарной матрицы 
        /// </summary>
        /// <param name="F">Множество номеров дивайдеров</param>
        /// <param name="Theta">Матрица передачи</param>
        /// <returns></returns>
        public static int FindTrivialNetwork(int[] F, RoutingMatrix Theta)
        {
            //Найти элементарную подсеть 
            int trivial_ind = 0;
            bool success = false;
            for (; trivial_ind < Theta.CountForker; trivial_ind++)
            {
                if (IsTrivialMatrix(Theta, trivial_ind, F))
                {
                    success = true;
                    break;
                }
            }
            if (success == false)
            {
                return -1;
            }
            return trivial_ind;
        }

        /// <summary>
        /// Выполние редукции относительно элементарной подсети k
        /// </summary>
        /// <param name="Network"></param>
        /// <param name="k">Номер элементарной подсети</param>
        /// <returns></returns>
        public static DescriptionOFJQN ReduceTrivialSubNetwork(DescriptionOFJQN Description, int k)
        {
            //Генерация фазового распределения для элементраной сети
#if (DEBUG)
            Console.WriteLine("Выполнение редукции");
#endif
            PhaseTypeVarible TrivialPH = ResponseTimeDistributionForTrivialSubNetwork(Description.S, Description.F, Description.J, Description.Theta, Description.mu, k);

            int Y = TrivialPH.NumberOfPhases;
            var A = TrivialPH.SubGenerator;
            var alpha = TrivialPH.InitialDistribution;
            //Число всех систем в сети
            int L = Description.Theta.Dimention - 1;

            //2) Множества номеров базовых систем, дивайдеров, интеграторов для новой сети
            List<int> S = new List<int>();
            //Вектор интенсивностей осблуживания для новой сети
            List<double> mu = new List<Double>();
            for (int i = 0; i < Description.S.Length; i++)
            {
                mu.Add(Description.mu[i]);
                S.Add(Description.S[i]);
            }
            for (int i = 0; i < Y; i++)
            {
                mu.Add(-A[i, i]);
                S.Add(L + i + 1);
            }
            //Дивайдеры и интеграторы для новой сети 
            List<int> F = new List<int>();
            List<int> J = new List<int>();
            for (int i = 0; i < Description.F.Length; i++)
            {
                F.Add(Description.F[i]);
                J.Add(Description.J[i]);
            }

            //3) Создаем новую матрицу передачи
            RoutingMatrix ReducedTheta = new RoutingMatrix(Y + Description.Theta.Dimention, Description.Theta.CountForker);
            //Копирование исходной матрицы в матрицу для новой сети
            for (int l = 0; l < Description.Theta.CountForker; l++)
            {
                for (int i = 0; i < Description.Theta.Dimention; i++)
                {
                    for (int j = 0; j < Description.Theta.Dimention; j++)
                    {
                        ReducedTheta[l][i, j] = Description.Theta[l][i, j];
                    }
                }
            }

            //Переходы между новыми базовыми системами
            for (int l = 0; l < ReducedTheta.CountForker; l++)
            {
                //Возможность перехода для l-фрагмента в элементарную подсеть H_k (т.е. в дивайдер F_k)
                bool transition = false;
                for (int i = 0; i < Description.Theta.Dimention; i++)
                {
                    if (Description.Theta[l][i, Description.F[k - 1]] > 0)
                    {
                        transition = true;
                        break;
                    }
                }
                if (transition == false)
                {
                    continue;
                }
                for (int i = 1; i <= Y; i++)
                {
                    for (int j = 1; j <= Y; j++)
                    {
                        if (i != j)
                        {
                            ReducedTheta[l][i + L, j + L] = -A[i - 1, j - 1] / A[i - 1, i - 1];
                        }
                    }
                }
            }

            //вероятности перехода в подсеть и выхода из подсети
            for (int l = 0; l < ReducedTheta.CountForker; l++)
            {
                for (int i = 0; i <= L; i++)
                {
                    for (int j = 1; j <= Y; j++)
                    {
                        ReducedTheta[l][i, L + j] = alpha[j - 1] * Description.Theta[l][i, Description.F[k - 1]];
                    }
                }
            }

            for (int l = 0; l < ReducedTheta.CountForker; l++)
            {
                for (int i = 1; i <= Y; i++)
                {
                    for (int j = 0; j <= L; j++)
                    {
                        double a_star = -A.Row(i - 1).Sum();
                        ReducedTheta[l][L + i, j] = -a_star / A[i - 1, i - 1] * Description.Theta[l][Description.J[k - 1], j];
                    }
                }
            }

            //Удаление строк и столбцов 
            ReducedTheta.DeleteMatrix(k, F[k - 1], J[k - 1]);

            int xF = F[k - 1];
            F.RemoveAt(k - 1); //Удаление дивайдера
            Reorder(F, xF);
            Reorder(S, xF);
            Reorder(J, xF);

            int xJ = J[k - 1];
            J.RemoveAt(k - 1);//Удаление интегратора
            Reorder(F, xJ);
            Reorder(S, xJ);
            Reorder(J, xJ);

            //Здесь нужно убрать все недостижимые базовые системы



            return new DescriptionOFJQN(S.ToArray(), F.ToArray(), J.ToArray(), mu.ToArray(), null,
                ReducedTheta, Description.Lambda0);
        }

        private static void Reorder(List<int> list, int y)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] > y)
                {
                    list[i]--;
                }
            }
        }



        public static PhaseTypeVarible ResponseTimeForBaseNetwork(DescriptionOFJQN Description)
        {
            int L = Description.Theta.Dimention - 1;
            PhaseTypeVarible ph = new PhaseTypeVarible(new Matrix(L, L), new double[L]);
            for (int i = 0; i < L; i++)
            {
                ph.InitialDistribution[i] = Description.Theta[0][0, i + 1];
            }

            for (int i = 0; i < L; i++)
            {
                for (int j = 0; j < L; j++)
                {
                    ph.SubGenerator[i, j] = Description.mu[i] * Description.Theta[0][i + 1, j + 1];
                    if (i == j)
                    {
                        ph.SubGenerator[i, j] = -Description.mu[i];
                    }
                }
            }


            return ph;

        }


        public static PhaseTypeVarible ResponseTimeDistribution(DescriptionOFJQN Description)
        {
            PhaseTypeVarible ph = new PhaseTypeVarible();

            //Находим первую элементарную подсеть 
            int k = FindTrivialNetwork(Description.F, Description.Theta);
#if(DEBUG)
            Console.WriteLine("Найдена подсеть H{0} в качестве элемнтарной", k);
#endif

            DescriptionOFJQN ReducedNetwork = Description;

            //Пока вся сеть не стала элементарной 
            while (k != 0)
            {
                //Выполняем редукцию 
                ReducedNetwork = ReduceTrivialSubNetwork(ReducedNetwork, k);

                //Поиск новой элементарной подсети
                k = FindTrivialNetwork(ReducedNetwork.F, ReducedNetwork.Theta);
            }

            ph = ResponseTimeForBaseNetwork(ReducedNetwork);
#if debug
            Console.WriteLine("Порядок матрицы {0}", ph.NumberOfPhases);
            Console.WriteLine("Число элементов в матрице {0}", ph.NumberOfPhases * ph.NumberOfPhases);
            Console.WriteLine("Число ненулевых  элементов в матрице {0} ({1:f4})", ph.Generator.NonZeroElements(),
                (double)ph.NumberOfPhases * ph.NumberOfPhases / ph.Generator.NonZeroElements());
#endif
            return ph;
        }


    }
}
