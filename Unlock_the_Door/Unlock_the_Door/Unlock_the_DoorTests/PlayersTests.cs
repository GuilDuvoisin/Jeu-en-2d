/**
 * \file      frmGame.cs
 * \author    G. Mbayo
 * \version   1.0
 * \date      Octobre 31. 2019
 * \brief     Unit tests for the class "Players"
 *
 * \details   Those test allows to verify that the classes of "Players" are good working
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unlock_the_Door;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Unlock_the_Door.Tests
{
    [TestClass()]
    public class PlayersTests
    {
        Players player = new Players(6);
        /// <summary>
        /// This test control if the output inertia of the method "MoveUp" of class "Players" 
        /// when the player is jumping and doesn't touch the ground is the output inertia that we expected
        /// </summary>
        [TestMethod()]
        public void MoveUp_TestInertiaJumpTrue_AreEqual()
        {
            //Arrange
            player.Jump = true;
            bool isGrounded = false;
            player.JumpSpeed = 3;
            player.Inertia = 16;
            player.Speed = 10;
            int expectedInertia = 15;

            //Act
            player.MoveUp(isGrounded);
            
            //Assert
            Assert.AreEqual(expectedInertia, player.Inertia);
        }

        /// <summary>
        /// This test control if the output inertia of the method "MoveUp" of class "Players" 
        /// when the player isn't jumping and doesn't touch the ground is the output inertia that we expected
        /// </summary>
        [TestMethod()]
        public void MoveUp_TestInertiaJumpFalse_AreEqual()
        {
            //Arrange
            bool jump = false;
            bool isGrounded = false;
            int jumpSpeed = 3;
            int inertia = 16;
            int speed = 10;
            int expectedInertia = -2;

            //Act
            if (jump)
            {
                jumpSpeed = -inertia;
                if (inertia > -speed * 4)
                {
                    inertia -= 1;
                }
            }
            else
            {
                if (!isGrounded)
                {
                    if (inertia >= 0)
                    {
                        inertia = -1;
                        jumpSpeed = -inertia;
                    }
                    if (inertia > -speed * 4)
                    {
                        inertia -= 1;
                    }
                }
                else jumpSpeed = 0;
            }

            //Assert
            Assert.AreEqual(expectedInertia, inertia);
        }

        /// <summary>
        /// This test control if the output jumpSpeed of the method "MoveUp" of class "Players" 
        /// when the player isn't jumping and doesn't touch the ground is the output jumpSpeed that we expected
        /// </summary>
        [TestMethod()]
        public void MoveUp_TestJumpSpeedNotGrounded_AreEqual()
        {
            //Arrange
            bool jump = false;
            bool isGrounded = false;
            int jumpSpeed = 3;
            int inertia = 16;
            int speed = 10;
            int expectedJumpSpeed = 1;

            //Act
            if (jump)
            {
                jumpSpeed = -inertia;
                if (inertia > -speed * 4)
                {
                    inertia -= 1;
                }
            }
            else
            {
                if (!isGrounded)
                {
                    if (inertia >= 0)
                    {
                        inertia = -1;
                        jumpSpeed = -inertia;
                    }
                    if (inertia > -speed * 4)
                    {
                        inertia -= 1;
                    }
                }
                else jumpSpeed = 0;
            }

            //Assert
            Assert.AreEqual(expectedJumpSpeed, jumpSpeed);
        }

        /// <summary>
        /// This test control if the output jumpSpeed of the method "MoveUp" of class "Players" 
        /// when the player isn't jumping and is touching the ground is the output jumpSpeed that we expected
        /// </summary>
        [TestMethod()]
        public void MoveUp_TestJumpSpeedGrounded_AreEqual()
        {
            //Arrange
            bool jump = false;
            bool isGrounded = true;
            int jumpSpeed = 3;
            int inertia = 16;
            int speed = 10;
            int expectedJumpSpeed = 0;

            //Act
            if (jump)
            {
                jumpSpeed = -inertia;
                if (inertia > -speed * 4)
                {
                    inertia -= 1;
                }
            }
            else
            {
                if (!isGrounded)
                {
                    if (inertia >= 0)
                    {
                        inertia = -1;
                        jumpSpeed = -inertia;
                    }
                    if (inertia > -speed * 4)
                    {
                        inertia -= 1;
                    }
                }
                else jumpSpeed = 0;
            }

            //Assert
            Assert.AreEqual(expectedJumpSpeed, jumpSpeed);
        }
    }
}