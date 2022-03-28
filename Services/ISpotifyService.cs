using API.Models;

namespace API.Services
{
    public interface ISpotifyService
    {
        Task<IEnumerable<Release>> GetNewReleases(string CountryCode, int limit, string accessToken);
    }
}
