using Microsoft.AspNetCore.Mvc;
using AI_Programming_Assistant.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AI_Programming_Assistant.Controllers
{
    public class Code_ExplainerController : Controller
    {
        private readonly HttpClient _httpClient;

        public Code_ExplainerController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            // ✅ timeout أطول لأن Ollama يحتاج وقت
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
        }

        [HttpGet]
        public IActionResult Code_Explainer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> getcurrentExplanation_currentFixedCode(
            [FromBody] CodeRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Code))
                return BadRequest("Invalid request");

            var ollamaRequest = new
            {
                model = "qwen2.5-coder:3b",
                stream = false, 
                messages = new[]
                {
                    new
                    {
                     role = "user",
content = $@"
You are a professional programming expert and code reviewer.

Your tasks:
1. Explain the code line by line way as if teaching a complete beginner.
2. Identify any errors, bugs, or bad practices in the code.
3. If there are errors, fix them and provide the corrected version of the code.

Rules:
- Keep the explanation simple and concise.
- Do NOT add any extra text outside the required format.
- If there are NO errors, return 'null' as the fixed code.

Output format (STRICT):

  ""explanation here"",
  ""fixed code here or null""

Code:
{request.Code}
",
                        images = string.IsNullOrEmpty(request.Image)
                            ? null
                            : new[] { request.Image }
                    }
                }
            };

            var json = JsonSerializer.Serialize(ollamaRequest);

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync(
                    "http://localhost:11434/api/chat",
                    new StringContent(json, Encoding.UTF8, "application/json")
                );
            }
            catch (TaskCanceledException)
            {
                return StatusCode(504, "Ollama timeout — النموذج استغرق وقتاً طويلاً");
            }
            catch (HttpRequestException)
            {
                return StatusCode(503, "Ollama غير متاح — تأكد أنه يعمل على المنفذ 11434");
            }

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Ollama request failed");

            var resultString = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(resultString);
            var reply = doc.RootElement
    .GetProperty("message")
    .GetProperty("content")
    .GetString() ?? "";

            // 1- شيل الأقواس لو موجودة
            reply = reply.Trim('[', ']');

            // 2- اقسم على أول فاصلة فقط (أفضل من Split العادي)
            var index = reply.IndexOf(',');

            string explanation = "";
            string fixedCode = "No errors in your code , Keep going .";

            if (index != -1)
            {
                explanation = reply.Substring(0, index).Trim();
                fixedCode = reply.Substring(index + 1).Trim();
            }
            else
            {
                explanation = reply.Trim();
            }

            Console.WriteLine(reply);

            return Json(new
            {
                explanation,
                fixedCode
            });
        }
    }
}