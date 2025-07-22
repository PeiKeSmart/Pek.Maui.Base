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
            
            // 设置调试模式
            CN.Jpush.Android.Api.JPushInterface.SetDebugMode(true);
            System.Diagnostics.Debug.WriteLine("JPush调试模式已开启");
            
            // 初始化JPush
            CN.Jpush.Android.Api.JPushInterface.Init(this);
            System.Diagnostics.Debug.WriteLine("JPush初始化成功，APP_KEY: d47b7681630e2d2c3cea43b5");
            
            // 验证初始化
            System.Diagnostics.Debug.WriteLine("JPush初始化验证 - 调试模式设置已完成");
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"JPush初始化失败: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"堆栈跟踪: {ex.StackTrace}");
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
