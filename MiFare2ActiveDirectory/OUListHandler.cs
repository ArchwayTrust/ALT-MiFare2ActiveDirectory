using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MiFare2ActiveDirectory
{
    public class OUListHandler(string jsonFilePath)
    {
        public List<OURecord>? OUs { get; private set; } = LoadOUsFromJson(jsonFilePath);

        private static List<OURecord> LoadOUsFromJson(string jsonFilePath)
        {
            try
            {
                var jsonString = File.ReadAllText(jsonFilePath);
                var ouList = JsonSerializer.Deserialize<OUList>(jsonString);
                return ouList?.OUs ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading OUs from JSON: {ex.Message}");
                return [];
            }
        }

        public List<string> GetGroups()
        {
            return OUs?.Select(ou => ou.Group).Distinct().ToList() ?? [];
        }

        public List<string> GetOUsByGroup(string group)
        {
            return OUs?.Where(ou => ou.Group.Equals(group, StringComparison.OrdinalIgnoreCase))
                       .Select(ou => ou.Name)
                       .ToList() ?? [];
        }
    }

    public class OUList
    {
        public List<OURecord> OUs { get; set; } = [];
    }

    public class OURecord
    {
        public string Group { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
