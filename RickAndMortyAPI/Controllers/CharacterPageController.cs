using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RickAndMortyAPI.Models;

namespace RickAndMortyAPI.Controllers
{
    public class CharacterPageController : Controller
    {
        public async Task<IActionResult >Index(int id)
        {
            var client = new HttpClient();
            var apiUrl = $"https://rickandmortyapi.com/api/character/{id}";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(apiUrl)
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var character = JsonConvert.DeserializeObject<Character>(body);

                return View(character);
            }

            return View(null);
        }
    }
}
