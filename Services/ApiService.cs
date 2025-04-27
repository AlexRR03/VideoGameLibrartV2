using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProyectoJuegos.Models;
using System.Net.Http.Headers;
using System.Text;

namespace ProyectoJuegos.Services
{
    public class ApiService
    {
        private string urlApi;
        private MediaTypeWithQualityHeaderValue header;
        public ApiService(IConfiguration configuration)
        {
            this.urlApi = configuration.GetValue<string>("ApiUrls:BaseUrl");
            this.header = new MediaTypeWithQualityHeaderValue("application/json");
        }
        public async Task<string> GetTokenAsync(string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/Auth/Login";
                client.BaseAddress = new Uri(this.urlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                User user = new User()
                {
                    Email = email,
                    Password = password
                };
                string json = JsonConvert.SerializeObject(user);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject keys = JObject.Parse(data);
                    string token = keys.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }

        }
        public async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.urlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                HttpResponseMessage response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }
        public async Task<T> CallApiAsync<T>(string request, string token) 
        { 
        
            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.urlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                HttpResponseMessage response = await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }
        public async Task<List<VideoGame>> GetVideoGamesAsync()
        {
            string request = "api/VideoGames";
            List<VideoGame> videoGames = await this.CallApiAsync<List<VideoGame>>(request);
            return videoGames;
        }
        public async Task<List<VideoGame>> VideoGameSearchAsync(string? name, string? genre, int? year, string? developer)
        {
            string request = "api/VideoGames/search";
            if (!string.IsNullOrEmpty(name))
            {
                request += $"?name={name}";
            }
            if (!string.IsNullOrEmpty(genre))
            {
                request += $"&genre={genre}";
            }
            if (year.HasValue)
            {
                request += $"&year={year}";
            }
            if (!string.IsNullOrEmpty(developer))
            {
                request += $"&developer={developer}";
            }
            List<VideoGame> videoGames = await this.CallApiAsync<List<VideoGame>>(request);
            return videoGames;
        }
        public async Task<VideoGame> FindVideoGameAsync(int id)
        {
            string request = $"api/VideoGames/{id}";
            VideoGame videoGame = await this.CallApiAsync<VideoGame>(request);
            return videoGame;
        }
        public async Task<List<string>> GetPlatformsGameAsync(string name)
        {
            string request = $"api/VideoGames/platforms/{name}";
            List<string> platforms = await this.CallApiAsync<List<string>>(request);
            return platforms;
        }



    }
}
