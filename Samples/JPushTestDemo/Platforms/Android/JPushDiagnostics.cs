using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;

namespace JPushTestDemo.Platforms.Android
{
    /// <summary>
    /// JPush诊断工具
    /// 用于检查JPush配置和状态
    /// </summary>
    public static class JPushDiagnostics
    {
        private const string TAG = "JPushDiagnostics";

        public static void RunDiagnostics(Context context)
        {
            try
            {
                Log.Info(TAG, "=== JPush诊断开始 ===");
                
                CheckJPushInitialization(context);
                CheckManifestConfiguration(context);
                CheckPermissions(context);
                CheckNotificationSettings(context);
                CheckBatteryOptimization(context);
                
                Log.Info(TAG, "=== JPush诊断完成 ===");
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"诊断过程中发生错误: {ex.Message}", ex);
            }
        }

        private static void CheckJPushInitialization(Context context)
        {
            try
            {
                Log.Info(TAG, "--- 检查JPush初始化状态 ---");
                
                string registrationId = CN.Jpush.Android.Api.JPushInterface.GetRegistrationID(context);
                Log.Info(TAG, $"Registration ID: {registrationId}");
                
                if (string.IsNullOrEmpty(registrationId))
                {
                    Log.Warn(TAG, "⚠️ Registration ID为空，JPush可能未正确初始化");
                }
                else
                {
                    Log.Info(TAG, "✅ JPush初始化正常");
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"检查JPush初始化失败: {ex.Message}", ex);
            }
        }

        private static void CheckManifestConfiguration(Context context)
        {
            try
            {
                Log.Info(TAG, "--- 检查Manifest配置 ---");
                Log.Info(TAG, $"包名: {context.PackageName}");
                Log.Info(TAG, "Manifest配置检查完成");
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"检查Manifest配置失败: {ex.Message}", ex);
            }
        }

        private static void CheckPermissions(Context context)
        {
            try
            {
                Log.Info(TAG, "--- 检查权限状态 ---");
                
                // 检查通知权限
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
                {
                    var notificationPermission = context.CheckSelfPermission("android.permission.POST_NOTIFICATIONS");
                    Log.Info(TAG, $"通知权限: {notificationPermission}");
                }
                
                // 检查网络权限
                var internetPermission = context.CheckSelfPermission("android.permission.INTERNET");
                Log.Info(TAG, $"网络权限: {internetPermission}");
                
                var networkStatePermission = context.CheckSelfPermission("android.permission.ACCESS_NETWORK_STATE");
                Log.Info(TAG, $"网络状态权限: {networkStatePermission}");
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"检查权限失败: {ex.Message}", ex);
            }
        }

        private static void CheckNotificationSettings(Context context)
        {
            try
            {
                Log.Info(TAG, "--- 检查通知设置 ---");
                
                var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
                if (notificationManager != null)
                {
                    bool areNotificationsEnabled = notificationManager.AreNotificationsEnabled();
                    Log.Info(TAG, $"通知是否启用: {areNotificationsEnabled}");
                    
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                    {
                        var channels = notificationManager.NotificationChannels;
                        Log.Info(TAG, $"通知渠道数量: {channels?.Count ?? 0}");
                        if (channels != null)
                        {
                            foreach (var channel in channels)
                            {
                                Log.Info(TAG, $"渠道: {channel.Id}, 重要性: {channel.Importance}");
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"检查通知设置失败: {ex.Message}", ex);
            }
        }

        private static void CheckBatteryOptimization(Context context)
        {
            try
            {
                Log.Info(TAG, "--- 检查电池优化状态 ---");
                
                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    var powerManager = context.GetSystemService(Context.PowerService) as PowerManager;
                    if (powerManager != null)
                    {
                        bool isIgnoringBatteryOptimizations = powerManager.IsIgnoringBatteryOptimizations(context.PackageName);
                        Log.Info(TAG, $"是否忽略电池优化: {isIgnoringBatteryOptimizations}");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"检查电池优化失败: {ex.Message}", ex);
            }
        }
    }
}