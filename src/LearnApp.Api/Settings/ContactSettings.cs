using System.ComponentModel.DataAnnotations;

namespace LearnApp.Api.Settings
{
    public class ContactSettings
    {
        public const string SectionName = "ContactSettings";
        [Range(1, 100)] public int DefaultPageSize { get; set; } = 10;
        [Range(1, 100)] public int MaxPageSize { get; set; } = 50;
    }
    
}
