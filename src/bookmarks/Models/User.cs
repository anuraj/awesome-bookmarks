using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

namespace bookmarks.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProviderId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public List<Bookmark> Bookmarks { get; set; }
    }
}