using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RickAndMortyAPI.Models;
using System.Diagnostics;
using System.Text.Json.Serialization;
using X.PagedList;

namespace RickAndMortyAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(string search, int page=1)
        {
            var client = new HttpClient();
            var apiUrl = "https://rickandmortyapi.com/api/character";

            

            var allCharacters = new List<Character>();
            var currentPage = 1;
            bool morePages = true;

            while (morePages)
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{apiUrl}/?page={currentPage}")
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    var characters = JsonConvert.DeserializeObject<Characters>(body);
                    allCharacters.AddRange(characters.results);

                    currentPage++;
                    morePages = characters.info.next != null;


                }
            }

            if (!string.IsNullOrEmpty(search))
            {
                allCharacters = allCharacters
                    .Where(c => c.name.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                return View(allCharacters.ToPagedList(1,allCharacters.Count));
            }




            ViewData["Search"] = search;
            return View(allCharacters.ToPagedList(page, 5));




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
