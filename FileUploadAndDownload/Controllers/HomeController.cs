using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace FileUploadAndDownload.Controllers
{
    [Route("[Controller]")]
    public class HomeController : Controller
    {
        [HttpGet("index")]
        public IActionResult Index()
        {
            var files = new List<File>{
                new File
                {
                    Name = "test1",
                    Title = "文件1",
                    Url = @"C:\Users\laishilin\Desktop\test1.txt"
                },
                new File
                {
                    Name = "test2",
                    Title = "文件2",
                    Url = @"C:\Users\laishilin\Desktop\test2.txt"
                },
            };
            return View(files);
        }
    }
}
