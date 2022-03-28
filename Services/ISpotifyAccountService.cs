namespace API.Services
{
    public interface ISpotifyAccountService
    {
        Task<string> GetToken(string cliendId,string clientSecret);
    }
}
