using System;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;

namespace DDNS.Utility
{
    public static class VerifyCodeUtil
    {
        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="codeLength"></param>
        /// <returns></returns>
        public static MemoryStream GenerateCode(out string code, int codeLength = 4)
        {
            code = GetRandomNum(codeLength);
            var random = new Random();
            //验证码颜色集合
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //验证码字体集合
            string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial" };
            //定义图像的大小，生成图像的实例
            var image = new Bitmap(code.Length * 30, 38);
            var g = Graphics.FromImage(image);
            g.Clear(Color.White);//背景设为白色
            for (var i = 0; i < 100; i++)
            {
                var x = random.Next(image.Width);
                var y = random.Next(image.Height);
                g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
            }
            //验证码绘制在g中  
            for (var i = 0; i < code.Length; i++)
            {
                var cindex = random.Next(c.Length);//随机颜色索引值 
                var findex = random.Next(fonts.Length);//随机字体索引值 
                var f = new Font(fonts[findex], 15, FontStyle.Bold);//字体 
                Brush b = new SolidBrush(c[cindex]);//颜色  
                var ii = 4;
                if ((i + 1) % 2 == 0)
                {
                    ii = 2;
                }
                g.DrawString(code.Substring(i, 1), f, b, 20 + (i * 20), ii);//绘制一个验证字符  
            }
            var ms = new MemoryStream();//生成内存流对象
            image.Save(ms, ImageFormat.Jpeg);
            g.Dispose();
            image.Dispose();
            return ms;
        }

        private static string GetRandomNum(int codeLength)
        {
            const string vchar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
            var vcharArray = vchar.Split(new[] { ',' });
            var code = "";
            var temp = -1;
            var rand = new Random();
            for (var i = 1; i < codeLength + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }
                var t = rand.Next(vcharArray.Length);
                if (temp != -1 && temp == t)
                {
                    return GetRandomNum(codeLength);
                }
                temp = t;
                code += vcharArray[t];
            }
            return code;
        }
    }
}