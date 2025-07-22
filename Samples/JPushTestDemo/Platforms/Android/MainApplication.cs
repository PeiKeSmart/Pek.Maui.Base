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
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"JPush初始化失败: {ex.Message}");
			System.Diagnostics.Debug.WriteLine($"堆栈跟踪: {ex.StackTrace}");
		}
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
