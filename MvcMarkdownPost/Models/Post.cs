using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMarkdownPost.Models
{
    [Table("POST")]
    public class Post
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("POST_TEXT")]
        public string PostText { get; set; }
        [Column("DATE_POSTED")]
        public DateTime DatePosted { get; set; }
    }
}
