using Android.App;
using Android.Runtime;
using JPushTestDemo.Configuration;

namespace JPushTestDemo;

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
		
		// Initialize JPush
		try
		{
			// 设置调试模式
			CN.Jpush.Android.Api.JPushInterface.SetDebugMode(JPushConfig.DEBUG_MODE);
			System.Diagnostics.Debug.WriteLine($"JPush调试模式设置为: {JPushConfig.DEBUG_MODE}");
			
			// 初始化JPush
			CN.Jpush.Android.Api.JPushInterface.Init(this);
			System.Diagnostics.Debug.WriteLine($"JPush初始化成功，APP_KEY: {JPushConfig.APP_KEY}");
			
			// 设置保活相关配置
			CN.Jpush.Android.Api.JPushInterface.SetLatestNotificationNumber(this, 5);
			// 设置推送时间（全天24小时都可推送，不设置具体的禁推时间）
			CN.Jpush.Android.Api.JPushInterface.SetPushTime(this, null, 0, 23);
			System.Diagnostics.Debug.WriteLine("JPush保活配置设置完成");
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"JPush初始化失败: {ex.Message}");
			System.Diagnostics.Debug.WriteLine($"堆栈跟踪: {ex.StackTrace}");
		}
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
