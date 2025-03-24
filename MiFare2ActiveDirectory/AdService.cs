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

        private List<string> GetUsersInOu(string distinguishedName)
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
            return userList;
        }

        public void GetDistinctUsersFromOus(List<string> distinguishedNames)
        {
            var allUsers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var distinguishedName in distinguishedNames)
            {
                List<string> users = GetUsersInOu(distinguishedName);
                if (users != null)
                {
                    allUsers.UnionWith(allUsers);
                }
            }

            _availableUsers = [.. allUsers];
        }
    }
}