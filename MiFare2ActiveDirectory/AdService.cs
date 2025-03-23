using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace MiFare2ActiveDirectory
{
public class AdService(string domain, string svcUsername, string svcPassword)
    {
        private readonly string _svcUsername = svcUsername;
        private readonly string _svcPassword = svcPassword;
        private readonly string _domain = domain;

        public void UpdateExtensionAttribute14(string username, string mifareNumber)
        {
            using var context = new PrincipalContext(ContextType.Domain, _domain, _svcUsername, _svcPassword);
            var user = UserPrincipal.FindByIdentity(context, username);
            if (user != null)
            {
                DirectoryEntry directoryEntry = (DirectoryEntry)user.GetUnderlyingObject();
                directoryEntry.Properties["extensionAttribute14"].Value = mifareNumber;
                directoryEntry.CommitChanges();
            }
            else
            {
                throw new Exception("User not found in Active Directory.");
            }
        }
    }
}