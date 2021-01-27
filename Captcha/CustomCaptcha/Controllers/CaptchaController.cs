using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Captcha.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Captcha.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaptchaController : Controller
    {
        [HttpGet("index")]
        public  IActionResult Index()
        {
            return View("Index");
        }


        [HttpGet]
        public async Task<IActionResult> CaptchaAsync([FromServices] ICaptcha _captcha)
        {
            var code = await _captcha.GenerateRandomCaptchaAsync();

            var result = await _captcha.GenerateCaptchaImageAsync(code);

            return File(result.CaptchaMemoryStream.ToArray(), "image/png");
        }
    }
}
