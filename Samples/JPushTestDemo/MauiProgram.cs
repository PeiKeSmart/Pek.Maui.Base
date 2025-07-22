using Microsoft.Extensions.Logging;
using JPushTestDemo.Services;

namespace JPushTestDemo;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Register JPush service
#if ANDROID
		builder.Services.AddSingleton<IJPushService, Platforms.Android.Services.JPushServiceAndroid>();
#else
		// For other platforms, you can add platform-specific implementations
		// or a default implementation that does nothing
		builder.Services.AddSingleton<IJPushService, DefaultJPushService>();
#endif

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
