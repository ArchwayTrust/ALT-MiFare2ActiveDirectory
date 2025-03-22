using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

namespace MiFare2ActiveDirectory
{
    public partial class MainForm : Form
    {
        private readonly AppSettingsManager _appSettingsManager;

        public MainForm()
        {
            InitializeComponent();
            _appSettingsManager = new AppSettingsManager();
            ReadSettings();
        }

        private void ReadSettings()
        {
            string _svcADusername = _appSettingsManager.GetEncryptedSetting("SeviceAccount", "Username");
            string _svcADpassword = _appSettingsManager.GetEncryptedSetting("SeviceAccount", "Password");
        }
    }
}
