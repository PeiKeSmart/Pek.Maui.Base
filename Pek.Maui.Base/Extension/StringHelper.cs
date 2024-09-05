using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Pek;

/// <summary>字符串助手类</summary>
/// <remarks>
/// </remarks>
public static class StringHelper
{
    #region 字符串扩展
    /// <summary>忽略大小写的字符串相等比较，判断是否与任意一个待比较字符串相等</summary>
    /// <param name="value">字符串</param>
    /// <param name="strs">待比较字符串数组</param>
    /// <returns></returns>
    public static Boolean EqualIgnoreCase(this String? value, params String?[] strs)
    {
        foreach (var item in strs)
        {
            if (String.Equals(value, item, StringComparison.OrdinalIgnoreCase)) return true;
        }
        return false;
    }

    /// <summary>忽略大小写的字符串开始比较，判断是否与任意一个待比较字符串开始</summary>
    /// <param name="value">字符串</param>
    /// <param name="strs">待比较字符串数组</param>
    /// <returns></returns>
    public static Boolean StartsWithIgnoreCase(this String? value, params String?[] strs)
    {
        if (value == null || String.IsNullOrEmpty(value)) return false;

        foreach (var item in strs)
        {
            if (!String.IsNullOrEmpty(item) && value.StartsWith(item, StringComparison.OrdinalIgnoreCase)) return true;
        }
        return false;
    }

    /// <summary>忽略大小写的字符串结束比较，判断是否以任意一个待比较字符串结束</summary>
    /// <param name="value">字符串</param>
    /// <param name="strs">待比较字符串数组</param>
    /// <returns></returns>
    public static Boolean EndsWithIgnoreCase(this String? value, params String?[] strs)
    {
        if (value == null || String.IsNullOrEmpty(value)) return false;

        foreach (var item in strs)
        {
            if (item != null && value.EndsWith(item, StringComparison.OrdinalIgnoreCase)) return true;
        }
        return false;
    }

    /// <summary>指示指定的字符串是 null 还是 String.Empty 字符串</summary>
    /// <param name="value">字符串</param>
    /// <returns></returns>
    public static Boolean IsNullOrEmpty([NotNullWhen(false)] this String? value) => value == null || value.Length <= 0;

    /// <summary>是否空或者空白字符串</summary>
    /// <param name="value">字符串</param>
    /// <returns></returns>
    public static Boolean IsNullOrWhiteSpace([NotNullWhen(false)] this String? value)
    {
        if (value != null)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (!Char.IsWhiteSpace(value[i])) return false;
            }
        }
        return true;
    }

    /// <summary>拆分字符串，过滤空格，无效时返回空数组</summary>
    /// <param name="value">字符串</param>
    /// <param name="separators">分组分隔符，默认逗号分号</param>
    /// <returns></returns>
    public static String[] Split(this String? value, params String[] separators)
    {
        //!! netcore3.0中新增Split(String? separator, StringSplitOptions options = StringSplitOptions.None)，优先于StringHelper扩展
        if (value == null || String.IsNullOrEmpty(value)) return [];
        if (separators == null || separators.Length <= 0 || separators.Length == 1 && separators[0].IsNullOrEmpty()) separators = [",", ";"];

        return value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
    }

    /// <summary>拆分字符串成为整型数组，默认逗号分号分隔，无效时返回空数组</summary>
    /// <remarks>过滤空格、过滤无效、不过滤重复</remarks>
    /// <param name="value">字符串</param>
    /// <param name="separators">分组分隔符，默认逗号分号</param>
    /// <returns></returns>
    public static Int32[] SplitAsInt(this String? value, params String[] separators)
    {
        if (value == null || String.IsNullOrEmpty(value)) return [];
        if (separators == null || separators.Length <= 0) separators = [",", ";"];

        var ss = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        var list = new List<Int32>();
        foreach (var item in ss)
        {
            if (!Int32.TryParse(item.Trim(), out var id)) continue;

            // 本意只是拆分字符串然后转为数字，不应该过滤重复项
            //if (!list.Contains(id))
            list.Add(id);
        }

        return list.ToArray();
    }

    /// <summary>追加分隔符字符串，忽略开头，常用于拼接</summary>
    /// <param name="sb">字符串构造者</param>
    /// <param name="separator">分隔符</param>
    /// <returns></returns>
    public static StringBuilder Separate(this StringBuilder sb, String separator)
    {
        if (/*sb == null ||*/ String.IsNullOrEmpty(separator)) return sb;

        if (sb.Length > 0) sb.Append(separator);

        return sb;
    }

    /// <summary>字符串转数组</summary>
    /// <param name="value">字符串</param>
    /// <param name="encoding">编码，默认utf-8无BOM</param>
    /// <returns></returns>
    public static Byte[] GetBytes(this String? value, Encoding? encoding = null)
    {
        //if (value == null) return null;
        if (String.IsNullOrEmpty(value)) return [];

        encoding ??= Encoding.UTF8;
        return encoding.GetBytes(value);
    }

    /// <summary>指定输入是否匹配目标表达式，支持*匹配</summary>
    /// <param name="pattern">匹配表达式</param>
    /// <param name="input">输入字符串</param>
    /// <param name="comparisonType">字符串比较方式</param>
    /// <returns></returns>
    public static Boolean IsMatch(this String pattern, String input, StringComparison comparisonType = StringComparison.CurrentCulture)
    {
        if (pattern.IsNullOrEmpty()) return false;

        // 单独*匹配所有，即使输入字符串为空
        if (pattern == "*") return true;
        if (input.IsNullOrEmpty()) return false;

        // 普通表达式，直接包含
        var p = pattern.IndexOf('*');
        if (p < 0) return String.Equals(input, pattern, comparisonType);

        // 表达式分组
        var ps = pattern.Split('*');

        // 头尾专用匹配
        if (ps.Length == 2)
        {
            if (p == 0) return input.EndsWith(ps[1], comparisonType);
            if (p == pattern.Length - 1) return input.StartsWith(ps[0], comparisonType);
        }

        // 逐项跳跃式匹配
        p = 0;
        for (var i = 0; i < ps.Length; i++)
        {
            // 最后一组反向匹配
            if (i == ps.Length - 1)
                p = input.LastIndexOf(ps[i], input.Length - 1, input.Length - p, comparisonType);
            else
                p = input.IndexOf(ps[i], p, comparisonType);
            if (p < 0) return false;

            // 第一组必须开头
            if (i == 0 && p > 0) return false;

            p += ps[i].Length;
        }

        // 最后一组*允许不到边界
        if (ps[^1].IsNullOrEmpty()) return p <= input.Length;

        // 最后一组必须结尾
        return p == input.Length;
    }
    #endregion
}
