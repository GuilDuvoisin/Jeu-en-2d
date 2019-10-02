using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unlock_the_Door
{
    public class Players
    {
        //Attributs
        private const int cstInertia = 16;
        private int speed = 10;
        private int jumpSpeed;
        private int inertia;
        private bool goLeft;
        private bool goRight;
        private bool jump;
        private bool collisionRight;
        private bool collisionLeft;
        private bool collisionBottom;
        private bool action = false;
        private int lifes;
        private bool dammaged = false;
        private bool dead = false;

        //Constructor
        public Players(int lifes)
        {
            this.Lifes = lifes;
        }

        //Properties
        public int Speed { get => speed; set => speed = value; }
        public int Inertia { get => inertia; set => inertia = value; }
        public bool GoLeft { get => goLeft; set => goLeft = value; }
        public bool GoRight { get => goRight; set => goRight = value; }
        public bool Jump { get => jump; set => jump = value; }
        public int JumpSpeed { get => jumpSpeed; set => jumpSpeed = value; }
        public bool CollisionRight { get => collisionRight; set => collisionRight = value; }
        public bool CollisionLeft { get => collisionLeft; set => collisionLeft = value; }
        public bool CollisionBottom { get => collisionBottom; set => collisionBottom = value; }
        public bool Action { get => action; set => action = value; }
        public int Lifes { get => lifes; set => lifes = value; }
        public bool Dammaged { get => dammaged; set => dammaged = value; }
        public bool Dead { get => dead; set => dead = value; }


        //Methodes
        public int MoveLeft()
        {
            return -this.speed;
        }
        public int MoveRight()
        {
            return this.speed;
        }
        public void MoveUp(bool isGrounded)
        {
            if (this.jump)
            {
                this.jumpSpeed = -this.inertia;
                if (this.inertia > -this.speed*4) this.inertia -= 1;

            }
            else
            {
                if (!isGrounded)
                {
                    if (this.inertia >= 0) this.inertia = -1;
                    this.jumpSpeed = -this.inertia;
                    if (this.inertia > -this.speed*4) this.inertia -= 1;
                }
                else this.jumpSpeed = 0;
            }
        }
        public void RefillInertia()
        {
            this.inertia = cstInertia;
        }
    }
}

