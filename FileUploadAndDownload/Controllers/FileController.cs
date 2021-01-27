using System.IO.Compression;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IO;
using System;

namespace FileUploadAndDownload.Controllers
{
    [Route("api/file")]
    public class FileController : Controller
    {
        [HttpGet("download")]
        public IActionResult FileDownload(File f)
        {
            if (System.IO.File.Exists(@"C:\Users\laishilin\Desktop\demo.zip"))
            {
                System.IO.File.Delete(@"C:\Users\laishilin\Desktop\demo.zip");
            }

            using (ZipArchive zipArchive = ZipFile.Open(@"C:\Users\laishilin\Desktop\demo.zip", ZipArchiveMode.Update))
            {
                var extension = Path.GetExtension(f.Url);
                var name = System.IO.Path.GetFileName(f.Url);
                zipArchive.CreateEntryFromFile(f.Url, $"{name}");
            }
            
            return File(System.IO.File.OpenRead(@"C:\Users\laishilin\Desktop\demo.zip"), "application/octet-stream", $"导出测试{DateTime.Now.ToString("yyyyMMddHHmmss")}.zip");
            
        }
        ///<summary>
        /// 清空指定的文件夹，但不删除文件夹
        /// </summary>
        /// <param name="dir"></param>
        // public static void DeleteFolder(string dir)
        // {
        //     foreach (string d in Directory.GetFileSystemEntries(dir))
        //     {
        //         if (File.Exists(d))
        //         {
        //             FileInfo fi = new FileInfo(d);
        //             if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
        //                 fi.Attributes = FileAttributes.Normal;
        //             File.Delete(d);//直接删除其中的文件  
        //         }
        //         else
        //         {
        //             DirectoryInfo d1 = new DirectoryInfo(d);
        //             if (d1.GetFiles().Length != 0)
        //             {
        //                 DeleteFolder(d1.FullName);递归删除子文件夹
        //             }
        //             Directory.Delete(d);
        //         }
        //     }
        // } 


        [HttpPost("upload")]
        public async Task<IActionResult> FileUpload([FromForm] FileModel model)
        {
            // 文件总大小
            long size = model.Files.Sum(f => f.Length);

            foreach (var file in model.Files)
            {
                string name = file.FileName;
                int index = name.LastIndexOf(".");
                if (index != -1)
                {
                    name = name.Substring(index);
                }

                if (file.Length > 0)
                {
                    using (var fs = System.IO.File.Create($@"C:\Users\laishilin\Desktop\{model.Title}{name}"))
                    {
                        await file.CopyToAsync(fs);
                    }
                }
            }

            return Ok($"文件大小 {size} 字节");
        }
    }

    public class FileModel
    {
        public string Title { get; set; }

        public List<IFormFile> Files { get; set; }
    }

    public class File
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
    }
}