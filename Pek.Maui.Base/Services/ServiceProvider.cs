namespace Pek;

public static class ServiceProvider
{
    static IServiceProvider Current =>
#if WINDOWS10_0_17763_0_OR_GREATER
			MauiWinUIApplication.Current.Services;
#elif ANDROID
            ((IPlatformApplication)MauiApplication.Current).Services;
#elif IOS || MACCATALYST
            ((IPlatformApplication)MauiUIApplicationDelegate.Current).Services;
#else
            throw new NotImplementedException();
#endif

    public static T GetService<T>() where T : notnull
        => Current.GetRequiredService<T>();
}