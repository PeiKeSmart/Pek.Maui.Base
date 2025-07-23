using Android.App;
using Android.Runtime;
using JPushTestDemo.Configuration;

namespace JPushTestDemo.Platforms.Android;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}

	public override void OnCreate()
	{
		base.OnCreate();
		
		// Initialize JPush - 确保在应用进程启动时就初始化
		try
		{
			System.Diagnostics.Debug.WriteLine("MainApplication.OnCreate - 开始初始化JPush");
			
			// 设置调试模式
			CN.Jpush.Android.Api.JPushInterface.SetDebugMode(JPushConfig.DEBUG_MODE);
			System.Diagnostics.Debug.WriteLine($"JPush调试模式设置为: {JPushConfig.DEBUG_MODE}");
			
			// 初始化JPush - 这是关键步骤，确保静态接收器能正常工作
			CN.Jpush.Android.Api.JPushInterface.Init(this);
			System.Diagnostics.Debug.WriteLine($"JPush初始化成功，APP_KEY: {JPushConfig.APP_KEY}");
			
			// 设置推送相关配置
			CN.Jpush.Android.Api.JPushInterface.SetLatestNotificationNumber(this, 5);
			CN.Jpush.Android.Api.JPushInterface.SetPushTime(this, null, 0, 23);
			
			// 获取Registration ID用于调试
			string registrationId = CN.Jpush.Android.Api.JPushInterface.GetRegistrationID(this);
			System.Diagnostics.Debug.WriteLine($"JPush Registration ID: {registrationId}");
			
			System.Diagnostics.Debug.WriteLine("JPush完整初始化完成");
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"JPush初始化失败: {ex.Message}");
			System.Diagnostics.Debug.WriteLine($"堆栈跟踪: {ex.StackTrace}");
		}
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
