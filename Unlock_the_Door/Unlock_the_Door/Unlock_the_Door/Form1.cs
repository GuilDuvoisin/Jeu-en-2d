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
    public partial class frmMainWindow : Form
    {
        private int deathDelay = 50;
        //Déclaration du joueur
        Players player;
        //Déclaration des objets
            //Déclaration des clés
        LevelsKeys blueKey;
        LevelsKeys greenKey;
        LevelsKeys redKey;
        LevelsKeys yellowKey;
            //Déclaration des portes
        Doors endDoor;
            //Déclaration des piques
        Spikes spike;
            //Déclaration des slims
        Slims slim;
            //Déclaration des sprites
        PictureBox pbPlayer;
        public frmMainWindow()
        {
            InitializeComponent();
            Level2();
        }

        private void frmMainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    player.GoLeft = true;
                    break;
                case Keys.D:
                    player.GoRight = true;
                    break;
                case Keys.W:
                    player.Jump = true;
                    break;
                case Keys.E:
                    player.Action = true;
                    break;
                default:
                    break;
            }
        }

        private void frmMainWindow_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    player.GoLeft = false;
                    break;
                case Keys.D:
                    player.GoRight = false;
                    break;
                case Keys.E:
                    player.Action = false;
                    break;
                default:
                    break;
            }
            if (player.Jump) { player.Jump = false; }
        }
        //Idée: Créer un ou plusieurs autre timer pour séparer les actions qu'ils contiennent (lisibilité du code)
        //1 timer pour les annimations, 1 timer pour les actions, 1 timer pour les déplacements et collisions
        private void tmrMain_Tick(object sender, EventArgs e)
        {
            if (player.Jump && player.Inertia < 0) { player.Jump = false; }

            //Si le player se rapproche du bord de l'écran, la caméra se déplace avec lui (bas)
            if (!player.Jump && !player.CollisionBottom && pbPlayer.Bottom >= this.Bottom - this.Height / 10)
            {
                foreach (PictureBox rect in this.Controls)
                {
                    if (rect.Tag != "player" && rect.Tag != "lifes" && rect.Tag != "inventory")
                        rect.Top -= player.JumpSpeed;
                }
            }
            else { pbPlayer.Top += player.JumpSpeed; }
            //Si le player se rapproche du bord de l'écran, la caméra se déplace avec lui (droite)
            if (player.GoRight && !player.CollisionRight && pbPlayer.Left >= this.Right - this.Width/3.5)
            {
                foreach (PictureBox rect in this.Controls)
                {
                    if (rect.Tag != "player" && rect.Tag != "lifes" && rect.Tag != "inventory")
                        rect.Left -= player.MoveRight();
                }
            }
            else if (player.GoRight && !player.CollisionRight) { pbPlayer.Left += player.MoveRight(); }
            //Si le player se rapproche du bord de l'écran, la caméra se déplace avec lui (gauche)
            if (player.GoLeft && !player.CollisionLeft && pbPlayer.Left <= this.Width / 3.5)
            {
                foreach (PictureBox rect in this.Controls)
                {
                    if (rect.Tag != "player" && rect.Tag != "lifes" && rect.Tag != "inventory")
                        rect.Left -= player.MoveLeft();
                }
            }
            else if (player.GoLeft && !player.CollisionLeft) { pbPlayer.Left += player.MoveLeft(); }
            
            player.CollisionLeft = false;
            player.CollisionRight = false;
            player.CollisionBottom = false;
            foreach (PictureBox rect in this.Controls)
            {
                switch (rect.Tag)
                {
                    case "platform":
                        //collision en bas
                        if (pbPlayer.Bottom >= rect.Top && pbPlayer.Top < rect.Top && pbPlayer.Left < rect.Right && pbPlayer.Right > rect.Left)
                        {
                            player.RefillInertia();   //redonne la capacité de sauter

                            pbPlayer.Top = rect.Top - pbPlayer.Height;

                            //si le player touche le sol
                            player.CollisionBottom = true;
                        }
                        //collision a droite
                        if (pbPlayer.Right >= rect.Left && pbPlayer.Bottom > rect.Top && pbPlayer.Top < rect.Bottom && pbPlayer.Right < rect.Left + player.Speed)
                        {
                            player.GoRight = false;
                            player.CollisionRight = true;
                        }
                        //collision  gauche
                        if (pbPlayer.Left <= rect.Right && pbPlayer.Bottom > rect.Top && pbPlayer.Top < rect.Bottom && pbPlayer.Left > rect.Right - player.Speed)
                        {
                            player.GoLeft = false;
                            player.CollisionLeft = true;
                        }
                        break;
                    case "slims":
                        //déplacement des slims
                        if (slim.GoRight)
                        {
                            rect.Left += slim.MoveRight();
                        }
                        else
                        {
                            rect.Left += slim.MoveLeft();
                        }
                        break;
                    default:
                        break;
                }
                if (rect.Name == "pbPlateformBase" && pbPlayer.Top > rect.Bottom + 50) { player.Lifes = 0; } 
                if(player.Lifes <= 0)
                {
                    player.Dead = true;
                }
            }
            player.MoveUp(player.CollisionBottom);
        }

        private void tmrAction_Tick(object sender, EventArgs e)
        {
            foreach (PictureBox rect in this.Controls)
            {
                switch (rect.Tag)
                {
                    case "key":
                        if (pbPlayer.Bounds.IntersectsWith(rect.Bounds) && rect.Enabled != false)
                        {
                            switch (rect.Name)
                            {
                                case "pbBlueKey":
                                    blueKey.Taken = true;
                                    rect.Location = new Point(275, 30);
                                    rect.Tag = "inventory";

                                    break;
                                case "pbGreenKey":
                                    greenKey.Taken = true;
                                    rect.Location = new Point(345, 30);
                                    rect.Tag = "inventory";
                                    break;
                                case "pbRedKey":
                                    redKey.Taken = true;
                                    rect.Location = new Point(415, 30);
                                    rect.Tag = "inventory";
                                    break;
                                case "pbYellowKey":
                                    yellowKey.Taken = true;
                                    rect.Location = new Point(485, 30);
                                    rect.Tag = "inventory";
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case "door":
                        //remplir l'interaction avec le joueur en fonction des clés en sa possession
                        if (pbPlayer.Bounds.IntersectsWith(rect.Bounds) && player.Action)
                        {
                            switch (rect.Name)
                            {
                                case "pbEndDoor":
                                    //La clé bleue doit toujours être celle de la porte finale
                                    if (blueKey.Taken && !endDoor.Open)
                                    {
                                        endDoor.Open = true;
                                        rect.Image = Image.FromFile(Application.StartupPath + @"\res\door.open.png");
                                        player.Action = false;
                                    }
                                    else if (endDoor.Open)
                                    {
                                        ClearForm(this, typeof(PictureBox));
                                        //Switch qui permet de lancer le bon niveau en fonction de la porte prise
                                        switch (endDoor.Output)
                                        {
                                            case DoorsOutputs.Level1:
                                                Level1();
                                                break;
                                            case DoorsOutputs.Level2:
                                                Level2();
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case "spikes":
                        if (spike.Hurt)
                        {
                            if (pbPlayer.Right <= rect.Left + rect.Width/2 + pbPlayer.Width/2 && pbPlayer.Bounds.IntersectsWith(rect.Bounds))
                            {
                                player.Lifes -= spike.DealDammage();
                                player.RefillInertia();
                                player.Jump = true;
                                pbPlayer.Left -= spike.PushPlayer();
                            }
                            else if(pbPlayer.Bounds.IntersectsWith(rect.Bounds))
                            {
                                player.Lifes -= spike.DealDammage();
                                player.RefillInertia();
                                player.Jump = true;
                                pbPlayer.Left += spike.PushPlayer();
                            }
                        }
                        break;
                    case "slims":
                        if (pbPlayer.Bounds.IntersectsWith(rect.Bounds))
                        {
                            player.Lifes -= slim.DealDammage();
                            player.RefillInertia();
                            player.Jump = true;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void tmrDelays_Tick(object sender, EventArgs e)
        {
            //Spikes
            if (spike != null)
            {
                spike.Reload();
            }
            if (!spike.Hurt) { player.Dammaged = true; }
            else if (!slim.Hurt) { player.Dammaged = true; }
            else { player.Dammaged = false; }
            //Slim
            if (slim != null)
            {
                slim.Reload();
            }
            //Mort du player
            if (player.Dead)
            {
                if(deathDelay <= 0)
                {
                    switch (endDoor.Output)
                    {
                        case DoorsOutputs.Level1:
                            ClearForm(this, typeof(PictureBox));
                            Level2();
                            break;
                        case DoorsOutputs.Level2:
                            ClearForm(this, typeof(PictureBox));
                            Level1();
                            break;
                        default:
                            break;
                    }
                    deathDelay = 50;
                }
                else deathDelay -= 1;
                if(deathDelay == 48)
                {
                    tmrMain.Enabled = false;
                    tmrAction.Enabled = false;
                    tmrAnimations.Enabled = false;
                }
            }
        }

        private void tmrAnimations_Tick(object sender, EventArgs e)
        {
            //expressions facial du player
            if (player.Lifes <= 0)
            {
                pbPlayer.Image = Image.FromFile(Application.StartupPath + @"\res\blocker.dead.png");
            }
            else if (player.Dammaged) { pbPlayer.Image = Image.FromFile(Application.StartupPath + @"\res\blocker.sad.png"); }
            else { pbPlayer.Image = Image.FromFile(Application.StartupPath + @"\res\blocker.happy.png"); }

            //affichage des lifes
            foreach (PictureBox rect in this.Controls)
            { 
                if(rect.Tag == "lifes")
                {
                    switch (player.Lifes)
                    {
                        case 5:
                            if (rect.Name == "pbLifes3") rect.Image = Image.FromFile(Application.StartupPath + @"\res\heart.half.png");
                            break;
                        case 4:
                            if (rect.Name == "pbLifes3") rect.Image = Image.FromFile(Application.StartupPath + @"\res\heart.empty.png");
                            break;
                        case 3:
                            if (rect.Name == "pbLifes2") rect.Image = Image.FromFile(Application.StartupPath + @"\res\heart.half.png");
                            if (rect.Name == "pbLifes3") rect.Image = Image.FromFile(Application.StartupPath + @"\res\heart.empty.png");
                            break;
                        case 2:
                            if (rect.Name == "pbLifes2") rect.Image = Image.FromFile(Application.StartupPath + @"\res\heart.empty.png");
                            if (rect.Name == "pbLifes3") rect.Image = Image.FromFile(Application.StartupPath + @"\res\heart.empty.png");
                            break;
                        case 1:
                            if (rect.Name == "pbLifes1") rect.Image = Image.FromFile(Application.StartupPath + @"\res\heart.half.png");
                            if (rect.Name == "pbLifes3") rect.Image = Image.FromFile(Application.StartupPath + @"\res\heart.empty.png");
                            if (rect.Name == "pbLifes2") rect.Image = Image.FromFile(Application.StartupPath + @"\res\heart.empty.png");
                            break;
                        case 0:
                            if (rect.Name == "pbLifes1") rect.Image = Image.FromFile(Application.StartupPath + @"\res\heart.empty.png");
                            if (rect.Name == "pbLifes3") rect.Image = Image.FromFile(Application.StartupPath + @"\res\heart.empty.png");
                            if (rect.Name == "pbLifes2") rect.Image = Image.FromFile(Application.StartupPath + @"\res\heart.empty.png");
                            break;
                        default:
                            break;
                    }
                }
            }
            //Animation slim
            foreach (PictureBox rect in this.Controls)
            {
                if (rect.Tag == "slims" && slim.AnimationDelay <=0)
                {
                    if (slim.GoRight)
                    {
                        switch (slim.Animation)
                        {
                            case 0:
                                rect.Image = Image.FromFile(Application.StartupPath + @"\res\slime.right.1.png");
                                slim.Animation = 1;
                                break;
                            case 1:
                                rect.Image = Image.FromFile(Application.StartupPath + @"\res\slime.right.2.png");
                                slim.Animation = 2;
                                break;
                            case 2:
                                rect.Image = Image.FromFile(Application.StartupPath + @"\res\slime.right.3.png");
                                slim.Animation = 0;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (slim.Animation)
                        {
                            case 0:
                                rect.Image = Image.FromFile(Application.StartupPath + @"\res\slime.left.1.png");
                                slim.Animation = 1;
                                break;
                            case 1:
                                rect.Image = Image.FromFile(Application.StartupPath + @"\res\slime.left.2.png");
                                slim.Animation = 2;
                                break;
                            case 2:
                                rect.Image = Image.FromFile(Application.StartupPath + @"\res\slime.left.3.png");
                                slim.Animation = 0;
                                break;
                            default:
                                break;
                        }
                    }
                    
                }
            }
        }

        private void Level1()
        {
            CreateBaseInterface();
            //Instanciation du player et de son sprite
            player = new Players(6);

            //Instanciation des objets du niveau
                //Instanciation des clés
            blueKey = new LevelsKeys(1, false);
                //Instanciation des portes
            endDoor = new Doors(1, false, DoorsOutputs.Level2);
            //Déclaration et instanciation des sprites du niveau
                //Création des plateforms
            PictureBox pbPlateformBase = new PictureBox();
            PictureBox pbBloc1 = new PictureBox();
            PictureBox pbBloc2 = new PictureBox();
                //Création des clés
            PictureBox pbBlueKey = new PictureBox();
                //Création des portes

            //Initialisation des sprites du niveau
            //Initialisation des sprites de plateformes
            CreatePB(pbPlateformBase, "pbPlateformBase", 3072, 70, 10, 900, Application.StartupPath + @"\res\base.png", "platform");
            CreatePB(pbBloc1, "pbBloc1" ,70, 70, 1000, 830, Application.StartupPath + @"\res\stone.broken.1.png", "platform");
            CreatePB(pbBloc2, "pbBloc2", 140, 140, 1140, 760, Application.StartupPath + @"\res\stone.broken.4.png", "platform");
                //Initialisation des sprites de clés
            CreatePB(pbBlueKey, "pbBlueKey",70, 65, 1300, 830, Application.StartupPath + @"\res\key.blue.png", "key");
                //Initialisation des sprites de porte
            
        }
        private void Level2()
        {
            CreateBaseInterface();
            //Instanciation du player
            player = new Players(6);
            

            //Instanciation des objets du niveau
                //Instanciation des clés
            blueKey = new LevelsKeys(1, false);
            redKey = new LevelsKeys(1, false);
            greenKey = new LevelsKeys(1, false);
            yellowKey = new LevelsKeys(1, false);
                //Instanciation des portes
                //Instanciation des piques
            spike = new Spikes();
                //Instanciation des slims
            slim = new Slims(100);
            //Déclaration et instanciation des sprites du niveau
                //Création des plateforms
            PictureBox pbPlateformBase = new PictureBox();
            PictureBox pbBloc1 = new PictureBox();
            PictureBox pbBloc2 = new PictureBox();
                //Création des clés
            PictureBox pbBlueKey = new PictureBox();
            PictureBox pbRedKey = new PictureBox();
            PictureBox pbGreenKey = new PictureBox();
            PictureBox pbYellowKey = new PictureBox();
                //Création des portes
                //Création de piques
            PictureBox pbSpike1 = new PictureBox();
                //Création de slims
            PictureBox pbSlim1 = new PictureBox();
            //Initialisation des sprites du niveau
                //Initialisation des sprites de plateformes
            CreatePB(pbPlateformBase, "pbPlateformBase", 3072, 70, 10, 900, Application.StartupPath + @"\res\base.png", "platform");
            CreatePB(pbBloc1, "pbBloc1", 70, 70, 1000, 830, Application.StartupPath + @"\res\stone.broken.1.png", "platform");
                //Initialisation des sprites de slims
            CreatePB(pbSlim1, "pbSlim1", 70, 70, 2050, 865, Application.StartupPath + @"\res\slime.right.1.png", "slims");
                //Initialisation des sprites de clés
            CreatePB(pbBlueKey, "pbBlueKey", 70, 65, 1300, 830, Application.StartupPath + @"\res\key.blue.png", "key");
            CreatePB(pbRedKey, "pbRedKey", 70, 65, 1380, 830, Application.StartupPath + @"\res\key.red.png", "key");
            CreatePB(pbGreenKey, "pbGreenKey", 70, 65, 1460, 830, Application.StartupPath + @"\res\key.green.png", "key");
            CreatePB(pbYellowKey, "pbYellowKey", 70, 65, 1540, 830, Application.StartupPath + @"\res\key.yellow.png", "key");
                //Initialisation des sprites de porte
                //Initialisation des sprites de piques
            CreatePB(pbSpike1, "pbSpike1", 70, 95, 1650, 865, Application.StartupPath + @"\res\spikes.png", "spikes");

        }

        private void CreatePB(PictureBox pbName, string name,int width, int height, int xLoc, int yLoc, string path, string tag)
        {
            pbName.Parent = this;
            pbName.Name = name;
            pbName.Width = width;
            pbName.Height = height;
            pbName.Location = new Point(xLoc, yLoc);
            pbName.BackColor = Color.Transparent;
            pbName.Image = Image.FromFile(path);
            pbName.Tag = tag;
            pbName.BringToFront();
            this.Controls.Add(pbName);
        }

        //Code de ClearForm récupéré sur https://social.msdn.microsoft.com/Forums/vstudio/en-US/9c85d1f3-66a1-42be-9a6e-175fc3c6e739/remove-picture-box-controls?forum=csharpgeneral
        private void ClearForm(Control control, Type type)
        {
            List<Control> controls = new List<Control>();
            Stack<Control> stack = new Stack<Control>();

            stack.Push(control);
            
            while (stack.Count > 0)
            {
                Control ctrl = stack.Pop();

                foreach (Control child in ctrl.Controls)
                {
                    if (child.GetType() == type)
                    {
                        controls.Add(child);
                    }
                    else if (true == child.HasChildren)
                    {
                        stack.Push(child);
                    }
                }
            }
            
            foreach (Control ctrl in controls)
            {
                control.Controls.Remove(ctrl);
                ctrl.Dispose();
            }
        }

        private void CreateBaseInterface()
        {
            tmrMain.Enabled = true;
            tmrAction.Enabled = true;
            tmrAnimations.Enabled = true;
            tmrDelays.Enabled = true;
            pbPlayer = new PictureBox();
            endDoor = new Doors(1, false, DoorsOutputs.Level1);

            PictureBox pbEndDoor = new PictureBox();
            //Création des vies;
            PictureBox pbLifes1 = new PictureBox();
            PictureBox pbLifes2 = new PictureBox();
            PictureBox pbLifes3 = new PictureBox();
            //Initialisation des sprites de vies du player
            CreatePB(pbLifes1, "pbLifes1", 55, 50, 50, 40, Application.StartupPath + @"\res\heart.full.png", "lifes");
            CreatePB(pbLifes2, "pbLifes2", 55, 50, 105, 40, Application.StartupPath + @"\res\heart.full.png", "lifes");
            CreatePB(pbLifes3, "pbLifes3", 55, 50, 160, 40, Application.StartupPath + @"\res\heart.full.png", "lifes");
            //Initialisation du sprite du player
            CreatePB(pbPlayer, "pbPlayer", 70, 70, 180, 300, Application.StartupPath + @"\res\blocker.happy.png", "player");
            //Initialisation de la porte de sortie
            CreatePB(pbEndDoor, "pbEndDoor", 70, 70, 2700, 830, Application.StartupPath + @"\res\door.closed.png", "door");
        }
    }
}
