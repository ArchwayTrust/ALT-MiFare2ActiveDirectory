using System;
using System.Configuration;
using System.Security;
using System.Windows.Forms;

namespace MiFare2ActivDirectory
{
    public class ConfigHandler : ApplicationSettingsBase
    {

        readonly static byte[] entropy = System.Text.Encoding.Unicode.GetBytes("Salt Is Not A Password");

        [UserScopedSetting()]
        public string ArchwayLogin
        {
            get
            {
                return (string)this["ArchwayLogin"];
            }
            set
            {
                this["ArchwayLogin"] = value;
            }
        }

        [UserScopedSetting()]
        public string ArchwayPassword
        {
            get
            {
                return ToInsecureString(DecryptString((string)this["ArchwayPassword"]));
            }
            set
            {
                this["ArchwayPassword"] = EncryptString(ToSecureString(value));
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
            SecureString secure = new SecureString();
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
