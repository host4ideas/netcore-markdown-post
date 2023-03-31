using Markdig;
using Microsoft.AspNetCore.Mvc;
using MvcMarkdownPost.Models;
using MvcMarkdownPost.Repositories;

namespace MvcMarkdownPost.Controllers
{
    public class PostsController : Controller
    {
        private readonly RepositoryPosts repositoryPosts;

        public PostsController(RepositoryPosts repositoryPosts)
        {
            this.repositoryPosts = repositoryPosts;
        }

        public async Task<IActionResult> Index()
        {
            List<Post> posts = await this.repositoryPosts.GetPostsAsync();
            ViewData["SUCCESS"] = TempData["SUCCESS"];
            ViewData["ERROR"] = TempData["ERROR"];
            return View(posts);
        }

        public async Task<IActionResult> Details(int postId)
        {
            Post? post = await this.repositoryPosts.FindPostByIdAsync(postId);
            if (post == null)
            {
                return RedirectToAction("Index");
            }
            post.PostText = Markdown.ToHtml(post.PostText);
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> NewPost(string text)
        {
            if (text == null || !text.Any())
            {
                return RedirectToAction("Index");
            }

            try
            {
                await this.repositoryPosts.CreatePostAsync(text);
                TempData["SUCCESS"] = "Post creado correctamente";
            }
            catch (Exception ex)
            {
                TempData["ERROR"] = "Error al subir el post: " + ex.Message;
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeletePost(int postId)
        {
            await this.repositoryPosts.DeletePostAsync(postId);
            return RedirectToAction("Index");
        }
    }
}
