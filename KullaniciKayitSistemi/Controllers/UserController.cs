using KullaniciKayitSistemi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KullaniciKayitSistemi.Controllers
{
    public class UserController : Controller
    {
        private readonly IOptions<ApiSettings> _apiSettings;
        private readonly IOptions<GoogleCAPTCHA> _captchaSettings;
        public UserController(IOptions<ApiSettings> apiSettings, IOptions<GoogleCAPTCHA> captchaSettings)
        {
            _apiSettings = apiSettings;
            _captchaSettings = captchaSettings;
        }
        [HttpGet]
        public IActionResult AddUser()
        {
            ViewBag.ApiBaseUrl = _apiSettings.Value.BaseUrl;
            ViewBag.CaptchaKey = _captchaSettings.Value.Key;

            return View();
        }

        [HttpGet]
        public IActionResult Users()
        {
            ViewBag.ApiBaseUrl = _apiSettings.Value.BaseUrl;
            

            return View();
        }

    }
}
