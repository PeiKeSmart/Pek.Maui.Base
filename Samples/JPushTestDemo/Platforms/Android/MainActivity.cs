using Android.App;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace JPushTestDemo;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    private const int NOTIFICATION_PERMISSION_REQUEST_CODE = 1001;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        
        // 请求通知权限（Android 13+）
        RequestNotificationPermission();
        
        // JPush 相关初始化
        InitializeJPush();
    }

    private void RequestNotificationPermission()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu) // Android 13+
        {
            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.PostNotifications) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new[] { Android.Manifest.Permission.PostNotifications }, NOTIFICATION_PERMISSION_REQUEST_CODE);
            }
        }
    }

    private void InitializeJPush()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("开始初始化JPush，上下文类型: " + this.GetType().Name);
            
            // 检查通知权限状态
            CheckNotificationPermissions();
            
            // 设置调试模式
            CN.Jpush.Android.Api.JPushInterface.SetDebugMode(true);
            System.Diagnostics.Debug.WriteLine("JPush调试模式已开启");
            
            // 初始化JPush
            CN.Jpush.Android.Api.JPushInterface.Init(this);
            System.Diagnostics.Debug.WriteLine("JPush初始化成功，APP_KEY: d47b7681630e2d2c3cea43b5");
            
            // 验证初始化
            System.Diagnostics.Debug.WriteLine("JPush初始化验证 - 调试模式设置已完成");
            
            // 延迟检查JPush状态
            var handler = new Android.OS.Handler(Android.OS.Looper.MainLooper);
            handler.PostDelayed(() => {
                CheckJPushStatus();
            }, 3000); // 3秒后检查
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"JPush初始化失败: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"堆栈跟踪: {ex.StackTrace}");
        }
    }

    private void CheckNotificationPermissions()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("=== 通知权限检查开始 ===");
            
            // 检查 POST_NOTIFICATIONS 权限 (Android 13+)
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                var postNotificationPermission = ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.PostNotifications);
                System.Diagnostics.Debug.WriteLine($"POST_NOTIFICATIONS 权限状态: {postNotificationPermission}");
            }
            
            // 检查通知是否启用
            var notificationManager = NotificationManagerCompat.From(this);
            bool areNotificationsEnabled = notificationManager.AreNotificationsEnabled();
            System.Diagnostics.Debug.WriteLine($"系统通知是否启用: {areNotificationsEnabled}");
            
            // 检查JPush通知状态
            int jpushNotificationStatus = CN.Jpush.Android.Api.JPushInterface.IsNotificationEnabled(this);
            System.Diagnostics.Debug.WriteLine($"JPush通知状态: {jpushNotificationStatus} (0=禁用, 1=启用, -1=未知)");
            
            System.Diagnostics.Debug.WriteLine("=== 通知权限检查结束 ===");
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"通知权限检查失败: {ex.Message}");
        }
    }

    private void CheckJPushStatus()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("=== JPush 状态检查开始 ===");
            
            // 获取Registration ID
            string regId = CN.Jpush.Android.Api.JPushInterface.GetRegistrationID(this);
            System.Diagnostics.Debug.WriteLine($"Registration ID: {regId}");
            
            // 检查连接状态
            bool isConnected = CN.Jpush.Android.Api.JPushInterface.GetConnectionState(this);
            System.Diagnostics.Debug.WriteLine($"JPush连接状态: {isConnected}");
            
            // 检查推送是否停止
            bool isPushStopped = CN.Jpush.Android.Api.JPushInterface.IsPushStopped(this);
            System.Diagnostics.Debug.WriteLine($"推送是否停止: {isPushStopped}");
            
            System.Diagnostics.Debug.WriteLine("=== JPush 状态检查结束 ===");
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"JPush状态检查失败: {ex.Message}");
        }
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        
        if (requestCode == NOTIFICATION_PERMISSION_REQUEST_CODE)
        {
            if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
            {
                System.Diagnostics.Debug.WriteLine("通知权限已授予");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("通知权限被拒绝");
            }
        }
    }
}
