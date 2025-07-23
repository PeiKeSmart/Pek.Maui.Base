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
    [IntentFilter(new[] { 
        Intent.ActionBootCompleted,
        "com.test.static.broadcast",
        "android.intent.action.TEST_BROADCAST"
    })]
    public class SimpleTestReceiver : BroadcastReceiver
    {
        private const string TAG = "SimpleTestReceiver";

        public override void OnReceive(Context? context, Intent? intent)
        {
            if (context == null || intent == null)
                return;

            try
            {
                string action = intent.Action ?? "";
                
                // 记录到持久化日志 - 这是关键！
                PersistentLogger.LogEvent(context, "静态广播接收", $"Action: {action}");
                
                string testData = intent.GetStringExtra("test_data") ?? "";
                if (!string.IsNullOrEmpty(testData))
                {
                    PersistentLogger.LogDiagnostic(context, TAG, $"测试数据: {testData}");
                }
                
                // 创建通知
                CreateSimpleNotification(context, action, testData);
                
                PersistentLogger.LogDiagnostic(context, TAG, "静态广播处理完成");
            }
            catch (System.Exception ex)
            {
                PersistentLogger.LogError(context, TAG, "静态广播处理失败", ex);
            }
        }

        private void CreateSimpleNotification(Context context, string action, string testData)
        {
            try
            {
                const string channelId = "simple_test_channel";
                
                // 创建通知渠道
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    var channel = new NotificationChannel(
                        channelId,
                        "静态广播测试",
                        NotificationImportance.High);

                    var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
                    notificationManager?.CreateNotificationChannel(channel);
                }

                string title = "静态广播正常工作";
                string content = string.IsNullOrEmpty(testData) ? $"收到: {action}" : testData;

                // 创建通知
                var notification = new NotificationCompat.Builder(context, channelId)
                    .SetContentTitle(title)
                    .SetContentText(content)
                    .SetSmallIcon(17301632)
                    .SetAutoCancel(true)
                    .Build();

                var notificationManager2 = context.GetSystemService(Context.NotificationService) as NotificationManager;
                notificationManager2?.Notify(4001, notification);

                Log.Info(TAG, $"✅ 静态广播通知创建成功: {action}");
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"创建静态广播通知失败: {ex.Message}", ex);
            }
        }
    }
}