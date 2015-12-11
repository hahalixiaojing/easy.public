using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;

namespace Easy.Public.ImageHelper
{
    public static class ImageScaleUtility
    {
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="extension"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static Image Scale(String imagePath, String extension, ImageScaleParameter parameter)
        {
            if (!File.Exists(imagePath)) throw new FileNotFoundException(imagePath);
            Image image = Bitmap.FromFile(imagePath);

            return ImageScaleUtility.Scale(image, extension, parameter);
        }
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="extension"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static Image Scale(Image bitmap, String extension, ImageScaleParameter parameter)
        {
            Size newSize = GetScaleImageSize(bitmap.Size, parameter.ScaleSize, parameter.ScaleType);
            Image thumbnailImage = new Bitmap(newSize.Width, newSize.Height);
            using (Graphics g = Graphics.FromImage(thumbnailImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bitmap, new Rectangle(0, 0, newSize.Width, newSize.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel);
            }
            return thumbnailImage;
        }

        private static Size GetScaleImageSize(Size originalSize, Size maxScaleSize, ScaleType type)
        {
            if (type == ScaleType.HeightAndWidth)
            {
                return ImageScaleUtility.GetScaleImageSize(originalSize, maxScaleSize);
            }
            else if (type == ScaleType.Width)
            {
                Int32 scaleWith = maxScaleSize.Width;
                if (originalSize.Width < maxScaleSize.Width)
                {
                    scaleWith = originalSize.Width;
                }
                return new Size(scaleWith, scaleWith * originalSize.Height / originalSize.Width);
            }
            else
            {//ScaleType.Height
                Int32 scaleHeight = maxScaleSize.Height;
                if (originalSize.Height < maxScaleSize.Height)
                {
                    scaleHeight = originalSize.Height;
                }
                return new Size(scaleHeight * originalSize.Width / originalSize.Height, scaleHeight);
            }
        }
        /// <summary>
        /// 计算缩略图尺寸
        /// </summary>
        /// <param name="originalSize"></param>
        /// <param name="maxScaleSize"></param>
        /// <returns></returns>
        private static Size GetScaleImageSize(Size originalSize, Size maxScaleSize)
        {

            Int32 scaleWidth = 0;
            Int32 scaleHeight = 0;
            if (originalSize.Width > maxScaleSize.Width)
            {
                scaleWidth = maxScaleSize.Width;
                scaleHeight = maxScaleSize.Width * originalSize.Height / originalSize.Width;
                if (scaleHeight > maxScaleSize.Height)
                {
                    scaleWidth = maxScaleSize.Height * scaleWidth / scaleHeight;
                    scaleHeight = maxScaleSize.Height;
                }
            }
            else if (originalSize.Height > maxScaleSize.Height)
            {
                scaleHeight = maxScaleSize.Height;
                scaleWidth = maxScaleSize.Height * originalSize.Width / originalSize.Height;
                if (scaleWidth > maxScaleSize.Width)
                {
                    scaleHeight = maxScaleSize.Width * scaleHeight / scaleWidth;
                    scaleWidth = maxScaleSize.Width;
                }
            }
            else
            {
                return originalSize;
            }
            return new Size(scaleWidth, scaleHeight);
        }
    }
}
