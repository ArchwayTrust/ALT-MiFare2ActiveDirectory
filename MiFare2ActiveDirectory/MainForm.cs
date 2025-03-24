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

        private int _cardReaderId;
        private string _cardReaderName;
        private readonly string[] _cardReaderNames;

        private readonly string[] _availableOus;


        private string _cardNumber;

        private readonly AdService _adService;
        private readonly MiFareCardReader _cardReader;

        public MainForm()
        {
            InitializeComponent();
            _appSettingsManager = new AppSettingsManager();
            _svcADusername = String.Empty;
            _svcADpassword = String.Empty;
            _cardReaderId = 0;
            _cardNumber = "No Card Detected";

            ReadSettings();

            _adService = new AdService("BCA.internal", _svcADusername, _svcADpassword);
            _availableOus = [.. _adService.GetAvailableOUs()];

            CBAvailableOUs.DataSource = _availableOus;

            _cardReader = new MiFareCardReader();
            _cardReaderNames = [.. _cardReader.GetAvailableReaders()];

            if (_cardReaderNames.Length == 0)
            {
                MessageBox.Show("No card readers found. Please ensure the reader is connected and drivers are installed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            try 
            {
                _cardReaderName = _cardReaderNames[_cardReaderId];
            }
            catch
            { 
                _cardReaderName = _cardReaderNames[0];
            }

            CBCardReaders.DataSource = _cardReaderNames;

            try
            {
                CBCardReaders.SelectedIndex = _cardReaderId;
            }
            catch
            {
                CBCardReaders.SelectedIndex = 0;
            }


            _cardReader.CardRead += OnCardRead;

            _cardReader.StartMonitoring(_cardReaderName);
        }

        private void ReadSettings()
        {
            _svcADusername = _appSettingsManager.SvcUsername;
            _svcADpassword = _appSettingsManager.SvcPassword;
            _cardReaderId = _appSettingsManager.CardReaderId;

            TBSvcUsername.Text = _svcADusername;
            TBSvcPassword.Text = _svcADpassword;
        }

        private void SaveSettings()
        {
            _svcADusername = TBSvcUsername.Text;
            _svcADpassword = TBSvcPassword.Text;
            _cardReaderId = CBCardReaders.SelectedIndex;
            _cardReaderName = _cardReaderNames[_cardReaderId];

            _appSettingsManager.SvcUsername = _svcADusername;
            _appSettingsManager.SvcPassword = _svcADpassword;
            _appSettingsManager.CardReaderId = _cardReaderId;

            _appSettingsManager.Save();

            MessageBox.Show("Settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Btn_UpdateSvcAccount_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void OnCardRead(object? sender, string cardNumber)
        {
            _cardNumber = cardNumber;

            // GUI on different thread so this handles that...
            if (LBLMiFareNumber.InvokeRequired)
            {
                LBLMiFareNumber.Invoke(new Action(() => LBLMiFareNumber.Text = "The last card read was " + _cardNumber));
            }
            else
            {
                LBLMiFareNumber.Text = "The last card read was " + _cardNumber;
            }

        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _cardReader.StopMonitoring();
        }

        private void BTNWriteToAd_Click(object sender, EventArgs e)
        {
            string usernameToUpdate = TBUserToUpdate.Text;

            if (MessageBox.Show("Are you sure you want to update AD for " + usernameToUpdate + " with " + _cardNumber + "?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!string.IsNullOrEmpty(usernameToUpdate))
                {
                    _adService.UpdateExtensionAttribute15(usernameToUpdate, _cardNumber);
                }
            }
        }
    }
}
