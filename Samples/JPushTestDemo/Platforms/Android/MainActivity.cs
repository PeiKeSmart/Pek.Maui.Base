using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net;
using Android.OS;
using Android.Provider;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace JPushTestDemo;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    private const int NOTIFICATION_PERMISSION_REQUEST_CODE = 1001;
    private const int BATTERY_OPTIMIZATION_REQUEST_CODE = 1002;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        
        // 请求通知权限（Android 13+）
        RequestNotificationPermission();
        
        // 请求忽略电池优化
        RequestIgnoreBatteryOptimization();
        
        // JPush 相关初始化（MainApplication已经初始化过了，这里只是验证）
        VerifyJPushInitialization();
        
        // 运行JPush诊断
        JPushTestDemo.Platforms.Android.JPushDiagnostics.RunDiagnostics(this);
        
        // 运行广播诊断
        JPushTestDemo.Platforms.Android.BroadcastDiagnostics.RunFullDiagnostics(this);
        
        // 记录应用启动事件
        JPushTestDemo.Platforms.Android.PersistentLogger.LogEvent(this, "应用启动", "MainActivity.OnCreate");
        
        // 显示历史日志信息
        ShowLogInfo();
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

    private void RequestIgnoreBatteryOptimization()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
        {
            try
            {
                var powerManager = (PowerManager?)GetSystemService(PowerService);
                if (powerManager != null && !powerManager.IsIgnoringBatteryOptimizations(PackageName))
                {
                    var intent = new Intent(Settings.ActionRequestIgnoreBatteryOptimizations);
                    intent.SetData(Android.Net.Uri.Parse($"package:{PackageName}"));
                    StartActivityForResult(intent, BATTERY_OPTIMIZATION_REQUEST_CODE);
                    System.Diagnostics.Debug.WriteLine("请求忽略电池优化");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("已忽略电池优化或不支持");
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"请求忽略电池优化失败: {ex.Message}");
            }
        }
    }

    private void VerifyJPushInitialization()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("MainActivity - 验证JPush初始化状态");
            
            // 获取Registration ID来验证初始化状态
            string registrationId = CN.Jpush.Android.Api.JPushInterface.GetRegistrationID(this);
            System.Diagnostics.Debug.WriteLine($"MainActivity - JPush Registration ID: {registrationId}");
            
            if (string.IsNullOrEmpty(registrationId))
            {
                System.Diagnostics.Debug.WriteLine("MainActivity - Registration ID为空，JPush可能未正确初始化");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("MainActivity - JPush初始化验证成功");
            }
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"MainActivity - JPush初始化验证失败: {ex.Message}");
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

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        base.OnActivityResult(requestCode, resultCode, data);
        
        if (requestCode == BATTERY_OPTIMIZATION_REQUEST_CODE)
        {
            System.Diagnostics.Debug.WriteLine($"电池优化设置结果: {resultCode}");
        }
    }

    private void ShowLogInfo()
    {
        try
        {
            // 获取日志文件信息
            string logInfo = JPushTestDemo.Platforms.Android.PersistentLogger.GetLogFileInfo(this);
            System.Diagnostics.Debug.WriteLine($"日志文件信息: {logInfo}");
            
            // 显示最近的日志（用于调试）
            string recentLogs = JPushTestDemo.Platforms.Android.PersistentLogger.ReadRecentLogs(this, 20);
            System.Diagnostics.Debug.WriteLine("=== 最近的诊断日志 ===");
            System.Diagnostics.Debug.WriteLine(recentLogs);
            System.Diagnostics.Debug.WriteLine("=== 日志结束 ===");
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"显示日志信息失败: {ex.Message}");
        }
    }
}
