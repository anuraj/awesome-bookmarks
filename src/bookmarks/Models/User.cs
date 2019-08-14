using System;
using System.ComponentModel.DataAnnotations;

namespace bookmarks.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProviderId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}