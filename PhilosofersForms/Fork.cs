using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhilosofersForms
{
    internal class Fork
    {
        public static int _count = 1;
        public int Id { get; private set; }

        public Fork()
        {
            this.Id = _count++;
        }
    }
}
