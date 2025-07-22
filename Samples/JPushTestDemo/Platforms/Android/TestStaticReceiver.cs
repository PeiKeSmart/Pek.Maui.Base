using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using AndroidX.Core.App;

namespace JPushTestDemo.Platforms.Android
{
    /// <summary>
    /// 测试静态广播接收器
    /// 用于验证静态广播接收是否正常工作
    /// </summary>
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter(new[] { 
        Intent.ActionBootCompleted,
        Intent.ActionPackageReplaced,
        "android.intent.action.MY_PACKAGE_REPLACED"
    })]
    public class TestStaticReceiver : BroadcastReceiver
    {
        private const string TAG = "TestStaticReceiver";

        public override void OnReceive(Context? context, Intent? intent)
        {
            if (context == null || intent == null)
                return;

            try
            {
                string action = intent.Action ?? "";
                Log.Info(TAG, $"[测试接收器] 收到系统广播: {action}");

                // 创建测试通知
                CreateTestNotification(context, $"系统广播: {action}");
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"[测试接收器] 处理广播失败: {ex.Message}", ex);
            }
        }

        private void CreateTestNotification(Context context, string message)
        {
            try
            {
                const string channelId = "test_static_channel";
                const int notificationId = 3001;

                // 创建通知渠道
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    var channel = new NotificationChannel(
                        channelId,
                        "测试静态广播",
                        NotificationImportance.High)
                    {
                        Description = "测试静态广播接收器是否工作"
                    };

                    var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
                    notificationManager?.CreateNotificationChannel(channel);
                }

                // 创建通知
                var notification = new NotificationCompat.Builder(context, channelId)
                    .SetContentTitle("静态广播测试")
                    .SetContentText(message)
                    .SetSmallIcon(17301632)
                    .SetAutoCancel(true)
                    .SetPriority(NotificationCompat.PriorityHigh)
                    .Build();

                var notificationManager2 = context.GetSystemService(Context.NotificationService) as NotificationManager;
                notificationManager2?.Notify(notificationId, notification);

                Log.Info(TAG, "[测试接收器] 测试通知创建成功");
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"[测试接收器] 创建测试通知失败: {ex.Message}", ex);
            }
        }
    }
}