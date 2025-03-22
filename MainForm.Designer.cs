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
            Tb_svcUsername = new TextBox();
            Tb_svcPassword = new TextBox();
            panel1 = new Panel();
            Btn_UpdateSvcAccount = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // Tb_svcUsername
            // 
            Tb_svcUsername.Location = new Point(89, 15);
            Tb_svcUsername.Name = "Tb_svcUsername";
            Tb_svcUsername.Size = new Size(196, 23);
            Tb_svcUsername.TabIndex = 0;
            // 
            // Tb_svcPassword
            // 
            Tb_svcPassword.Location = new Point(89, 56);
            Tb_svcPassword.Name = "Tb_svcPassword";
            Tb_svcPassword.PasswordChar = '*';
            Tb_svcPassword.Size = new Size(196, 23);
            Tb_svcPassword.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Controls.Add(Btn_UpdateSvcAccount);
            panel1.Controls.Add(Tb_svcPassword);
            panel1.Controls.Add(Tb_svcUsername);
            panel1.Location = new Point(399, 108);
            panel1.Name = "panel1";
            panel1.Size = new Size(319, 163);
            panel1.TabIndex = 3;
            // 
            // Btn_UpdateSvcAccount
            // 
            Btn_UpdateSvcAccount.Location = new Point(89, 114);
            Btn_UpdateSvcAccount.Name = "Btn_UpdateSvcAccount";
            Btn_UpdateSvcAccount.Size = new Size(196, 23);
            Btn_UpdateSvcAccount.TabIndex = 2;
            Btn_UpdateSvcAccount.Text = "Update Service Account";
            Btn_UpdateSvcAccount.UseVisualStyleBackColor = true;
            Btn_UpdateSvcAccount.Click += Btn_UpdateSvcAccount_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Name = "MainForm";
            Text = "Form1";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox Tb_svcUsername;
        private TextBox Tb_svcPassword;
        private Panel panel1;
        private Button Btn_UpdateSvcAccount;
    }
}
