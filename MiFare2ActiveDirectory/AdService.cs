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

        public void UpdateExtensionAttribute15(string username, string mifareNumber)
        {
            using var context = new PrincipalContext(ContextType.Domain, _domain, _svcUsername, _svcPassword);
            var user = UserPrincipal.FindByIdentity(context, username);
            if (user != null)
            {
                DirectoryEntry directoryEntry = (DirectoryEntry)user.GetUnderlyingObject();
                directoryEntry.Properties["extensionAttribute15"].Value = mifareNumber;
                directoryEntry.CommitChanges();
            }
            else
            {
                throw new Exception("User not found in Active Directory.");
            }
        }

        public List<string> GetAvailableOUs()
        {
            var ous = new HashSet<string>();
            try
            {
                using var entry = new DirectoryEntry($"LDAP://{_domain}", _svcUsername, _svcPassword);
                using var searcher = new DirectorySearcher(entry)
                {
                    Filter = "(objectClass=organizationalUnit)"
                };
                searcher.PropertiesToLoad.Add("distinguishedName");

                foreach (SearchResult result in searcher.FindAll())
                {
                    if (result.Properties.Contains("distinguishedName") && result.Properties["distinguishedName"][0] != null)
                    {
                        var ouValue = result.Properties["distinguishedName"][0]?.ToString();
                        if (!string.IsNullOrEmpty(ouValue) && ouValue.StartsWith("OU=Staff,") && ouValue.Contains("OU=Users") && !ouValue.Contains("OU=Leavers"))
                        {
                            ous.Add(ouValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving OUs: {ex.Message}");
            }

            var ouList = ous.ToList();
            ouList.Sort();
            return ouList;
        }
        public List<string> GetUsersInOu(string distinguishedName)
        {
            var users = new List<string>();
            try
            {
                using var entry = new DirectoryEntry($"LDAP://{_domain}", _svcUsername, _svcPassword);
                using var searcher = new DirectorySearcher(entry)
                {
                    Filter = $"(&(objectClass=user)(objectCategory=person)(memberOf:1.2.840.113556.1.4.1941:={distinguishedName}))"
                };
                searcher.PropertiesToLoad.Add("sAMAccountName");

                foreach (SearchResult result in searcher.FindAll())
                {
                    if (result.Properties.Contains("sAMAccountName") && result.Properties["sAMAccountName"][0] != null)
                    {
                        var username = result.Properties["sAMAccountName"][0]?.ToString();
                        if (!string.IsNullOrEmpty(username))
                        {
                            users.Add(username);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving users: {ex.Message}");
            }

            return users;
        }
    }
}