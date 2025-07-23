using JPushTestDemo.Services;

namespace JPushTestDemo.Services;

/// <summary>
/// 默认的JPush服务实现（用于非Android平台）
/// </summary>
public class DefaultJPushService : IJPushService
{
    public string GetRegistrationId()
    {
        return "不支持当前平台";
    }

    public void SetAlias(string alias, int sequence = 0)
    {
        System.Diagnostics.Debug.WriteLine($"DefaultJPushService.SetAlias: {alias} (不支持当前平台)");
    }

    public void SetTags(ISet<string> tags, int sequence = 0)
    {
        System.Diagnostics.Debug.WriteLine($"DefaultJPushService.SetTags: {string.Join(", ", tags)} (不支持当前平台)");
    }

    public void DeleteAlias(int sequence = 0)
    {
        System.Diagnostics.Debug.WriteLine("DefaultJPushService.DeleteAlias (不支持当前平台)");
    }

    public void DeleteTags(ISet<string> tags, int sequence = 0)
    {
        System.Diagnostics.Debug.WriteLine($"DefaultJPushService.DeleteTags: {string.Join(", ", tags)} (不支持当前平台)");
    }

    public void CleanTags(int sequence = 0)
    {
        System.Diagnostics.Debug.WriteLine("DefaultJPushService.CleanTags (不支持当前平台)");
    }

    public void GetAlias(int sequence = 0)
    {
        System.Diagnostics.Debug.WriteLine("DefaultJPushService.GetAlias (不支持当前平台)");
    }

    public void GetTags(int sequence = 0)
    {
        System.Diagnostics.Debug.WriteLine("DefaultJPushService.GetTags (不支持当前平台)");
    }

    public void StopPush()
    {
        System.Diagnostics.Debug.WriteLine("DefaultJPushService.StopPush (不支持当前平台)");
    }

    public void ResumePush()
    {
        System.Diagnostics.Debug.WriteLine("DefaultJPushService.ResumePush (不支持当前平台)");
    }

    public bool IsPushStopped()
    {
        System.Diagnostics.Debug.WriteLine("DefaultJPushService.IsPushStopped (不支持当前平台)");
        return true;
    }

    public string CheckPushReceiveStatus()
    {
        return "❌ 当前平台不支持JPush推送服务";
    }

    public string GetBroadcastHistory(int maxLines = 200)
    {
        return "❌ 当前平台不支持广播历史记录功能";
    }

    public void ClearBroadcastHistory()
    {
        System.Diagnostics.Debug.WriteLine("DefaultJPushService.ClearBroadcastHistory (不支持当前平台)");
    }

    public string GetHistoryFileInfo()
    {
        return "❌ 当前平台不支持历史记录文件信息";
    }
}
