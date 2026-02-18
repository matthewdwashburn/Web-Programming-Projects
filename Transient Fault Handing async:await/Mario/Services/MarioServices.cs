using Mario.Entities;
using Newtonsoft.Json;
using Polly;
using System.Net;
using System.Text.Json.Serialization;

namespace Mario.Services
{
    public class MarioServices : IMarioServices
    {
        private readonly HttpClient httpClient = new();

        // Task: Guarentee that at some point I will give you a string but I can't yet
        public async Task<MarioEntity?> GetMarioLevelStatusAsync(string move)
        {
            Random random = new Random();
            var policy = Policy.HandleInner<HttpRequestException>(ex =>
            {
                return ex?.StatusCode == HttpStatusCode.ServiceUnavailable;
            }).WaitAndRetryAsync(10, retryAttempt =>
            {
                int randomOffset = random.Next(0, 201);
                return TimeSpan.FromMilliseconds((100 * Math.Pow(2, retryAttempt)) + randomOffset);
            }
            );
            var marioEntity = await policy.ExecuteAsync(async () =>
            {
                var response = await httpClient.GetAsync("https://bethelwebprogrammingmario.azurewebsites.net/api/mario/" + move);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<MarioEntity?>();
            });
            return marioEntity;
            
        }
    }
}
