using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using AndroidX.Core.App;

namespace JPushTestDemo.Platforms.Android
{
    /// <summary>
    /// JPush静态广播接收器
    /// 专门处理应用未启动时的推送消息接收
    /// 这个接收器可以在应用完全关闭的情况下被系统唤醒
    /// </summary>
    [BroadcastReceiver(
        Enabled = true, 
        Exported = true,
        Name = "jpushtestdemo.platforms.android.JPushStaticReceiver")]
    [IntentFilter(new[] { 
        "cn.jpush.android.intent.NOTIFICATION_RECEIVED_PROXY",
        "cn.jpush.android.intent.NOTIFICATION_OPENED_PROXY",
        "cn.jpush.android.intent.SERVICE_MESSAGE"
    }, Categories = new[] { "net.peikesmart.test" })]
    public class JPushStaticReceiver : BroadcastReceiver
    {
        private const string TAG = "JPushStaticReceiver";

        public override void OnReceive(Context? context, Intent? intent)
        {
            if (context == null || intent == null)
                return;

            try
            {
                string action = intent.Action ?? "";
                
                // 记录到持久化日志 - 关键诊断信息！
                PersistentLogger.LogEvent(context, "JPush静态广播", $"Action: {action}");

                // 记录所有接收到的Intent数据
                LogIntentData(context, intent);

                switch (action)
                {
                    case "cn.jpush.android.intent.NOTIFICATION_RECEIVED_PROXY":
                        HandleNotificationReceived(context, intent);
                        break;
                    case "cn.jpush.android.intent.NOTIFICATION_OPENED_PROXY":
                        HandleNotificationOpened(context, intent);
                        break;
                    case "cn.jpush.android.intent.SERVICE_MESSAGE":
                        HandleCustomMessage(context, intent);
                        break;
                }
                
                PersistentLogger.LogDiagnostic(context, TAG, "JPush广播处理完成");
            }
            catch (System.Exception ex)
            {
                PersistentLogger.LogError(context, TAG, "JPush广播处理失败", ex);
            }
        }

        private void LogIntentData(Context context, Intent intent)
        {
            try
            {
                var extras = intent.Extras;
                if (extras != null)
                {
                    PersistentLogger.LogDiagnostic(context, TAG, "Intent数据:");
                    foreach (string key in extras.KeySet())
                    {
                        var value = extras.Get(key);
                        PersistentLogger.LogDiagnostic(context, TAG, $"  {key} = {value}");
                    }
                }
            }
            catch (System.Exception ex)
            {
                PersistentLogger.LogError(context, TAG, "记录Intent数据失败", ex);
            }
        }

        private void HandleNotificationReceived(Context context, Intent intent)
        {
            try
            {
                Log.Info(TAG, "[静态接收器] 处理通知接收事件");
                
                // 获取推送数据
                var extras = intent.Extras;
                if (extras != null)
                {
                    string title = extras.GetString("cn.jpush.android.TITLE", "");
                    string content = extras.GetString("cn.jpush.android.MESSAGE", "");
                    string extra = extras.GetString("cn.jpush.android.EXTRA", "");
                    
                    Log.Info(TAG, $"[静态接收器] 标题: {title}");
                    Log.Info(TAG, $"[静态接收器] 内容: {content}");
                    Log.Info(TAG, $"[静态接收器] 附加数据: {extra}");
                    
                    // 记录到持久化日志
                    PersistentLogger.LogPushNotification(context, "通知接收", title, content, extra);
                    
                    // 创建本地通知（简化逻辑，总是创建通知）
                    CreateLocalNotification(context, title, content);
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"[静态接收器] 处理通知接收事件失败: {ex.Message}", ex);
                PersistentLogger.LogError(context, TAG, "处理通知接收事件失败", ex);
            }
        }

        private void HandleNotificationOpened(Context context, Intent intent)
        {
            try
            {
                Log.Info(TAG, "[静态接收器] 处理通知点击事件");
                
                // 启动应用
                var launchIntent = context.PackageManager?.GetLaunchIntentForPackage(context.PackageName);
                if (launchIntent != null)
                {
                    launchIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTop);
                    context.StartActivity(launchIntent);
                    Log.Info(TAG, "[静态接收器] 应用启动成功");
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"[静态接收器] 处理通知点击事件失败: {ex.Message}", ex);
            }
        }

        private void HandleCustomMessage(Context context, Intent intent)
        {
            try
            {
                Log.Info(TAG, "[静态接收器] 处理自定义消息");
                
                var extras = intent.Extras;
                if (extras != null)
                {
                    string message = extras.GetString("cn.jpush.android.MESSAGE", "");
                    string extra = extras.GetString("cn.jpush.android.EXTRA", "");
                    
                    Log.Info(TAG, $"[静态接收器] 自定义消息: {message}");
                    Log.Info(TAG, $"[静态接收器] 附加数据: {extra}");
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"[静态接收器] 处理自定义消息失败: {ex.Message}", ex);
            }
        }



        private void CreateLocalNotification(Context context, string title, string content)
        {
            try
            {
                const string channelId = "jpush_static_channel";
                const int notificationId = 2001;

                // 创建通知渠道
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    var channel = new NotificationChannel(
                        channelId,
                        "JPush推送通知",
                        NotificationImportance.High)
                    {
                        Description = "接收JPush推送消息"
                    };

                    var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
                    notificationManager?.CreateNotificationChannel(channel);
                }

                // 创建点击意图
                var launchIntent = context.PackageManager?.GetLaunchIntentForPackage(context.PackageName);
                var pendingIntent = PendingIntent.GetActivity(
                    context, 
                    0, 
                    launchIntent, 
                    PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

                // 创建通知
                var notification = new NotificationCompat.Builder(context, channelId)
                    .SetContentTitle(title)
                    .SetContentText(content)
                    .SetSmallIcon(17301632) // 系统图标
                    .SetAutoCancel(true)
                    .SetContentIntent(pendingIntent)
                    .SetPriority(NotificationCompat.PriorityHigh)
                    .Build();

                var notificationManager2 = context.GetSystemService(Context.NotificationService) as NotificationManager;
                notificationManager2?.Notify(notificationId, notification);

                Log.Info(TAG, "[静态接收器] 本地通知创建成功");
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"[静态接收器] 创建本地通知失败: {ex.Message}", ex);
            }
        }
    }
}