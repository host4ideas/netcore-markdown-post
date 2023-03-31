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
            // Enviamos texto opcional directamente a la vista
            ViewData["SUCCESS"] = TempData["SUCCESS"];
            ViewData["ERROR"] = TempData["ERROR"];
            return View(posts);
        }

        public async Task<IActionResult> Details(int postId)
        {
            Post? post = await this.repositoryPosts.FindPostByIdAsync(postId);
            if (post == null)
            {
            // Redirigimos al controller Index
                return RedirectToAction("Index");
            }
            // Utilizamos la libreria MarkDig para parsear el markdown de la BBDD a HTML para la vista
            // En este caso utilizamos UseAdvancedExtensions() para añadir soporte para tablas y algunos elementos mas
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

            post.PostText = Markdown.ToHtml(post.PostText, pipeline);
            return View(post);
        }

        // Decoracion que indica que este metodo solo recibe peticiones POST
        [HttpPost]
        public async Task<IActionResult> NewPost(string text)
        {
            // Si no se ha especificado ningun texto redirigimos sin realizar cambios
            if (text == null || !text.Any())
            {
                return RedirectToAction("Index");
            }

            try
            {
                // Esta operacion devolvera una excepcion si algo va mal
                await this.repositoryPosts.CreatePostAsync(text);
                // Enviamos un texto al siguiente controller
                TempData["SUCCESS"] = "Post creado correctamente";
            }
            catch (Exception ex)
            {
                // Enviamos un texto al siguiente controller
                TempData["ERROR"] = "Error al subir el post: " + ex.Message;
            }
            // Redirigimos al controller Index
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeletePost(int postId)
        {
            await this.repositoryPosts.DeletePostAsync(postId);
            // Redirigimos al controller Index
            return RedirectToAction("Index");
        }
    }
}
