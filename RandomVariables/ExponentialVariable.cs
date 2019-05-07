using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariables
{
    public class ExponentialVariable : RandomVariable
    {
        //Параметр для экспоненциального распределения случайной велечины
        public double Rate { get; set; }

        //Следующее значение случайной величины
        public override double NextValue()
        {
            return -1.0 / Rate * Math.Log(Random.NextDouble());
        }

        //Генератор экспоненциально распределенной случайной велечины
        public ExponentialVariable(Random random, double Rate)
        {
            Random = random;
            this.Rate = Rate;
        }
    }
}
