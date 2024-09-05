using System.Diagnostics.CodeAnalysis;

namespace Pek;

/// <summary>字符串助手类</summary>
/// <remarks>
/// </remarks>
public static class StringHelper
{
    #region 字符串扩展
    /// <summary>指示指定的字符串是 null 还是 String.Empty 字符串</summary>
    /// <param name="value">字符串</param>
    /// <returns></returns>
    public static Boolean IsNullOrEmpty([NotNullWhen(false)] this String? value) => value == null || value.Length <= 0;
    #endregion
}
