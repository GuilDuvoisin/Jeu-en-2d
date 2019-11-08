namespace Unlock_the_Door
{
    partial class frmMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMenu));
            this.cmdJouer = new System.Windows.Forms.Button();
            this.cmdRegles = new System.Windows.Forms.Button();
            this.cmdSelection = new System.Windows.Forms.Button();
            this.cmdQuitter = new System.Windows.Forms.Button();
            this.lblAide = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmdJouer
            // 
            this.cmdJouer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmdJouer.Location = new System.Drawing.Point(456, 118);
            this.cmdJouer.Name = "cmdJouer";
            this.cmdJouer.Size = new System.Drawing.Size(229, 76);
            this.cmdJouer.TabIndex = 0;
            this.cmdJouer.Text = "Jouer";
            this.cmdJouer.UseVisualStyleBackColor = true;
            this.cmdJouer.Click += new System.EventHandler(this.cmdJouer_Click);
            // 
            // cmdRegles
            // 
            this.cmdRegles.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmdRegles.Enabled = false;
            this.cmdRegles.Location = new System.Drawing.Point(456, 220);
            this.cmdRegles.Name = "cmdRegles";
            this.cmdRegles.Size = new System.Drawing.Size(229, 76);
            this.cmdRegles.TabIndex = 1;
            this.cmdRegles.Text = "Comment jouer";
            this.cmdRegles.UseVisualStyleBackColor = true;
            this.cmdRegles.Click += new System.EventHandler(this.cmdRegles_Click);
            // 
            // cmdSelection
            // 
            this.cmdSelection.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmdSelection.Enabled = false;
            this.cmdSelection.Location = new System.Drawing.Point(456, 311);
            this.cmdSelection.Name = "cmdSelection";
            this.cmdSelection.Size = new System.Drawing.Size(229, 76);
            this.cmdSelection.TabIndex = 2;
            this.cmdSelection.Text = "Séléction du niveau";
            this.cmdSelection.UseVisualStyleBackColor = true;
            this.cmdSelection.Click += new System.EventHandler(this.cmdSelection_Click);
            // 
            // cmdQuitter
            // 
            this.cmdQuitter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmdQuitter.Location = new System.Drawing.Point(456, 409);
            this.cmdQuitter.Name = "cmdQuitter";
            this.cmdQuitter.Size = new System.Drawing.Size(229, 76);
            this.cmdQuitter.TabIndex = 3;
            this.cmdQuitter.Text = "Quitter";
            this.cmdQuitter.UseVisualStyleBackColor = true;
            this.cmdQuitter.Click += new System.EventHandler(this.cmdQuitter_Click);
            // 
            // lblAide
            // 
            this.lblAide.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblAide.AutoSize = true;
            this.lblAide.Enabled = false;
            this.lblAide.Location = new System.Drawing.Point(251, 122);
            this.lblAide.Name = "lblAide";
            this.lblAide.Size = new System.Drawing.Size(0, 13);
            this.lblAide.TabIndex = 4;
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1053, 545);
            this.ControlBox = false;
            this.Controls.Add(this.lblAide);
            this.Controls.Add(this.cmdQuitter);
            this.Controls.Add(this.cmdSelection);
            this.Controls.Add(this.cmdRegles);
            this.Controls.Add(this.cmdJouer);
            this.Name = "Menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Tag = "Menu";
            this.Text = "Menu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdJouer;
        private System.Windows.Forms.Button cmdRegles;
        private System.Windows.Forms.Button cmdSelection;
        private System.Windows.Forms.Button cmdQuitter;
        private System.Windows.Forms.Label lblAide;
    }
}