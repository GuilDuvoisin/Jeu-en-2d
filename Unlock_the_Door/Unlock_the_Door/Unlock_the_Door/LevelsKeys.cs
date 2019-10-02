using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unlock_the_Door
{
    public class LevelsKeys
    {
        private int id;
        private bool taken = false;

        public LevelsKeys(int id, bool taken)
        {
            this.id = id;
            this.taken = taken;
        }

        public bool Taken { get => taken; set => taken = value; }
    }
}
