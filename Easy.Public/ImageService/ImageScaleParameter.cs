using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Easy.Public.ImageHelper
{
    public class ImageScaleParameter
    {
        public ImageScaleParameter()
        {
            this.ScaleType = ImageHelper.ScaleType.HeightAndWidth;
        }
        public Size ScaleSize { get; set; }
        public String Name { get; set; }
        /// <summary>
        /// 指定图片的缩放类类型
        /// Width:以指定宽度为参考进行缩放，不考虑高度
        /// Height:以指定高度为参考进行缩放，不考虑宽度
        /// HeightAndWidth:在指定的高度和宽度范围内进行等比缩放
        /// </summary>
        public ScaleType ScaleType { get; set; }
    }

    public enum ScaleType
    {
        Width = 1,
        Height = 2,
        HeightAndWidth = 3
    }
}
