using System.ComponentModel.DataAnnotations;

namespace bookmarks.Models
{
    public class Bookmark
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public User User { get; set; }
    }
}