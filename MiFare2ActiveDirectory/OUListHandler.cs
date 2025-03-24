using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MiFare2ActiveDirectory
{
    public class OUListHandler
    {
        public List<OURecord> OUs { get; private set; }

        public OUListHandler(string jsonFilePath)
        {
            OUs = LoadOUsFromJson(jsonFilePath);
        }

        private List<OURecord> LoadOUsFromJson(string jsonFilePath)
        {
            try
            {
                var jsonString = File.ReadAllText(jsonFilePath);
                var ouList = JsonSerializer.Deserialize<OUList>(jsonString);
                return ouList?.OUs ?? new List<OURecord>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading OUs from JSON: {ex.Message}");
                return new List<OURecord>();
            }
        }

        public List<string> GetGroups()
        {
            return OUs.Select(ou => ou.Group).Distinct().ToList();
        }

        public List<string> GetOUsByGroup(string group)
        {
            return OUs.Where(ou => ou.Group.Equals(group, StringComparison.OrdinalIgnoreCase))
                      .Select(ou => ou.Name)
                      .ToList();
        }
    }

    public class OUList
    {
        public List<OURecord> OUs { get; set; }
    }

    public class OURecord
    {
        public string Group { get; set; }
        public string Name { get; set; }
    }
}
