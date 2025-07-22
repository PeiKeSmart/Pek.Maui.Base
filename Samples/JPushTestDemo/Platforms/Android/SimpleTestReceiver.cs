using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using AndroidX.Core.App;

namespace JPushTestDemo.Platforms.Android
{
    /// <summary>
    /// 简单测试接收器
    /// 测试最基本的静态广播功能
    /// </summary>
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class SimpleTestReceiver : BroadcastReceiver
    {
        private const string TAG = "SimpleTestReceiver";

        public override void OnReceive(Context? context, Intent? intent)
        {
            if (context == null || intent == null)
                return;

            try
            {
                Log.Info(TAG, "简单测试接收器被触发");
                
                // 创建简单通知
                CreateSimpleNotification(context);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"简单测试接收器失败: {ex.Message}", ex);
            }
        }

        private void CreateSimpleNotification(Context context)
        {
            try
            {
                const string channelId = "simple_test_channel";
                
                // 创建通知渠道
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    var channel = new NotificationChannel(
                        channelId,
                        "简单测试",
                        NotificationImportance.High);

                    var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
                    notificationManager?.CreateNotificationChannel(channel);
                }

                // 创建通知
                var notification = new NotificationCompat.Builder(context, channelId)
                    .SetContentTitle("静态广播工作正常")
                    .SetContentText("系统重启后收到此通知")
                    .SetSmallIcon(17301632)
                    .SetAutoCancel(true)
                    .Build();

                var notificationManager2 = context.GetSystemService(Context.NotificationService) as NotificationManager;
                notificationManager2?.Notify(4001, notification);

                Log.Info(TAG, "简单测试通知创建成功");
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"创建简单测试通知失败: {ex.Message}", ex);
            }
        }
    }
}