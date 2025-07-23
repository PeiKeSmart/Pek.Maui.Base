using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using AndroidX.Core.App;

namespace JPushTestDemo.Platforms.Android
{
    /// <summary>
    /// 广播接收器诊断工具
    /// 用于分析静态广播是否被系统限制
    /// </summary>
    public static class BroadcastDiagnostics
    {
        private const string TAG = "BroadcastDiagnostics";

        /// <summary>
        /// 运行完整的广播诊断
        /// </summary>
        public static void RunFullDiagnostics(Context context)
        {
            Log.Info(TAG, "=== 广播接收器诊断开始 ===");
            
            CheckAndroidVersion();
            CheckManifestRegistration(context);
            CheckAppStandbyState(context);
            CheckBatteryOptimization(context);
            CheckAutoStartPermission(context);
            TestDynamicBroadcast(context);
            SendTestBroadcast(context);
            
            Log.Info(TAG, "=== 广播接收器诊断完成 ===");
        }

        /// <summary>
        /// 检查Android版本和限制
        /// </summary>
        private static void CheckAndroidVersion()
        {
            Log.Info(TAG, "--- 检查Android版本限制 ---");
            Log.Info(TAG, $"Android版本: {Build.VERSION.Release} (API {Build.VERSION.SdkInt})");
            
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                Log.Warn(TAG, "⚠️ Android 8.0+ 对静态广播有严格限制");
                Log.Info(TAG, "大部分隐式广播被禁用，只有少数例外");
            }
            
            if (Build.VERSION.SdkInt >= BuildVersionCodes.P)
            {
                Log.Warn(TAG, "⚠️ Android 9.0+ 进一步限制后台活动");
            }
            
            Log.Info(TAG, $"设备厂商: {Build.Manufacturer}");
            Log.Info(TAG, $"设备型号: {Build.Model}");
        }

