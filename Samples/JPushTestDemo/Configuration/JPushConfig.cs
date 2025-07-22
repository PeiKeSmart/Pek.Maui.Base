namespace JPushTestDemo.Configuration;

/// <summary>
/// JPush配置类
/// </summary>
public static class JPushConfig
{
    /// <summary>
    /// JPush应用密钥 - 需要在JPush控制台获取
    /// </summary>
    public const string APP_KEY = "YOUR_JPUSH_APP_KEY_HERE";

    /// <summary>
    /// 调试模式开关
    /// </summary>
    public const bool DEBUG_MODE = true;

    /// <summary>
    /// 频道（Android 8.0+需要）
    /// </summary>
    public const string CHANNEL = "default_channel";

    /// <summary>
    /// 获取JPush应用密钥
    /// </summary>
    /// <returns>如果配置了返回密钥，否则返回null</returns>
    public static string? GetAppKey()
    {
        return APP_KEY == "YOUR_JPUSH_APP_KEY_HERE" ? null : APP_KEY;
    }

    /// <summary>
    /// 检查是否已配置AppKey
    /// </summary>
    /// <returns>true表示已配置，false表示未配置</returns>
    public static bool IsAppKeyConfigured()
    {
        return !string.IsNullOrEmpty(GetAppKey());
    }
}
