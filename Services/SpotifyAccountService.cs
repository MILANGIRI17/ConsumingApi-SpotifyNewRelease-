using API.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace API.Services
{
    public class SpotifyAccountService:ISpotifyAccountService
    {
        private readonly HttpClient _httpClient;

        public SpotifyAccountService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }
        public async Task<string> GetToken(string cliendId, string clientSecret)
        {
            var request=new HttpRequestMessage(HttpMethod.Post, "token");
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic",Convert.ToBase64String(Encoding.UTF8.GetBytes($"{cliendId}:{clientSecret}")));

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string> {
                {"grant_type","client_credentials"}
            });

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            using var responseStream=await response.Content.ReadAsStreamAsync();
            var authResult=await JsonSerializer.DeserializeAsync<AuthResult>(responseStream);
            //var authResult =await JsonSerializer.DeserializeAsync<AuthResult>(responseStream);

            return authResult.access_token;

        }
    }
}
    