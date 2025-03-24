namespace MiFare2ActiveDirectory
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LBLMiFareNumber = new Label();
            TBSvcUsername = new TextBox();
            TBSvcPassword = new TextBox();
            BTNUpdateSvcAccount = new Button();
            CBCardReaders = new ComboBox();
            LBLSvcUsername = new Label();
            LBLSvcPassword = new Label();
            LBLCardReaderSelection = new Label();
            panel1 = new Panel();
            LBLUserToUpdate = new Label();
            BTNWriteToAd = new Button();
            CBAvailableOUs = new ComboBox();
            CBADUsers = new ComboBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // LBLMiFareNumber
            // 
            LBLMiFareNumber.AutoSize = true;
            LBLMiFareNumber.Location = new Point(38, 224);
            LBLMiFareNumber.Name = "LBLMiFareNumber";
            LBLMiFareNumber.Size = new Size(147, 15);
            LBLMiFareNumber.TabIndex = 4;
            LBLMiFareNumber.Text = "No card has been read yet.";
            // 
            // TBSvcUsername
            // 
            TBSvcUsername.Location = new Point(176, 10);
            TBSvcUsername.Name = "TBSvcUsername";
            TBSvcUsername.Size = new Size(196, 23);
            TBSvcUsername.TabIndex = 0;
            // 
            // TBSvcPassword
            // 
            TBSvcPassword.Location = new Point(176, 39);
            TBSvcPassword.Name = "TBSvcPassword";
            TBSvcPassword.PasswordChar = '*';
            TBSvcPassword.Size = new Size(196, 23);
            TBSvcPassword.TabIndex = 1;
            // 
            // BTNUpdateSvcAccount
            // 
            BTNUpdateSvcAccount.Location = new Point(176, 106);
            BTNUpdateSvcAccount.Name = "BTNUpdateSvcAccount";
            BTNUpdateSvcAccount.Size = new Size(196, 23);
            BTNUpdateSvcAccount.TabIndex = 2;
            BTNUpdateSvcAccount.Text = "Update Settings";
            BTNUpdateSvcAccount.UseVisualStyleBackColor = true;
            BTNUpdateSvcAccount.Click += Btn_UpdateSvcAccount_Click;
            // 
            // CBCardReaders
            // 
            CBCardReaders.FormattingEnabled = true;
            CBCardReaders.Location = new Point(176, 68);
            CBCardReaders.Name = "CBCardReaders";
            CBCardReaders.Size = new Size(196, 23);
            CBCardReaders.TabIndex = 6;
            // 
            // LBLSvcUsername
            // 
            LBLSvcUsername.AutoSize = true;
            LBLSvcUsername.Location = new Point(17, 10);
            LBLSvcUsername.Name = "LBLSvcUsername";
            LBLSvcUsername.Size = new Size(148, 15);
            LBLSvcUsername.TabIndex = 7;
            LBLSvcUsername.Text = "Service Account Username";
            // 
            // LBLSvcPassword
            // 
            LBLSvcPassword.AutoSize = true;
            LBLSvcPassword.Location = new Point(20, 39);
            LBLSvcPassword.Name = "LBLSvcPassword";
            LBLSvcPassword.Size = new Size(145, 15);
            LBLSvcPassword.TabIndex = 8;
            LBLSvcPassword.Text = "Service Account Password";
            // 
            // LBLCardReaderSelection
            // 
            LBLCardReaderSelection.AutoSize = true;
            LBLCardReaderSelection.Location = new Point(99, 68);
            LBLCardReaderSelection.Name = "LBLCardReaderSelection";
            LBLCardReaderSelection.Size = new Size(71, 15);
            LBLCardReaderSelection.TabIndex = 9;
            LBLCardReaderSelection.Text = "Card Reader";
            // 
            // panel1
            // 
            panel1.Controls.Add(LBLCardReaderSelection);
            panel1.Controls.Add(LBLSvcPassword);
            panel1.Controls.Add(LBLSvcUsername);
            panel1.Controls.Add(CBCardReaders);
            panel1.Controls.Add(BTNUpdateSvcAccount);
            panel1.Controls.Add(TBSvcPassword);
            panel1.Controls.Add(TBSvcUsername);
            panel1.Location = new Point(405, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(383, 134);
            panel1.TabIndex = 3;
            // 
            // LBLUserToUpdate
            // 
            LBLUserToUpdate.AutoSize = true;
            LBLUserToUpdate.Location = new Point(38, 264);
            LBLUserToUpdate.Name = "LBLUserToUpdate";
            LBLUserToUpdate.Size = new Size(224, 15);
            LBLUserToUpdate.TabIndex = 6;
            LBLUserToUpdate.Text = "Select username to write card number to:";
            // 
            // BTNWriteToAd
            // 
            BTNWriteToAd.Location = new Point(331, 301);
            BTNWriteToAd.Name = "BTNWriteToAd";
            BTNWriteToAd.Size = new Size(158, 23);
            BTNWriteToAd.TabIndex = 7;
            BTNWriteToAd.Text = "Update User in AD";
            BTNWriteToAd.UseVisualStyleBackColor = true;
            BTNWriteToAd.Click += BTNWriteToAd_Click;
            // 
            // CBAvailableOUs
            // 
            CBAvailableOUs.FormattingEnabled = true;
            CBAvailableOUs.Location = new Point(38, 176);
            CBAvailableOUs.Name = "CBAvailableOUs";
            CBAvailableOUs.Size = new Size(750, 23);
            CBAvailableOUs.TabIndex = 8;
            CBAvailableOUs.SelectedIndexChanged += CBAvailableOUs_SelectedIndexChanged;
            // 
            // CBADUsers
            // 
            CBADUsers.FormattingEnabled = true;
            CBADUsers.Location = new Point(268, 261);
            CBADUsers.Name = "CBADUsers";
            CBADUsers.Size = new Size(340, 23);
            CBADUsers.TabIndex = 9;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(CBADUsers);
            Controls.Add(CBAvailableOUs);
            Controls.Add(BTNWriteToAd);
            Controls.Add(LBLUserToUpdate);
            Controls.Add(LBLMiFareNumber);
            Controls.Add(panel1);
            Name = "MainForm";
            Text = "ALT MiFare to Active Directory";
            Load += MainForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label LBLMiFareNumber;
        private TextBox TBSvcUsername;
        private TextBox TBSvcPassword;
        private Button BTNUpdateSvcAccount;
        private ComboBox CBCardReaders;
        private Label LBLSvcUsername;
        private Label LBLSvcPassword;
        private Label LBLCardReaderSelection;
        private Panel panel1;
        private Label LBLUserToUpdate;
        private Button BTNWriteToAd;
        private ComboBox CBAvailableOUs;
        private ComboBox CBADUsers;
    }
}
