using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace MiFare2ActiveDirectory
{
    public class AdService(string domain, string svcUsername, string svcPassword)
    {
        public string _svcUsername = svcUsername;
        public string _svcPassword = svcPassword;
        private readonly string _domain = domain;

        public List<string>? _availableOus;
        public List<string>? _availableUsers;

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

        public void GetAvailableOUs()
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
                        if (!string.IsNullOrEmpty(ouValue) && (ouValue.StartsWith("OU=Staff,") || ouValue.StartsWith("OU=Bluecoat IT")) && ( ouValue.Contains("OU=User Resources") || (ouValue.Contains("OU=Archway Learning Trust Super System") && ouValue.Contains("OU=Users"))) && !ouValue.Contains("OU=Leavers"))
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
            this._availableOus = ouList;
        }
        public void GetUsersInOu(string distinguishedName)
        {
            var users = new List<string>();
            try
            {
                using var entry = new DirectoryEntry($"LDAP://{_domain}/{distinguishedName}", _svcUsername, _svcPassword);
                using var searcher = new DirectorySearcher(entry)
                {
                    Filter = "(&(objectClass=user)(objectCategory=person)(!(userAccountControl:1.2.840.113556.1.4.803:=2)))",
                    SearchScope = SearchScope.Subtree
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

            var userList = users.ToList();
            userList.Sort();
            this._availableUsers = userList;
        }
    }
}