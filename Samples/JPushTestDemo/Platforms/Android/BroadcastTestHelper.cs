using Android.App;
using Android.Content;
using Android.Util;

namespace JPushTestDemo.Platforms.Android
{
    /// <summary>
    /// 广播测试助手
    /// 提供手动测试广播接收器的方法
    /// </summary>
    public static class BroadcastTestHelper
    {
        private const string TAG = "BroadcastTestHelper";

        /// <summary>
        /// 测试静态广播接收器
        /// </summary>
        public static void TestStaticBroadcast(Context context)
        {
            try
            {
                Log.Info(TAG, "=== 开始测试静态广播接收器 ===");
                
                // 1. 测试显式广播
                TestExplicitBroadcast(context);
                
                // 2. 测试隐式广播
                TestImplicitBroadcast(context);
                
                // 3. 测试JPush相关广播
                TestJPushBroadcast(context);
                
                Log.Info(TAG, "=== 静态广播测试完成 ===");
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"测试静态广播失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 测试显式广播（直接指定接收器）
        /// </summary>
        private static void TestExplicitBroadcast(Context context)
        {
            try
            {
                Log.Info(TAG, "--- 测试显式广播 ---");
                
                // 发送给SimpleTestReceiver
                var explicitIntent = new Intent(context, typeof(SimpleTestReceiver));
                explicitIntent.SetAction("com.test.static.broadcast");
                explicitIntent.PutExtra("test_data", "显式广播测试 - 应该能收到");
                
                context.SendBroadcast(explicitIntent);
                Log.Info(TAG, "✅ 显式广播已发送");
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"发送显式广播失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 测试隐式广播（通过Action匹配）
        /// </summary>
        private static void TestImplicitBroadcast(Context context)
        {
            try
            {
                Log.Info(TAG, "--- 测试隐式广播 ---");
                
                // 发送隐式广播
                var implicitIntent = new Intent("android.intent.action.TEST_BROADCAST");
                implicitIntent.PutExtra("test_data", "隐式广播测试 - Android 8.0+可能收不到");
                
                context.SendBroadcast(implicitIntent);
                Log.Info(TAG, "✅ 隐式广播已发送");
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"发送隐式广播失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 测试JPush相关广播
        /// </summary>
        private static void TestJPushBroadcast(Context context)
        {
            try
            {
                Log.Info(TAG, "--- 测试JPush广播 ---");
                
                // 模拟JPush通知接收广播
                var jpushIntent = new Intent("cn.jpush.android.intent.NOTIFICATION_RECEIVED_PROXY");
                jpushIntent.SetPackage(context.PackageName);
                jpushIntent.AddCategory("net.peikesmart.test");
                jpushIntent.PutExtra("cn.jpush.android.TITLE", "测试推送标题");
                jpushIntent.PutExtra("cn.jpush.android.MESSAGE", "测试推送内容");
                jpushIntent.PutExtra("cn.jpush.android.EXTRA", "{}");
                
                context.SendBroadcast(jpushIntent);
                Log.Info(TAG, "✅ JPush测试广播已发送");
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"发送JPush测试广播失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 检查广播接收器状态
        /// </summary>
        public static void CheckReceiverStatus(Context context)
        {
            try
            {
                Log.Info(TAG, "=== 检查接收器状态 ===");
                
                var packageManager = context.PackageManager;
                
                // 检查SimpleTestReceiver
                CheckSpecificReceiver(context, typeof(SimpleTestReceiver), "SimpleTestReceiver");
                
                // 检查JPushStaticReceiver
                CheckSpecificReceiver(context, typeof(JPushStaticReceiver), "JPushStaticReceiver");
                
                Log.Info(TAG, "=== 接收器状态检查完成 ===");
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"检查接收器状态失败: {ex.Message}", ex);
            }
        }

        private static void CheckSpecificReceiver(Context context, System.Type receiverType, string receiverName)
        {
            try
            {
                var componentName = new ComponentName(context, receiverType);
                var packageManager = context.PackageManager;
                
                var state = packageManager?.GetComponentEnabledSetting(componentName);
                Log.Info(TAG, $"{receiverName} 状态: {state}");
                
                if (state == ComponentEnabledState.Disabled)
                {
                    Log.Warn(TAG, $"⚠️ {receiverName} 被禁用");
                }
                else if (state == ComponentEnabledState.Enabled || state == ComponentEnabledState.Default)
                {
                    Log.Info(TAG, $"✅ {receiverName} 已启用");
                }
            }
            catch (System.Exception ex)
            {
                Log.Error(TAG, $"检查 {receiverName} 失败: {ex.Message}", ex);
            }
        }
    }
}