using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Pek.Message;

public static class MessageBox
{
    public static String ALERT_TITLE { get; set; } = "湖北登灏";

    /// <summary>
    /// 弹出提示信息,约2秒左右会消失
    /// </summary>
    /// <param name="Message"></param>
    /// <returns></returns>
    public static async Task DHToast(String Message)
    {
        var cancellationTokenSource = new CancellationTokenSource();

        var duration = ToastDuration.Short;
        double fontSize = 14;

        var toast = Toast.Make(Message, duration, fontSize);

        await toast.Show(cancellationTokenSource.Token);
    }

    /// <summary>
    /// 弹出提示信息，约3.5秒左右会消失
    /// </summary>
    /// <param name="Message"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task DHToast(String Message, CancellationToken cancellationToken)
    {
        var duration = ToastDuration.Long;
        double fontSize = 14;

        var toast = Toast.Make(Message, duration, fontSize);

        await toast.Show(cancellationToken);
    }

    /// <summary>
    /// 在调试控制台中编写调试消息
    /// </summary>
    /// <param name="message">要显示的消息</param>
    public static void Debug(string message) => System.Diagnostics.Debug.WriteLine(message);

    /// <summary>
    /// 显示消息
    /// </summary>
    /// <param name="message">要显示的消息</param>
    /// <param name="title">消息标题</param>
    /// <param name="page">页面</param>
    /// <returns>要执行的任务</returns>
    public static Task ShowAlert(this Page page, String message, String? title = null) => page.DisplayAlert(title.IsNullOrWhiteSpace() ? ALERT_TITLE : title, message, "OK");

}
