namespace Pek.IO;

public static class DeviceHelper
{
    /// <summary>
    /// 获取APP包名
    /// </summary>
    /// <returns></returns>
    public static String? GetPackageName()
    {
        var packageName = String.Empty;

#if ANDROID
            packageName = Android.App.Application.Context.PackageName;
#elif IOS
        packageName = Foundation.NSBundle.MainBundle.BundleIdentifier;
#endif

        return packageName;
    }
}
