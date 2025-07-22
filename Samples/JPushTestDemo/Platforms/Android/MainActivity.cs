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
        
        // JPush 相关初始化
        InitializeJPush();
        
        // 启动保活服务
        StartKeepAliveService();
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

    private void InitializeJPush()
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("开始初始化JPush，上下文类型: " + this.GetType().Name);
            
            // 设置调试模式
            CN.Jpush.Android.Api.JPushInterface.SetDebugMode(true);
            System.Diagnostics.Debug.WriteLine("JPush调试模式已开启");
            
            // 初始化JPush
            CN.Jpush.Android.Api.JPushInterface.Init(this);
            System.Diagnostics.Debug.WriteLine("JPush初始化成功，APP_KEY: d47b7681630e2d2c3cea43b5");
            
            // 设置保活策略
            CN.Jpush.Android.Api.JPushInterface.SetLatestNotificationNumber(this, 5);
            System.Diagnostics.Debug.WriteLine("JPush保活策略设置完成");
            
            // 验证初始化
            System.Diagnostics.Debug.WriteLine("JPush初始化验证 - 调试模式设置已完成");
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"JPush初始化失败: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"堆栈跟踪: {ex.StackTrace}");
        }
    }

    private void StartKeepAliveService()
    {
        try
        {
            var serviceIntent = new Intent(this, typeof(JPushTestDemo.Platforms.Android.JPushKeepAliveService));
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                StartForegroundService(serviceIntent);
            }
            else
            {
                StartService(serviceIntent);
            }
            System.Diagnostics.Debug.WriteLine("JPush保活服务启动成功");
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"启动JPush保活服务失败: {ex.Message}");
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
}
