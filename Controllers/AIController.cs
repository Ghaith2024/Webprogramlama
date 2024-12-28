using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Nodes;

namespace BarberApp.Controllers
{
    public class AIController : Controller
    {
        private readonly HttpClient _httpClient;

        public AIController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string description)
        {
            string Key = "AIzaSyBkZt4gygXJzzbuzgtJiqgH9-nk4S2zwns";
            string Url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={Key}";
            

            var prompt = $"Bir kullanıcı, '{description}' tarzında bir saç modeli istiyor. Ona uygun birkaç farklı saç modeli önerin ve açıklamalarını yapın.";
            var requestBody = new
            {
                contents = new[]
                   {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
          
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // API isteğini gönder
            var response = await _httpClient.PostAsync(Url, content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = $"API isteği başarısız oldu: {response.StatusCode}";
                return View();
            }

            // API yanıtını al ve deserialize et
            var result = await response.Content.ReadAsStringAsync();
            //var apiResponse = JsonSerializer.Deserialize<object>(result);
            var jsonResponse = JsonNode.Parse(result);
            // Yanıtı doğrudan ViewBag'e string olarak ekleyin
            string suggestionsText = jsonResponse?["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString() ?? "Öneriler alınamadı.";
            //ViewBag.Suggestions = suggestionsText;

            string[] AIList = suggestionsText.Split(new[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
            ViewBag.SuggestionsList = AIList;
            //ViewBag.Suggestions = apiResponse;
            return View();
        }
    }
}