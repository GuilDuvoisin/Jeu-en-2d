/**
 * \file      frmGame.cs
 * \author    G. Mbayo
 * \version   1.0
 * \date      Octobre 31. 2019
 * \brief     Form of menu.
 *
 * \details   This form contains the interaction and design of the menu
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Unlock_the_Door
{
    public partial class frmMenu : Form
    {
        public int level = 0;

        /// <summary>
        /// Initialisation
        /// </summary>
        public frmMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Launch the level 1 of the game and close the menu form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdJouer_Click(object sender, EventArgs e)
        {
            level = 1;
            this.Close();
        }

        /// <summary>
        /// Display the rules of the game and hide the buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdRegles_Click(object sender, EventArgs e)
        {
            foreach(Control button in this.Controls)
            {
                if(button.GetType()== typeof(Button))
                {
                    button.Enabled = false;
                    button.Visible = false;
                }
            }
            lblAide.Enabled = true;
            lblAide.Text = "Le but du jeu est de récupérer les clefs qui permetteront d'ouvrir la porte pour passer au niveau suppérieur";
            lblAide.Text += Environment.NewLine;
            lblAide.Text += "tout en évitant les enemis soit en sautant par dessus soit en les tuant avec les armes .";
            lblAide.Text += Environment.NewLine;
            lblAide.Text += "A chaque niveaux passés vous obtenez un code qui vous permet de sauter les niveau si vous recommencer le jeu";

        }

        /// <summary>
        /// Display the levels list and launch the selectionned level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSelection_Click(object sender, EventArgs e)
        {
            foreach (Button button in this.Controls)
            {
                button.Enabled = false;
                button.Visible = false;
            }

        }

        /// <summary>
        /// Close the menu form and the main app form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdQuitter_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
