/**
 * \file      frmGame.cs
 * \author    G. Mbayo
 * \version   1.0
 * \date      Octobre 31. 2019
 * \brief     Form to play.
 *
 * \details   This form enable to launch the menu, and then to play the different levels of the game. It contains all the display functions
 * \          as well as the background workers.
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
using System.Threading;

namespace Unlock_the_Door
{
    public partial class frmGame : Form
    {
        private int deathDelay = 50; //Le délai avant que le niveau ne recommence après la mort du joueur
        private bool directClose = false; //Le formulaire se ferme directement ou fait apparaître une popup
        //En partie
        bool inGame; //Détermine si la partie est en cours ou en pause

        //Threads
        BackgroundWorker bw_move;
        BackgroundWorker bw_action;
        BackgroundWorker bw_animation;
        BackgroundWorker bw_delay;
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
            //Déclaration de la liste de slims
        List<Slims> slims = new List<Slims>();
            //Déclaration de la liste de fireballs
        List<Fireballs> fireballs = new List<Fireballs>();

        /// <summary>
        /// Initialize the componants and the backgroundWorkers
        /// </summary>
        public frmGame()
        {
            InitializeComponent();

            // Initialisation des BackgroudWorkers
            this.bw_move = new BackgroundWorker();
            this.bw_action = new BackgroundWorker();
            this.bw_animation = new BackgroundWorker();
            this.bw_delay = new BackgroundWorker();
            bw_move.WorkerSupportsCancellation = true;
            bw_action.WorkerSupportsCancellation = true;
            bw_animation.WorkerSupportsCancellation = true;
            bw_delay.WorkerSupportsCancellation = true;
        }

        /// <summary>
        /// Launch the menu at the loading of the app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMainWindow_Load(object sender, EventArgs e)
        {
            //Niveau à appeler au début du programme
            Menu();
        }

        /// <summary>
        /// Describe the events when certains key from keyboard are pressed 
        /// The keys are: A, W, D, E, Right_arrow and Left_arrow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                case Keys.Left:
                    if(player.FireDelay <= 0)
                    {
                        Fireballs fireL = new Fireballs(false);
                        CreatePB(fireL, "fireL", 27, 27, player.Left-27, player.Top, Application.StartupPath + @"\res\fireball.png", "fireballs");
                        fireballs.Add(fireL);
                        player.FireDelay = 20;
                    }
                    break;
                case Keys.Right:
                    if (player.FireDelay <= 0)
                    {
                        Fireballs fireR = new Fireballs(true);
                        CreatePB(fireR, "fireR", 27, 27, player.Right, player.Top, Application.StartupPath + @"\res\fireball.png", "fireballs");
                        fireballs.Add(fireR);
                        player.FireDelay = 20;
                    }
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// Describe the events when certains key are released and stop the players jump action
        /// the keys are A, W, D
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Contains the main moving and collision events in the game
        /// This function is executed inside of the BackgroundWorker bw_move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrMain_Tick(object sender, DoWorkEventArgs e)
        {
            while (this.inGame)
            {
                Thread.Sleep(20);
                if (deathDelay >= 48)
                {
                    this.Invoke(new Action(() => playerCollisions()));

                    this.Invoke(new Action(() => moveSlimes()));

                    this.Invoke(new Action(() => firaballsInteraction()));
                }
            } // While(inGame)
        }

        /// <summary>
        /// Describes the collision between player and plateforms, the movements of the camera (focus) and if the player can move or not
        /// This function is executed inside of the BackgroundWorker bw_move
        /// </summary>
        private void playerCollisions()
        {
            if (player.Jump && player.Inertia < 0) { player.Jump = false; }

            //Si le player se rapproche du bord de l'écran, la caméra se déplace avec lui (bas)
            if (!player.Jump && !player.CollisionBottom && player.Bottom >= this.Bottom - this.Height / 10)
            {
                this.Invoke(new Action(() => reduceTopSpeed(player.JumpSpeed)));
            }
            else
            {
                this.Invoke(new Action(() => increasePlayerSpeed(player.JumpSpeed)));
            }

            //Si le player se rapproche du bord de l'écran, la caméra se déplace avec lui (droite)
            if (player.GoRight && !player.CollisionRight && player.Left >= this.Right - this.Width / 3.5)
            {
                foreach (PictureBox rect in this.Controls)
                {
                    if (rect.Tag.ToString() != "player" && rect.Tag.ToString() != "lifes" && rect.Tag.ToString() != "inventory")
                        rect.Left -= player.MoveRight();
                }
            }
            else if (player.GoRight && !player.CollisionRight) { player.Left += player.MoveRight(); }

            //Si le player se rapproche du bord de l'écran, la caméra se déplace avec lui (gauche)
            if (player.GoLeft && !player.CollisionLeft && player.Left <= this.Width / 3.5)
            {
                foreach (PictureBox rect in this.Controls)
                {
                    if (rect.Tag.ToString() != "player" && rect.Tag.ToString() != "lifes" && rect.Tag.ToString() != "inventory")
                        rect.Left -= player.MoveLeft();
                }
            }
            else if (player.GoLeft && !player.CollisionLeft) { player.Left += player.MoveLeft(); }

            //Les collision sont à false par défaut avant de checker si il y a une collision
            player.CollisionLeft = false;
            player.CollisionRight = false;
            player.CollisionBottom = false;

            //On vérifie les collisons du joueur avec un plateforme
            //Tend à être modifier lorsque les blocs deviendrons également des objets
            foreach (PictureBox rect in this.Controls)
            {
                switch (rect.Tag)
                {
                    case "platform":
                        //collision en bas
                        if (player.Bottom >= rect.Top && player.Top < rect.Top && player.Left < rect.Right && player.Right > rect.Left)
                        {
                            player.RefillInertia();   //redonne la capacité de sauter

                            player.Top = rect.Top - player.Height; //replace le player sur la plateforme pour qu'il ne passe pas à travers
                            player.CollisionBottom = true;
                        }

                        //collision a droite
                        if (player.Right >= rect.Left && player.Bottom > rect.Top && player.Top < rect.Bottom && player.Right < rect.Left + player.Speed)
                        {
                            player.GoRight = false;
                            player.CollisionRight = true;
                        }

                        //collision  gauche
                        if (player.Left <= rect.Right && player.Bottom > rect.Top && player.Top < rect.Bottom && player.Left > rect.Right - player.Speed)
                        {
                            player.GoLeft = false;
                            player.CollisionLeft = true;
                        }
                        break;
                    default:
                        break;
                }
                if (rect.Name == "pbPlateformBase" && player.Top > rect.Bottom + 50) { player.Lifes = 0; } //tue le joueurs si il tombe de la plateforme de base
                                                                                                           //Active la mort du joueur si ses vies tombent à 0
                if (player.Lifes <= 0)
                {
                    player.Dead = true;
                }
            }
            player.MoveUp(player.CollisionBottom); //test si le joueur est en train de sauter/tomber
        }

        /// <summary>
        /// Describe the comportement of the fireballs:
        /// When it hurt something, when its time to live is ended, the way it move and when it inflict dammages
        /// This function is executed inside of the BackgroundWorker bw_move
        /// </summary>
        private void firaballsInteraction()
        {
            //Fireballs
            foreach (var fire in fireballs)
            {
                fire.CollisionBottom = false;
                //Collison
                //Tend à disparaître si chaque objet est enregistrer dans un liste
                foreach (Control ctrl in this.Controls)
                {

                    if (ctrl.GetType().IsSubclassOf(typeof(PictureBox)) || ctrl.GetType() == typeof(PictureBox))
                    {
                        switch (ctrl.Tag)
                        {
                            case "spikes":
                                if (fire.Bounds.IntersectsWith(ctrl.Bounds))
                                {
                                    fire.Ttl = 0;
                                }
                                break;
                            case "platform":
                                //Si elle tombe sur une plateforme, elle rebondit, si elle percute le côté d'une plateforme, elle meurt
                                if (fire.Bottom >= ctrl.Top && fire.Top < ctrl.Top && fire.Left < ctrl.Right && fire.Right > ctrl.Left)
                                {
                                    fire.Top = ctrl.Top - fire.Height;
                                    fire.CollisionBottom = true;
                                }
                                else if (fire.Bounds.IntersectsWith(ctrl.Bounds))
                                {
                                    fire.Ttl = 0;
                                }
                                break;
                            default:
                                break;
                        }
                    }

                }
                //Si elle touche une slim
                foreach (Slims slims in slims)
                {
                    if (fire.Bounds.IntersectsWith(slims.Bounds) && fire.Hurt)
                    {
                        fire.Hurt = false;
                        slims.Lifes -= fire.Dammages;
                        fire.Ttl = 0;
                    }
                }
                if (fire.Ttl > 0) fire.MoveUp(fire.CollisionBottom); //test si la ball rebondit ou non
            }
        }

        /// <summary>
        /// Contains the movement (right or left) of each slim in the level
        /// This function is executed inside of the BackgroundWorker bw_move
        /// </summary>
        private void moveSlimes()
        {
            //Déplacement des slims
            foreach (Slims slim in slims)
            {
                if (slim.GoRight)
                {
                    slim.MoveRight();
                }
                else
                {
                    slim.MoveLeft();
                }
            }
        }

        /// <summary>
        /// Contains the acceleration of the player (when he falls)
        /// This function is executed inside of the BackgroundWorker bw_move
        /// </summary>
        /// <param name="speed">The speed of the acceleration</param>
        private void increasePlayerSpeed(int speed)
        {
            player.Top += speed;
        }

        /// <summary>
        /// Contains the deceleration of the player
        /// This function is executed inside of the BackgroundWorker bw_move
        /// </summary>
        /// <param name="speed">The speed of the deceleration</param>
        private void reduceTopSpeed(int speed)
        {
            foreach (PictureBox rect in this.Controls)
            {
                if ((String)rect.Tag != "player" && rect.Tag.ToString() != "lifes" && rect.Tag.ToString() != "inventory")
                {
                    rect.Top -= speed;
                }
            }
        }

        /// <summary>
        /// contain the function describing the interraction between player and the levels objects (spikes, slims, doors and keys)
        /// This function is executed inside of the BackgroundWorker bw_action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrAction_Tick(object sender, DoWorkEventArgs e)
        {
            while (this.inGame)
            {
                Thread.Sleep(20);
                if (deathDelay >= 48)
                {
                    this.Invoke(new Action(() => actions()));
                }
            }
        }

        /// <summary>
        /// Describe the interraction between player and the levels objects (spikes, slims, doors and keys)
        /// Is not used if the player is dead
        /// This function is executed inside of the BackgroundWorker bw_action
        /// </summary>
        private void actions()
        {
            foreach (PictureBox rect in this.Controls)
            {
                switch (rect.Tag)
                {
                    case "key":
                        if (player.Bounds.IntersectsWith(rect.Bounds) && rect.Enabled != false)
                        {
                            switch (rect.Name)
                            {
                                case "blueKey":
                                    blueKey.Taken = true;
                                    rect.Location = new Point(275, 30);
                                    rect.Tag = "inventory";

                                    break;
                                case "greenKey":
                                    greenKey.Taken = true;
                                    rect.Location = new Point(345, 30);
                                    rect.Tag = "inventory";
                                    break;
                                case "redKey":
                                    redKey.Taken = true;
                                    rect.Location = new Point(415, 30);
                                    rect.Tag = "inventory";
                                    break;
                                case "yellowKey":
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
                        if (player.Bounds.IntersectsWith(rect.Bounds) && player.Action)
                        {
                            switch (rect.Name)
                            {
                                case "endDoor":
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
                                            case DoorsOutputs.Level3:
                                                Level3();
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
                            if (player.Right <= rect.Left + rect.Width / 2 + player.Width / 2 && player.Bounds.IntersectsWith(rect.Bounds))
                            {
                                player.Lifes -= spike.DealDammage();
                                player.RefillInertia();
                                player.Jump = true;
                                player.Left -= spike.PushPlayer();
                            }
                            else if (player.Bounds.IntersectsWith(rect.Bounds))
                            {
                                player.Lifes -= spike.DealDammage();
                                player.RefillInertia();
                                player.Jump = true;
                                player.Left += spike.PushPlayer();
                            }
                        }
                        break;
                    case "slims":
                        if (player.Bounds.IntersectsWith(rect.Bounds))
                        {
                            player.Lifes -= slims[0].DealDammage();
                            player.RefillInertia();
                            player.Jump = true;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Contains the functions describing the animations delays and the players death
        /// This function is executed inside of the BackgroundWorker bw_delay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrDelays_Tick(object sender, DoWorkEventArgs e)
        {
            while (this.inGame)
            {
                Thread.Sleep(20);
                if(deathDelay >= 48)
                {
                    this.Invoke(new Action(() => animations()));
                }
                this.Invoke(new Action(() => playerDeath()));
            }
        }

        /// <summary>
        /// Deacrease the amount of time while the spike and slimes don't hurt and decreas the time to live of fireballs
        /// This function is executed inside of the BackgroundWorker bw_delay
        /// </summary>
        private void animations()
        {
            player.Dammaged = false;
            //Spikes
            if (spike != null)
            {
                spike.Reload();
            }
            if (!spike.Hurt && spike != null) { player.Dammaged = true; }
            //Slim
            foreach (Slims slim in slims)
            {
                if (slim != null)
                {
                    slim.Reload();
                }
                if (!slim.Hurt) { player.Dammaged = true; }
                if (slim.Lifes <= 0)
                {
                    slim.Dead = true;
                }
            }

            // fireballs
            if (player.FireDelay > 0)
            {
                player.FireDelay -= 1;
            }
        }

        /// <summary>
        /// Describe the events between the player death and the levels reload
        /// This function is executed inside of the BackgroundWorker bw_delay
        /// </summary>
        private void playerDeath()
        {
            //Mort du player
            if (player.Dead)
            {
                if (deathDelay <= 0)
                {
                    switch (endDoor.Output)
                    {
                        case DoorsOutputs.Level1:
                            ClearForm(this, typeof(PictureBox));
                            Level3();
                            break;
                        case DoorsOutputs.Level2:
                            ClearForm(this, typeof(PictureBox));
                            Level1();
                            break;
                        case DoorsOutputs.Level3:
                            ClearForm(this, typeof(PictureBox));
                            Level2();
                            break;
                        default:
                            break;
                    }
                    deathDelay = 50;
                }
                else deathDelay -= 1;
            }
        }

        /// <summary>
        /// Contains the functions displaying the lifes and the animations of the player, the fireballs and the slims
        /// This function is executed inside of the BackgroundWorker bw_animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrAnimations_Tick(object sender, DoWorkEventArgs e)
        {
            while (this.inGame)
            {
                Thread.Sleep(20);
                if (deathDelay >= 48)
                {
                    this.Invoke(new Action(() => playersFacial()));
                    this.Invoke(new Action(() => lifesDisplay()));
                    this.Invoke(new Action(() => slimDisplay()));
                    this.Invoke(new Action(() => fireballsAnimations()));
                }
            }
        }

        /// <summary>
        /// Describe the different animations of the player (the face that he makes) in different situations (death, damaged, normal)
        /// This function is executed inside of the BackgroundWorker bw_animation
        /// </summary>
        private void playersFacial()
        {
            //expressions facial du player
            if (player.Lifes <= 0)
            {
                player.Image = Image.FromFile(Application.StartupPath + @"\res\blocker.dead.png");
            }
            else if (player.Dammaged) { player.Image = Image.FromFile(Application.StartupPath + @"\res\blocker.sad.png"); }
            else { player.Image = Image.FromFile(Application.StartupPath + @"\res\blocker.happy.png"); }
        }

        /// <summary>
        /// Display the lifes and its animation when the player is hurt
        /// This function is executed inside of the BackgroundWorker bw_animation
        /// </summary>
        private void lifesDisplay()
        {
            //affichage des lifes
            foreach (PictureBox rect in this.Controls)
            {
                if (rect.Tag.ToString() == "lifes")
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
        }

        /// <summary>
        /// Describe the animation of slimes (going right, left or dying)
        /// This function is executed inside of the BackgroundWorker bw_animation
        /// </summary>
        private void slimDisplay()
        {
            //Animation slim
            foreach (Slims slim in slims)
            {
                if (!slim.Dead)
                {
                    if (slim.AnimationDelay <= 0)
                    {
                        if (slim.GoRight)
                        {
                            switch (slim.Animation)
                            {
                                case 0:
                                    slim.Image = Image.FromFile(Application.StartupPath + @"\res\slime.right.1.png");
                                    slim.Animation = 1;
                                    break;
                                case 1:
                                    slim.Image = Image.FromFile(Application.StartupPath + @"\res\slime.right.2.png");
                                    slim.Animation = 2;
                                    break;
                                case 2:
                                    slim.Image = Image.FromFile(Application.StartupPath + @"\res\slime.right.3.png");
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
                                    slim.Image = Image.FromFile(Application.StartupPath + @"\res\slime.left.1.png");
                                    slim.Animation = 1;
                                    break;
                                case 1:
                                    slim.Image = Image.FromFile(Application.StartupPath + @"\res\slime.left.2.png");
                                    slim.Animation = 2;
                                    break;
                                case 2:
                                    slim.Image = Image.FromFile(Application.StartupPath + @"\res\slime.left.3.png");
                                    slim.Animation = 0;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    switch (slim.Animation)
                    {
                        case 0:
                            if (slim.GoRight)
                            {
                                slim.Image = Image.FromFile(Application.StartupPath + @"\res\slime.right.dead.png");
                            }
                            else
                            {
                                slim.Image = Image.FromFile(Application.StartupPath + @"\res\slime.left.dead.png");
                            }
                            slim.Animation = 1;
                            break;
                        case 1:
                            if (slim.GoRight)
                            {
                                slim.Image = Image.FromFile(Application.StartupPath + @"\res\slime.right.dead.png");
                            }
                            else
                            {
                                slim.Image = Image.FromFile(Application.StartupPath + @"\res\slime.left.dead.png");
                            }
                            slim.Animation = 2;
                            break;
                        case 2:
                            slim.Image = Image.FromFile(Application.StartupPath + @"\res\smoke.orange.1.png");
                            slim.Animation = 3;
                            break;
                        case 3:
                            slim.Image = Image.FromFile(Application.StartupPath + @"\res\smoke.orange.2.png");
                            slim.Animation = 4;
                            break;
                        case 4:
                            slim.Image = Image.FromFile(Application.StartupPath + @"\res\smoke.orange.3.png");
                            slim.Animation = 5;
                            break;
                        case 5:
                            slim.Dispose();
                            break;
                        default:
                            break;
                    }
                }

            }
        }

        /// <summary>
        /// Describe the animation of fireballs, it's deplacement and it's death
        /// This function is executed inside of the BackgroundWorker bw_animation
        /// </summary>
        private void fireballsAnimations()
        {
            //fireballs animation
            try
            {
                foreach (var fireball in fireballs)
                {
                    fireball.BringToFront();
                    fireball.Animation();
                    if (fireball.Ttl > 0)
                    {
                        if (fireball.GoRight)
                        {
                            fireball.Left += fireball.Speed;
                        }
                        else
                        {
                            fireball.Left -= fireball.Speed;
                        }
                    }
                    else
                    {
                        if (fireball.TimerSmoke > 0)
                        {
                            switch (fireball.TimerSmoke)
                            {
                                case 6:
                                case 5:
                                    fireball.Image = Image.FromFile(Application.StartupPath + @"\res\smoke.gray.1.png");
                                    fireball.SizeMode = PictureBoxSizeMode.StretchImage;
                                    fireball.TimerSmoke -= 1;
                                    break;
                                case 4:
                                case 3:
                                    fireball.Image = Image.FromFile(Application.StartupPath + @"\res\smoke.gray.2.png");
                                    fireball.SizeMode = PictureBoxSizeMode.StretchImage;
                                    fireball.TimerSmoke -= 1;
                                    break;
                                case 2:
                                case 1:
                                    fireball.Image = Image.FromFile(Application.StartupPath + @"\res\smoke.gray.3.png");
                                    fireball.SizeMode = PictureBoxSizeMode.StretchImage;
                                    fireball.TimerSmoke -= 1;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            fireballs.Remove(fireball);
                            fireball.Dispose();
                        }
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Call the menu form and determine wich action to make depending on the choice made on menu by the user
        /// </summary>
        private void Menu()
        {
            frmMenu menu = new frmMenu();
            menu.ShowDialog();

            switch (menu.level)
            {
                case 1:
                    Level1();
                    break;
                case 2:
                    Level2();
                    break;
                case 3:
                    Level3();
                    break;
                default:
                    directClose = true;
                    this.Close();
                    break;
            }
            menu.Dispose();
        }

        /// <summary>
        /// Build the level 1 of the game
        /// </summary>
        private void Level1()
        {
            //Instanciation du player et de son sprite
            player = new Players(6);

            CreateBaseInterface();
            //Instanciation des objets du niveau
                //Instanciation des clés
            blueKey = new LevelsKeys(1, false);
                //Instanciation des portes
            endDoor = new Doors(1, false, DoorsOutputs.Level2);
                //Instanciation des piques
            spike = new Spikes();
            //Déclaration et instanciation des sprites du niveau
            //Création des plateforms
            PictureBox pbPlateformBase = new PictureBox();
            PictureBox pbBloc1 = new PictureBox();
            PictureBox pbBloc2 = new PictureBox();
                //Création des portes

            //Initialisation des sprites du niveau
            //Initialisation des sprites de plateformes
            CreatePB(pbPlateformBase, "pbPlateformBase", 3072, 70, 10, 900, Application.StartupPath + @"\res\base.png", "platform");
            CreatePB(pbBloc1, "pbBloc1" ,70, 70, 1000, 830, Application.StartupPath + @"\res\stone.broken.1.png", "platform");
            CreatePB(pbBloc2, "pbBloc2", 140, 140, 1140, 760, Application.StartupPath + @"\res\stone.broken.4.png", "platform");
                //Initialisation des sprites de clés
            CreatePB(blueKey, "blueKey",70, 65, 1300, 830, Application.StartupPath + @"\res\key.blue.png", "key");
                //Initialisation des sprites de porte
            
        }

        /// <summary>
        /// Build the level 2 of the game
        /// </summary>
        private void Level2()
        {
            //Instanciation du player
            player = new Players(6);

            CreateBaseInterface();

            //Instanciation des objets du niveau
                //Instanciation des clés
            blueKey = new LevelsKeys(1, false);
            redKey = new LevelsKeys(1, false);
            greenKey = new LevelsKeys(1, false);
            yellowKey = new LevelsKeys(1, false);
                //Instanciation des portes
            endDoor = new Doors(1, false, DoorsOutputs.Level3);
            //Instanciation des piques
            spike = new Spikes();
                //Instanciation des slims
            Slims slim1 = new Slims(100);
            //Déclaration et instanciation des sprites du niveau
                //Création des plateforms
            PictureBox pbPlateformBase = new PictureBox();
            PictureBox pbBloc1 = new PictureBox();
                //Création des portes
            //Initialisation des sprites du niveau
                //Initialisation des sprites de plateformes
            CreatePB(pbPlateformBase, "pbPlateformBase", 3072, 70, 10, 900, Application.StartupPath + @"\res\base.png", "platform");
            CreatePB(pbBloc1, "pbBloc1", 70, 70, 1000, 830, Application.StartupPath + @"\res\stone.broken.1.png", "platform");
                //Initialisation des sprites de slims
            CreatePB(slim1, "slim1", 40, 40, 2050, 865, Application.StartupPath + @"\res\slime.right.1.png", "slims");
            slims.Add(slim1);
                //Initialisation des sprites de clés
            CreatePB(blueKey, "blueKey", 70, 65, 1300, 830, Application.StartupPath + @"\res\key.blue.png", "key");
            CreatePB(redKey, "redKey", 70, 65, 1380, 830, Application.StartupPath + @"\res\key.red.png", "key");
            CreatePB(greenKey, "greenKey", 70, 65, 1460, 830, Application.StartupPath + @"\res\key.green.png", "key");
            CreatePB(yellowKey, "yellowKey", 70, 65, 1540, 830, Application.StartupPath + @"\res\key.yellow.png", "key");
                //Initialisation des sprites de porte
                //Initialisation des sprites de piques
            CreatePB(spike, "spike1", 70, 75, 1650, 865, Application.StartupPath + @"\res\spikes.png", "spikes");
        }

        /// <summary>
        /// Build the level 3 of the game
        /// </summary>
        private void Level3()
        {
            //Instanciation du player
            player = new Players(6);

            CreateBaseInterface();

            //Instanciation des objets du niveau
                //Instanciation des clés
            blueKey = new LevelsKeys(1, false);
                //Instanciation des piques
            spike = new Spikes();
            Spikes spike2 = new Spikes();
            Spikes spike3 = new Spikes();
            Spikes spike4 = new Spikes();
            Spikes spike5 = new Spikes();
            //Instanciation des slims
            Slims slim1 = new Slims(100);
            Slims slim2 = new Slims(50);
            Slims slim3 = new Slims(30);
            //Déclaration et instanciation des sprites du niveau
            //Création des plateforms
            PictureBox pbPlateformBase = new PictureBox();
            PictureBox pbBloc1 = new PictureBox();
            PictureBox pbBloc2 = new PictureBox();
            PictureBox pbBloc3 = new PictureBox();
            //Initialisation des sprites de plateformes
            CreatePB(pbPlateformBase, "pbPlateformBase", 3072, 70, 10, 900, Application.StartupPath + @"\res\base.png", "platform");
            CreatePB(pbBloc1, "pbBloc1", 70, 70, 1300, 830, Application.StartupPath + @"\res\stone.broken.1.png", "platform");
            CreatePB(pbBloc2, "pbBloc2", 70, 70, 1450, 520, Application.StartupPath + @"\res\stone.broken.1.png", "platform");
            CreatePB(pbBloc3, "pbBloc1", 70, 70, 1230, 650, Application.StartupPath + @"\res\stone.broken.1.png", "platform");
            //Initialisation des sprites de slims
            CreatePB(slim1, "slim1", 40, 40, 1250, 865, Application.StartupPath + @"\res\slime.right.1.png", "slims");
            CreatePB(slim2, "slim2", 40, 40, 1050, 865, Application.StartupPath + @"\res\slime.right.1.png", "slims");
            CreatePB(slim3, "slim3", 40, 40, 2200, 865, Application.StartupPath + @"\res\slime.right.1.png", "slims");
            slims.Add(slim1);
            slims.Add(slim2);
            slims.Add(slim3);
            //Initialisation des sprites de clés
            CreatePB(blueKey, "blueKey", 70, 65, 1450, 400, Application.StartupPath + @"\res\key.blue.png", "key");
            //Initialisation des sprites de piques
            CreatePB(spike, "spike1", 70, 75, 1650, 865, Application.StartupPath + @"\res\spikes.png", "spikes");
            CreatePB(spike2, "spike2", 70, 75, 1550, 865, Application.StartupPath + @"\res\spikes.png", "spikes");
            CreatePB(spike3, "spike3", 70, 75, 650, 865, Application.StartupPath + @"\res\spikes.png", "spikes");
            CreatePB(spike4, "spike4", 70, 75, 1850, 865, Application.StartupPath + @"\res\spikes.png", "spikes");
            CreatePB(spike5, "spike5", 70, 75, 1920, 865, Application.StartupPath + @"\res\spikes.png", "spikes");
        }

        /// <summary>
        /// Allow to give property to a PictureBox or a PictureBoxs child in only one line
        /// add this object to the form controls
        /// </summary>
        /// <param name="pbName">the object</param>
        /// <param name="name">name of the object</param>
        /// <param name="width">width of the object</param>
        /// <param name="height">height of the object</param>
        /// <param name="xLoc">the horizontal position (in pixels) of the left upper corner of the object</param>
        /// <param name="yLoc">the vertical position (in pixels) of the left upper corner of the object</param>
        /// <param name="path">the pathe of the objects image</param>
        /// <param name="tag">the tag (string) of the object</param>
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
            pbName.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pbName);
        }


        /// <summary>
        /// Put all the pictureBox and chils of pictureBox of the form in a list and delete them all
        /// Basic code of ClearForm récupéré sur https://social.msdn.microsoft.com/Forums/vstudio/en-US/9c85d1f3-66a1-42be-9a6e-175fc3c6e739/remove-picture-box-controls?forum=csharpgeneral
        /// </summary>
        /// <param name="control">the control in wich we want to delete the pictureBox (always "this")</param>
        /// <param name="type">the type that we want to delete (typeof(PictureBox))</param>
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
                    if (child.GetType() == type || child.GetType().IsSubclassOf(type))
                    {
                        controls.Add(child);
                    }
                    else if (child.HasChildren)
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

        /// <summary>
        /// Create the common base of each levels
        /// </summary>
        private void CreateBaseInterface()
        {
            try
            {
                this.bw_move.RunWorkerAsync();
                this.bw_action.RunWorkerAsync();
                this.bw_animation.RunWorkerAsync();
                this.bw_delay.RunWorkerAsync();
            }
            catch
            {

            }
            
            this.bw_move.DoWork += new DoWorkEventHandler(tmrMain_Tick);
            this.bw_action.DoWork += new DoWorkEventHandler(tmrAction_Tick);
            this.bw_animation.DoWork += new DoWorkEventHandler(tmrAnimations_Tick);
            this.bw_delay.DoWork += new DoWorkEventHandler(tmrDelays_Tick);
            inGame = true;
            endDoor = new Doors(1, false, DoorsOutputs.Level1);
            //Création des vies;
            PictureBox pbLifes1 = new PictureBox();
            PictureBox pbLifes2 = new PictureBox();
            PictureBox pbLifes3 = new PictureBox();
            //Initialisation des sprites de vies du player
            CreatePB(pbLifes1, "pbLifes1", 55, 50, 50, 40, Application.StartupPath + @"\res\heart.full.png", "lifes");
            CreatePB(pbLifes2, "pbLifes2", 55, 50, 105, 40, Application.StartupPath + @"\res\heart.full.png", "lifes");
            CreatePB(pbLifes3, "pbLifes3", 55, 50, 160, 40, Application.StartupPath + @"\res\heart.full.png", "lifes");
            //Initialisation du sprite du player
            CreatePB(player, "player", 70, 70, 180, 300, Application.StartupPath + @"\res\blocker.happy.png", "player");
            //Initialisation de la porte de sortie
            CreatePB(endDoor, "endDoor", 70, 70, 2700, 830, Application.StartupPath + @"\res\door.closed.png", "door");
        }

        /// <summary>
        /// Make a MessageBox appear when we clic the red cross to close the form instead of closing the form directly
        /// Cancel all the works of BackgroundWorkers
        /// Relaunch the work of BackgroundWorkers if we decide not to quit the game
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing||e.CloseReason == CloseReason.WindowsShutDown || e.CloseReason == CloseReason.ApplicationExitCall || e.CloseReason == CloseReason.TaskManagerClosing)
            {
                if (directClose)
                {
                    return;
                }
                this.inGame = false;
                this.bw_move.CancelAsync();
                this.bw_action.CancelAsync();
                this.bw_animation.CancelAsync();
                this.bw_delay.CancelAsync();

                switch (MessageBox.Show(this, "Are you sure you want to close?", "Closing", MessageBoxButtons.YesNo))
                {
                    case DialogResult.No:
                        this.inGame = true;

                        this.bw_move.RunWorkerAsync();
                        this.bw_action.RunWorkerAsync();
                        this.bw_animation.RunWorkerAsync();
                        this.bw_delay.RunWorkerAsync();

                        this.bw_move.DoWork += new DoWorkEventHandler(tmrMain_Tick);
                        this.bw_action.DoWork += new DoWorkEventHandler(tmrAction_Tick);
                        this.bw_animation.DoWork += new DoWorkEventHandler(tmrAnimations_Tick);
                        this.bw_delay.DoWork += new DoWorkEventHandler(tmrDelays_Tick);

                        e.Cancel = true;
                        break;
                    default:
                        return;
                }
                
            }
            e.Cancel = true;
        }
    }
}
