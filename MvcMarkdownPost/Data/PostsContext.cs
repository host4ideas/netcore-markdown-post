using Microsoft.EntityFrameworkCore;
using MvcMarkdownPost.Models;

namespace MvcMarkdownPost.Data
{
    public class PostsContext : DbContext
    {
        public PostsContext(DbContextOptions<PostsContext> options) : base(options) { }

        public DbSet<Post> Posts { get; set; }
    }
}
