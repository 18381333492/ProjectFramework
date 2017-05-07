using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Validate
{
    public class ValidateCodeImgGenerator
    {
        public static async Task<string> RandomCode()
        {
            return await Task.Run(() =>
            {
                int codeCount = Int32.Parse(web.config.WebConfig.LoadElement("imgCodeLength"));
                string randomChar = web.config.WebConfig.LoadElement("validateCodeRandomArray");
                string[] allCharArray = randomChar.Split(new char[] { ',' });

                System.Threading.Thread.Sleep(1);

                string RandomCode = "";
                int n = allCharArray.Length;

                //以时间转为int并用unchecked标识不检查溢出，并对结果按位取反，以此为随机数起始值
                System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));

                Parallel.For(0, codeCount, index =>
                {
                    int rnd = random.Next(0, n);
                    RandomCode += allCharArray[rnd];
                });
                return RandomCode;
            });
        }

        /// <summary>
        /// 获取带随机旋转角度的验证码
        /// </summary>
        /// <param name="randomcode">验证码文本</param>
        /// <returns></returns>
        public static async Task<byte[]> CreateCodeImage(string randomcode)
        {
            return await Task<byte[]>.Run(() =>
            {
                //随机转动角度  
                int randAngle = Int32.Parse(web.config.WebConfig.LoadElement("randomCodeAngle"));
                //图片宽度，字数长度 * 18像素，这个18像素是按到整的
                int mapwidth = (int)(randomcode.Length * 18);
                //生成后返回的字节数组
                byte[] byteRet = null;

                MemoryStream stream = null;
                Bitmap map = null;
                Graphics graph = null;

                try
                {
                    stream = new MemoryStream();
                    map = new Bitmap(mapwidth, 18);//创建图片背景
                    graph = Graphics.FromImage(map);

                    graph.Clear(Color.White);//清除画面，填充背景  

                    //graph.DrawRectangle(new Pen(Color.Silver, 0), 0, 0, map.Width - 1, map.Height - 1);//画一个边框  

                    Random rand = new Random();

                    //验证码旋转，防止机器识别  
                    char[] chars = randomcode.ToCharArray();//拆散字符串成单字符数组  
                    //文字居中
                    StringFormat format = new StringFormat(StringFormatFlags.NoClip);
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    //定义颜色  
                    Color[] c = { Color.Red, Color.Green, Color.Orange };

                    //画图片的背景噪音线  
                    for (int i = 0; i < 2; i++)
                    {
                        int x1 = rand.Next(10);
                        int x2 = rand.Next(map.Width - 10, map.Width);
                        int y1 = rand.Next(map.Height);
                        int y2 = rand.Next(map.Height);

                        graph.DrawLine(new Pen(c[rand.Next(3)]), x1, y1, x2, y2);
                    }

                    for (int i = 0; i < chars.Length; i++)
                    {
                        int cindex = rand.Next(3);                        
                        Font f = new System.Drawing.Font("幼圆", 18, System.Drawing.FontStyle.Regular);//字体样式(参数2为字体大小)  
                        Brush b = new System.Drawing.SolidBrush(c[cindex]);
                        Point dot = new Point(12, 10);
                        float angle = rand.Next(-randAngle, randAngle);//转动的度数  
                        graph.TranslateTransform(dot.X, dot.Y);//移动光标到指定位置  
                        graph.RotateTransform(angle);
                        graph.DrawString(chars[i].ToString(), f, b, 1, 1, format);
                        graph.RotateTransform(-angle);//转回去  
                        graph.TranslateTransform(2, -dot.Y);//移动光标到指定位置  
                    }
                    //生成图片                                  
                    map.Save(stream, ImageFormat.Jpeg);
                    byteRet = stream.ToArray();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (graph != null) { graph.Dispose(); }
                    if (map != null) { map.Dispose(); }
                    if (stream != null) { stream.Dispose(); }
                }
                return byteRet;
            });
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="chkCode"></param>
        /// <returns></returns>
        [Obsolete("暂时没有用这个方法，样式还可以调一下，主要是没得角度调整，用上面那个方法", true)]
        public static Byte[] CreateCodeImage2(string chkCode)
        {
            //颜色列表，用于验证码、噪线、噪点    
            Color[] color = { Color.Black, Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Brown, Color.Brown, Color.DarkOliveGreen };
            //字体列表，用于验证码    
            string[] font = { "Times New Roman", "MS Mincho", "Book Antiqua", "Gungsuh", "PMingLiU", "Impact" };
            Random rnd = new Random();
            Bitmap bmp = new Bitmap(100, 40);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            //画噪线    
            for (int i = 0; i < 3; i++)
            {
                int x1 = rnd.Next(100);
                int y1 = rnd.Next(40);
                int x2 = rnd.Next(100);
                int y2 = rnd.Next(40);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }
            //画验证码字符串    
            for (int i = 0; i < chkCode.Length; i++)
            {
                string fnt = font[rnd.Next(font.Length)];
                Font ft = new Font(fnt, 18);
                Color clr = color[rnd.Next(color.Length)];
                g.DrawString(chkCode[i].ToString(), ft, new SolidBrush(clr), (float)i * 20 + 8, (float)8);
            }
            //画噪点    
            for (int i = 0; i < 20; i++)
            {
                int x = rnd.Next(bmp.Width);
                int y = rnd.Next(bmp.Height);
                Color clr = color[rnd.Next(color.Length)];
                bmp.SetPixel(x, y, clr);
            }

            MemoryStream ms = new MemoryStream();
            try
            {
                bmp.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
            finally
            {
                //显式释放资源    
                bmp.Dispose();
                g.Dispose();
            }
        }

    }
}
