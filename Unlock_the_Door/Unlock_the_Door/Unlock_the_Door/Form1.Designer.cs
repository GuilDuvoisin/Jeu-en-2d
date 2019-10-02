namespace Unlock_the_Door
{
    partial class frmMainWindow
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tmrMain = new System.Windows.Forms.Timer(this.components);
            this.tmrAction = new System.Windows.Forms.Timer(this.components);
            this.tmrAnimations = new System.Windows.Forms.Timer(this.components);
            this.tmrDelays = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tmrMain
            // 
            this.tmrMain.Interval = 20;
            this.tmrMain.Tag = "MainTimer";
            this.tmrMain.Tick += new System.EventHandler(this.tmrMain_Tick);
            // 
            // tmrAction
            // 
            this.tmrAction.Interval = 20;
            this.tmrAction.Tag = "ActionsTimer";
            this.tmrAction.Tick += new System.EventHandler(this.tmrAction_Tick);
            // 
            // tmrAnimations
            // 
            this.tmrAnimations.Interval = 20;
            this.tmrAnimations.Tag = "AnimationsTimer";
            this.tmrAnimations.Tick += new System.EventHandler(this.tmrAnimations_Tick);
            // 
            // tmrDelays
            // 
            this.tmrDelays.Interval = 20;
            this.tmrDelays.Tag = "DelaysTimer";
            this.tmrDelays.Tick += new System.EventHandler(this.tmrDelays_Tick);
            // 
            // frmMainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "frmMainWindow";
            this.Tag = "MainWindow";
            this.Text = "Unlock The Door!";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMainWindow_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmMainWindow_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrMain;
        private System.Windows.Forms.Timer tmrAction;
        private System.Windows.Forms.Timer tmrAnimations;
        private System.Windows.Forms.Timer tmrDelays;
    }
}

