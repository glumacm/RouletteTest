namespace TestOffScreenBuffering
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.firstDozenButton = new System.Windows.Forms.Button();
            this.secondDozen = new System.Windows.Forms.Button();
            this.thirdDozen = new System.Windows.Forms.Button();
            this.oneThroughEighteen = new System.Windows.Forms.Button();
            this.even = new System.Windows.Forms.Button();
            this.odd = new System.Windows.Forms.Button();
            this.nineteenThroughtThirty = new System.Windows.Forms.Button();
            this.blackNumbers = new System.Windows.Forms.Button();
            this.redNumbers = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.singleNumberBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.WinningNumberValue = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.betAmount = new System.Windows.Forms.NumericUpDown();
            this.betSingleNumberButton = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.wallet = new System.Windows.Forms.Label();
            this.twoToOneFirst = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.twoToOneSecond = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.twoToOneThird = new System.Windows.Forms.Button();
            this.startRound = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.betAmount)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.Location = new System.Drawing.Point(12, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(10, 10);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox1_Paint);
            // 
            // firstDozenButton
            // 
            this.firstDozenButton.Location = new System.Drawing.Point(602, 135);
            this.firstDozenButton.Name = "firstDozenButton";
            this.firstDozenButton.Size = new System.Drawing.Size(75, 23);
            this.firstDozenButton.TabIndex = 3;
            this.firstDozenButton.Text = "1st dozen";
            this.firstDozenButton.UseVisualStyleBackColor = true;
            this.firstDozenButton.Click += new System.EventHandler(this.Button1_Click);
            // 
            // secondDozen
            // 
            this.secondDozen.Location = new System.Drawing.Point(602, 164);
            this.secondDozen.Name = "secondDozen";
            this.secondDozen.Size = new System.Drawing.Size(75, 23);
            this.secondDozen.TabIndex = 4;
            this.secondDozen.Text = "2nd dozen";
            this.secondDozen.UseVisualStyleBackColor = true;
            this.secondDozen.Click += new System.EventHandler(this.SecondDozen_Click);
            // 
            // thirdDozen
            // 
            this.thirdDozen.Location = new System.Drawing.Point(602, 193);
            this.thirdDozen.Name = "thirdDozen";
            this.thirdDozen.Size = new System.Drawing.Size(75, 23);
            this.thirdDozen.TabIndex = 5;
            this.thirdDozen.Text = "3rd dozen";
            this.thirdDozen.UseVisualStyleBackColor = true;
            this.thirdDozen.Click += new System.EventHandler(this.ThirdDozen_Click);
            // 
            // oneThroughEighteen
            // 
            this.oneThroughEighteen.Location = new System.Drawing.Point(520, 134);
            this.oneThroughEighteen.Name = "oneThroughEighteen";
            this.oneThroughEighteen.Size = new System.Drawing.Size(75, 23);
            this.oneThroughEighteen.TabIndex = 6;
            this.oneThroughEighteen.Text = "1-18";
            this.oneThroughEighteen.UseVisualStyleBackColor = true;
            this.oneThroughEighteen.Click += new System.EventHandler(this.OneThroughEighteen_Click);
            // 
            // even
            // 
            this.even.Location = new System.Drawing.Point(520, 164);
            this.even.Name = "even";
            this.even.Size = new System.Drawing.Size(75, 23);
            this.even.TabIndex = 7;
            this.even.Text = "Even";
            this.even.UseVisualStyleBackColor = true;
            this.even.Click += new System.EventHandler(this.Even_Click);
            // 
            // odd
            // 
            this.odd.Location = new System.Drawing.Point(520, 192);
            this.odd.Name = "odd";
            this.odd.Size = new System.Drawing.Size(75, 23);
            this.odd.TabIndex = 8;
            this.odd.Text = "Odd";
            this.odd.UseVisualStyleBackColor = true;
            this.odd.Click += new System.EventHandler(this.Odd_Click);
            // 
            // nineteenThroughtThirty
            // 
            this.nineteenThroughtThirty.Location = new System.Drawing.Point(437, 135);
            this.nineteenThroughtThirty.Name = "nineteenThroughtThirty";
            this.nineteenThroughtThirty.Size = new System.Drawing.Size(75, 23);
            this.nineteenThroughtThirty.TabIndex = 9;
            this.nineteenThroughtThirty.Text = "19-35";
            this.nineteenThroughtThirty.UseVisualStyleBackColor = true;
            this.nineteenThroughtThirty.Click += new System.EventHandler(this.NineteenThroughtThirty_Click);
            // 
            // blackNumbers
            // 
            this.blackNumbers.Location = new System.Drawing.Point(437, 165);
            this.blackNumbers.Name = "blackNumbers";
            this.blackNumbers.Size = new System.Drawing.Size(75, 23);
            this.blackNumbers.TabIndex = 10;
            this.blackNumbers.Text = "Black";
            this.blackNumbers.UseVisualStyleBackColor = true;
            this.blackNumbers.Click += new System.EventHandler(this.BlackNumbers_Click);
            // 
            // redNumbers
            // 
            this.redNumbers.Location = new System.Drawing.Point(437, 192);
            this.redNumbers.Name = "redNumbers";
            this.redNumbers.Size = new System.Drawing.Size(75, 23);
            this.redNumbers.TabIndex = 11;
            this.redNumbers.Text = "Red";
            this.redNumbers.UseVisualStyleBackColor = true;
            this.redNumbers.Click += new System.EventHandler(this.RedNumbers_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(434, 333);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 22);
            this.label1.TabIndex = 12;
            this.label1.Text = "SIngle numbers";
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(435, 372);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Number";
            this.label3.Click += new System.EventHandler(this.Label3_Click);
            // 
            // singleNumberBox
            // 
            this.singleNumberBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.singleNumberBox.FormattingEnabled = true;
            this.singleNumberBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35"});
            this.singleNumberBox.Location = new System.Drawing.Point(491, 372);
            this.singleNumberBox.Name = "singleNumberBox";
            this.singleNumberBox.Size = new System.Drawing.Size(121, 21);
            this.singleNumberBox.TabIndex = 18;
            this.singleNumberBox.SelectedIndexChanged += new System.EventHandler(this.SingleNumberBox_SelectedIndexChanged);
            this.singleNumberBox.SelectedValueChanged += new System.EventHandler(this.SingleNumberBox_SelectedValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(441, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(205, 29);
            this.label4.TabIndex = 19;
            this.label4.Text = "Winning number:";
            // 
            // WinningNumberValue
            // 
            this.WinningNumberValue.AutoSize = true;
            this.WinningNumberValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 18.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WinningNumberValue.Location = new System.Drawing.Point(677, 56);
            this.WinningNumberValue.Name = "WinningNumberValue";
            this.WinningNumberValue.Size = new System.Drawing.Size(0, 29);
            this.WinningNumberValue.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(443, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Bet Amount:";
            // 
            // betAmount
            // 
            this.betAmount.Location = new System.Drawing.Point(517, 99);
            this.betAmount.Name = "betAmount";
            this.betAmount.Size = new System.Drawing.Size(120, 20);
            this.betAmount.TabIndex = 22;
            // 
            // betSingleNumberButton
            // 
            this.betSingleNumberButton.Location = new System.Drawing.Point(438, 406);
            this.betSingleNumberButton.Name = "betSingleNumberButton";
            this.betSingleNumberButton.Size = new System.Drawing.Size(174, 23);
            this.betSingleNumberButton.TabIndex = 23;
            this.betSingleNumberButton.Text = "Bet Single Number";
            this.betSingleNumberButton.UseVisualStyleBackColor = true;
            this.betSingleNumberButton.Click += new System.EventHandler(this.BetSingleNumberButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(443, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 18);
            this.label2.TabIndex = 24;
            this.label2.Text = "Cash in a wallet:";
            // 
            // wallet
            // 
            this.wallet.AutoSize = true;
            this.wallet.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wallet.Location = new System.Drawing.Point(564, 22);
            this.wallet.Name = "wallet";
            this.wallet.Size = new System.Drawing.Size(16, 18);
            this.wallet.TabIndex = 25;
            this.wallet.Text = "0";
            // 
            // twoToOneFirst
            // 
            this.twoToOneFirst.Location = new System.Drawing.Point(634, 232);
            this.twoToOneFirst.Name = "twoToOneFirst";
            this.twoToOneFirst.Size = new System.Drawing.Size(75, 23);
            this.twoToOneFirst.TabIndex = 26;
            this.twoToOneFirst.Text = "2 to 1";
            this.twoToOneFirst.UseVisualStyleBackColor = true;
            this.twoToOneFirst.Click += new System.EventHandler(this.TwoToOneFirst_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(442, 237);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(166, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "1,4,7,10,13,16,19,22,25,28,31,34";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(443, 267);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(166, 13);
            this.label7.TabIndex = 28;
            this.label7.Text = "2,5,8,11,14,17,20,23,26,29,32,35";
            // 
            // twoToOneSecond
            // 
            this.twoToOneSecond.Location = new System.Drawing.Point(634, 262);
            this.twoToOneSecond.Name = "twoToOneSecond";
            this.twoToOneSecond.Size = new System.Drawing.Size(75, 23);
            this.twoToOneSecond.TabIndex = 29;
            this.twoToOneSecond.Text = "2 to 1";
            this.twoToOneSecond.UseVisualStyleBackColor = true;
            this.twoToOneSecond.Click += new System.EventHandler(this.TwoToOneSecond_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(444, 301);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(151, 13);
            this.label8.TabIndex = 30;
            this.label8.Text = "3,6,9,12,15,18,21,24,27,30,33";
            // 
            // twoToOneThird
            // 
            this.twoToOneThird.Location = new System.Drawing.Point(634, 296);
            this.twoToOneThird.Name = "twoToOneThird";
            this.twoToOneThird.Size = new System.Drawing.Size(75, 23);
            this.twoToOneThird.TabIndex = 31;
            this.twoToOneThird.Text = "2 to 1";
            this.twoToOneThird.UseVisualStyleBackColor = true;
            this.twoToOneThird.Click += new System.EventHandler(this.TwoToOneThird_Click);
            // 
            // startRound
            // 
            this.startRound.Location = new System.Drawing.Point(295, 367);
            this.startRound.Name = "startRound";
            this.startRound.Size = new System.Drawing.Size(75, 23);
            this.startRound.TabIndex = 32;
            this.startRound.Text = "Start round";
            this.startRound.UseVisualStyleBackColor = true;
            this.startRound.Click += new System.EventHandler(this.StartRound_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.startRound);
            this.Controls.Add(this.twoToOneThird);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.twoToOneSecond);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.twoToOneFirst);
            this.Controls.Add(this.wallet);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.betSingleNumberButton);
            this.Controls.Add(this.betAmount);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.WinningNumberValue);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.singleNumberBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.redNumbers);
            this.Controls.Add(this.blackNumbers);
            this.Controls.Add(this.nineteenThroughtThirty);
            this.Controls.Add(this.odd);
            this.Controls.Add(this.even);
            this.Controls.Add(this.oneThroughEighteen);
            this.Controls.Add(this.thirdDozen);
            this.Controls.Add(this.secondDozen);
            this.Controls.Add(this.firstDozenButton);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.betAmount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button firstDozenButton;
        private System.Windows.Forms.Button secondDozen;
        private System.Windows.Forms.Button thirdDozen;
        private System.Windows.Forms.Button oneThroughEighteen;
        private System.Windows.Forms.Button even;
        private System.Windows.Forms.Button odd;
        private System.Windows.Forms.Button nineteenThroughtThirty;
        private System.Windows.Forms.Button blackNumbers;
        private System.Windows.Forms.Button redNumbers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox singleNumberBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label WinningNumberValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown betAmount;
        private System.Windows.Forms.Button betSingleNumberButton;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label wallet;
        private System.Windows.Forms.Button twoToOneFirst;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button twoToOneSecond;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button twoToOneThird;
        private System.Windows.Forms.Button startRound;
    }
}

