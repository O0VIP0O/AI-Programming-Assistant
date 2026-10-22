using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AI_Programming_Assistant.Models;
using System.Text.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace AI_Programming_Assistant.Controllers
{
    public class LearningController : Controller
    {
        private readonly AiProgrammingAssistantContext _context;
        private readonly HttpClient _httpClient;

        public LearningController(AiProgrammingAssistantContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
        }

        // ─────────────────────────────────────────────
        // GET: /Learning/Learning
        // يعرض الصفحة الرئيسية ويمرر UserId من الـ Session
        // ─────────────────────────────────────────────
        public IActionResult Learning()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            // إذا لم يكن المستخدم مسجلاً دخوله، أعد توجيهه لصفحة Login
            if (userId == null)
                return RedirectToAction("login_create_account", "login_create_account");

            ViewBag.UserId = userId;
            return View();
        }

        // ─────────────────────────────────────────────
        // POST: /Learning/GetContent
        // يجلب محتوى الدرس والامتحان لمستوى معين في لغة معينة
        // ─────────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> GetContent([FromBody] LearningRequest request)
        {
            try
            {
                // التحقق من البيانات المُرسلة
                if (string.IsNullOrWhiteSpace(request.Language) || request.Level < 1 || request.UserId < 1)
                    return Json(new { success = false, message = "بيانات غير صالحة" });

                // 1. البحث عن اللغة في قاعدة البيانات أو إنشائها
                var language = await _context.ProgrammingLanguages
                    .FirstOrDefaultAsync(l => l.LanguageName.ToLower() == request.Language.ToLower());

                if (language == null)
                {
                    language = new ProgrammingLanguage { LanguageName = request.Language };
                    _context.ProgrammingLanguages.Add(language);
                    await _context.SaveChangesAsync();
                }

                // 2. جلب مستوى المستخدم الحالي (أقصى مستوى مفتوح)
                var userProgress = await _context.UsersProgrammingLanguages
                    .FirstOrDefaultAsync(up => up.UserId == request.UserId && up.LanguageId == language.LanguageId);

                int maxUnlocked = userProgress?.UserLevel ?? 1;

                // 3. البحث عن المحتوى المخزن مسبقاً
                var content = await _context.LearningContents
                    .FirstOrDefaultAsync(c => c.LanguageId == language.LanguageId && c.Level == request.Level);

                // 4. إذا لم يكن المحتوى موجوداً، ولّده عبر Ollama واحفظه
                if (content == null)
                {
                    var aiResponse = await GenerateFromOllama(request.Language, request.Level);

                    content = new LearningContent
                    {
                        LanguageId = language.LanguageId,
                        Level = request.Level,
                        Content = aiResponse.LessonHtml,
                        Exam = aiResponse.ExamJson
                    };

                    _context.LearningContents.Add(content);
                    await _context.SaveChangesAsync();
                }

                return Json(new
                {
                    success = true,
                    content = content.Content,
                    exam = content.Exam,          // JSON string للأسئلة
                    maxUnlocked = maxUnlocked,
                    title = $"المستوى {request.Level}: {request.Language}"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ─────────────────────────────────────────────
        // POST: /Learning/UpdateProgress
        // يحدّث مستوى المستخدم بعد نجاحه في الامتحان
        // ─────────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> UpdateProgress([FromBody] ProgressRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Language) || request.UserId < 1)
                    return Json(new { success = false, message = "بيانات غير صالحة" });

                var language = await _context.ProgrammingLanguages
                    .FirstOrDefaultAsync(l => l.LanguageName.ToLower() == request.Language.ToLower());

                if (language == null)
                    return Json(new { success = false, message = "اللغة غير موجودة" });

                var progress = await _context.UsersProgrammingLanguages
                    .FirstOrDefaultAsync(up => up.UserId == request.UserId && up.LanguageId == language.LanguageId);

                if (progress == null)
                {
                    // أول مرة يسجل فيها المستخدم تقدماً لهذه اللغة
                    progress = new UsersProgrammingLanguage
                    {
                        UserId = request.UserId,
                        LanguageId = language.LanguageId,
                        UserLevel = request.NewLevel
                    };
                    _context.UsersProgrammingLanguages.Add(progress);
                }
                else if (request.NewLevel > progress.UserLevel)
                {
                    // فقط نحدّث إذا كان المستوى الجديد أعلى من الحالي
                    progress.UserLevel = request.NewLevel;
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, newMaxUnlocked = progress.UserLevel });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ─────────────────────────────────────────────
        // POST: /Learning/GetUserCourses
        // يجلب جميع اللغات التي بدأها المستخدم مع مستوياتهم
        // (يُستخدم عند تحميل الصفحة لاستعادة الحالة)
        // ─────────────────────────────────────────────
        [HttpPost]
        public async Task<IActionResult> GetUserCourses([FromBody] UserCoursesRequest request)
        {
            try
            {
                var courses = await _context.UsersProgrammingLanguages
                    .Where(up => up.UserId == request.UserId)
                    .Include(up => up.Language)
                    .Select(up => new
                    {
                        language = up.Language.LanguageName,
                        maxUnlocked = up.UserLevel ?? 1
                    })
                    .ToListAsync();

                return Json(new { success = true, courses });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // ─────────────────────────────────────────────
        // Private: توليد المحتوى عبر Ollama
        // ─────────────────────────────────────────────
        private async Task<(string LessonHtml, string ExamJson)> GenerateFromOllama(string language, int level)
        {
            var prompt = $@"You are a Senior Computer Science Instructor.
Create a detailed lesson for Level {level} in {language} in ARABIC language.
STRICT RULES:
1. Use HTML tags like <h3>, <p>, and <pre><code> for the lesson.
2. Provide exactly 10 multiple-choice questions with 3 options each.
3. The correct answer index must be 0, 1, or 2 (zero-based).
4. Output MUST be a valid JSON object ONLY — no extra text.

Required JSON structure:
{{
  ""lesson"": ""<h3>...</h3><p>...</p>..."",
  ""questions"": [
    {{ ""id"": 1, ""question"": ""سؤال؟"", ""options"": [""أ"", ""ب"", ""ج""], ""correct"": 0 }}
  ]
}}";

            var payload = new
            {
                model = "qwen2.5-coder:3b",
                prompt = prompt,
                stream = false,
                format = "json"
            };

            var response = await _httpClient.PostAsync(
                "http://localhost:11434/api/generate",
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
                throw new Exception("تعذّر الاتصال بـ Ollama. تأكد أنه يعمل على المنفذ 11434.");

            var jsonRaw = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(jsonRaw);
            var aiText = doc.RootElement.GetProperty("response").GetString();

            // استخراج JSON صالح من الرد
            var cleanJson = Regex.Match(aiText ?? "", @"\{.*\}", RegexOptions.Singleline).Value;

            if (string.IsNullOrEmpty(cleanJson))
                throw new Exception("الرد من Ollama ليس JSON صالحاً.");

            var data = JsonDocument.Parse(cleanJson).RootElement;

            string lessonHtml = data.GetProperty("lesson").GetString() ?? "<p>لا يوجد محتوى</p>";
            string examJson = data.GetProperty("questions").ToString();

            return (lessonHtml, examJson);
        }
    }

    // ─── Request Models ───────────────────────────────
    public class LearningRequest
    {
        public string Language { get; set; } = "";
        public int Level { get; set; }
        public int UserId { get; set; }
    }

    public class ProgressRequest
    {
        public string Language { get; set; } = "";
        public int NewLevel { get; set; }
        public int UserId { get; set; }
    }

    public class UserCoursesRequest
    {
        public int UserId { get; set; }
    }
}