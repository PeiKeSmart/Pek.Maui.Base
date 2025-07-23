using Android.Content;
using Android.Util;

namespace JPushTestDemo.Platforms.Android
{
    /// <summary>
    /// 分层测试计划
    /// 逐步验证静态广播接收器的可行性
    /// </summary>
    public static class LayeredTestPlan
    {
        private const string TAG = "LayeredTestPlan";

        /// <summary>
        /// 执行完整的分层测试
        /// </summary>
        public static void ExecuteFullTest(Context context)
        {
            PersistentLogger.LogEvent(context, "分层测试开始", "LayeredTestPlan.ExecuteFullTest");
            
            // 第一层：基础系统广播测试
            TestLayer1_SystemBroadcast(context);
            
            // 第二层：应用生命周期广播测试
            TestLayer2_AppLifecycle(context);
            
            // 第三层：JPush广播测试
            TestLayer3_JPushBroadcast(context);
            
            PersistentLogger.LogEvent(context, "分层测试完成", "所有测试层已执行");
        }

        /// <summary>
        /// 第一层：测试最基础的系统广播
        /// 目标：验证静态广播接收器是否完全被禁用
        /// </summary>
        private static void TestLayer1_SystemBroadcast(Context context)
        {
            PersistentLogger.LogEvent(context, "第一层测试", "系统广播接收测试");
            
            try
            {
                // 记录测试开始
                PersistentLogger.LogDiagnostic(context, TAG, "开始测试BOOT_COMPLETED广播接收");
                
                // 这个测试只能通过重启手机来验证
                // 这里只是记录测试意图
                PersistentLogger.LogDiagnostic(context, TAG, "BOOT_COMPLETED测试需要重启手机验证");
                PersistentLogger.LogDiagnostic(context, TAG, "预期结果：重启后应该看到SimpleTestReceiver的日志");
                
            }
            catch (System.Exception ex)
            {
                PersistentLogger.LogError(context, TAG, "第一层测试失败", ex);
            }
        }

        /// <summary>
        /// 第二层：测试应用相关的广播
        /// 目标：验证应用相关的静态广播是否工作
        /// </summary>
        private static void TestLayer2_AppLifecycle(Context context)
        {
            PersistentLogger.LogEvent(context, "第二层测试", "应用生命周期广播测试");
            
            try
            {
                // 测试应用安装/更新广播
                PersistentLogger.LogDiagnostic(context, TAG, "应用生命周期广播测试");
                PersistentLogger.LogDiagnostic(context, TAG, "这些广播在应用更新时会触发");
                
                // 记录当前应用状态
                string packageName = context.PackageName;
                PersistentLogger.LogDiagnostic(context, TAG, $"当前包名: {packageName}");
                
            }
            catch (System.Exception ex)
            {
                PersistentLogger.LogError(context, TAG, "第二层测试失败", ex);
            }
        }

        /// <summary>
        /// 第三层：测试JPush相关广播
        /// 目标：验证JPush推送广播是否能被静态接收器接收
        /// </summary>
        private static void TestLayer3_JPushBroadcast(Context context)
        {
            PersistentLogger.LogEvent(context, "第三层测试", "JPush广播接收测试");
            
            try
            {
                // 检查JPush初始化状态
                string registrationId = CN.Jpush.Android.Api.JPushInterface.GetRegistrationID(context);
                PersistentLogger.LogDiagnostic(context, TAG, $"JPush Registration ID: {registrationId}");
                
                if (string.IsNullOrEmpty(registrationId))
                {
                    PersistentLogger.LogDiagnostic(context, TAG, "⚠️ JPush未初始化，推送测试无法进行");
                    return;
                }
                
                // 记录JPush广播测试说明
                PersistentLogger.LogDiagnostic(context, TAG, "JPush广播测试说明：");
                PersistentLogger.LogDiagnostic(context, TAG, "1. 应用需要完全关闭");
                PersistentLogger.LogDiagnostic(context, TAG, "2. 通过JPush控制台发送推送");
                PersistentLogger.LogDiagnostic(context, TAG, "3. 查看是否有JPushMessageReceiver的日志");
                
            }
            catch (System.Exception ex)
            {
                PersistentLogger.LogError(context, TAG, "第三层测试失败", ex);
            }
        }

        /// <summary>
        /// 分析测试结果
        /// </summary>
        public static void AnalyzeTestResults(Context context)
        {
            PersistentLogger.LogEvent(context, "测试结果分析", "开始分析各层测试结果");
            
            try
            {
                string allLogs = PersistentLogger.ReadAllLogs(context);
                
                // 分析第一层测试结果
                bool hasBootCompleted = allLogs.Contains("静态广播接收 - Action: android.intent.action.BOOT_COMPLETED");
                PersistentLogger.LogDiagnostic(context, TAG, $"第一层测试结果 - BOOT_COMPLETED: {(hasBootCompleted ? "✅ 通过" : "❌ 失败")}");
                
                // 分析第三层测试结果
                bool hasJPushBroadcast = allLogs.Contains("JPush静态广播") || allLogs.Contains("JPushMessageReceiver");
                PersistentLogger.LogDiagnostic(context, TAG, $"第三层测试结果 - JPush广播: {(hasJPushBroadcast ? "✅ 通过" : "❌ 失败")}");
                
                // 给出结论
                if (!hasBootCompleted)
                {
                    PersistentLogger.LogDiagnostic(context, TAG, "🔍 结论：静态广播接收器被系统限制");
                    PersistentLogger.LogDiagnostic(context, TAG, "原因：Android 8.0+限制或厂商定制限制");
                    PersistentLogger.LogDiagnostic(context, TAG, "建议：考虑使用厂商推送通道");
                }
                else if (hasBootCompleted && !hasJPushBroadcast)
                {
                    PersistentLogger.LogDiagnostic(context, TAG, "🔍 结论：系统广播正常，但JPush广播被限制");
                    PersistentLogger.LogDiagnostic(context, TAG, "原因：JPush相关广播可能被特别限制");
                    PersistentLogger.LogDiagnostic(context, TAG, "建议：检查JPush配置或使用厂商通道");
                }
                else if (hasBootCompleted && hasJPushBroadcast)
                {
                    PersistentLogger.LogDiagnostic(context, TAG, "🔍 结论：静态广播接收器工作正常");
                    PersistentLogger.LogDiagnostic(context, TAG, "建议：检查用户设置（自启动、电池优化等）");
                }
                
            }
            catch (System.Exception ex)
            {
                PersistentLogger.LogError(context, TAG, "分析测试结果失败", ex);
            }
        }
    }
}