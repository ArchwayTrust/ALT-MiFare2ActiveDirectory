using System;
using System.Security;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace MiFare2ActiveDirectory
{
    public class AppSettingsManager : ApplicationSettingsBase
    {
        readonly static byte[] entropy = System.Text.Encoding.Unicode.GetBytes("Salt Is Not A Password");

        [UserScopedSetting()]
        [DefaultSettingValue("0")]
        public int CardReaderId
        {
            get
            {
                return (int)this[nameof(CardReaderId)];
            }
            set
            {
                this[nameof(CardReaderId)] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string SvcUsername
        {
            get
            {
                return (string)this[nameof(SvcUsername)];
            }
            set
            {
                this[nameof(SvcUsername)] = value;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string SvcPassword
        {
            get
            {
                return ToInsecureString(DecryptString((string)this[nameof(SvcPassword)]));
            }
            set
            {
                this[nameof(SvcPassword)] = EncryptString(ToSecureString(value));
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string OUGroup
        {
            get
            {
                return (string)this[nameof(OUGroup)];
            }
            set
            {
                this[nameof(OUGroup)] = value;
            }
        }

        private static string EncryptString(System.Security.SecureString input)
        {
            byte[] encryptedData = System.Security.Cryptography.ProtectedData.Protect(System.Text.Encoding.Unicode.GetBytes(ToInsecureString(input)), entropy, System.Security.Cryptography.DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        private static SecureString DecryptString(string encryptedData)
        {
            try
            {
                byte[] decryptedData = System.Security.Cryptography.ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), entropy, System.Security.Cryptography.DataProtectionScope.CurrentUser);
                return ToSecureString(System.Text.Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        private static SecureString ToSecureString(string input)
        {
            var secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        private static string ToInsecureString(SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }
    }
}
