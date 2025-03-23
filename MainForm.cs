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

        private string _cardReaderName;
        private string _cardNumber;

        private readonly AdService _adService;
        private readonly MiFareCardReader _cardReader;

        public MainForm()
        {
            InitializeComponent();
            _appSettingsManager = new AppSettingsManager();
            _svcADusername = String.Empty;
            _svcADpassword = String.Empty;
            _cardNumber = "No Card Detected";

            ReadSettings();

            _adService = new AdService("BCA.internal", _svcADusername, _svcADpassword);

            _cardReader = new MiFareCardReader();
            _cardReaderName = _cardReader.GetAvailableReaders()[0];

            LblCardReader.Text = _cardReaderName;

            _cardReader.CardRead += OnCardRead;

            _cardReader.StartMonitoring(_cardReaderName);
        }

        private void ReadSettings()
        {
            _svcADusername = _appSettingsManager.SvcUsername;
            _svcADpassword = _appSettingsManager.SvcPassword;

            Tb_svcUsername.Text = _svcADusername;
            Tb_svcPassword.Text = _svcADpassword;
        }

        private void SaveSettings()
        {
            _svcADusername = Tb_svcUsername.Text;
            _svcADpassword = Tb_svcPassword.Text;

            _appSettingsManager.SvcUsername = _svcADusername;
            _appSettingsManager.SvcPassword = _svcADpassword;

            _appSettingsManager.Save();
        }

        private void Btn_UpdateSvcAccount_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void OnCardRead(object? sender, string cardNumber)
        {
            _cardNumber = cardNumber;

            MessageBox.Show(_cardNumber);

            // Optionally, you can also update the AD service with the card number
            // _adService.UpdateExtensionAttribute14("username", cardNumber);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _cardReader.StopMonitoring();
        }
    }
}
