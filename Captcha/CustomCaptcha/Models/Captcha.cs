using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Captcha.Models
{
    public class Captcha //: ICaptcha
    {
        private const string Letters = @"1,2,3,4,5,6,7,8,9,0,
                                         A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z,
                                         a,b,c,d,e,f,g,h,j,k,l,m,n,p,q,r,s,t,u,v,w,x,y,z";
        public static Task<MemoryStream> GenerateCaptchaImageAsync(string captchaCode, int width = 0, int height = 30)
        {
            //验证码颜色集合
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };

            //验证码字体集合
            string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial" };

            //定义图像的大小，生成图像的实例
            var image = new Bitmap(100, height);

            var g = Graphics.FromImage(image);

            //背景设为白色
            g.Clear(Color.Gray);

            var random = new Random();

            for (var i = 0; i < 100; i++)
            {
                var x = random.Next(image.Width);
                var y = random.Next(image.Height);
                g.DrawRectangle(new Pen(Color.LightGray, 0), width == 0 ? captchaCode.Length * 25 : x, y, 1, 1);
            }

            // 绘制干扰线条
            var lines = random.Next(8, 16);
            for (int i = 0; i < lines; i++)
            {
                var x1 = random.Next(20, width == 0 ? captchaCode.Length * 25 : width);
                var y1 = random.Next(width == 0 ? captchaCode.Length * 25 : height);
                var x2 = random.Next(20, width == 0 ? captchaCode.Length * 25 : width);
                var y2 = random.Next(width == 0 ? captchaCode.Length * 25 : height);
                g.DrawLine(new Pen(Color.Black, 0), x1, y1, x2, y2);
            }

            //验证码绘制在g中
            for (var i = 0; i < captchaCode.Length; i++)
            {
                //随机颜色索引值
                var cindex = random.Next(c.Length);

                //随机字体索引值
                var findex = random.Next(fonts.Length);

                //字体
                var f = new Font(fonts[findex], 15, FontStyle.Bold);

                //颜色  
                Brush b = new SolidBrush(c[cindex]);

                var n = 4;
                if ((i + 1) % 2 == 0)
                    n = 2;

                //绘制一个验证字符  
                g.DrawString(captchaCode.Substring(i, 1), f, b, 17 + (i * 17), n);
            }

            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);

            g.Dispose();
            image.Dispose();

            return Task.FromResult(ms);
            // new CaptchaResult
            // {
            //     CaptchaCode = captchaCode,
            //     CaptchaMemoryStream = ms,
            //     Timestamp = DateTime.Now
            // }
        }

        public static Task<string> GenerateRandomCaptchaAsync(int codeLength = 4)
        {
            var array = Letters.Split(new[] { ',' });

            var random = new Random();

            var temp = -1;

            var captcheCode = string.Empty;

            for (int i = 0; i < codeLength; i++)
            {
                if (temp != -1)
                    random = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));

                var index = random.Next(array.Length);

                if (temp != -1 && temp == index)
                    return GenerateRandomCaptchaAsync(codeLength);

                temp = index;

                captcheCode += array[index].Trim();
            }

            return Task.FromResult(captcheCode);
        }
    }
}