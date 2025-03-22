using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace MiFare2ActiveDirectory
{
    public class AppSettingsManager
    {
        private readonly IConfigurationRoot _configuration;
        private readonly byte[] _entropy = Encoding.Unicode.GetBytes("Salt Is Not A Password");

        public AppSettingsManager()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
        }

        public string GetEncryptedSetting(string section, string key)
        {
            var encryptedValue = _configuration.GetSection(section)[key];
            if (string.IsNullOrEmpty(encryptedValue))
            {
                return string.Empty;
            }
            return DecryptString(encryptedValue);
        }

        public void SetEncryptedSetting(string section, string key, string value)
        {
            var encryptedValue = EncryptString(value);
            var jsonFile = "appsettings.json";
            var json = File.ReadAllText(jsonFile);
            dynamic jsonObj = JObject.Parse(json);

            if (jsonObj[section] == null)
            {
                jsonObj[section] = new JObject();
            }

            jsonObj[section][key] = encryptedValue;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(jsonFile, output);
        }

        private string EncryptString(string plainText)
        {
            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(plainText), _entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        private string DecryptString(string encryptedData)
        {
            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), _entropy, DataProtectionScope.CurrentUser);
                return Encoding.Unicode.GetString(decryptedData);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}

