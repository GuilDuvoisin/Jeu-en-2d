/**
 * \file      frmGame.cs
 * \author    G. Mbayo
 * \version   1.0
 * \date      Octobre 31. 2019
 * \brief     Class of keys
 *
 * \details   This class contains the property and constructor of the class of the keys of the game
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Unlock_the_Door
{
    public class LevelsKeys: PictureBox
    {
        private int id;
        private bool taken = false;

        /// <summary>
        /// Constructor of the class LevelsKeys
        /// </summary>
        /// <param name="id">id of the keys</param>
        /// <param name="taken">is the key taken by player or not?</param>
        public LevelsKeys(int id, bool taken)
        {
            this.id = id;
            this.taken = taken;
        }

        public bool Taken { get => taken; set => taken = value; }
    }
}
