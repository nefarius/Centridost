namespace NetSupport
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelEnvStatus = new System.Windows.Forms.Label();
            this.buttonSwitchOn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelWinPwSet = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelTSSock = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelTSServ = new System.Windows.Forms.Label();
            this.labelSupSrvAvail = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelRConAllowed = new System.Windows.Forms.Label();
            this.groupBoxSshAccount = new System.Windows.Forms.GroupBox();
            this.labelConnectionStat = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.checkBoxConnect = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.backgroundWorkerFix = new System.ComponentModel.BackgroundWorker();
            this.groupBoxWinAccount = new System.Windows.Forms.GroupBox();
            this.labelWinUserName = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxWinPasswd = new System.Windows.Forms.TextBox();
            this.buttonSenWinPwd = new System.Windows.Forms.Button();
            this.backgroundWorkerSetWinPw = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxSshAccount.SuspendLayout();
            this.groupBoxWinAccount.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelEnvStatus);
            this.groupBox1.Controls.Add(this.buttonSwitchOn);
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // labelEnvStatus
            // 
            resources.ApplyResources(this.labelEnvStatus, "labelEnvStatus");
            this.labelEnvStatus.Name = "labelEnvStatus";
            // 
            // buttonSwitchOn
            // 
            resources.ApplyResources(this.buttonSwitchOn, "buttonSwitchOn");
            this.buttonSwitchOn.Name = "buttonSwitchOn";
            this.buttonSwitchOn.UseVisualStyleBackColor = true;
            this.buttonSwitchOn.Click += new System.EventHandler(this.buttonSwitchOn_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.labelWinPwSet, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelTSSock, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelTSServ, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelSupSrvAvail, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label8, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelRConAllowed, 1, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // labelWinPwSet
            // 
            resources.ApplyResources(this.labelWinPwSet, "labelWinPwSet");
            this.labelWinPwSet.Name = "labelWinPwSet";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // labelTSSock
            // 
            resources.ApplyResources(this.labelTSSock, "labelTSSock");
            this.labelTSSock.Name = "labelTSSock";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // labelTSServ
            // 
            resources.ApplyResources(this.labelTSServ, "labelTSServ");
            this.labelTSServ.Name = "labelTSServ";
            // 
            // labelSupSrvAvail
            // 
            resources.ApplyResources(this.labelSupSrvAvail, "labelSupSrvAvail");
            this.labelSupSrvAvail.Name = "labelSupSrvAvail";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // labelRConAllowed
            // 
            resources.ApplyResources(this.labelRConAllowed, "labelRConAllowed");
            this.labelRConAllowed.Name = "labelRConAllowed";
            // 
            // groupBoxSshAccount
            // 
            this.groupBoxSshAccount.Controls.Add(this.labelConnectionStat);
            this.groupBoxSshAccount.Controls.Add(this.label12);
            this.groupBoxSshAccount.Controls.Add(this.checkBoxConnect);
            this.groupBoxSshAccount.Controls.Add(this.label4);
            this.groupBoxSshAccount.Controls.Add(this.label3);
            this.groupBoxSshAccount.Controls.Add(this.textBoxPassword);
            this.groupBoxSshAccount.Controls.Add(this.textBoxUsername);
            resources.ApplyResources(this.groupBoxSshAccount, "groupBoxSshAccount");
            this.groupBoxSshAccount.Name = "groupBoxSshAccount";
            this.groupBoxSshAccount.TabStop = false;
            // 
            // labelConnectionStat
            // 
            this.labelConnectionStat.ForeColor = System.Drawing.Color.Blue;
            resources.ApplyResources(this.labelConnectionStat, "labelConnectionStat");
            this.labelConnectionStat.Name = "labelConnectionStat";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // checkBoxConnect
            // 
            resources.ApplyResources(this.checkBoxConnect, "checkBoxConnect");
            this.checkBoxConnect.Name = "checkBoxConnect";
            this.checkBoxConnect.UseVisualStyleBackColor = true;
            this.checkBoxConnect.CheckedChanged += new System.EventHandler(this.checkBoxConnect_CheckedChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // textBoxPassword
            // 
            resources.ApplyResources(this.textBoxPassword, "textBoxPassword");
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Enter += new System.EventHandler(this.textBoxPassword_Enter);
            // 
            // textBoxUsername
            // 
            resources.ApplyResources(this.textBoxUsername, "textBoxUsername");
            this.textBoxUsername.Name = "textBoxUsername";
            // 
            // backgroundWorkerFix
            // 
            this.backgroundWorkerFix.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerFix_DoWork);
            this.backgroundWorkerFix.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerFix_RunWorkerCompleted);
            // 
            // groupBoxWinAccount
            // 
            this.groupBoxWinAccount.Controls.Add(this.labelWinUserName);
            this.groupBoxWinAccount.Controls.Add(this.label10);
            this.groupBoxWinAccount.Controls.Add(this.label6);
            this.groupBoxWinAccount.Controls.Add(this.textBoxWinPasswd);
            this.groupBoxWinAccount.Controls.Add(this.buttonSenWinPwd);
            resources.ApplyResources(this.groupBoxWinAccount, "groupBoxWinAccount");
            this.groupBoxWinAccount.Name = "groupBoxWinAccount";
            this.groupBoxWinAccount.TabStop = false;
            // 
            // labelWinUserName
            // 
            resources.ApplyResources(this.labelWinUserName, "labelWinUserName");
            this.labelWinUserName.Name = "labelWinUserName";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // textBoxWinPasswd
            // 
            resources.ApplyResources(this.textBoxWinPasswd, "textBoxWinPasswd");
            this.textBoxWinPasswd.Name = "textBoxWinPasswd";
            // 
            // buttonSenWinPwd
            // 
            resources.ApplyResources(this.buttonSenWinPwd, "buttonSenWinPwd");
            this.buttonSenWinPwd.Name = "buttonSenWinPwd";
            this.buttonSenWinPwd.UseVisualStyleBackColor = true;
            this.buttonSenWinPwd.Click += new System.EventHandler(this.buttonSenWinPwd_Click);
            // 
            // backgroundWorkerSetWinPw
            // 
            this.backgroundWorkerSetWinPw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerSetWinPw_DoWork);
            this.backgroundWorkerSetWinPw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerSetWinPw_RunWorkerCompleted);
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxWinAccount);
            this.Controls.Add(this.groupBoxSshAccount);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyUp);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBoxSshAccount.ResumeLayout(false);
            this.groupBoxSshAccount.PerformLayout();
            this.groupBoxWinAccount.ResumeLayout(false);
            this.groupBoxWinAccount.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelTSSock;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelTSServ;
        private System.Windows.Forms.Label labelSupSrvAvail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelRConAllowed;
        private System.Windows.Forms.GroupBox groupBoxSshAccount;
        private System.Windows.Forms.Button buttonSwitchOn;
        private System.ComponentModel.BackgroundWorker backgroundWorkerFix;
        private System.Windows.Forms.Label labelEnvStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.CheckBox checkBoxConnect;
        private System.Windows.Forms.GroupBox groupBoxWinAccount;
        private System.Windows.Forms.TextBox textBoxWinPasswd;
        private System.Windows.Forms.Button buttonSenWinPwd;
        private System.Windows.Forms.Label labelWinUserName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelWinPwSet;
        private System.Windows.Forms.Label labelConnectionStat;
        private System.Windows.Forms.Label label12;
        private System.ComponentModel.BackgroundWorker backgroundWorkerSetWinPw;
    }
}

