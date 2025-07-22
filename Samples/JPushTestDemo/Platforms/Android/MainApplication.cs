using Android.App;
using Android.Runtime;

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
		// Note: Replace "YOUR_APP_KEY" with your actual JPush app key
		// CN.Jpush.Android.Api.JPushInterface.SetDebugMode(true);
		// CN.Jpush.Android.Api.JPushInterface.Init(this);
	}

	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
