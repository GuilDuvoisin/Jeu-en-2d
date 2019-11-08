/**
 * \file      frmGame.cs
 * \author    G. Mbayo
 * \version   1.0
 * \date      Octobre 31. 2019
 * \brief     Class of players
 *
 * \details   This class enable the interactions of the player (movements, jump, fall, ammount of lifes)
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Unlock_the_Door
{
    public class Players: PictureBox
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
        private int fireDelay = 0;
        
        /// <summary>
        /// Constructor of the class Players
        /// </summary>
        /// <param name="lifes">amount of lifes that the player has</param>
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
        public int FireDelay { get => fireDelay; set => fireDelay = value; }


        //Methodes
        /// <summary>
        /// The player go to the left
        /// </summary>
        /// <returns></returns>
        public int MoveLeft()
        {
            return -this.speed;
        }

        /// <summary>
        /// The player go to the right
        /// </summary>
        /// <returns></returns>
        public int MoveRight()
        {
            return this.speed;
        }

        /// <summary>
        /// Describe the comportement of the player when he jump
        /// </summary>
        /// <param name="isGrounded">Is the player touching the ground?</param>
        public void MoveUp(bool isGrounded)
        {
            if (this.jump)
            {
                this.jumpSpeed = -this.inertia;
                if (this.inertia > -this.speed * 4)
                {
                    this.inertia -= 1;
                }
            }
            else
            {
                if (!isGrounded)
                {
                    if (this.inertia >= 0)
                    {
                        this.inertia = -1;
                    }
                    this.jumpSpeed = -this.inertia;
                    if (this.inertia > -this.speed * 4)
                    {
                        this.inertia -= 1;
                    }
                }
                else this.jumpSpeed = 0;
            }
        }

        /// <summary>
        /// refill the inertia of the player (effect of acceleration when it go up and down) 
        /// </summary>
        public void RefillInertia()
        {
            this.inertia = cstInertia;
        }
    }
}

