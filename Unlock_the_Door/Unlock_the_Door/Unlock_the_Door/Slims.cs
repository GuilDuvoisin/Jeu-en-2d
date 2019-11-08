/**
 * \file      frmGame.cs
 * \author    G. Mbayo
 * \version   1.0
 * \date      Octobre 31. 2019
 * \brief     Class of ennemies of type "slims"
 *
 * \details   This class enable the interaction of the slims of the game (movement, dammages, death)
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Unlock_the_Door
{
    public class Slims: PictureBox
    {
        //Attributs
        private const int cstDelay = 50;
        private const int cstAnimationDelay = 20;
        private int permRange;
        private int lifes = 3;
        private int speed = 5;
        private bool goRight = true;
        private int dammages = 2;
        private bool hurt = true;
        private int delay = 0;
        private int animationDelay = cstAnimationDelay;
        private int animation = 0;
        private int range;
        private bool dead = false;

        //Constructor
        public Slims(int range)
        {
            this.permRange = range;
        }
        //Properties
        public int Dammages { get => dammages; }
        public int Lifes { get => lifes; set => lifes = value; }
        public int Speed { get => speed; }
        public bool GoRight { get => goRight; set => goRight = value; }
        public bool Hurt { get => hurt; set => hurt = value; }
        public int AnimationDelay { get => animationDelay; }
        public int Animation { get => animation; set => animation = value; }
        public bool Dead { get => dead; set => dead = value; }

        //Methodes
        /// <summary>
        /// The player go to the left
        /// </summary>
        public void MoveLeft()
        {
            if (animationDelay <= 0) animationDelay = cstAnimationDelay;
            else animationDelay -= 1;
            if (range <= 0) {this.goRight = true; this.range = this.permRange; }
            else this.range -= 1;
            if (!this.dead)
            {
                this.Left -= this.speed;
            }
            
        }

        /// <summary>
        /// The player go to the right
        /// </summary>
        public void MoveRight()
        {
            if (animationDelay <= 0) animationDelay = cstAnimationDelay;
            else animationDelay -= 1;
            if (range <= 0) { this.goRight = false; this.range = this.permRange; }
            else this.range -= 1;
            if (!this.dead)
            {
                this.Left += this.speed;
            }
        }

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
            if (this.Hurt)
            {
                this.Hurt = false;
                this.delay = cstDelay;
                return this.dammages;
            }
            else
            {
                return 0;
            }
        }
    }
}
