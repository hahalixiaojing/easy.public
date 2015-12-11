using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Easy.Public
{
    public class ValidateCodeHelper
    {
        static ValidateCodeHelper helper = null;
        static object obj = new object();

        private ValidateCodeHelper(Int32 padding, Int32 fontSize, Boolean chaos)
        {
            this.padding = padding;
            this.fontSize = fontSize;
            this.chaos = chaos;
        }

        public static ValidateCodeHelper Instance(Int32 padding, Int32 fontSize, Boolean chaos)
        {
            if (helper == null)
            {
                lock (obj)
                {
                    if (helper == null)
                    {
                        helper = new ValidateCodeHelper(padding, fontSize, chaos);
                    }
                }
            }
            return helper;
        }

        #region 验证码字体大小（为了显示扭曲效果，默认使用40像素，可以自行修改）
        int fontSize = 40;
        /// <summary>
        /// 验证码字体大小（为了显示扭曲效果，默认使用40像素，可以自行修改）
        /// </summary>
        private int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
        #endregion
        int padding = 1;
        /// <summary>
        /// 边框留白（默认1像素）
        /// </summary>
        private int Padding
        {
            get { return padding; }
            set { padding = value; }
        }

        #region 是否输出噪点（默认输出）
        bool chaos = true;
        /// <summary>
        /// 是否输出噪点（默认输出）
        /// </summary>
        public bool Chaos
        {
            get { return chaos; }
            set { chaos = value; }
        }
        #endregion

        Color chaosColor = Color.LightGray;
        /// <summary>
        /// 输出噪点的颜色（默认灰色）
        /// </summary>
        public Color ChaosColor
        {
            get { return chaosColor; }
            set { chaosColor = value; }
        }

        #region 自定义背景色（默认白色）
        Color backgroundColor = Color.White;
        /// <summary>
        /// 自定义背景色（默认白色）
        /// </summary>
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }
        #endregion

        #region 自定义随机颜色数组
        Color[] colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
        /// <summary>
        /// 自定义随机颜色数组
        /// </summary>
        public Color[] Colors
        {
            get { return colors; }
            set { colors = value; }
        }
        #endregion

        string[] fonts = { "Arial", "Georgia" };
        /// <summary>
        /// 自定义字体数组
        /// </summary>
        private string[] Fonts
        {
            get { return fonts; }
            set { fonts = value; }
        }
        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;

        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXdir">如果扭曲，则选择True</param>
        /// <param name="dMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="dPhase"></param>
        /// <returns></returns>
        private System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXdir, double dMultValue, double dPhase)
        {
            System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            //将位图背景填充成白色
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXdir ? (double)destBmp.Height : (double)destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXdir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    //取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXdir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXdir ? j : j + (int)(dy * dMultValue);

                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }
        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Bitmap Create(string code)
        {
            int fSize = FontSize;
            int fWidth = fSize + Padding;

            int imageWidth = (int)(code.Length * fWidth) + 4 + Padding * 2;
            int imageHeight = fSize * 2 + Padding;

            Bitmap image = new Bitmap(imageWidth, imageHeight);

            Graphics g = Graphics.FromImage(image);
            g.Clear(BackgroundColor);

            Random rand = new Random();

            //给背景添加随机生成的噪点
            if (this.Chaos)
            {
                Pen pen = new Pen(ChaosColor, 0);
                int c = code.Length * 10;

                for (int i = 0; i < c; i++)
                {
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);

                    g.DrawRectangle(pen, x, y, 1, 1);
                }
            }

            int left = 0, top = 0, top1 = 1, top2 = 1;

            int n1 = (imageWidth - FontSize - Padding * 2);
            int n2 = 4;
            top1 = n2;
            top2 = n2 * 2;

            Font f;
            Brush b;

            int cindex, findex;

            //随机自己和颜色的验证码字符
            for (int i = 0; i < code.Length; i++)
            {
                cindex = rand.Next(Colors.Length - 1);
                findex = rand.Next(Fonts.Length - 1);

                f = new Font(Fonts[findex], fSize, FontStyle.Bold);
                b = new SolidBrush(Colors[cindex]);

                if (i % 2 == 1)
                {
                    top = top2;
                }
                else
                {
                    top = top1;
                }
                left = i * fWidth;

                g.DrawString(code.Substring(i, 1), f, b, left, top);
            }

            //画一个边框，边框颜色为Color.GainSboro
            g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();

            //产生波形
            image = TwistImage(image, true, 4, 2);

            return image;
        }
    }
}
