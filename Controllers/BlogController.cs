using API.Models;
using API.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BlogController : Controller
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly IDistributedCache cache;
        private List<Blog> blogs;

       

        public BlogController(IHttpClientFactory clientFactory,IDistributedCache cache)
        {
            this.clientFactory = clientFactory;
            this.cache = cache;
        }

        public async Task<IActionResult> Index()
        {
            var blog =await LoadBlog();
            return View(blog);
            
        }
        public async Task<List<Blog>> LoadBlog()
        {
            blogs = null;
            string recordKey = "Blog_" + DateTime.Now.ToString("yyyyMMdd_hhmm");
            blogs = await cache.GetRecordAsync<Blog>(recordKey);

            if (blogs == null)
            {
                var httpClient = clientFactory.CreateClient("blog");
                var URL = httpClient.BaseAddress;
                blogs = await httpClient.GetFromJsonAsync<List<Blog>>(URL);
                ViewData["loadLocation"] = $"Loaded From API at {DateTime.Now}";
                ViewData["isCachedData"] = "text-primary";
                await cache.SetRecordAsync(recordKey, blogs, TimeSpan.FromMinutes(2), TimeSpan.FromSeconds(20));
            }
            else
            {
                ViewData["loadLocation"] = $"Loaded From Cache at {DateTime.Now}";
                ViewData["isCachedData"] = "text-danger";
            }
            return blogs;
        }

            public async Task<IActionResult> Detail(string q)
        {
            var response = await LoadBlog();
            var blog = response.FirstOrDefault(x => x.Title == q);
            return View(blog);
        }
    }
}
