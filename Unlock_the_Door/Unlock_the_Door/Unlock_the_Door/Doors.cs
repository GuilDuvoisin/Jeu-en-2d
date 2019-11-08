/**
 * \file      frmGame.cs
 * \author    G. Mbayo
 * \version   1.0
 * \date      Octobre 31. 2019
 * \brief     Class of doors
 *
 * \details   This class contains the property and the constructor of class Doors
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Unlock_the_Door
{
    public class Doors: PictureBox
    {
        private int id;
        private bool open;
        private DoorsOutputs output;

        /// <summary>
        /// Constructor of class Doors
        /// </summary>
        /// <param name="id">id of the object</param>
        /// <param name="open">The door is open or not (bool)</param>
        /// <param name="output">The level or place where we go with that door</param>
        public Doors(int id, bool open, DoorsOutputs output)
        {
            this.id = id;
            this.open = open;
            this.output = output;
        }

        //Properties
        public int Id { get => id; set => id = value; }
        public bool Open { get => open; set => open = value; }
        public DoorsOutputs Output { get => output; set => output = value; }
    }
}
