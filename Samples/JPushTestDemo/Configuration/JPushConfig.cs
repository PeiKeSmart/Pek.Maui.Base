namespace JPushTestDemo.Configuration;

/// <summary>
/// JPush配置类
/// </summary>
public static class JPushConfig
{
    /// <summary>
    /// JPush应用密钥 - 需要在JPush控制台获取
    /// </summary>
    public const string APP_KEY = "d47b7681630e2d2c3cea43b5";

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
        return APP_KEY == "d1f3429f6d486b93b2133794" ? null : APP_KEY;
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
