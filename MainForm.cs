using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;

namespace MiFare2ActiveDirectory
{
    public partial class MainForm : Form
    {
        private readonly AppSettingsManager _appSettingsManager;
        private string _svcADusername;
        private string _svcADpassword;

        private readonly AdService _adService;

        public MainForm()
        {
            InitializeComponent();
            _appSettingsManager = new AppSettingsManager();
            _svcADusername = String.Empty;
            _svcADpassword = String.Empty;

            ReadSettings();

            _adService = new AdService("BCA.internal", _svcADusername, _svcADpassword);

        }

        private void ReadSettings()
        {
            _svcADusername = _appSettingsManager.GetEncryptedSetting("SeviceAccount", "Username");
            _svcADpassword = _appSettingsManager.GetEncryptedSetting("SeviceAccount", "Password");

            Tb_svcUsername.Text = _svcADusername;
            Tb_svcPassword.Text = _svcADpassword;
        }

        private void SaveSettings()
        {
            _svcADusername = Tb_svcUsername.Text;
            _svcADpassword = Tb_svcPassword.Text;

            _appSettingsManager.SetEncryptedSetting("SeviceAccount", "Username", _svcADusername);
            _appSettingsManager.SetEncryptedSetting("SeviceAccount", "Password", _svcADpassword);
        }

        private void Btn_UpdateSvcAccount_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}
