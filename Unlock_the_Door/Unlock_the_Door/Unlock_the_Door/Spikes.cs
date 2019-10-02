using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unlock_the_Door
{
    class Spikes
    {
        private const int cstDelay = 50;
        private const int cstPushDist = 15;
        private int dammage = 1;
        private bool hurt = true;
        private int delay = 0;
        public int Dammage { get => dammage; }
        public bool Hurt { get => hurt; set => hurt = value; }

        public void Reload()
        {
            if (this.delay <= 0)
            {
                this.hurt = true;
            }
            else
            {
                this.delay -= 1;
            }
        }
        public int DealDammage()
        {
            if (this.hurt)
            {
                this.hurt = false;
                this.delay = cstDelay;
                return dammage;
            }
            else
            {
                return 0;
            }
        }

        public int PushPlayer()
        {
            if( this.hurt)
            {
                return cstPushDist;
            }
            else
            {
                return 0;
            }
        }
    }
}
