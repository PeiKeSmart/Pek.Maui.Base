using System.Collections;
using System.Text;

using SkiaSharp;

namespace Pek.Images;

public static class ImageHelper
{
    //二值图 （也叫单值图像）每个像素一个bit ，即黑白图像，每个像素点的值非 0 即 1。
    //灰度图像 每个像素8bit，范围从 0 ~ 255. 具有调色板，像素值是表项入口。
    //伪彩图像 每个像素8bit，范围从0-255.具有调色板，像素值是表项入口。
    //真彩图像 每个像素 24bit ，每个像素由独立的 R，G，B 分量组成，每个分量各占8bit。

    /// <summary>
    /// 图像灰度处理
    /// </summary>
    /// <param name="img"></param>
    /// <returns></returns>
    public static SKBitmap ToGray(SKBitmap img)
    {
        for (var i = 0; i < img.Width; i++)
        {
            for (var j = 0; j < img.Height; j++)
            {
                var color = img.GetPixel(i, j);
                //计算灰度值
                var gray = (byte)(color.Red * 0.3 + color.Green * 0.59 + color.Blue * 0.11);
                var newColor = new SKColor(gray, gray, gray);
                //修改该像素点的RGB的颜色
                img.SetPixel(i, j, newColor);
            }
        }
        return img;
    }

    /// <summary>
    /// 黑白二值化
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    public static SKBitmap Binaryzation(SKBitmap image)
    {
        image = ToGray(image);//先灰度处理
        var threshold = 180;//定义阈值
        for (var i = 0; i < image.Width; i++)
        {
            for (var j = 0; j < image.Height; j++)
            {
                //获取该像素点的RGB的颜色
                var color = image.GetPixel(i, j);
                //计算颜色,大于平均值为黑,小于平均值为白
                var newColor = color.Blue < threshold ? new SKColor(0, 0, 0) : new SKColor(255, 255, 255);

                //修改该像素点的RGB的颜色
                image.SetPixel(i, j, newColor);
            }
        }
        return image;
    }

    /// <summary>
    /// 图像灰度处理和黑白二值化
    /// </summary>
    /// <param name="image"></param>
    /// <param name="threshold">定义阈值</param>
    /// <returns></returns>
    public static SKBitmap DHBinaryzation(SKBitmap image, Int32 threshold = 180)
    {
        var width = image.Width;
        var height = image.Height;

        // 创建一个新的 Bitmap 来存储处理结果
        var resultImage = new SKBitmap(width, height);

        // 获取原始图像的像素数据
        var originalPixels = image.Pixels;
        var resultPixels = resultImage.Pixels;

        // 批量处理像素
        for (var i = 0; i < width * height; i++)
        {
            var color = originalPixels[i];

            // 计算灰度值
            var gray = (byte)(color.Red * 0.3 + color.Green * 0.59 + color.Blue * 0.11);

            // 二值化处理
            var newColor = gray < threshold ? new SKColor(0, 0, 0) : new SKColor(255, 255, 255);

            // 修改该像素点的RGB的颜色
            resultPixels[i] = newColor;
        }

        return resultImage; // 返回新的二值化图像
    }

    /// <summary>
    /// 图像灰度处理和黑白二值化
    /// </summary>
    /// <param name="image"></param>
    /// <param name="threshold">定义阈值</param>
    /// <returns></returns>
    public static IList<Int32> ProcessBitArray(SKBitmap image, Int32 threshold = 180)
    {
        var width = image.Width;
        var height = image.Height;

        // 创建一个新的 Bitmap 来存储处理结果
        var resultImage = new SKBitmap(width, height);

        // 获取原始图像的像素数据
        var originalPixels = image.Pixels;

        var list = new List<Int32>();

        // 批量处理像素
        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height; j++)
            {
                var color = image.GetPixel(i, j);

                // 计算灰度值
                var gray = (byte)(color.Red * 0.3 + color.Green * 0.59 + color.Blue * 0.11);

                // 二值化处理
                var newColor = gray < threshold ? new SKColor(0, 0, 0) : new SKColor(255, 255, 255);

                if (newColor.Red > 0 || newColor.Green > 0 || newColor.Blue > 0)
                {
                    list.Add(1);
                }
                else
                {
                    list.Add(0);
                }
            }
        }

        return list;
    }

    /// <summary>
    /// Bit 高底位转换,反转整个byte 中的 8位
    /// </summary>
    /// <param name="oldBit">需要转换的byte[]</param>
    /// <param name="newBit">转换到的新byte[]</param>
    /// <returns>返回反转的数据</returns>
    public static byte[] BitReverse(BitArray oldBit, byte[] newBit)
    {
        var bitArray = new BitArray(new byte[1]);
        for (var i = 0; i < oldBit.Count / 8; i++)
        {
            for (var j = 0; j < 8; j++)
            {
                bitArray[j] = oldBit[i * 8 + (7 - j)];
            }

            bitArray.CopyTo(newBit, i);
        }

        return newBit;
    }

    /// <summary>
    /// 十六进制转字节数组
    /// </summary>
    /// <param name="hexString">十六进制字符</param>
    /// <returns></returns>
    public static byte[] StrToToHexByte(string hexString)
    {
        hexString = hexString.Replace(" ", "");
        if (hexString.Length % 2 != 0)
        {
            hexString += " ";
        }

        var array = new byte[hexString.Length / 2];
        for (var i = 0; i < array.Length; i++)
        {
            array[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        }

        return array;
    }

    /// <summary>
    /// 字节数组转16进制字符串：空格分隔
    /// </summary>
    /// <param name="byteDatas">字节数组</param>
    /// <returns></returns>
    public static string ToHexStrFromByte(this byte[] byteDatas)
    {
        var stringBuilder = new StringBuilder();
        for (var i = 0; i < byteDatas.Length; i++)
        {
            stringBuilder.Append($"{byteDatas[i]:X2} ");
        }

        return stringBuilder.ToString().Trim();
    }
}