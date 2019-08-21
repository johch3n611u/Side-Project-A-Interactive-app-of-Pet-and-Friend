using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
using System.Web.SessionState;

namespace petpettest.QueenTyphoonContent
{
    /// <summary>
    /// _40GetAuthImg 的摘要描述
    /// </summary>
    public class VerificationImg : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            int NumCount = 6;
            string strNumber = GetARandomNumber(NumCount);
            context.Session["VerificationImgNumber"] = strNumber;

            //產生一個圖片
            Bitmap img = new Bitmap(strNumber.Length * 25, 40);
            //用bitmap產生一個Graphics物件
            Graphics g = Graphics.FromImage(img);

            Random r = new Random();
            int intRed = r.Next(0, 256);
            int intGreen = r.Next(0, 256);
            int intBlue = r.Next(0, 256);

            g.Clear(Color.FromArgb(1, intRed, intGreen, intBlue));


            //畫噪音線
            for (int i = 0; i < 60; i++)
            {
                int x1 = r.Next(img.Width);
                int y1 = r.Next(img.Height);
                int x2 = r.Next(img.Width);
                int y2 = r.Next(img.Height);

                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }

            //用矩形物件產生一個矩形
            Rectangle MyRect = new Rectangle(0, 0, img.Width, img.Height);
            intRed = r.Next(0, 256);
            intGreen = r.Next(0, 256);
            intBlue = r.Next(0, 256);
            Color color1 = Color.FromArgb(intRed, intGreen, intBlue);

            intRed = r.Next(0, 256);
            intGreen = r.Next(0, 256);
            intBlue = r.Next(0, 256);
            Color color2 = Color.FromArgb(intRed, intGreen, intBlue);

            //文字的顏色(漸層色)
            System.Drawing.Drawing2D.LinearGradientBrush brush =
                new System.Drawing.Drawing2D.LinearGradientBrush(MyRect, color1, color2, 1.2f);

            //設定字型
            Font font = new Font("Arial Black", 20, FontStyle.Bold);

            //把字畫在矩形框中
            g.DrawString(strNumber, font, brush, 5, 5);

            //噪音點
            for (int i = 0; i < 300; i++)
            {
                int x1 = r.Next(img.Width);
                int y1 = r.Next(img.Height);

                img.SetPixel(x1, y1, Color.FromArgb(r.Next(256)));
            }

            //畫外框線
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, img.Width - 1, img.Height - 1);

            //把Bitmap放入Image物件中
            Image image = img;

            //將圖片存成JPG格式置於記憶體中
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            //output
            context.Response.Clear();
            context.Response.ContentType = "image/jpeg";
            context.Response.BinaryWrite(ms.ToArray());

        }


        private string GetARandomNumber(int n)
        {
            string strNumber = "";
            Random r = new Random();
            for (int i = 0; i < n; i++)
            {
                strNumber += r.Next(0, 10);
            }

            return strNumber;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}