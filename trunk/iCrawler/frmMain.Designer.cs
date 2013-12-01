namespace iCrawler
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnAutoStart = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblICrawler = new System.Windows.Forms.Label();
            this.lblUrlRunning = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(897, 371);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(803, 371);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 1;
            this.btnRun.Text = "Run Crawler";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnAutoStart
            // 
            this.btnAutoStart.Location = new System.Drawing.Point(710, 371);
            this.btnAutoStart.Name = "btnAutoStart";
            this.btnAutoStart.Size = new System.Drawing.Size(75, 23);
            this.btnAutoStart.TabIndex = 2;
            this.btnAutoStart.Text = "Auto Start";
            this.btnAutoStart.UseVisualStyleBackColor = true;
            this.btnAutoStart.Click += new System.EventHandler(this.btnAutoStart_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblICrawler
            // 
            this.lblICrawler.AutoSize = true;
            this.lblICrawler.Location = new System.Drawing.Point(12, 384);
            this.lblICrawler.Name = "lblICrawler";
            this.lblICrawler.Size = new System.Drawing.Size(80, 13);
            this.lblICrawler.TabIndex = 3;
            this.lblICrawler.Text = "iCrawler 1.0.0.0";
            // 
            // lblUrlRunning
            // 
            this.lblUrlRunning.AutoSize = true;
            this.lblUrlRunning.Location = new System.Drawing.Point(12, 361);
            this.lblUrlRunning.Name = "lblUrlRunning";
            this.lblUrlRunning.Size = new System.Drawing.Size(80, 13);
            this.lblUrlRunning.TabIndex = 4;
            this.lblUrlRunning.Text = "iCrawler 1.0.0.0";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 406);
            this.Controls.Add(this.lblUrlRunning);
            this.Controls.Add(this.lblICrawler);
            this.Controls.Add(this.btnAutoStart);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.button1);
            this.Name = "frmMain";
            this.Text = "iCrawler 1.0.0.0";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnAutoStart;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblICrawler;
        private System.Windows.Forms.Label lblUrlRunning;
    }
}

