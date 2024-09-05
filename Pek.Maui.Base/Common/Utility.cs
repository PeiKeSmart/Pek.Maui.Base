using System.ComponentModel;

using Pek;

namespace System;

/// <summary>工具类</summary>
/// <remarks>
/// 采用静态架构，允许外部重载工具类的各种实现<seealso cref="DefaultConvert"/>。
/// 所有类型转换均支持默认值，默认值为该default(T)，在转换失败时返回默认值。
/// </remarks>
public static class Utility
{
    #region 类型转换
    /// <summary>类型转换提供者</summary>
    /// <remarks>重载默认提供者<seealso cref="DefaultConvert"/>并赋值给<see cref="Convert"/>可改变所有类型转换的行为</remarks>
    public static DefaultConvert Convert { get; set; } = new DefaultConvert();

    /// <summary>转为整数，转换失败时返回默认值。支持字符串、全角、字节数组（小端）、时间（Unix秒不转UTC）</summary>
    /// <remarks>Int16/UInt32/Int64等，可以先转为最常用的Int32后再二次处理</remarks>
    /// <param name="value">待转换对象</param>
    /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
    /// <returns></returns>
    public static Int32 ToInt(this Object? value, Int32 defaultValue = 0) => Convert.ToInt(value, defaultValue);
    #endregion
}

/// <summary>默认转换</summary>
[EditorBrowsable(EditorBrowsableState.Advanced)]
public class DefaultConvert
{
    private static readonly DateTime _dt1970 = new(1970, 1, 1);
    private static readonly DateTimeOffset _dto1970 = new(new DateTime(1970, 1, 1));

    /// <summary>转为整数，转换失败时返回默认值。支持字符串、全角、字节数组（小端）、时间（Unix秒）</summary>
    /// <param name="value">待转换对象</param>
    /// <param name="defaultValue">默认值。待转换对象无效时使用</param>
    /// <returns></returns>
    public virtual Int32 ToInt(Object? value, Int32 defaultValue)
    {
        if (value is Int32 num) return num;
        if (value == null || value == DBNull.Value) return defaultValue;

        // 支持表单提交的StringValues
        if (value is IList<String> list)
        {
            if (list.Count == 0) return defaultValue;
            value = list.FirstOrDefault(e => !e.IsNullOrEmpty());
            if (value == null) return defaultValue;
        }

        // 特殊处理字符串，也是最常见的
        if (value is String str)
        {
            // 拷贝而来的逗号分隔整数
            str = str.Replace(",", null);
            str = ToDBC(str).Trim();
            return str.IsNullOrEmpty() ? defaultValue : Int32.TryParse(str, out var n) ? n : defaultValue;
        }

        // 特殊处理时间，转Unix秒
#if NET6_0_OR_GREATER
        if (value is DateOnly date) value = date.ToDateTime(TimeOnly.MinValue);
#endif
        if (value is DateTime dt)
        {
            if (dt == DateTime.MinValue) return 0;
            if (dt == DateTime.MaxValue) return -1;

            //// 先转UTC时间再相减，以得到绝对时间差
            //return (Int32)(dt.ToUniversalTime() - _dt1970).TotalSeconds;
            // 保存时间日期由Int32改为UInt32，原截止2038年的范围扩大到2106年
            var n = (dt - _dt1970).TotalSeconds;
            return n >= Int32.MaxValue ? throw new InvalidDataException("Time too long, value exceeds Int32.MaxValue") : (Int32)n;
        }
        if (value is DateTimeOffset dto)
        {
            if (dto == DateTimeOffset.MinValue) return 0;

            //return (Int32)(dto - _dto1970).TotalSeconds;
            var n = (dto - _dto1970).TotalSeconds;
            return n >= Int32.MaxValue ? throw new InvalidDataException("Time too long, value exceeds Int32.MaxValue") : (Int32)n;
        }

        if (value is Byte[] buf)
        {
            if (buf == null || buf.Length <= 0) return defaultValue;

            switch (buf.Length)
            {
                case 1:
                    return buf[0];
                case 2:
                    return BitConverter.ToInt16(buf, 0);
                case 3:
                    return BitConverter.ToInt32([buf[0], buf[1], buf[2], 0], 0);
                case 4:
                    return BitConverter.ToInt32(buf, 0);
                default:
                    break;
            }
        }

        try
        {
            return Convert.ToInt32(value);
        }
        catch { return defaultValue; }
    }

    /// <summary>全角为半角</summary>
    /// <remarks>全角半角的关系是相差0xFEE0</remarks>
    /// <param name="str"></param>
    /// <returns></returns>
    private static String ToDBC(String str)
    {
        var ch = str.ToCharArray();
        for (var i = 0; i < ch.Length; i++)
        {
            // 全角空格
            if (ch[i] == 0x3000)
                ch[i] = (Char)0x20;
            else if (ch[i] is > (Char)0xFF00 and < (Char)0xFF5F)
                ch[i] = (Char)(ch[i] - 0xFEE0);
        }
        return new String(ch);
    }
}