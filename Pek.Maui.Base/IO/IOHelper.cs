using System.Globalization;

namespace Pek;

/// <summary>IO工具类</summary>
public static class IOHelper
{
    #region 复制数据流
    /// <summary>复制数组</summary>
    /// <param name="src">源数组</param>
    /// <param name="offset">起始位置。一般从0开始</param>
    /// <param name="count">复制字节数。用-1表示截取剩余所有数据</param>
    /// <returns>返回复制的总字节数</returns>
    public static Byte[] ReadBytes(this Byte[] src, Int32 offset, Int32 count)
    {
        if (count == 0) return [];

        // 即使是全部，也要复制一份，而不只是返回原数组，因为可能就是为了复制数组
        if (count < 0) count = src.Length - offset;

        var bts = new Byte[count];
        Buffer.BlockCopy(src, offset, bts, 0, bts.Length);
        return bts;
    }
    #endregion

    #region 十六进制编码
    /// <summary>把字节数组编码为十六进制字符串</summary>
    /// <param name="data">字节数组</param>
    /// <param name="offset">偏移</param>
    /// <param name="count">数量。超过实际数量时，使用实际数量</param>
    /// <returns></returns>
    public static String ToHex(this Byte[]? data, Int32 offset = 0, Int32 count = -1)
    {
        if (data == null || data.Length <= 0) return "";

        if (count < 0)
            count = data.Length - offset;
        else if (offset + count > data.Length)
            count = data.Length - offset;
        if (count == 0) return "";

        //return BitConverter.ToString(data).Replace("-", null);
        // 上面的方法要替换-，效率太低
        var cs = new Char[count * 2];
        // 两个索引一起用，避免乘除带来的性能损耗
        for (Int32 i = 0, j = 0; i < count; i++, j += 2)
        {
            var b = data[offset + i];
            cs[j] = GetHexValue(b >> 4);
            cs[j + 1] = GetHexValue(b & 0x0F);
        }
        return new String(cs);
    }

    /// <summary>1个字节转为2个16进制字符</summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public static String ToHex(this Byte b)
    {
        //Convert.ToString(b, 16);
        var cs = new Char[2];
        var ch = b >> 4;
        var cl = b & 0x0F;
        cs[0] = (Char)(ch >= 0x0A ? ('A' + ch - 0x0A) : ('0' + ch));
        cs[1] = (Char)(cl >= 0x0A ? ('A' + cl - 0x0A) : ('0' + cl));

        return new String(cs);
    }

    private static Char GetHexValue(Int32 i) => i < 10 ? (Char)(i + '0') : (Char)(i - 10 + 'A');

    /// <summary>解密</summary>
    /// <param name="data">Hex编码的字符串</param>
    /// <param name="startIndex">起始位置</param>
    /// <param name="length">长度</param>
    /// <returns></returns>
    public static Byte[] ToHex(this String? data, Int32 startIndex = 0, Int32 length = -1)
    {
        if (data.IsNullOrEmpty()) return [];

        // 过滤特殊字符
        data = data.Trim()
            .Replace("-", null)
            .Replace("0x", null)
            .Replace("0X", null)
            .Replace(" ", null)
            .Replace("\r", null)
            .Replace("\n", null)
            .Replace(",", null);

        if (length <= 0) length = data.Length - startIndex;

        var bts = new Byte[length / 2];
        for (var i = 0; i < bts.Length; i++)
        {
            bts[i] = Byte.Parse(data.Substring(startIndex + 2 * i, 2), NumberStyles.HexNumber);
        }
        return bts;
    }
    #endregion
}
