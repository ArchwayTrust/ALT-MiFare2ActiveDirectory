using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using PCSC;

namespace MiFare2ActiveDirectory
{
    public partial class MainForm : Form
    {
        private readonly AppSettingsManager _appSettingsManager;

        //private string _cardReaderName;

        private string _cardNumber;

        private readonly AdService _adService;
        private readonly MiFareCardReader _cardReader;
        private readonly OUListHandler _ouListHandler;

        public MainForm()
        {
            InitializeComponent();
            _appSettingsManager = new AppSettingsManager();
            _cardNumber = "No Card Detected";

            _adService = new AdService("BCA.internal", _appSettingsManager.SvcUsername, _appSettingsManager.SvcPassword);
            _cardReader = new MiFareCardReader();
            _ouListHandler = new OUListHandler("OUList.json");
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshCardReaderList();

            CBAvailableOUs.DataSource = _ouListHandler.GetGroups();

            ReadSettings();
            RefreshUserList();

        }

        private void ReadSettings()
        {
            _adService._svcUsername = _appSettingsManager.SvcUsername;
            _adService._svcPassword = _appSettingsManager.SvcPassword;

            TBSvcUsername.Text = _adService._svcUsername;
            TBSvcPassword.Text = _adService._svcPassword;
            CBCardReaders.SelectedIndex = _appSettingsManager.CardReaderId;
            CBAvailableOUs.Text = _appSettingsManager.OUGroup;
        }

        private void SaveSettings()
        {
            _appSettingsManager.SvcUsername = TBSvcUsername.Text;
            _appSettingsManager.SvcPassword = TBSvcPassword.Text;
            _appSettingsManager.CardReaderId = CBCardReaders.SelectedIndex;
            _appSettingsManager.OUGroup = CBAvailableOUs.Text;

            _appSettingsManager.Save();

            _adService._svcUsername = _appSettingsManager.SvcUsername;
            _adService._svcPassword = _appSettingsManager.SvcPassword;
            _cardReader.CardReaderName = CBCardReaders.Text;
       

            RefreshUserList();
            RefreshCardReaderList();

            MessageBox.Show("Settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RefreshCardReaderList()
        {
            _cardReader.StopMonitoring();

            _cardReader.GetAvailableReaders();

            if (_cardReader.CardReaderNames == null || _cardReader.CardReaderNames.Count == 0)
            {
                MessageBox.Show("No card readers found. Please ensure the reader is connected and drivers are installed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            CBCardReaders.DataSource = _cardReader.CardReaderNames;

            try
            {
                _cardReader.CardReaderName = _cardReader.CardReaderNames[_appSettingsManager.CardReaderId];
            }
            catch
            {
                _cardReader.CardReaderName = _cardReader.CardReaderNames[0];
            }

            try
            {
                CBCardReaders.SelectedIndex = _appSettingsManager.CardReaderId;
            }
            catch
            {
                CBCardReaders.SelectedIndex = 0;
            }

            _cardReader.CardRead += OnCardRead;

            _cardReader.StartMonitoring();
        }

        private void ChangeCardReader()
        {
            _cardReader.StopMonitoring();
            _cardReader.CardReaderName = CBCardReaders.Text;
            _cardReader.CardRead += OnCardRead;
            _cardReader.StartMonitoring();
        }

        private void RefreshUserList()
        {
            _adService.GetDistinctUsersFromOus(_ouListHandler.GetOUsByGroup(CBAvailableOUs.Text));
            CBADUsers.DataSource = _adService._availableUsers;
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
            string usernameToUpdate = CBADUsers.Text;

            if (MessageBox.Show("Are you sure you want to update AD for " + usernameToUpdate + " with " + _cardNumber + "?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!string.IsNullOrEmpty(usernameToUpdate))
                {
                    _adService.UpdateExtensionAttribute15(usernameToUpdate, _cardNumber);
                }
            }
        }

        private void CBAvailableOUs_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshUserList();
        }

        private void CBCardReaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeCardReader();
        }
    }
}

