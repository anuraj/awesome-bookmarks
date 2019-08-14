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
        [Required]
        public string Name { get; set; }
        [Required]
        public string ProviderId { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public List<Bookmark> Bookmarks { get; set; }
        public bool IsActivated { get; set; } = false;
    }
}