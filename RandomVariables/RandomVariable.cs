using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomVariables
{
    public abstract class RandomVariable
    {
        protected Random Random { get; set; }

        public abstract double NextValue();
    }
}
