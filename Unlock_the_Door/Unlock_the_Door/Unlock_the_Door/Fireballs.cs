/**
 * \file      frmGame.cs
 * \author    G. Mbayo
 * \version   1.0
 * \date      Octobre 31. 2019
 * \brief     Class of fireballs
 *
 * \details   This class contains the property, constructor and methods that allows movements and animations of the fireballs
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Unlock_the_Door
{
    public class Fireballs: PictureBox
    {
        private const int cstInertia = 20;
        private bool goRight;
        private int dammages = 1;
        private int speed = 15;
        private int rotation = 1;
        private int ttl = 50;
        private int timerSmoke = 5;
        private int inertia;
        private int jumpSpeed;
        private bool collisionBottom;
        private bool hurt = true;

        /// <summary>
        /// Constructor of the class Fireballs
        /// </summary>
        /// <param name="goRight">Do th ball go right? (bool) if not, go left</param>
        public Fireballs(bool goRight)
        {
            this.goRight = goRight;
        }

        public int Rotation { get => rotation; set => rotation = value; }
        public bool GoRight { get => goRight; set => goRight = value; }
        public int Speed { get => speed; set => speed = value; }
        public int Dammages { get => dammages; }
        public int Ttl { get => ttl; set => ttl = value; }
        public int TimerSmoke { get => timerSmoke; set => timerSmoke = value; }
        public int Inertia { get => inertia; set => inertia = value; }
        public bool CollisionBottom { get => collisionBottom; set => collisionBottom = value; }
        public int JumpSpeed { get => jumpSpeed; set => jumpSpeed = value; }
        public bool Hurt { get => hurt; set => hurt = value; }

        /// <summary>
        /// Display the animation of the fireballs (spinning)
        /// </summary>
        public void Animation()
        {
            switch (this.Rotation)
            {
                case 1:
                    this.Rotation = 2;
                    break;
                case 2:
                    this.Rotation = 3;
                    break;
                case 3:
                    this.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    this.Rotation = 1;
                    break;
                default:
                    this.Rotation = 1;
                    break;
            }
            this.ttl -= 1;
        }

        /// <summary>
        /// Describe the movement of the fireballs (bouncing)
        /// </summary>
        /// <param name="isGrounded">Do the fireball touch the ground?</param>
        public void MoveUp(bool isGrounded)
        {
            if (isGrounded)
            {
                RefillInertia();
                this.jumpSpeed = -this.inertia;
                this.inertia -= 1;
            }
            else
            {
                this.JumpSpeed = -this.inertia;
                this.inertia -= 3;
            }
            this.Top += jumpSpeed;
        }

        /// <summary>
        /// refill the inertia of the fireball (effect of acceleration when it go up and down) 
        /// </summary>
        public void RefillInertia()
        {
            this.Inertia = cstInertia;
        }
    }
}
