using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;

namespace JPushTestDemo.Platforms.Android
{
    /// <summary>
    /// JPush保活前台服务
    /// 用于在应用后台运行时保持JPush连接活跃
    /// </summary>
    [Service(Enabled = true, Exported = false)]
    public class JPushKeepAliveService : Service
    {
        private const int NOTIFICATION_ID = 1001;
        private const string CHANNEL_ID = "jpush_keepalive_channel";
        private const string TAG = "JPushKeepAliveService";

        public override IBinder? OnBind(Intent? intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
        {
            try
            {
                CreateNotificationChannel();
                StartForeground(NOTIFICATION_ID, CreateNotification());
                
                System.Diagnostics.Debug.WriteLine("JPush保活服务已启动");
                
                // 确保JPush连接
                EnsureJPushConnection();
                
                return StartCommandResult.Sticky; // 服务被杀死后会自动重启
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"启动JPush保活服务失败: {ex.Message}");
                return StartCommandResult.NotSticky;
            }
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(
                    CHANNEL_ID,
                    "JPush保活服务",
                    NotificationImportance.Low)
                {
                    Description = "保持JPush推送服务运行"
                };

                var notificationManager = GetSystemService(NotificationService) as NotificationManager;
                notificationManager?.CreateNotificationChannel(channel);
            }
        }

        private Notification CreateNotification()
        {
            // 使用系统内置图标资源ID，避免引用问题
            const int iconResource = 17301632; // android.R.drawable.stat_notify_sync
            
            var builder = new NotificationCompat.Builder(this, CHANNEL_ID)
                .SetContentTitle("推送服务运行中")
                .SetContentText("正在保持推送连接活跃")
                .SetSmallIcon(iconResource)
                .SetPriority(NotificationCompat.PriorityLow)
                .SetOngoing(true)
                .SetShowWhen(false);

            return builder.Build();
        }

        private void EnsureJPushConnection()
        {
            try
            {
                // 检查并重新连接JPush
                CN.Jpush.Android.Api.JPushInterface.Init(this);
                System.Diagnostics.Debug.WriteLine("JPush连接检查完成");
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"JPush连接检查失败: {ex.Message}");
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            System.Diagnostics.Debug.WriteLine("JPush保活服务已停止");
        }
    }
}