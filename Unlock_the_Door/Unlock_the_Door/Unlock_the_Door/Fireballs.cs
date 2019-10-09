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
        private bool goRight;
        private int dammages = 1;
        private int speed = 15;
        private int rotation = 1;
        private int ttl = 50;
        private int timerSmoke = 5;
        private const int cstInertia = 20;
        private int inertia;
        private int jumpSpeed;
        private bool collisionBottom;
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
        public void RefillInertia()
        {
            this.Inertia = cstInertia;
        }
    }
}
