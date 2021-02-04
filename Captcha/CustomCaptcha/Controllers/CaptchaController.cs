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
        public async Task<IActionResult> CaptchaAsync()
        {
            // var code = await Captcha.Models.Captcha.GenerateRandomCaptchaAsync();

            // var result = await Captcha.Models.Captcha.GenerateCaptchaImageAsync(code);

            return File((await Captcha.Models.Captcha.GenerateCaptchaImageAsync(
                            await Captcha.Models.Captcha.GenerateRandomCaptchaAsync())).ToArray(), "image/png");
        }
    }
}
