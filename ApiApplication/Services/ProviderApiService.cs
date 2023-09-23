using ApiApplication.Services.DTO;
using ApiApplication.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public class ProviderApiService : IProviderApiService, IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly IDistributedCache cache;

        public ProviderApiService(IOptions<ProviderAPISettings> apiSettings, IHttpClientFactory httpClientFactory, IDistributedCache cache)
        {
            this.httpClient = httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X-Apikey", apiSettings.Value.ApiKey);
            httpClient.BaseAddress = new Uri(apiSettings.Value.Address);
            this.cache = cache;
        }

        public async Task<IEnumerable<MovieDTO>> GetAllMoviesAsync()
        {
            var response = await httpClient.GetAsync("movies");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<MovieDTO>>(json);
        }

        public async Task<MovieDTO> GetMovieAsync(string id)
        {
            var idKey = $"MovieObjId{id}";
            MovieDTO movie = null;
            var movieJson = await cache.GetStringAsync(idKey);

            if (movieJson != null)
            {
                movie = JsonConvert.DeserializeObject<MovieDTO>(movieJson);
            }

            if (movie != null)
            {
                return movie;
            }

            var response = await httpClient.GetAsync($"v1/movies/{id}");
            response.EnsureSuccessStatusCode();
            movieJson = await response.Content.ReadAsStringAsync();
            await cache.SetStringAsync(idKey, movieJson);
            return JsonConvert.DeserializeObject<MovieDTO>(movieJson);

        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}
