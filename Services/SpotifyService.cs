using API.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace API.Services
{
    public class SpotifyService : ISpotifyService
    {
        private readonly HttpClient httpClient;

        public SpotifyService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<Release>> GetNewReleases(string CountryCode, int limit, string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",accessToken);

            var response = await httpClient.GetAsync($"browse/new-releases?country={CountryCode}&limit={limit}");

            response.EnsureSuccessStatusCode();
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var responseObject = await JsonSerializer.DeserializeAsync<GetNewReleaseResult>(responseStream);
            return responseObject?.albums?.items.Select(i => new Release
            {
                Name = i.name,
                Date=i.release_date,
                ImageUrl=i.images.FirstOrDefault().url,
                Link=i.external_urls.spotify,
                Artists=String.Join(",", i.artists.Select(i=>i.name))
            });

        }
    }
}
