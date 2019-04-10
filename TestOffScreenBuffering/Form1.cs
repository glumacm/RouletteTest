using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace TestOffScreenBuffering
{
    public partial class Form1 : Form
    {

        public Bitmap offScreenBmp;
        public Graphics offScreenGr;
        public RouletteContainer rc;
        public Boolean initRC = true;
        public Boolean offScreenFinished = false;
        public Boolean isThreadRunning = false;
        public long currmilis;

        public RouletteBall rBall;




        //drawing purposes
        public SolidBrush black = new SolidBrush(Color.Black);
        public SolidBrush white = new SolidBrush(Color.White);
        public SolidBrush bisque = new SolidBrush(Color.Bisque);
        public SolidBrush magenta = new SolidBrush(Color.Magenta);

        public Pen p; //= new Pen(black);
        public Pen pt;// = new Pen(white);


        public int selectedNumber = -1;

        public int defaultRotatingTime;
        public int timeToStop;

        public DateTime startTime;
        public TimeSpan elapsedTime;
        public TimeSpan rejectBetsTime;

        public Boolean ballHasSpeed = true;

        public Boolean roundRunning = false;
        public Double ballSpeedStep = 4;

        DispatcherTimer tc;
        DispatcherTimer ballMovement;


        public int cashAmount;

        public GameRound game;

        public List<Bet> bets;


        public Form1()
        {
            InitializeComponent();
            this.BackColor = Color.ForestGreen;
            
            cashAmount = 1000;
            pictureBox1.BackColor = Color.Transparent;
            p = new Pen(this.black);
            pt = new Pen(this.white);
            
            this.DoubleBuffered = true;


            createRoulette();//narisano
            

            changeCashAmountLabel();

            tc = new DispatcherTimer();
            tc.Interval = new TimeSpan(0, 0, 0, 0, 24);
            tc.Tick += Tc_Tick;

            ballMovement = new DispatcherTimer();
            ballMovement.Interval = new TimeSpan(0, 0, 0, 0, 20);
            ballMovement.Tick += BallMovement_Tick;

            /*
            Task showBall = new Task(ballMovement.Start);
            showBall.Start();
            */

            /*
            startTime = DateTime.Now;
            Random rand = new Random();
            elapsedTime = new TimeSpan(0, 0, 0, 0, rand.Next(8500, 12000));
            */
            pictureBox1.Refresh();
            /*
            Task rotateRoulette = new Task(tc.Start);
            rotateRoulette.Start();*/

            
        }

        private void BallMovement_Tick(object sender, EventArgs e)
        {
            //update ball position add call refresh picturebox
            if (DateTime.Now - startTime > elapsedTime)
            {
                
                RouletteSlot[] slots = rc.getSlots();
                if (ballHasSpeed)
                {
                    roundRunning = false;
                    for (int i = 0; i < slots.Length; i++)
                    {
                        RouletteSlot slot = slots[i];
                        Double moveToSameAsSlot = (rBall.getBallCurrentAngle() + 90)%360;
                        //Console.WriteLine("Slot: " + i + " je med kotoma " + slot.getStartAngle() + "   " + slot.getEndAngle());
                        if (slot.getStartAngle() <= moveToSameAsSlot && slot.getEndAngle() > moveToSameAsSlot)
                        {
                            WinningNumberValue.Text = slot.getSlotNumber(); //"Zmagal je : " + slot.getSlotNumber();
                            game.winningNumber = Int32.Parse(slot.getSlotNumber());
                            if(game.winningNumber == 0)
                            {
                                game.winningColor = "green";
                            }else if (game.winningNumber % 2 ==0)
                            {
                                game.winningColor = "black";
                            }
                            else
                            {
                                game.winningColor = "red";
                            }
                            game.bets = bets;
                            Task sendDataToServer = new Task(() =>
                            {
                                try
                                {
                                    
                                    NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", "gameLogicServer", PipeDirection.InOut);
                                    pipeClient.Connect(400);
                                    pipeClient.ReadMode = PipeTransmissionMode.Message;
                                    IFormatter f = new BinaryFormatter();
                                    String jsonConvert = JsonConvert.SerializeObject(game);

                                    byte[] message = Encoding.UTF8.GetBytes(jsonConvert);
                                    pipeClient.Write(message, 0, message.Length);
                                    GameRound returnedMessage = getSerializedObject(pipeClient);
                                    pipeClient.Close();
                                    int winnings = 0;
                                    for(int ind = 0; ind < returnedMessage.bets.Count; ind++)
                                    {
                                        if (returnedMessage.bets[ind].winnable == true)
                                        {
                                            winnings += returnedMessage.bets[ind].winAmount;
                                        }
                                    }
                                    cashAmount += winnings;

                                    MessageBox.Show("You won :" + winnings + " EUR");
                                    changeCashAmountLabel();
                                }
                                catch(Exception error)
                                {
                                    Console.WriteLine("Prislo do napake  " + error.Message);
                                    MessageBox.Show("Could not send bets to server!");
                                }
                            });
                            sendDataToServer.Start();
                            break;
                        }
                    }
                    ballHasSpeed = false;
                }
                rBall.updateBallPosition(2, false);
            }
            else
            {
                rBall.updateBallPosition(ballSpeedStep, true);
            }
            //pictureBox1.Refresh();
        }

        public static GameRound getSerializedObject(NamedPipeClientStream npss)
        {
            StringBuilder sb = new StringBuilder();
            String messageArrivedd = String.Empty;
            byte[] message = new byte[50];
            do
            {
                npss.Read(message, 0, message.Length);// nastavimo delcek sporocila v seznam
                messageArrivedd = Encoding.UTF8.GetString(message);
                sb.Append(messageArrivedd);
                message = new byte[message.Length];
            } while (!npss.IsMessageComplete);
            //Console.WriteLine("izven doja");
            //Console.WriteLine(sb.ToString());
            GameRound tttt = JsonConvert.DeserializeObject<GameRound>(sb.ToString());
            return tttt;
        }

        private void Tc_Tick(object sender, EventArgs e)
        {

            if (isThreadRunning == false)
            {
                this.Refresh();
            }
            if(rc != null && rc.getAllSlotsDrawn())
            {
                rc.setAllSlotsDrawn(false);
                //Task updateLogic = new Task(new Action(updateRoulette));
                //updateLogic.Start();
                //DoWork();
                //Thread thread = new Thread(new ThreadStart(updateRoulette));
                isThreadRunning = true;
                updateRoulette();
                //thread.Start();
            }
        }


        public async Task DoWork()
        {
            //bool res = await Task.FromResult<Boolean>(updateRoulette());
            this.Refresh();
        }

        private int GetSum(int a, int b)
        {
            return a + b;
        }


        private void updateRoulette()
        {
            offScreenBmp = new Bitmap(this.Width, this.Height);
            //offScreenBmp.Dispose();
            offScreenGr = Graphics.FromImage(offScreenBmp);
            //Console.WriteLine("hugadaa11!");
            //run update
            rc.updateSlots(2);
            
            RouletteSlot[] slots = rc.getSlots();
            //GraphicsPath gp = new GraphicsPath();

            //offScreenGr.FillEllipse(bisque, rc.getCenterPoint().X, rc.getCenterPoint().Y, 200, 200);


            //gp.AddEllipse(rc.getCenterPoint().X, rc.getCenterPoint().Y, 330, 330);
            //offScreenGr.FillPath(bisque, gp);
            for (var i = 0; i < slots.Length; i++)
            {
                RouletteSlot slot = slots[i];
                //SolidBrush red = new SolidBrush(Color.Red);
               
                
                GraphicsPath slotData = slot.getRoulleteSlotGraphics();

                GraphicsPath textDat = slot.getTextData();
                //Rectangle tt = new Rectangle(rc.getCenterPoint().X, rc.getCenterPoint().Y, 200,200);

                //offScreenGr.FillEllipse(bisque, rc.getCenterPoint().X, rc.getCenterPoint().Y, 200,200); 
                offScreenGr.DrawPath(p, slotData);
                offScreenGr.DrawPath(pt, textDat);

                //slotData.Dispose();
                //textDat.Dispose();

                


            }

            if(DateTime.Now - startTime >= rejectBetsTime)
            {
                toggleAccesToButtons(false);
            }
            offScreenFinished = true;
            //return true;
            //this.Refresh();
            isThreadRunning = false;
        }


        private void toggleAccesToButtons(bool access)
        {
            betSingleNumberButton.Enabled = access;
            twoToOneFirst.Enabled = access;
            twoToOneSecond.Enabled = access;
            twoToOneThird.Enabled = access;
            firstDozenButton.Enabled = access;
            secondDozen.Enabled = access;
            thirdDozen.Enabled = access;
            odd.Enabled = access;
            even.Enabled = access;
            blackNumbers.Enabled = access;
            redNumbers.Enabled = access;
            nineteenThroughtThirty.Enabled = access;
            oneThroughEighteen.Enabled = access;
        }
        private void createRoulette()
        {
            Random rnd = new Random();
            toggleAccesToButtons(true);
            rejectBetsTime = new TimeSpan(0, 0, 0, 0, rnd.Next(7800, 11000));
            ballSpeedStep = rnd.Next(4, 7);
            rc = new RouletteContainer(220, 300, 0, new Point(200, 180));
            rc.createRouletteSlots();
            WinningNumberValue.Text= "";
            game = new GameRound();
            bets = new List<Bet>();
            offScreenBmp = new Bitmap(this.Width, this.Height);
            offScreenGr = Graphics.FromImage(offScreenBmp);
            Point ballP = new Point(rc.getCenterPoint().X, rc.getCenterPoint().Y);
            rBall = new RouletteBall(ballP, 0, 3, 2, 10, 10, rc.getRouletteOuterDiameter(), Color.Bisque);
            //
            startTime = DateTime.Now;
            Random rand = new Random();
            elapsedTime = new TimeSpan(0, 0, 0, 0, rand.Next(12500, 16000));
            ballHasSpeed = true;
            pictureBox1.Refresh();
            initRC = false;
            this.Refresh();
        }



        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen p = new Pen(new SolidBrush(Color.Black));
            if (initRC == false)
            {
                
                RouletteSlot[] slots = rc.getSlots();
                for(int i = 0; i< slots.Length; i++)
                {
                    SolidBrush slotColor = new SolidBrush(slots[i].getSlotColor());
                    GraphicsPath slotGP = slots[i].getRoulleteSlotGraphics();
                    GraphicsPath textGP = slots[i].getTextData();
                    Matrix tr = new Matrix();
                    tr.RotateAt(2, rc.getCenterPoint());
                    e.Graphics.FillPath(slotColor, slotGP);
                    e.Graphics.DrawPath(p, slotGP);
                    e.Graphics.FillPath(new SolidBrush(Color.White), textGP);
                }
            }

            if (offScreenFinished)
            {
                
                e.Graphics.DrawImage(offScreenBmp,0,0);
                offScreenBmp.Dispose();
                offScreenGr.Dispose();
                rc.setAllSlotsDrawn(true);
                offScreenFinished = false;
            }
            pictureBox1.Top = (int)rBall.positionY;
            pictureBox1.Left = (int)rBall.positionX;
        }





        private void changeCashAmountLabel()
        {
            wallet.Text = cashAmount.ToString() + " EUR";
        }



        /*
         * 
         * Buttons and text handlers
         * 
         */


        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics bTemp = Graphics.FromImage(rBall.getBallImg());

            e.Graphics.DrawImage(rBall.getBallImg(),(int)0,(int)0);
            //e.Graphics.DrawImage(rBall.getBallImg(), (int)rBall.positionX, (int)rBall.positionY);
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //first dozen
            if (betAmount.Value <= 0)
            {
                string text = "Bet amount must be higher then 0!";
                MessageBox.Show(text);
            }
            else if (betAmount.Value > 0 && cashAmount < betAmount.Value)
            {
                string text = "You cannot more cash than you have!";
                MessageBox.Show(text);
            }
            else
            {
                Bet b = new Bet();
                b.betAmount = (int)betAmount.Value;
                b.betType = "firstDozen";
                b.winnable = false;
                b.winAmount = 0;
                bets.Insert(bets.Count, b);

                cashAmount -= (int)betAmount.Value;
                
                changeCashAmountLabel();
            }
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void BetSingleNumberButton_Click(object sender, EventArgs e)
        {
            if( betAmount.Value <= 0)
            {
                string text = "Bet amount must be higher then 0!";
                MessageBox.Show(text);
            }else if(singleNumberBox.SelectedItem == null || singleNumberBox.SelectedItem == "")//.SelectedItem
            {
                string text = "You must select a number!" + singleNumberBox.SelectedItem;
                MessageBox.Show(text);
            }else if(betAmount.Value > 0 && cashAmount < betAmount.Value)
            {
                string text = "You cannot bet more cash than you have!";
                MessageBox.Show(text);
            }
            else
            {
                Bet b = new Bet();
                b.betAmount = (int) betAmount.Value;
                b.betType = "singleNumber";
                selectedNumber =Int32.Parse(singleNumberBox.SelectedItem.ToString());
                b.singleNumber = selectedNumber;
                b.winnable = false;
                bets.Append(b);
                bets.Insert(bets.Count, b);

                cashAmount -= (int)betAmount.Value;
                changeCashAmountLabel();
            }
            
        }


        private void NineteenThroughtThirty_Click(object sender, EventArgs e)
        {
            if (betAmount.Value <= 0)
            {
                string text = "Bet amount must be higher then 0!";
                MessageBox.Show(text);
            }
            else if (betAmount.Value > 0 && cashAmount < betAmount.Value)
            {
                string text = "You cannot more cash than you have!";
                MessageBox.Show(text);
            }
            else
            {
                Bet b = new Bet();
                b.betAmount = (int)betAmount.Value;
                b.betType = "19to35";
                b.winnable = false;
                b.winAmount = 0;
                bets.Insert(bets.Count, b);
                cashAmount -= (int)betAmount.Value;
                changeCashAmountLabel();
            }
        }

        private void OneThroughEighteen_Click(object sender, EventArgs e)
        {
            if (betAmount.Value <= 0)
            {
                string text = "Bet amount must be higher then 0!";
                MessageBox.Show(text);
            }
            else if (betAmount.Value > 0 && cashAmount < betAmount.Value)
            {
                string text = "You cannot more cash than you have!";
                MessageBox.Show(text);
            }
            else
            {
                Bet b = new Bet();
                b.betAmount = (int)betAmount.Value;
                b.betType = "1to18";
                b.winnable = false;
                b.winAmount = 0;
                bets.Insert(bets.Count, b);
                cashAmount -= (int)betAmount.Value;
                changeCashAmountLabel();
            }
        }

        private void BlackNumbers_Click(object sender, EventArgs e)
        {
            if (betAmount.Value <= 0)
            {
                string text = "Bet amount must be higher then 0!";
                MessageBox.Show(text);
            }
            else if (betAmount.Value > 0 && cashAmount < betAmount.Value)
            {
                string text = "You cannot more cash than you have!";
                MessageBox.Show(text);
            }
            else
            {
                Bet b = new Bet();
                b.betAmount = (int)betAmount.Value;
                b.betType = "black";
                b.winnable = false;
                b.winAmount = 0;
                bets.Insert(bets.Count, b);
                cashAmount -= (int)betAmount.Value;
                changeCashAmountLabel();
            }
        }

        private void Even_Click(object sender, EventArgs e)
        {
            if (betAmount.Value <= 0)
            {
                string text = "Bet amount must be higher then 0!";
                MessageBox.Show(text);
            }
            else if (betAmount.Value > 0 && cashAmount < betAmount.Value)
            {
                string text = "You cannot more cash than you have!";
                MessageBox.Show(text);
            }
            else
            {
                Bet b = new Bet();
                b.betAmount = (int)betAmount.Value;
                b.betType = "even";
                b.winnable = false;
                b.winAmount = 0;
                bets.Insert(bets.Count, b);
                cashAmount -= (int)betAmount.Value;
                changeCashAmountLabel();
            }
        }

        private void SecondDozen_Click(object sender, EventArgs e)
        {
            if (betAmount.Value <= 0)
            {
                string text = "Bet amount must be higher then 0!";
                MessageBox.Show(text);
            }
            else if (betAmount.Value > 0 && cashAmount < betAmount.Value)
            {
                string text = "You cannot more cash than you have!";
                MessageBox.Show(text);
            }
            else
            {
                Bet b = new Bet();
                b.betAmount = (int)betAmount.Value;
                b.betType = "secondDozen";
                b.winnable = false;
                b.winAmount = 0;
                bets.Insert(bets.Count, b);
                cashAmount -= (int)betAmount.Value;
                changeCashAmountLabel();
            }
        }

        private void RedNumbers_Click(object sender, EventArgs e)
        {
            if (betAmount.Value <= 0)
            {
                string text = "Bet amount must be higher then 0!";
                MessageBox.Show(text);
            }
            else if (betAmount.Value > 0 && cashAmount < betAmount.Value)
            {
                string text = "You cannot more cash than you have!";
                MessageBox.Show(text);
            }
            else
            {
                Bet b = new Bet();
                b.betAmount = (int)betAmount.Value;
                b.betType = "red";
                b.winnable = false;
                b.winAmount = 0;
                bets.Insert(bets.Count, b);
                cashAmount -= (int)betAmount.Value;
                changeCashAmountLabel();
            }
        }

        private void Odd_Click(object sender, EventArgs e)
        {
            if (betAmount.Value <= 0)
            {
                string text = "Bet amount must be higher then 0!";
                MessageBox.Show(text);
            }
            else if (betAmount.Value > 0 && cashAmount < betAmount.Value)
            {
                string text = "You cannot more cash than you have!";
                MessageBox.Show(text);
            }
            else
            {
                Bet b = new Bet();
                b.betAmount = (int)betAmount.Value;
                b.betType = "odd";
                b.winnable = false;
                b.winAmount = 0;
                bets.Insert(bets.Count, b);
                cashAmount -= (int)betAmount.Value;
                changeCashAmountLabel();
            }
        }

        private void ThirdDozen_Click(object sender, EventArgs e)
        {
            if (betAmount.Value <= 0)
            {
                string text = "Bet amount must be higher then 0!";
                MessageBox.Show(text);
            }
            else if (betAmount.Value > 0 && cashAmount < betAmount.Value)
            {
                string text = "You cannot more cash than you have!";
                MessageBox.Show(text);
            }
            else
            {
                Bet b = new Bet();
                b.betAmount = (int)betAmount.Value;
                b.betType = "thirdDozen";
                b.winnable = false;
                b.winAmount = 0;
                bets.Insert(bets.Count, b);
                cashAmount -= (int)betAmount.Value;
                changeCashAmountLabel();
            }
        }

        private void TwoToOneFirst_Click(object sender, EventArgs e)
        {
            if (betAmount.Value <= 0)
            {
                string text = "Bet amount must be higher then 0!";
                MessageBox.Show(text);
            }
            else if (betAmount.Value > 0 && cashAmount < betAmount.Value)
            {
                string text = "You cannot more cash than you have!";
                MessageBox.Show(text);
            }
            else
            {
                Bet b = new Bet();
                b.betAmount = (int)betAmount.Value;
                b.betType = "2to1_1";
                b.winnable = false;
                b.winAmount = 0;
                bets.Insert(bets.Count, b);
                cashAmount -= (int)betAmount.Value;
                changeCashAmountLabel();
            }
        }

        private void TwoToOneSecond_Click(object sender, EventArgs e)
        {
            if (betAmount.Value <= 0)
            {
                string text = "Bet amount must be higher then 0!";
                MessageBox.Show(text);
            }
            else if (betAmount.Value > 0 && cashAmount < betAmount.Value)
            {
                string text = "You cannot more cash than you have!";
                MessageBox.Show(text);
            }
            else
            {
                Bet b = new Bet();
                b.betAmount = (int)betAmount.Value;
                b.betType = "2to1_2";
                b.winnable = false;
                b.winAmount = 0;
                bets.Insert(bets.Count, b);
                cashAmount -= (int)betAmount.Value;
                changeCashAmountLabel();
            }
        }

        private void TwoToOneThird_Click(object sender, EventArgs e)
        {
            if (betAmount.Value <= 0)
            {
                string text = "Bet amount must be higher then 0!";
                MessageBox.Show(text);
            }
            else if (betAmount.Value > 0 && cashAmount < betAmount.Value)
            {
                string text = "You cannot more cash than you have!";
                MessageBox.Show(text);
            }
            else
            {
                Bet b = new Bet();
                b.betAmount = (int)betAmount.Value;
                b.betType = "2to1_3";
                b.winnable = false;
                b.winAmount = 0;
                bets.Insert(bets.Count, b);
                //createBet
                cashAmount -= (int)betAmount.Value;
                changeCashAmountLabel();
            }
        }

        private void SingleNumberBox_SelectedValueChanged(object sender, EventArgs e)
        {
            

        }

        private void SingleNumberBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
        }

        private void StartRound_Click(object sender, EventArgs e)
        {
            tc.Stop();
            ballMovement.Stop();
            createRoulette();
            roundRunning = true;
            Task showBall = new Task(ballMovement.Start);
            showBall.Start();
            Task rotateRoulette = new Task(tc.Start);
            rotateRoulette.Start();
        }
    }


    public class GameRound
    {
        public int winningNumber { get; set; }
        public String winningColor { get; set; }
        public List<Bet> bets { get; set; }
    }

    public class Bet
    {
        public int betAmount { get; set; }
        public String betType { get; set; } // firstDozen,secondDozen,thirdDozen, 1to18, 19to35, even,odd,singleNumber, black, red, 2to1_1, 2to1_2,2to1_3
        public int singleNumber { get; set; }

        public int winAmount { get; set; }

        public bool winnable { get; set; }
    }


    public class RouletteBall
    {
        private Point rouletteCenter;
        private Double currentAngle;
        private Double speed;
        private Double rotationStep;
        private Double ballWidth;
        private Double ballHeight;
        private Double rouletteOuterDiameter;
        private Double offsetFromRoulette = 30;
        private Color ballColor;

        public Double radius;

        //private Graphics ballG;
        //private GraphicsPath bGP;
        private Bitmap ballImg;


        

        //
        public Double positionX = 0;
        public Double positionY = 0;

        public RouletteBall(Point rouCnt, Double startAng, Double spd, Double rotStep, Double ballW, Double ballH, Double roulOutDiam, Color ballClr)
        {
            this.rouletteCenter = rouCnt;
            this.currentAngle = startAng;
            this.speed = spd;
            this.rotationStep = rotStep;
            this.ballWidth = ballW;
            this.ballHeight = ballH;
            this.rouletteOuterDiameter = roulOutDiam;
            this.radius = (roulOutDiam/2);
            this.ballColor = ballClr;
            //this.bGP = new GraphicsPath();
            this.ballImg = new Bitmap((int)this.ballWidth, (int) this.ballHeight);

            this.createBall();
        }

        public void createBall()
        {
            Graphics ballG = Graphics.FromImage(this.ballImg);
            Pen blackB = new Pen(new SolidBrush(Color.Black));
            this.positionX = UtilityHelpers.GetXFromAngleAndRadius(this.radius, this.currentAngle) + this.rouletteCenter.X;
            this.positionY = UtilityHelpers.GetYFromAngleAndRadius(this.radius, this.currentAngle) + this.rouletteCenter.Y;

            Rectangle rc = new Rectangle((int)0, (int)0, (int)this.ballWidth, (int)this.ballHeight);
            ballG.FillEllipse(new SolidBrush(Color.Bisque),rc);

            //ballG.DrawImage(this.ballImg,(float) this.positionX, (float)this.positionY); //ker moramo v picturebox vedno na (0,0) risati

            ballG.DrawImage(this.ballImg, 0, 0);
            blackB.Dispose();
            ballG.Dispose();
        }

        public Bitmap getBallImg()
        {
            return this.ballImg;
        }
        public void setBallImage(Bitmap bb)
        {
            this.ballImg = bb;
        }

        public Double getBallCurrentAngle()
        {
            return this.currentAngle;
        }

        public void updateBallPosition(Double angle, Boolean stillSpinning)
        {

            this.currentAngle += angle;
            this.currentAngle = this.currentAngle % 360;
            this.positionX = UtilityHelpers.GetXFromAngleAndRadius(this.radius, this.currentAngle) + this.rouletteCenter.X ;
            this.positionY = UtilityHelpers.GetYFromAngleAndRadius(this.radius, this.currentAngle) + this.rouletteCenter.Y;



            //pretvori novi kot v pozicijo x in y;

        }
    }



    public class RouletteSlot
    {
        private Double startAngle;
        private Double endAngle;
        private GraphicsPath gpData;
        private Point rouletteCenter;
        private Point slotCenter;
        private Double rouletteInnerDiameter;
        private Double rouletteOuterDiameter;
        private Color slotColor;
        private GraphicsPath textData;
        private String slotNumber;


        public RouletteSlot(double start, double end, Point rouletteCenterPoint, Double rouletteInnerDiameterSize, Double rouletteOuterDiameterSize, Color slClr, String slotIndx)
        {
            this.startAngle = start;
            this.endAngle = start+end;
            this.gpData = new GraphicsPath();
            this.rouletteCenter = rouletteCenterPoint;
            this.slotCenter = new Point();
            this.rouletteInnerDiameter = rouletteInnerDiameterSize;
            this.rouletteOuterDiameter = rouletteOuterDiameterSize;
            this.slotColor = slClr;
            this.textData = new GraphicsPath();
            this.slotNumber = slotIndx;

            this.createRouletteSlot(start, rouletteCenterPoint, rouletteInnerDiameterSize, rouletteOuterDiameterSize, slotIndx);

        }

        public Double getStartAngle()
        {
            return this.startAngle;
        }

        public String getSlotNumber()
        {
            return this.slotNumber;
        }

        public Double getEndAngle()
        {
            return this.endAngle;
        }

        public void setStartAngle(Double newAngle)
        {
            this.startAngle= newAngle;
        }

        public void setEndAngle(Double newAngle)
        {
            this.endAngle = newAngle;
        }

        public Color getSlotColor()
        {
            return this.slotColor;
        }

        public void setSlotColor(Color newColor)
        {
            this.slotColor = newColor;
        }

        public GraphicsPath getTextData()
        {
            return this.textData;
        }

        public void setRoulleteSlotGraphics(GraphicsPath gp)
        {
            this.gpData = (GraphicsPath)gp.Clone();
        }

        public void setRouletteSlotText(GraphicsPath txt)
        {
            this.textData = txt;
        }

        public GraphicsPath getRoulleteSlotGraphics()
        {
            return this.gpData;
        }


        public void createRouletteSlot(double startingAngle, Point centerPoint, Double innerDiameter, Double outerDiameter, String slotIndex)//, RouletteContainer rc)
        {

            int numberOfNumbers = 36;//37
            Double angleOfEachPart = UtilityHelpers.RoundTrip((float)360 / numberOfNumbers);

            Double eachPointAngle = UtilityHelpers.RoundTrip(angleOfEachPart / 3);
            Double x = 0;
            Double y = 0;

            /*
             Podamo startingAngle (ker bomo dinamicno klicali vec zaporedoma
             Podamo angleOfEachPart, ker nam pomaga, da postavimo skrajno desno mejo vsakega slota
             Podamo -90 ker koordinatni sistem zacenja po pravilih matematike .. torej 0 stopinj je skrajno desno. Za 
             lazjo predstavitev kroga sem raje prestavil zacetek v skrajni center (gor).
             */
            Double startAngle = startingAngle + angleOfEachPart - 90;
            Point[] points = new Point[4];
            Point[] rightSide = new Point[2];
            Point[] leftSide = new Point[2];

            // RouletteContainer rc;
            for (int i = 0; i < 4; i++)
            {
                Double correctAngle = UtilityHelpers.RoundTrip(Math.PI * startAngle / 180);//pretvori v stopinje ker uporablja funkcija radiane...
                x = UtilityHelpers.GetXPointFromAngleAndOrigin(centerPoint.X, correctAngle, innerDiameter / 2);
                y = UtilityHelpers.GetYPointFromAngleAndOrigin(centerPoint.Y, correctAngle, innerDiameter / 2);
                Point p1 = new Point((int)Math.Floor(x), (int)Math.Floor(y));
                points[i] = p1;
                startAngle = (startAngle - eachPointAngle);//mogoce bo potrebno dodati UtilityHelpers.RoundTrip(..);
            }

            startAngle = startingAngle - 90;
            leftSide[0] = new Point((int)UtilityHelpers.GetXPointFromAngleAndOrigin(centerPoint.X, UtilityHelpers.GetAngleRadian(startAngle), outerDiameter / 2), (int)UtilityHelpers.GetYPointFromAngleAndOrigin(centerPoint.Y, UtilityHelpers.GetAngleRadian(startAngle), outerDiameter / 2));
            leftSide[1] = new Point((int)UtilityHelpers.GetXPointFromAngleAndOrigin(centerPoint.X, UtilityHelpers.GetAngleRadian(startAngle), innerDiameter / 2), (int)UtilityHelpers.GetYPointFromAngleAndOrigin(centerPoint.Y, UtilityHelpers.GetAngleRadian(startAngle), innerDiameter / 2));

            Point[] ps = new Point[4];
            x = 0;
            y = 0;
            startAngle = startingAngle - 90;
            for (int i = 0; i < 4; i++)
            {
                Double correctAngle = Math.PI * startAngle / 180;//pretvori v stopinje ker uporablja funkcija radiane...
                x = UtilityHelpers.GetXPointFromAngleAndOrigin(centerPoint.X, correctAngle, outerDiameter / 2);
                y = UtilityHelpers.GetYPointFromAngleAndOrigin(centerPoint.Y, correctAngle, outerDiameter / 2);
                Point p1 = new Point((int)x, (int)y);
                ps[i] = p1;
                startAngle += eachPointAngle;
            }

            startAngle -= eachPointAngle;
            rightSide[0] = new Point((int)UtilityHelpers.GetXPointFromAngleAndOrigin(centerPoint.X, UtilityHelpers.GetAngleRadian(startAngle), innerDiameter / 2), (int)UtilityHelpers.GetYPointFromAngleAndOrigin(centerPoint.Y, UtilityHelpers.GetAngleRadian(startAngle), innerDiameter / 2));
            rightSide[1] = new Point((int)UtilityHelpers.GetXPointFromAngleAndOrigin(centerPoint.X, UtilityHelpers.GetAngleRadian(startAngle), outerDiameter / 2), (int)UtilityHelpers.GetYPointFromAngleAndOrigin(centerPoint.Y, UtilityHelpers.GetAngleRadian(startAngle), outerDiameter / 2));
            float tension = 100.0F;

            // Create a GraphicsPath object and add a curve.
            GraphicsPath myPath = new GraphicsPath();
            GraphicsPath m2 = new GraphicsPath();
            myPath.AddCurve(points, 0.9f);
            //
            myPath.AddLine(leftSide[1], leftSide[0]);
            myPath.AddCurve(ps, .9f);
            myPath.AddLine(rightSide[1], rightSide[0]);
            RectangleF bouds = myPath.GetBounds();

            Matrix rotatep = new Matrix();
            float slotCenterX = bouds.X + (bouds.Width / 2);
            float slotCenterY = bouds.Y + (bouds.Height / 2);


            string stringText = slotIndex;
            FontFamily family = new FontFamily("Times New Roman");
            int fontStyle = (int)FontStyle.Italic;

            int emSize = 10;
            Point origin = new Point((int)slotCenterX, (int)slotCenterY);
            StringFormat format = StringFormat.GenericDefault;

            // Add the string to the path.
            this.textData.AddString(stringText,
                family,
                fontStyle,
                emSize,
                origin,
                format);
            //rotatep.RotateAt(10,new Point((int)v,(int)vy));
            //rotatep.RotateAt(-90, new Point(centerX, centerY));
            this.gpData = myPath;
        }
    }
    public class RouletteContainer
    {
        private Double rouletteInnerDiameter;
        private Double rouletteOuterDiameter;
        private RouletteSlot[] slots;
        private int NUMBER_OF_SLOTS = 36;//37
        private Double rouletteRotationAngle;
        private Boolean allSlotsDrawn;
        private Point centerPoint;

        public RouletteContainer()
        {
            this.rouletteInnerDiameter = 30;
            this.rouletteOuterDiameter = 50;
            this.slots = new RouletteSlot[this.NUMBER_OF_SLOTS];
            this.allSlotsDrawn = false;
            this.centerPoint = new Point(0, 0);
        }
        public RouletteContainer(Double innerDiameter, Double outerDiameter, Double startingAngle)
        {
            this.rouletteInnerDiameter = innerDiameter;
            this.rouletteOuterDiameter = outerDiameter;
            this.slots = new RouletteSlot[this.NUMBER_OF_SLOTS];
            this.rouletteRotationAngle = startingAngle;
        }
        public RouletteContainer(Double innerDiameter, Double outerDiameter, Double startingAngle, Point cntPt)
        {
            this.rouletteInnerDiameter = innerDiameter;
            this.rouletteOuterDiameter = outerDiameter;
            this.slots = new RouletteSlot[this.NUMBER_OF_SLOTS];
            this.rouletteRotationAngle = startingAngle;
            this.centerPoint = cntPt;
        }


        public void updateSlots(Double rotateForAngle)
        {

            this.allSlotsDrawn = false;
            Matrix tr = new Matrix();
            tr.RotateAt((float)rotateForAngle, this.centerPoint);
            for (int i = 0; i<this.slots.Length; i++)
            {
                RouletteSlot s = slots[i];
                GraphicsPath slot = s.getRoulleteSlotGraphics();
                GraphicsPath txt = s.getTextData();
                s.setStartAngle((s.getStartAngle() + rotateForAngle) % 360);
                s.setEndAngle((s.getEndAngle() + rotateForAngle) % 360);
                slot.Transform(tr);
                txt.Transform(tr);
                s.setRoulleteSlotGraphics(slot);
                s.setRouletteSlotText(txt);
                slots[i] = s;
            }
            this.allSlotsDrawn = true;
        }

        public void setAllSlotsDrawn(Boolean newValue)
        {
            this.allSlotsDrawn = newValue;
        }

        public Boolean getAllSlotsDrawn()
        {
            return this.allSlotsDrawn;
        }

        public Point getCenterPoint()
        {
            return this.centerPoint;
        }


        public Double getRotationAngle()
        {
            return this.rouletteRotationAngle;
        }
        public void setRotationAngle(Double newAngle)
        {
            this.rouletteRotationAngle = newAngle % 360;
        }


        public void createRouletteSlots()
        {
            this.allSlotsDrawn = false;
            Double startingPointAngle = this.rouletteRotationAngle;
            Double slotSizeAngle = UtilityHelpers.RoundTrip(360 / this.NUMBER_OF_SLOTS);
            //int[] numbers = new int[] { 0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 36, 11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31, 9, 22, 18, 29, 7, 28, 12, 35, 3, 26 };
            int[] numbers = new int[] { 0, 32, 15, 19, 4, 21, 2, 25, 17, 34, 6, 27, 13, 11, 30, 8, 23, 10, 5, 24, 16, 33, 1, 20, 14, 31, 9, 22, 18, 29, 7, 28, 12, 35, 3, 26 };
            for (int i = 0; i < this.NUMBER_OF_SLOTS; i++)
            {
                //new Point(350, 150)
                RouletteSlot slot = new RouletteSlot(startingPointAngle, slotSizeAngle, this.getCenterPoint(), this.rouletteInnerDiameter, this.rouletteOuterDiameter, Color.Black, numbers[i].ToString());
                SolidBrush red = new SolidBrush(Color.Red);
                SolidBrush black = new SolidBrush(Color.Black);

                GraphicsPath slotData = slot.getRoulleteSlotGraphics();

                if (i % 2 == 0 && i != 0)
                {
                    slot.setSlotColor(Color.Red);
                }else if(i == 0)
                {
                    slot.setSlotColor(Color.Green);
                }
                else
                {
                    slot.setSlotColor(Color.Black);
                    //e.Graphics.FillPath(black, slotData);
                }
                slots[i] = slot;
                /*
                e.Graphics.DrawPath(myPen, slotData);
                slotStart += angleOfEachPart;*/
                startingPointAngle += slotSizeAngle;
                startingPointAngle = startingPointAngle % 360;
            }
            this.allSlotsDrawn = true;
        }

        public Double getNumberOfSlots()
        {
            return this.NUMBER_OF_SLOTS;
        }

        public RouletteSlot[] getSlots()
        {
            return this.slots;
        }





        public Double getRouletteInnerDiameter()
        {
            return this.rouletteInnerDiameter;
        }

        public Double getRouletteOuterDiameter()
        {
            return this.rouletteOuterDiameter;
        }



    }

    public class UtilityHelpers
    {
        public static double RoundTrip(double d)
        {
            return Double.Parse(d.ToString("R"));
        }

        public static Double GetXPointFromAngleAndOrigin(Double centerX, Double angle, Double rouletteRadius)
        {
            //x(t) = r cos(t) + j
            return (rouletteRadius * Math.Cos(angle)) + centerX;
        }

        public static Double GetAngleRadian(Double angle)
        {
            return Math.PI * angle / 180;
        }

        public static Double GetYPointFromAngleAndOrigin(Double centerY, Double angle, Double rouletteRadius)
        {
            //y(t) = r sin(t) + k
            return (rouletteRadius * Math.Sin(angle)) + centerY;
        }

        public static Double GetXFromAngleAndRadius(Double radius, Double angle)
        {
            //return x = r * cos(angle_in_radians)
            return UtilityHelpers.RoundTrip( radius * UtilityHelpers.RoundTrip(Math.Cos(UtilityHelpers.GetAngleRadian(angle))));
        }
        public static Double GetYFromAngleAndRadius(Double radius, Double angle)
        {
            //return x = r * cos(angle_in_radians)
            return UtilityHelpers.RoundTrip(radius * UtilityHelpers.RoundTrip(Math.Sin(UtilityHelpers.GetAngleRadian(angle))));
        }






        public static Double GetCircleCircumference(Double diameter)
        {
            return diameter * Math.PI;
        }
    }
}
