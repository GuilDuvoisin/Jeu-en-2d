using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unlock_the_Door
{
    public class Slims
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
        //Methodes
        public int MoveLeft()
        {
            if (animationDelay <= 0) animationDelay = cstAnimationDelay;
            else animationDelay -= 1;
            if (range <= 0) {this.goRight = true; this.range = this.permRange; }
            else this.range -= 1;
            return -this.speed;
        }
        public int MoveRight()
        {
            if (animationDelay <= 0) animationDelay = cstAnimationDelay;
            else animationDelay -= 1;
            if (range <= 0) { this.goRight = false; this.range = this.permRange; }
            else this.range -= 1;
            return this.speed;
        }
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