        /// <summary>
        /// 检查Manifest中的接收器注册
        /// </summary>
        private static void CheckManifestRegistration(Context context)
        {
            try
            {
                Log.Info(TAG, "--- 检查Manifest注册 ---");
                
                var packageManager = context.PackageManager;
                var packageInfo = packageManager?.GetPackageInfo(context.PackageName, PackageInfoFlags.Receivers);
                
                if (packageInfo?.Receivers != null)
                {
                    Log.Info(TAG, $"✅ 找到 {packageInfo.Receivers.Count} 个注册的接收器");
                    
                    foreach (var receiver in packageInfo.Receivers)
                    {
                        Log.Info(TAG, $"接收器: {receiver.Name}");
                        Log.Info(TAG, $"  - 启用: {receiver.Enabled}");
                        Log.Info(TAG, $"  - 导出: {receiver.Exported}");
                    }
                }
                else
                {
                    Log.Error(TAG, "❌ 未找到任何注册的接收器");
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"检查Manifest注册失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 检查应用待机状态
        /// </summary>
        private static void CheckAppStandbyState(Context context)
        {
            try
            {
                Log.Info(TAG, "--- 检查应用待机状态 ---");
                
                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    var usageStatsManager = context.GetSystemService(Context.UsageStatsService) as global::Android.App.Usage.UsageStatsManager;
                    if (usageStatsManager != null)
                    {
                        bool isAppInactive = usageStatsManager.IsAppInactive(context.PackageName);
                        Log.Info(TAG, $"应用是否处于待机状态: {isAppInactive}");
                        
                        if (isAppInactive)
                        {
                            Log.Warn(TAG, "⚠️ 应用处于待机状态，可能影响广播接收");
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"检查应用待机状态失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 检查电池优化状态
        /// </summary>
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
                        
                        if (!isIgnoringBatteryOptimizations)
                        {
                            Log.Warn(TAG, "⚠️ 应用未忽略电池优化，可能影响后台运行");
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"检查电池优化失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 检查自启动权限（厂商相关）
        /// </summary>
        private static void CheckAutoStartPermission(Context context)
        {
            Log.Info(TAG, "--- 检查自启动权限 ---");
            
            string manufacturer = Build.Manufacturer.ToLower();
            Log.Info(TAG, $"设备厂商: {manufacturer}");
            
            switch (manufacturer)
            {
                case "huawei":
                case "honor":
                    Log.Warn(TAG, "⚠️ 华为/荣耀设备需要手动设置自启动管理");
                    break;
                case "xiaomi":
                case "redmi":
                    Log.Warn(TAG, "⚠️ 小米设备需要关闭MIUI优化和设置自启动");
                    break;
                case "oppo":
                case "oneplus":
                    Log.Warn(TAG, "⚠️ OPPO/一加设备需要设置自启动和后台运行");
                    break;
                case "vivo":
                    Log.Warn(TAG, "⚠️ VIVO设备需要设置自启动和后台高耗电");
                    break;
                default:
                    Log.Info(TAG, "原生或其他厂商设备");
                    break;
            }
        }

        /// <summary>
        /// 测试动态广播接收器
        /// </summary>
        private static void TestDynamicBroadcast(Context context)
        {
            try
            {
                Log.Info(TAG, "--- 测试动态广播接收器 ---");
                
                var dynamicReceiver = new DynamicTestReceiver();
                var intentFilter = new IntentFilter("com.test.dynamic.broadcast");
                
                context.RegisterReceiver(dynamicReceiver, intentFilter);
                Log.Info(TAG, "✅ 动态接收器注册成功");
                
                // 发送测试广播
                var testIntent = new Intent("com.test.dynamic.broadcast");
                testIntent.PutExtra("test_data", "动态广播测试");
                context.SendBroadcast(testIntent);
                Log.Info(TAG, "✅ 测试广播已发送");
                
                // 延迟注销接收器
                var handler = new Handler(Looper.MainLooper);
                handler.PostDelayed(() =>
                {
                    try
                    {
                        context.UnregisterReceiver(dynamicReceiver);
                        Log.Info(TAG, "✅ 动态接收器已注销");
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error(TAG, $"注销动态接收器失败: {ex.Message}", ex);
                    }
                }, 2000);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"测试动态广播失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 发送测试广播给静态接收器
        /// </summary>
        private static void SendTestBroadcast(Context context)
        {
            try
            {
                Log.Info(TAG, "--- 发送测试广播给静态接收器 ---");
                
                // 发送显式广播给我们的接收器
                var explicitIntent = new Intent(context, typeof(SimpleTestReceiver));
                explicitIntent.SetAction("com.test.static.broadcast");
                explicitIntent.PutExtra("test_data", "静态广播测试");
                
                context.SendBroadcast(explicitIntent);
                Log.Info(TAG, "✅ 显式广播已发送给SimpleTestReceiver");
                
                // 发送系统广播（如果可能）
                var systemIntent = new Intent("android.intent.action.TEST_BROADCAST");
                context.SendBroadcast(systemIntent);
                Log.Info(TAG, "✅ 系统测试广播已发送");
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"发送测试广播失败: {ex.Message}", ex);
            }
        }
    }

    /// <summary>
    /// 动态测试接收器
    /// </summary>
    public class DynamicTestReceiver : BroadcastReceiver
    {
        private const string TAG = "DynamicTestReceiver";

        public override void OnReceive(Context? context, Intent? intent)
        {
            if (context == null || intent == null)
                return;

            try
            {
                string testData = intent.GetStringExtra("test_data") ?? "";
                Log.Info(TAG, $"✅ 动态接收器收到广播: {testData}");
                
                // 创建通知确认收到
                CreateTestNotification(context, "动态广播正常", testData);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"动态接收器处理失败: {ex.Message}", ex);
            }
        }

        private void CreateTestNotification(Context context, string title, string content)
        {
            try
            {
                const string channelId = "dynamic_test_channel";
                
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    var channel = new NotificationChannel(
                        channelId,
                        "动态广播测试",
                        NotificationImportance.High);

                    var notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
                    notificationManager?.CreateNotificationChannel(channel);
                }

                var notification = new NotificationCompat.Builder(context, channelId)
                    .SetContentTitle(title)
                    .SetContentText(content)
                    .SetSmallIcon(17301632)
                    .SetAutoCancel(true)
                    .Build();

                var notificationManager2 = context.GetSystemService(Context.NotificationService) as NotificationManager;
                notificationManager2?.Notify(5001, notification);
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"创建测试通知失败: {ex.Message}", ex);
            }
        }
    }
}