using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace API.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISpotifyAccountService spotifyAccountService;
        private readonly IConfiguration configuration;
        private readonly ISpotifyService spotifyService;
        private readonly IHttpClientFactory clientFactory;

        public HomeController(ISpotifyAccountService spotifyAccountService, IConfiguration configuration, ISpotifyService spotifyService,IHttpClientFactory clientFactory)
        {
            this.spotifyAccountService = spotifyAccountService;
            this.configuration = configuration;
            this.spotifyService = spotifyService;
            this.clientFactory = clientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var newReleases = await GetReleases();
            return View(newReleases);
        }
        public async Task<IEnumerable<Release>> GetReleases()
        {
            try
            {
                var token = await spotifyAccountService.GetToken(configuration["Spotify:ClientId"], configuration["Spotify:ClientSecret"]);
                var newRelease = await spotifyService.GetNewReleases("FR", 50, token);
                return newRelease;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                return Enumerable.Empty<Release>();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}