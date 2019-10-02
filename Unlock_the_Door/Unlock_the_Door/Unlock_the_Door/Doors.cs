using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unlock_the_Door
{
    public class Doors
    {
        private int id;
        private bool open;
        private DoorsOutputs output;

        public Doors(int id, bool open, DoorsOutputs output)
        {
            this.id = id;
            this.open = open;
            this.output = output;
        }

        public int Id { get => id; set => id = value; }
        public bool Open { get => open; set => open = value; }
        public DoorsOutputs Output { get => output; set => output = value; }
    }
}
