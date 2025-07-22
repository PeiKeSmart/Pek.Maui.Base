namespace JPushTestDemo.Services;

/// <summary>
/// JPush服务接口
/// </summary>
public interface IJPushService
{
    /// <summary>
    /// 获取Registration ID
    /// </summary>
    /// <returns>Registration ID</returns>
    string GetRegistrationId();

    /// <summary>
    /// 设置别名
    /// </summary>
    /// <param name="alias">别名</param>
    /// <param name="sequence">序列号</param>
    void SetAlias(string alias, int sequence = 0);

    /// <summary>
    /// 设置标签
    /// </summary>
    /// <param name="tags">标签集合</param>
    /// <param name="sequence">序列号</param>
    void SetTags(ISet<string> tags, int sequence = 0);

    /// <summary>
    /// 删除别名
    /// </summary>
    /// <param name="sequence">序列号</param>
    void DeleteAlias(int sequence = 0);

    /// <summary>
    /// 删除标签
    /// </summary>
    /// <param name="tags">要删除的标签集合</param>
    /// <param name="sequence">序列号</param>
    void DeleteTags(ISet<string> tags, int sequence = 0);

    /// <summary>
    /// 清除所有标签
    /// </summary>
    /// <param name="sequence">序列号</param>
    void CleanTags(int sequence = 0);

    /// <summary>
    /// 获取别名
    /// </summary>
    /// <param name="sequence">序列号</param>
    void GetAlias(int sequence = 0);

    /// <summary>
    /// 获取标签
    /// </summary>
    /// <param name="sequence">序列号</param>
    void GetTags(int sequence = 0);

    /// <summary>
    /// 停止推送服务
    /// </summary>
    void StopPush();

    /// <summary>
    /// 恢复推送服务
    /// </summary>
    void ResumePush();

    /// <summary>
    /// 检查推送服务是否停止
    /// </summary>
    /// <returns>true表示已停止，false表示正在运行</returns>
    bool IsPushStopped();

    /// <summary>
    /// 检查推送接收状态和配置
    /// </summary>
    /// <returns>推送状态检查结果</returns>
    string CheckPushReceiveStatus();
}
