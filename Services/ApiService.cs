using Azure;
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
        private IHttpContextAccessor conntextAccesor;
        public ApiService(IConfiguration configuration, IHttpContextAccessor conntextAccesor)
        {
            this.urlApi = configuration.GetValue<string>("ApiUrls:BaseUrl");
            this.header = new MediaTypeWithQualityHeaderValue("application/json");
            this.conntextAccesor = conntextAccesor;
        }
        public async Task<string> GetTokenAsync(string email, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = $"api/Auth/Login?email={email}&password={password}";  // Usando query parameters en lugar de JSON
                client.BaseAddress = new Uri(this.urlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));  // Asegúrate de que se acepte JSON

                // Realiza la solicitud POST con los parámetros como query
                HttpResponseMessage response = await client.PostAsync(request, null);  // Aquí no necesitamos un cuerpo, ya que se pasan como query parameters

                // Verifica si la respuesta es exitosa
                if (response.IsSuccessStatusCode)
                {
                    string token = await response.Content.ReadAsStringAsync();
                     

                    return token;
                }
                return null;

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

            using (HttpClient client = new HttpClient())
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

            List<string> queryParams = new List<string>();

            if (!string.IsNullOrEmpty(name))
            {
                queryParams.Add($"name={Uri.EscapeDataString(name)}");
            }
            if (!string.IsNullOrEmpty(genre))
            {
                queryParams.Add($"genre={Uri.EscapeDataString(genre)}");
            }
            if (year.HasValue)
            {
                queryParams.Add($"year={year.Value}");
            }
            if (!string.IsNullOrEmpty(developer))
            {
                queryParams.Add($"developer={Uri.EscapeDataString(developer)}");
            }

            if (queryParams.Count > 0)
            {
                request += "?" + string.Join("&", queryParams);
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
        public async Task<List<UserVideoGameModel>> GetVideoGamesByUserAsync()
        {
            string request = "api/UserVideoGames/VideoGamesUser";
            string token = this.conntextAccesor.HttpContext.User.FindFirst("TOKEN")?.Value;
            List<UserVideoGameModel> videoGames = await this.CallApiAsync<List<UserVideoGameModel>>(request,token);
            return videoGames;
        }






    } 
}

