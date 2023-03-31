using Microsoft.EntityFrameworkCore;
using MvcMarkdownPost.Data;
using MvcMarkdownPost.Models;

namespace MvcMarkdownPost.Repositories
{
    public class RepositoryPosts
    {
        private readonly PostsContext context;

        public RepositoryPosts(PostsContext context)
        {
            this.context = context;
        }

        public Task<List<Post>> GetPostsAsync()
        {
            return this.context.Posts.ToListAsync();
        }

        public async Task<Post?> FindPostByIdAsync(int postId)
        {
            return await this.context.Posts.FindAsync(postId);
        }

        private async Task<int> GetMaxPostAsync()
        {
            if (!await context.Posts.AnyAsync())
            {
                return 1;
            }

            return await this.context.Posts.MaxAsync(x => x.Id) + 1;
        }

        public async Task CreatePostAsync(string text)
        {
            await this.context.Posts.AddAsync(new Post()
            {
                Id = await this.GetMaxPostAsync(),
                DatePosted = DateTime.Now,
                PostText = text
            });
            await this.context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int postId)
        {
            Post? post = await this.FindPostByIdAsync(postId);
            if (post != null)
            {
                this.context.Posts.Remove(post);
                await this.context.SaveChangesAsync();
            }
        }
    }
}
