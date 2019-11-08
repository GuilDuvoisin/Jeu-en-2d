/**
 * \file      frmGame.cs
 * \author    G. Mbayo
 * \version   1.0
 * \date      Octobre 31. 2019
 * \brief     Class of spikes
 *
 * \details   This class enable the interaction of the spikes (dammages and ability to push the player)
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Unlock_the_Door
{
    class Spikes: PictureBox
    {
        private const int cstDelay = 50;
        private const int cstPushDist = 15;
        private int dammage = 1;
        private bool hurt = true;
        private int delay = 0;
        public int Dammage { get => dammage; }
        public bool Hurt { get => hurt; set => hurt = value; }

        /// <summary>
        /// If the object havent caused dammage to player for enough time, hurt the player when it touch him
        /// else doesn't deal any dammages
        /// </summary>
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

        /// <summary>
        /// If the function Reload says that the object deal dammages, this function deal the dammages
        /// </summary>
        /// <returns>If Hurt is true, deal dammages. Else don't deal dammages</returns>
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

        /// <summary>
        /// If the object deal dammages, the player is pushed away on the closer side of the spikes.
        /// </summary>
        /// <returns>If Hurt is true, push the player. Else don't</returns>
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
